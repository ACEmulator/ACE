using ACE.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ACE.Network
{
    public enum ConnectionType
    {
        Login,
        World
    }

    public class ConnectionListener
    {
        public Socket Socket { get; private set; }

        private ConnectionType listenerType;

        private uint listeningPort;
        private byte[] buffer = new byte[Packet.MaxPacketSize];

        public ConnectionListener(uint port, ConnectionType type)
        {
            listenerType  = type;
            listeningPort = port;
        }

        public void Start()
        {
            try
            {
                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                Socket.Bind(new IPEndPoint(IPAddress.Any, (int)listeningPort));
                Listen();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public void Shutdown()
        {
            if (Socket != null && Socket.IsBound)
                Socket.Close();
        }

        private void Listen()
        {
            try
            {
                EndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref clientEndPoint, OnDataReceieve, Socket);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void OnDataReceieve(IAsyncResult result)
        {
            EndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);

            byte[] data;
            try
            {
                int dataSize = Socket.EndReceiveFrom(result, ref clientEndPoint);

                data = new byte[dataSize];
                Buffer.BlockCopy(buffer, 0, data, 0, dataSize);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return;
            }

            var session = WorldManager.Find((IPEndPoint)clientEndPoint);

            var packet = new ClientPacket(data);
            PacketManager.HandlePacket(listenerType, packet, session);

            #region DEBUG
                uint checksum = packet.CalculateChecksum(session, listenerType);
                Console.WriteLine($"Received({listenerType.ToString()}): Size: {data.Length}, Hash: 0x{checksum.ToString("X8")} 0x{ packet.Header.Checksum.ToString("X8")}, Fragments: {packet.Fragments.Count}, Flags: {packet.Header.Flags}");
            #endregion

            Listen();
        }
    }

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

        private static byte[] ConstructPacket(ConnectionType type, Packet packet, Session session)
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
                            fragment.Header.Sequence = connectionData.FragmentSequence++;
                            fragment.Header.Count    = 1;
                            fragment.Header.Size     = (ushort)(PacketFragementHeader.HeaderSize + fragment.Payload.BaseStream.Length);
                            fragment.Header.Index    = (ushort)i;
                            //fragment.Header.Group    = 5;
                            fragment.Header.Id       = 0x30B0008; // this seems to be a global incremental value

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
                    packet.Header.Checksum = packet.CalculateChecksum(session, type);

                    packetWriter.Seek(0, SeekOrigin.Begin);
                    packetWriter.Write(packet.Header.GetRaw());

                    return packetStream.ToArray();
                }
            }
        }

        public static void SendPacket(ConnectionType type, ServerPacket packet, Session session)
        {
            GetSocket(type).SendTo(ConstructPacket(type, packet, session), session.EndPoint);
        }
    }
}
