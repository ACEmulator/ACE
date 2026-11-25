using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using ACE.Common.Logging;
using ACE.Server.Network.Managers;

namespace ACE.Server.Network
{
    // Reference: https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets.socket.beginreceivefrom?view=net-7.0
    public class ConnectionListener
    {
        private static readonly ILogger log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILogger packetLog = LogManager.GetLogger("Packets");

        public Socket Socket { get; private set; }

        public IPEndPoint ListenerEndpoint { get; private set; }

        private readonly uint listeningPort;

        private readonly byte[] buffer = new byte[ClientPacket.MaxPacketSize];

        private readonly IPAddress listeningHost;

        public ConnectionListener(IPAddress host, uint port)
        {
            log.LogDebug("ConnectionListener ctor, host {Host} port {Port}", host, port);

            listeningHost = host;
            listeningPort = port;
        }

        public void Start()
        {
            log.LogDebug("Starting ConnectionListener, host {ListeningHost} port {ListeningPort}", listeningHost, listeningPort);

            try
            {
                ListenerEndpoint = new IPEndPoint(listeningHost, (int)listeningPort);
                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                //{
                //    var sioUdpConnectionReset = -1744830452;
                //    var inValue = new byte[] { 0 };
                //    var outValue = new byte[] { 0 };
                //    Socket.IOControl(sioUdpConnectionReset, inValue, outValue);
                //}

                Socket.Bind(ListenerEndpoint);
                Listen();
            }
            catch (Exception exception)
            {
                log.LogCritical("Network Socket has thrown: {Message}", exception.Message);
            }
        }

        public void Shutdown()
        {
            log.LogDebug("Shutting down ConnectionListener, host {ListeningHost} port {ListeningPort}", listeningHost, listeningPort);

            if (Socket != null && Socket.IsBound)
                Socket.Close();
        }

        private void Listen()
        {
            try
            {
                EndPoint clientEndPoint = new IPEndPoint(listeningHost, 0);
                Socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref clientEndPoint, OnDataReceive, Socket);
            }
            catch (SocketException socketException)
            {
                log.LogDebug("ConnectionListener({ListeningHost}, {ListeningPort}).Listen() has thrown {SocketErrorCode}: {Message}", listeningHost, listeningPort, socketException.SocketErrorCode, socketException.Message);
                Listen();
            }
            catch (Exception exception)
            {
                log.LogCritical("ConnectionListener({ListeningHost}, {ListeningPort}).Listen() has thrown: {Message}", listeningHost, listeningPort, exception.Message);
            }
        }

        private void OnDataReceive(IAsyncResult result)
        {
            EndPoint clientEndPoint = null;

            try
            {
                clientEndPoint = new IPEndPoint(listeningHost, 0);
                int dataSize = Socket.EndReceiveFrom(result, ref clientEndPoint);

                IPEndPoint ipEndpoint = (IPEndPoint)clientEndPoint;

                // TO-DO: generate ban entries here based on packet rates of endPoint, IP Address, and IP Address Range

                if (packetLog.IsEnabled(LogLevel.Debug))
                {
                    byte[] data = new byte[dataSize];
                    Buffer.BlockCopy(buffer, 0, data, 0, dataSize);

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"Received Packet (Len: {data.Length}) [{ipEndpoint.Address}:{ipEndpoint.Port}=>{ListenerEndpoint.Address}:{ListenerEndpoint.Port}]");
                    sb.AppendLine(data.BuildPacketString());
                    packetLog.LogDebug("{Sb}", sb);
                }

                var packet = new ClientPacket();

                if (packet.Unpack(buffer, dataSize))
                    NetworkManager.ProcessPacket(this, packet, ipEndpoint);

                packet.ReleaseBuffer();
            }
            catch (SocketException socketException)
            {
                // If we get "Connection has been forcibly closed..." error, just eat the exception and continue on
                // This gets sent when the remote host terminates the connection (on UDP? interesting...)
                // TODO: There might be more, should keep an eye out. Logged message will help here.
                if (socketException.SocketErrorCode == SocketError.MessageSize ||
                    socketException.SocketErrorCode == SocketError.NetworkReset ||
                    socketException.SocketErrorCode == SocketError.ConnectionReset)
                {
                    log.LogDebug("ConnectionListener({ListeningHost}, {ListeningPort}).OnDataReceieve() has thrown {SocketErrorCode}: {Message} from client {ClientEndPoint}", listeningHost, listeningPort, socketException.SocketErrorCode, socketException.Message, (clientEndPoint != null ? clientEndPoint.ToString() : "Unknown"));
                }
                else
                {
                    log.LogCritical("ConnectionListener({ListeningHost}, {ListeningPort}).OnDataReceieve() has thrown {SocketErrorCode}: {Message} from client {ClientEndPoint}", listeningHost, listeningPort, socketException.SocketErrorCode, socketException.Message, (clientEndPoint != null ? clientEndPoint.ToString() : "Unknown"));
                    return;
                }
            }

            if (result.CompletedSynchronously)
                Task.Run(() => Listen());
            else
                Listen();
        }
    }
}
