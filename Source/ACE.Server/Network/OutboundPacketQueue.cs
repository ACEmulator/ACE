using ACE.Server.Network.Enum;
using log4net;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ACE.Server.Network
{
    public class OutboundPacketQueue
    {
        public class RawOutboundPacket
        {
            public EndPoint Them { get; set; }
            public Session Session { get; set; }
            public byte[] Packet { get; set; }
        }
        private Socket SendingSocket = null;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog packetLog = LogManager.GetLogger(System.Reflection.Assembly.GetEntryAssembly(), "Packets");
        private ConcurrentQueue<RawOutboundPacket> UnprocessedOutboundPackets = new ConcurrentQueue<RawOutboundPacket>();
        public int QueueLength => UnprocessedOutboundPackets.Count;

        public OutboundPacketQueue(Socket SendingSocket)
        {
            this.SendingSocket = SendingSocket;
        }
        public void Enqueue(RawOutboundPacket rip)
        {
            UnprocessedOutboundPackets.Enqueue(rip);
        }
        public void SendAll()
        {
            RawOutboundPacket rop = null;
            IPEndPoint listenerEndpoint = (IPEndPoint)SendingSocket.LocalEndPoint;
            while (UnprocessedOutboundPackets.TryDequeue(out rop))
            {
                if (packetLog.IsDebugEnabled)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(string.Format("[{5}] Sending Packet (Len: {0}) [{1}:{2}=>{3}:{4}]", rop.Packet.Length, listenerEndpoint.Address, listenerEndpoint.Port, rop.Session.EndPoint.Address, rop.Session.EndPoint.Port, rop.Session.Network.ClientId));
                    sb.AppendLine(rop.Packet.BuildPacketString());
                    packetLog.Debug(sb.ToString());
                }
                try
                {
                    SendingSocket.SendTo(rop.Packet, rop.Packet.Length, SocketFlags.None, rop.Session.EndPoint);
                }
                catch (SocketException ex)
                {
                    // Unhandled Exception: System.Net.Sockets.SocketException: A message sent on a datagram socket was larger than the internal message buffer or some other network limit, or the buffer used to receive a datagram into was smaller than the datagram itself
                    // at System.Net.Sockets.Socket.UpdateStatusAfterSocketErrorAndThrowException(SocketError error, String callerName)
                    // at System.Net.Sockets.Socket.SendTo(Byte[] buffer, Int32 offset, Int32 size, SocketFlags socketFlags, EndPoint remoteEP)

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(ex.ToString());
                    sb.AppendLine(string.Format("[{5}] Sending Packet (Len: {0}) [{1}:{2}=>{3}:{4}]", rop.Packet.Length, listenerEndpoint.Address, listenerEndpoint.Port, rop.Session.EndPoint.Address, rop.Session.EndPoint.Port, rop.Session.Network.ClientId));
                    log.Error(sb.ToString());

                    rop.Session.Terminate(SessionTerminationReason.SendToSocketException, null, null, ex.Message);
                }
            }
        }
    }
}
