using System;
using System.Net;
using System.Net.Sockets;

using ACE.Managers;
using ACE.Network.Managers;
using System.Text;

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
        private IPEndPoint listenerEndpoint;

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
                listenerEndpoint = new IPEndPoint(IPAddress.Any, (int)listeningPort);
                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                Socket.Bind(listenerEndpoint);
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
            IPEndPoint ipEndpoint = (IPEndPoint)clientEndPoint;
            var session = WorldManager.Find(ipEndpoint);
#if NETWORKDEBUG
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("Received Packet (Len: {0}) on {1} [{2}:{3}=>{4}:{5}]", data.Length, listenerType, ipEndpoint.Address, ipEndpoint.Port, listenerEndpoint.Address, listenerEndpoint.Port));
            sb.AppendLine(data.BuildPacketString());
            Console.WriteLine(sb.ToString());
#endif
            var packet = new ClientPacket(data);
            session.HandlePacket(listenerType, packet);

            Listen();
        }
    }
}
