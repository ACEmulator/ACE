using ACE.Common.Cryptography;
using ACE.Server.Managers;
using log4net;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ACE.Server.Network
{
    public class InboundPacketQueue
    {
        public class RawInboundPacket
        {
            public IPEndPoint Us { get; set; }
            public IPEndPoint Them { get; set; }
            public byte[] Packet { get; set; }
        }
        private static readonly ILog packetLog = LogManager.GetLogger(System.Reflection.Assembly.GetEntryAssembly(), "Packets");
        private BlockingCollection<RawInboundPacket> UnprocessedInboundPackets = new BlockingCollection<RawInboundPacket>();
        private Task _readerTask;
        private Thread InboundPacketQueueProcessor = null;
        public int QueueLength => UnprocessedInboundPackets.Count;

        public InboundPacketQueue()
        {
            InboundPacketQueueProcessor = new Thread(new ThreadStart(Consumer))
            {
                Name = "InboundPacketQueueManager"
            };
            InboundPacketQueueProcessor.Start();
        }
        public void Shutdown()
        {
            UnprocessedInboundPackets.CompleteAdding();
            Task.WaitAll(_readerTask);
        }
        public void Consumer()
        {
            _readerTask = Task.Factory.StartNew(() =>
            {
                foreach (RawInboundPacket rip in UnprocessedInboundPackets.GetConsumingEnumerable())
                {
                    // TO-DO: generate ban entries here based on packet rates of endPoint, IP Address, and IP Address Range
                    if (packetLog.IsDebugEnabled)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"Received Packet (Len: {rip.Packet.Length}) [{rip.Them.Address}:{rip.Them.Port}=>{rip.Us.Address}:{rip.Us.Port}]");
                        sb.AppendLine(rip.Packet.BuildPacketString());
                        packetLog.Debug(sb.ToString());
                    }
                    ClientPacket packet = new ClientPacket(rip.Packet);
                    if (packet.SuccessfullyParsed)
                    {
                        CryptoSystem crypto = null;
                        if (packet.Header.HasFlag(PacketHeaderFlags.EncryptedChecksum))
                        {
                            Session session = WorldManager.Sessions.Values.FirstOrDefault(k => k.EndPoint.Equals(rip.Them));
                            if (session != null)
                            {
                                crypto = session.Network.ConnectionData.CryptoClient;
                            }
                        }
                        else if (packet.Header.HasFlag(PacketHeaderFlags.RequestRetransmit))
                        {
                            // discard retransmission request with cleartext CRC
                            // client sends one encrypted version and one non encrypted version of each retransmission request
                            // honoring both causes client to drop because it's only expecting one of the two retransmission requests to be honored
                            // and it's more secure to only accept the trusted version
                            continue;
                        }
                        if (!packet.ValidateCRC(crypto, true))
                        {
                            // discard corrupt or forged packet
                            continue;
                        }
                        WorldManager.ProcessPacket(packet, rip.Them, rip.Us);
                    }
                }
            });
        }

        public void AddItem(RawInboundPacket rip)
        {
            UnprocessedInboundPackets.Add(rip);
        }
    }
}
