using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ACE.Managers;
using ACE.Common;

namespace ACE.Network
{
    public class ConnectionListener
    {
        public Socket Socket { get; private set; }

        private IPEndPoint listenerEndpoint;

        private readonly uint listeningPort;

        private readonly byte[] buffer = new byte[Packet.MaxPacketSize];

        private readonly IPAddress listeningHost;

        public ConnectionListener(IPAddress host, uint port)
        {
            listeningHost = host;
            listeningPort = port;
        }

        public void Start()
        {
            try
            {
                listenerEndpoint = new IPEndPoint(listeningHost, (int)listeningPort);
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
                EndPoint clientEndPoint = new IPEndPoint(listeningHost, 0);
                Socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref clientEndPoint, OnDataReceieve, Socket);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void OnDataReceieve(IAsyncResult result)
        {
            EndPoint clientEndPoint = new IPEndPoint(listeningHost, 0);

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
            sb.AppendLine(String.Format("Received Packet (Len: {0}) [{1}:{2}=>{3}:{4}]", data.Length, ipEndpoint.Address, ipEndpoint.Port, listenerEndpoint.Address, listenerEndpoint.Port));
            sb.AppendLine(data.BuildPacketString());
            Console.WriteLine(sb.ToString());
#endif
            var packet = new ClientPacket(data);
            session.HandlePacket(packet);

            Listen();
        }
    }
}
