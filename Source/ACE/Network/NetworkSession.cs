using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using ACE.Network.GameMessages;
using ACE.Network.Managers;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using ACE.Network.Handlers;

namespace ACE.Network
{
    public class NetworkSession
    {
        private const int minimumTimeBetweenBundles = 5; // 5ms
        private const int timeBetweenTimeSync = 20000; // 20s
        private const int timeBetweenAck = 2000; // 2s

        private ConnectionType connectionType;
        private Session session;
        private NetworkBundle currentBundle;
        private DateTime nextResync;
        private DateTime nextAck;
        private DateTime nextSend;
        private bool sendAck = false;
        private bool sendResync = false;

        private uint lastReceivedSequence = 0;
        private ConcurrentDictionary<uint /*seq*/, ServerPacket2> CachedPackets { get; } = new ConcurrentDictionary<uint, ServerPacket2>();
        private ConcurrentQueue<ServerPacket2> PacketQueue { get; } = new ConcurrentQueue<ServerPacket2>();

        public NetworkSession(Session session, ConnectionType connType)
        {
            this.session = session;
            this.connectionType = connType;
            currentBundle = new NetworkBundle();
            nextSend = DateTime.Now;
            nextResync = DateTime.Now;
            nextAck = DateTime.Now;
        }

        public void Enqueue(GameMessage message)
        {
            currentBundle.encryptedChecksum = true;
            currentBundle.Messages.Enqueue(message);
        }

        public void Enqueue(IEnumerable<GameMessage> messages)
        {
            currentBundle.encryptedChecksum = true;
            foreach(var message in messages)
                currentBundle.Messages.Enqueue(message);
        }

        public void Enqueue(ServerPacket2 packet)
        {
            PacketQueue.Enqueue(packet);
        }

        public void SetTimers()
        {
            nextResync = DateTime.Now;
            nextAck = DateTime.Now.AddMilliseconds(timeBetweenAck);
        }

        public void StartResync()
        {
            sendResync = true;
        }

        public void Update()
        {
            if (sendResync && !currentBundle.TimeSync && DateTime.Now > nextResync)
            {
                currentBundle.TimeSync = true;
                currentBundle.encryptedChecksum = true;
                nextResync = DateTime.Now.AddMilliseconds(timeBetweenTimeSync);
            }
            if (sendAck && !currentBundle.SendAck && DateTime.Now > nextAck)
            {
                currentBundle.SendAck = true;
                nextAck = DateTime.Now.AddMilliseconds(timeBetweenAck);
            }
            if (currentBundle.NeedsSending && DateTime.Now > nextSend)
            {
                var bundleToSend = currentBundle;
                currentBundle = new NetworkBundle();
                SendBundle(bundleToSend);
                nextSend = DateTime.Now.AddMilliseconds(minimumTimeBetweenBundles);
            }
            FlushPackets();
        }

        public void HandlePacket(ClientPacket packet)
        {
            lastReceivedSequence = packet.Header.Sequence;

            if (packet.Header.HasFlag(PacketHeaderFlags.EchoRequest))
            {
                FlagEcho(packet.HeaderOptional.ClientTime);
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.AckSequence))
            {
                AcknowledgeSequence(packet.HeaderOptional.Sequence);
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.TimeSynch))
            {
                //Do something with this...
                //Based on network traces these are not 1:1.  Server seems to send them every 20 seconds per port.
                //Client seems to send them alternatingly every 2 or 4 seconds per port.
                //We will send this at a 20 second time interval.  I don't know what to do with these when we receive them at this point.
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.RequestRetransmit))
            {
                foreach (uint sequence in packet.HeaderOptional.RetransmitData)
                {
                    Retransmit(sequence);
                }
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest))
            {
                AuthenticationHandler.HandleLoginRequest(packet, session);
                return;
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.WorldLoginRequest))
            {
                AuthenticationHandler.HandleWorldLoginRequest(packet, session);
                return;
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.ConnectResponse))
            {
                if (connectionType == ConnectionType.Login)
                {
                    AuthenticationHandler.HandleConnectResponse(packet, session);
                    return;
                }

                AuthenticationHandler.HandleWorldConnectResponse(packet, session);
                return;
            }

            foreach (ClientPacketFragment fragment in packet.Fragments)
            {
                PacketManager.HandleClientFragment(fragment, session);
            }
        }

        private void FlagEcho(float clientTime)
        {
            //Debug.Assert(clientTime == -1f, "Multiple EchoRequests before Flush, potential issue with network logic!");
            currentBundle.ClientTime = clientTime;
            currentBundle.encryptedChecksum = true;
        }

        private void AcknowledgeSequence(uint sequence)
        {
            //TODO Sending Acks seems to cause some issues.  Needs further research.
            //if (!sendAck)
            //    sendAck = true;
            var removalList = CachedPackets.Where(x => x.Key < sequence);
            foreach (var item in removalList)
            {
                ServerPacket2 removedPacket;
                CachedPackets.TryRemove(item.Key, out removedPacket);
            }
        }

        private void Retransmit(uint sequence)
        {
            ServerPacket2 cachedPacket;
            if (CachedPackets.TryGetValue(sequence, out cachedPacket))
            {
                Console.WriteLine("Retransmit " + sequence);
                if (!cachedPacket.Header.HasFlag(PacketHeaderFlags.Retransmission))
                {
                    cachedPacket.Header.Flags |= PacketHeaderFlags.Retransmission;
                }
                SendPacketRaw(cachedPacket);
            }
        }

        private void FlushPackets()
        {
            var connectionData = (connectionType == ConnectionType.Login ? session.LoginConnection : session.WorldConnection);
            while (PacketQueue.Count > 0)
            {
                ServerPacket2 packet;
                if (PacketQueue.TryDequeue(out packet))
                {
                    if (packet.Header.HasFlag(PacketHeaderFlags.EncryptedChecksum) && connectionData.PacketSequence < 2)
                        connectionData.PacketSequence = 2;

                    packet.Header.Sequence = connectionData.PacketSequence++;
                    packet.Header.Id = (ushort)(connectionType == ConnectionType.Login ? 0x0B : 0x18);
                    packet.Header.Table = 0x14;
                    packet.Header.Time = (ushort)connectionData.ServerTime;

                    if (packet.Header.Sequence >= 2u)
                        CachedPackets.TryAdd(packet.Header.Sequence, packet);

                    SendPacket(packet);
                }
            }
        }

        private void SendPacket(ServerPacket2 packet)
        {
            if(packet.Header.HasFlag(PacketHeaderFlags.EncryptedChecksum))
            {
                uint issacXor = session.GetIssacValue(PacketDirection.Server, connectionType);
                packet.IssacXor = issacXor;
            }
            SendPacketRaw(packet);
        }

        private void SendPacketRaw(ServerPacket2 packet)
        {
            byte[] payload = packet.GetPayload();
#if NETWORKDEBUG
            Console.WriteLine("Send Packet");
            payload.OutputDataToConsole();
            Console.WriteLine();
#endif
            NetworkManager.GetSocket(this.connectionType).SendTo(payload, session.EndPoint);
        }

        /// <summary>
        /// This function handles turning a bundle of messages (representing all messages accrued in a timeslice),
        /// into 1 or more packets, combining multiple messages into one packet or spliting large message across
        /// several packets as needed.
        /// 
        /// 1 Create Packet
        /// 2 If first packet for this bundle, fill any optional headers
        /// 3 Append fragments that will fit
        /// 3.1 Check each fragment to see if it is a partial fit
        /// 4 If we have a partial message remaining or more messages, create additional packets.
        /// </summary>
        /// <param name="bundle"></param>
        private void SendBundle(NetworkBundle bundle)
        {
            
            var connectionData = (connectionType == ConnectionType.Login ? session.LoginConnection : session.WorldConnection);
            bool firstPacket = true;

            MessageFragment carryOverMessage = null;

            while (firstPacket || carryOverMessage != null || bundle.Messages.Count > 0)
            {
                ServerPacket2 packet = new ServerPacket2();
                PacketHeader packetHeader = packet.Header;
                if (bundle.encryptedChecksum)
                    packetHeader.Flags |= PacketHeaderFlags.EncryptedChecksum;

                uint availableSpace = Packet.MaxPacketDataSize;

                if (firstPacket)
                {
                    firstPacket = false;
                    using (var bodyStream = new MemoryStream())
                    {
                        using (var bodyWriter = new BinaryWriter(bodyStream))
                        {

                            if (bundle.SendAck) //0x4000
                            {
                                packetHeader.Flags |= PacketHeaderFlags.AckSequence;
                                bodyWriter.Write(lastReceivedSequence);
                            }
                            if (bundle.TimeSync) //0x1000000
                            {
                                packetHeader.Flags |= PacketHeaderFlags.TimeSynch;
                                bodyWriter.Write(connectionData.ServerTime);
                            }
                            if (bundle.ClientTime != -1f) //0x4000000
                            {
                                packetHeader.Flags |= PacketHeaderFlags.EchoResponse;
                                bodyWriter.Write(bundle.ClientTime);
                                bodyWriter.Write((float)connectionData.ServerTime - bundle.ClientTime);
                            }
                            bodyWriter.Flush();
                        }
                        byte[] body = bodyStream.ToArray();
                        packet.SetBody(body);
                        availableSpace -= (uint)body.Length;
                    }
                }

                if (carryOverMessage != null || bundle.Messages.Count > 0)
                {
                    packetHeader.Flags |= PacketHeaderFlags.BlobFragments;

                    while (carryOverMessage != null || bundle.Messages.Count > 0)
                    {
                        if (availableSpace <= PacketFragmentHeader.HeaderSize + 4)
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
                            currentMessageFragment = new MessageFragment(bundle.Messages.Dequeue());
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
                        fragmentHeader.Count = currentMessageFragment.count;
                        fragmentHeader.Index = currentMessageFragment.index;
                        fragmentHeader.Group = currentMessageFragment.message.Group;

                        currentGameMessage.Data.Seek(currentMessageFragment.position, SeekOrigin.Begin);
                        fragment.Body = new byte[dataToSend];
                        currentGameMessage.Data.Read(fragment.Body, 0, (int)dataToSend);

                        currentMessageFragment.position = currentMessageFragment.position + dataToSend;

                        availableSpace -= dataToSend;

                        packet.AddFragment(fragment);
                        if (currentMessageFragment.count > 1 && carryOverMessage == null)
                            break;
                    }
                }

                PacketQueue.Enqueue(packet);
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
    }
}
