using ACE.Server.Network.Enum;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

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
                try
                {
                    SendingSocket.SendTo(rop.Packet, rop.Packet.Length, SocketFlags.None, rop.Session.EndPoint);
                }
                catch (SocketException ex)
                {
                    rop.Session.Terminate(SessionTerminationReason.SendToSocketException, null, null, ex.Message);
                }
            }
        }
    }
}
