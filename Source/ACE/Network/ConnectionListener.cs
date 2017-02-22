using System;
using System.Net;
using System.Net.Sockets;

using ACE.Managers;
using ACE.Network.Managers;

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
            Console.WriteLine("Received");
            data.OutputDataToConsole();
            Console.WriteLine();
            var packet = new ClientPacket(data);
            session.HandlePacket(listenerType, packet);

            Listen();
        }
    }
}
