using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;

using ACE.Common;
using ACE.Common.Cryptography;

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

                    //if (type == ConnectionType.World && packet.Header.Sequence >= 2u)
                    //    session.CachedPackets.TryAdd(packet.Header.Sequence, new CachedPacket(issacXor, packet));

                    buffer = packetStream.ToArray();
                    return true;
                }
            }
        }

        public static void SendPacketDirect(ConnectionType type, ServerPacket2 packet, Session session) { GetSocket(type).SendTo(packet.GetPayload(), session.EndPoint); }

        public static void SendPacket(ConnectionType type, ServerPacket packet, Session session, bool useHeaders = false)
        {
            byte[] buffer;
            if (ConstructPacket(out buffer, type, packet, session, useHeaders))
                GetSocket(type).SendTo(buffer, session.EndPoint);
        }


        //Create Packet
        //If first packet for this bundle, fill any optional headers
        //Append fragments that will fit
        //--Check each fragment to see if it is a partial fit
        //If we have a carryOverFragment or more messages, create additional packets.

        public static void SendBundle(NetworkBundle bundle)
        {
            Socket socket = GetSocket(bundle.connectionType);
            Session session = bundle.sender;
            var connectionData = (bundle.connectionType == ConnectionType.Login ? session.LoginConnection : session.WorldConnection);
            bool firstPacket = true;

            MessageFragment carryOverMessage = null;

            while (firstPacket || carryOverMessage != null || bundle.messages.Count > 0)
            {
                uint issacXor = (bundle.encryptedChecksum) ? session.GetIssacValue(PacketDirection.Server, bundle.connectionType) : 0u;
                ServerPacket2 packet = new ServerPacket2(issacXor);
                PacketHeader packetHeader = packet.Header;
                if(bundle.encryptedChecksum)
                    packetHeader.Flags |= PacketHeaderFlags.EncryptedChecksum;

                uint availableSpace = Packet.MaxPacketDataSize;

                if (firstPacket)
                {
                    firstPacket = false;
                    using (var bodyStream = new MemoryStream())
                    {
                        using (var bodyWriter = new BinaryWriter(bodyStream))
                        {

                            if (bundle.ackSeq)
                            {
                                //Do AckSeq
                            }
                            if (bundle.timeSync)
                            {
                                //Do TimeSync
                            }
                            if (bundle.clientTime != -1f)
                            {
                                //Do EchoResponse
                            }
                            //TODO body content and checksum.
                            bodyWriter.Flush();
                            packet.SetBody(bodyStream.ToArray());
                        }
                    }
                }

                if (carryOverMessage != null || bundle.messages.Count > 0)
                {
                    packetHeader.Flags |= PacketHeaderFlags.BlobFragments;

                    while (carryOverMessage != null || bundle.messages.Count > 0)
                    {
                        if (availableSpace <= PacketFragmentHeader.HeaderSize)
                            break;

                        MessageFragment currentMessageFragment;

                        if (carryOverMessage != null)
                        {
                            currentMessageFragment = carryOverMessage;
                            carryOverMessage = null;
                            currentMessageFragment.index++;
                        }
                        else
                        {
                            currentMessageFragment = new MessageFragment(bundle.messages.Dequeue());
                        }

                        var currentGameMessage = currentMessageFragment.message;

                        ServerPacketFragment2 fragment = new ServerPacketFragment2();
                        PacketFragmentHeader fragmentHeader = fragment.Header;
                        availableSpace -= PacketFragmentHeader.HeaderSize;

                        currentMessageFragment.sequence = currentMessageFragment.sequence == 0 ? connectionData.FragmentSequence++ : currentMessageFragment.sequence;

                        uint dataToSend = (uint)currentGameMessage.Data.Length - currentMessageFragment.position;
                        if (dataToSend > availableSpace)
                        {
                            dataToSend = availableSpace;
                            carryOverMessage = currentMessageFragment;
                        }

                        if (currentMessageFragment.count == 0)
                        {
                            uint remainingData = (uint)currentGameMessage.Data.Length - dataToSend;
                            currentMessageFragment.count = (ushort)(Math.Ceiling((double)remainingData / PacketFragment.MaxFragmentDataSize) + 1);
                        }

                        fragmentHeader.Sequence = currentMessageFragment.sequence;
                        fragmentHeader.Id = 0x80000000;
                        fragmentHeader.Size = (ushort)(PacketFragmentHeader.HeaderSize + dataToSend);
                        fragmentHeader.Count = currentMessageFragment.count;
                        fragmentHeader.Index = currentMessageFragment.index;
                        fragmentHeader.Group = 10;

                        byte[] fragmentBodyBytes = new byte[dataToSend];
                        currentGameMessage.Data.Read(fragmentBodyBytes, (int)currentMessageFragment.position, (int)dataToSend);
                        fragment.Body = fragmentBodyBytes;

                        currentMessageFragment.position = currentMessageFragment.position + dataToSend;
                    }
                }

                if (packetHeader.HasFlag(PacketHeaderFlags.EncryptedChecksum) && connectionData.PacketSequence < 2)
                    connectionData.PacketSequence = 2;

                packetHeader.Sequence = connectionData.PacketSequence++;
                packetHeader.Table = 0x14;
                packetHeader.Time = (ushort)connectionData.ServerTime;

                if (bundle.connectionType == ConnectionType.World && packetHeader.Sequence >= 2u)
                    session.CachedPackets.TryAdd(packet.Header.Sequence, new CachedPacket(issacXor, packet));

                socket.SendTo(packet.GetPayload(), bundle.sender.EndPoint);   
            }
        }

        private class MessageFragment
        {
            public GameMessage message { get; private set; }
            public uint position { get; set; }
            public uint sequence { get; set; }
            public ushort index { get; set; }
            public ushort count { get; set; }

            public MessageFragment(GameMessage message)
            {
                this.message = message;
                index = 0;
                count = 0;
                position = 0;
                sequence = 0;
            }
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
    }
}
