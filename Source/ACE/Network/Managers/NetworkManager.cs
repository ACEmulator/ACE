using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;

using ACE.Common;

namespace ACE.Network.Managers
{
    public static class NetworkManager
    {
        private static List<ConnectionListener> loginListeners = new List<ConnectionListener>();
        private static List<ConnectionListener> worldListeners = new List<ConnectionListener>();

        public static void Initialise()
        {
            loginListeners.Add(new ConnectionListener(ConfigManager.Config.Server.Network.LoginPort, ConnectionType.Login));
            loginListeners.Add(new ConnectionListener(ConfigManager.Config.Server.Network.LoginPort + 1, ConnectionType.Login));

            foreach (var loginListener in loginListeners)
                loginListener.Start();

            worldListeners.Add(new ConnectionListener(ConfigManager.Config.Server.Network.WorldPort, ConnectionType.World));
            worldListeners.Add(new ConnectionListener(ConfigManager.Config.Server.Network.WorldPort + 1, ConnectionType.World));

            foreach (var worldListener in worldListeners)
                worldListener.Start();
        }
        
        private static Socket GetSocket(ConnectionType type)
        {
            if (type == ConnectionType.Login)
                return loginListeners[0].Socket;
            else // ConnectionListenerType.World
                return worldListeners[0].Socket;
        }

        // TODO: packet pipe really needs a rework...
        private static bool ConstructPacket(out byte[] buffer, ConnectionType type, ServerPacket packet, Session session, bool useHeaders = false)
        {
            var connectionData = (type == ConnectionType.Login ? session.LoginConnection : session.WorldConnection);
            using (var packetStream = new MemoryStream())
            {
                using (var packetWriter = new BinaryWriter(packetStream))
                {
                    packetWriter.Seek((int)PacketHeader.HeaderSize, SeekOrigin.Begin);
                    packetWriter.Write(packet.Data.ToArray());

                    if (packet.Fragments.Count > 0)
                    {
                        packet.Header.Flags |= PacketHeaderFlags.BlobFragments;

                        for (int i = 0; i < packet.Fragments.Count; i++)
                        {
                            var fragment = (ServerPacketFragment)packet.Fragments[i];

                            uint fragmentsRequired = ((uint)fragment.Data.Length / PacketFragment.MaxFragmentDataSize);
                            if ((fragment.Data.Length % PacketFragment.MaxFragmentDataSize) != 0)
                                fragmentsRequired++;

                            if (fragmentsRequired > 1u)
                            {
                                fragment.Data.Seek(0L, SeekOrigin.Begin);

                                uint dataToSend  = (uint)fragment.Data.Length;
                                uint fragmentSeq = connectionData.FragmentSequence++;

                                for (uint j = 0u; j < fragmentsRequired; j++)
                                {
                                    uint fragmentSize = dataToSend > PacketFragment.MaxFragmentDataSize ? PacketFragment.MaxFragmentDataSize : dataToSend;

                                    byte[] data = new byte[fragmentSize];
                                    fragment.Data.Read(data, 0, (int)fragmentSize);

                                    var newPacket   = new ServerPacket(packet.Header.Id, PacketHeaderFlags.EncryptedChecksum | PacketHeaderFlags.BlobFragments);
                                    var newFragment = new ServerPacketFragment(fragment.Header.Group);
                                    newFragment.Header.Sequence = fragmentSeq;
                                    newFragment.Header.Id       = 0x80000000;
                                    newFragment.Header.Count    = (ushort)fragmentsRequired;
                                    newFragment.Header.Index    = (ushort)j;
                                    newFragment.Data.Write(data, 0, (int)fragmentSize);
                                    newPacket.Fragments.Add(newFragment);

                                    SendPacket(type, newPacket, session, true);

                                    dataToSend -= fragmentSize;
                                }

                                buffer = null;
                                return false;
                            }

                            fragment.Header.Size = (ushort)(PacketFragmentHeader.HeaderSize + fragment.Payload.BaseStream.Length);

                            if (!useHeaders)
                            {
                                fragment.Header.Sequence = connectionData.FragmentSequence++;
                                fragment.Header.Id       = 0x80000000; // this seems to be a global incremental value
                                fragment.Header.Count    = 1;
                                fragment.Header.Index    = (ushort)i;
                            }

                            packetWriter.Write(fragment.Header.GetRaw());
                            packetWriter.Write(fragment.Data.ToArray());
                        }
                    }

                    if (packet.Header.HasFlag(PacketHeaderFlags.EncryptedChecksum) && connectionData.PacketSequence < 2)
                        connectionData.PacketSequence = 2;

                    packet.Header.Sequence = connectionData.PacketSequence++;
                    packet.Header.Size     = (ushort)(packetWriter.BaseStream.Length - PacketHeader.HeaderSize);
                    packet.Header.Table    = 0x14;
                    packet.Header.Time     = (ushort)connectionData.ServerTime;

                    uint issacXor;
                    packet.Header.Checksum = packet.CalculateChecksum(session, type, 0u, out issacXor);

                    packetWriter.Seek(0, SeekOrigin.Begin);
                    packetWriter.Write(packet.Header.GetRaw());

                    if (type == ConnectionType.World && packet.Header.Sequence >= 2u)
                        session.CachedPackets.TryAdd(packet.Header.Sequence, new CachedPacket(issacXor, packet));

                    buffer = packetStream.ToArray();
                    return true;
                }
            }
        }

        public static void SendPacketDirect(ConnectionType type, ServerPacket packet, Session session) { GetSocket(type).SendTo(packet.Data.ToArray(), session.EndPoint); }

        public static void SendPacket(ConnectionType type, ServerPacket packet, Session session, bool useHeaders = false)
        {
            byte[] buffer;
            if (ConstructPacket(out buffer, type, packet, session, useHeaders))
                GetSocket(type).SendTo(buffer, session.EndPoint);
        }

        //TODO Move group to per message property, think this broke sounds.
        //Hacky. TODO Need to rewrite some code to build packet/fragments differently
        //TODO Further testing on multifragment sending.
        //TODO Packet Queue per session, so in theory these calls only get called flushing packets out of the queue
        public static void SendWorldMessage(Session session, GameMessage message)
        {
            ServerPacketFragment fragment = new ServerPacketFragment(9, message);
            var packet = new ServerPacket(0x18, PacketHeaderFlags.EncryptedChecksum);
            packet.Fragments.Add(fragment);
            NetworkManager.SendPacket(ConnectionType.World, packet, session);
        }

        public static void SendWorldMessages(Session session, IEnumerable<GameMessage> messages)
        {
#if MultiFragment
            var packet = new ServerPacket(0x18, PacketHeaderFlags.EncryptedChecksum);
            foreach (var message in messages)
            {
                ServerPacketFragment fragment = new ServerPacketFragment(9, message);
                packet.Fragments.Add(fragment);
            }
            NetworkManager.SendPacket(ConnectionType.World, packet, session);
#else
            foreach(var message in messages)
            {
                SendWorldMessage(session, message);
            }
#endif
        }

        public static void SendTurbineChatMessage(Session session, GameMessage message)
        {
            ServerPacketFragment fragment = new ServerPacketFragment(4, message);
            var packet = new ServerPacket(0x18, PacketHeaderFlags.EncryptedChecksum);
            packet.Fragments.Add(fragment);
            NetworkManager.SendPacket(ConnectionType.World, packet, session);
        }
    }
}
