using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ACE.Managers;
using ACE.Common;
using log4net;

namespace ACE.Network
{
    public class ConnectionListener
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog packetLog = LogManager.GetLogger("Packets");
        public Socket Socket { get; private set; }

        private IPEndPoint listenerEndpoint;

        private readonly uint listeningPort;

        private readonly byte[] buffer = new byte[Packet.MaxPacketSize];

        private readonly IPAddress listeningHost;

        public ConnectionListener(IPAddress host, uint port)
        {
            log.DebugFormat("ConnectionListener ctor, host {0} port {1}", host, port);
            listeningHost = host;
            listeningPort = port;
        }

        public void Start()
        {
            log.DebugFormat("Starting ConnectionListener, host {0} port {1}", listeningHost, listeningPort);
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
                log.FatalFormat("Network Socket has thrown: {0}", exception.Message);
            }
        }

        public void Shutdown()
        {
            log.DebugFormat("Shutting down ConnectionListener, host {0} port {1}", listeningHost, listeningPort);
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
                log.FatalFormat("Network Socket has thrown: {0}", exception.Message);
            }
        }

        private void OnDataReceieve(IAsyncResult result)
        {
            EndPoint clientEndPoint = null;
            try
            {
                clientEndPoint = new IPEndPoint(listeningHost, 0);
                int dataSize = Socket.EndReceiveFrom(result, ref clientEndPoint);

                byte[] data = new byte[dataSize];
                Buffer.BlockCopy(buffer, 0, data, 0, dataSize);

                IPEndPoint ipEndpoint = (IPEndPoint)clientEndPoint;

                if (packetLog.IsDebugEnabled)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(String.Format("Received Packet (Len: {0}) [{1}:{2}=>{3}:{4}]", data.Length, ipEndpoint.Address, ipEndpoint.Port, listenerEndpoint.Address, listenerEndpoint.Port));
                    sb.AppendLine(data.BuildPacketString());
                    packetLog.Debug(sb.ToString());
                }

                var packet = new ClientPacket(data);
                WorldManager.ProcessPacket(packet, ipEndpoint);
            }
            catch (SocketException socketException)
            {
                // If we get "Connection has been forcibly closed..." error, just eat the exception and continue on
                // This gets sent when the remote host terminates the connection (on UDP? interesting...)
                // TODO: There might be more, should keep an eye out. Logged message will help here.
                if (socketException.ErrorCode == 0x2746)
                {
                    log.DebugFormat("Network Socket on IP {2} has thrown {0}: {1}", socketException.ErrorCode, socketException.Message, clientEndPoint != null ? clientEndPoint.ToString() : "Unknown");
                }
                else
                {
                    log.FatalFormat("Network Socket on IP {2} has thrown {0}: {1}", socketException.ErrorCode, socketException.Message, clientEndPoint != null ? clientEndPoint.ToString() : "Unknown");
                    return;
                }
            }
            Listen();
        }
    }
}
