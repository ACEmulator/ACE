using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net.Sockets;
using System.Text;

using ACE.Common;
using ACE.Network.GameMessages;
using ACE.Network.Handlers;
using ACE.Network.Managers;

using log4net;

namespace ACE.Network
{
    public class NetworkSession
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog packetLog = LogManager.GetLogger("Packets");

        private const int minimumTimeBetweenBundles = 5; // 5ms
        private const int timeBetweenTimeSync = 20000; // 20s
        private const int timeBetweenAck = 2000; // 2s

        private readonly Session session;

        private readonly Object currentBundleLock = new Object();
        private NetworkBundle currentBundle = new NetworkBundle();

        private SortedDictionary<uint, ClientPacket> outOfOrderPackets = new SortedDictionary<uint, ClientPacket>();
        private SortedDictionary<uint, MessageBuffer> partialFragments = new SortedDictionary<uint, MessageBuffer>();

        private DateTime nextResync = DateTime.UtcNow;
        private DateTime nextAck = DateTime.UtcNow;
        private DateTime nextSend = DateTime.UtcNow;
        private bool sendAck = false;
        private bool sendResync = false;
        private uint lastReceivedSequence = 0;

        /// <summary>
        /// This is referenced from many threads:<para />
        /// ConnectionListener.OnDataReceieve()->Session.HandlePacket()->This.HandlePacket(packet), This path can come from any client or other thinkable object.<para />
        /// WorldManager.UpdateWorld()->Session.Update(lastTick)->This.Update(lastTick)
        /// </summary>
        private readonly ConcurrentDictionary<uint /*seq*/, ServerPacket> cachedPackets = new ConcurrentDictionary<uint /*seq*/, ServerPacket>();

        /// <summary>
        /// This is referenced by one thread:<para />
        /// WorldManager.UpdateWorld()->Session.Update(lastTick)->This.Update(lastTick) <para />
        /// Technically, it is referenced ONCE by EnqueueSend when the client first connects to the server, but there's no collision risk at that point.
        /// </summary>
        private readonly Queue<ServerPacket> packetQueue = new Queue<ServerPacket>();

        public readonly SessionConnectionData ConnectionData = new SessionConnectionData();

        public ushort ClientId { get; }
        public ushort ServerId { get; }

        public NetworkSession(Session session, ushort clientId, ushort serverId)
        {
            this.session = session;
            ClientId = clientId;
            ServerId = serverId;
        }

        /// <summary>
        /// It is assumed that this might be called from any thread.
        /// </summary>
        public void EnqueueSend(params GameMessage[] messages)
        {
            lock (currentBundleLock)
            {
                currentBundle.EncryptedChecksum = true;

                foreach (var message in messages)
                    currentBundle.Enqueue(message);
            }
        }

        /// <summary>
        /// At the moment, this is only called once, when a client initially connects to the server.
        /// </summary>
        public void EnqueueSend(params ServerPacket[] packets)
        {
            foreach (var packet in packets)
                packetQueue.Enqueue(packet);
        }

        /// <summary>
        /// It is assumed that this will only be called from a single thread. WorldManager.UpdateWorld()->Session.Update(lastTick)->This
        /// </summary>
        public void Update(double lastTick)
        {
            ConnectionData.ServerTime += lastTick;

            lock (currentBundleLock)
            {
                if (sendResync && !currentBundle.TimeSync && DateTime.UtcNow > nextResync)
                {
                    currentBundle.TimeSync = true;
                    currentBundle.EncryptedChecksum = true;
                    nextResync = DateTime.UtcNow.AddMilliseconds(timeBetweenTimeSync);
                }

                if (sendAck && !currentBundle.SendAck && DateTime.UtcNow > nextAck)
                {
                    currentBundle.SendAck = true;
                    nextAck = DateTime.UtcNow.AddMilliseconds(timeBetweenAck);
                }

                if (currentBundle.NeedsSending && DateTime.UtcNow > nextSend)
                {
                    // Flush the bundle here
                    var bundleToSend = currentBundle;
                    currentBundle = new NetworkBundle();
                    SendBundle(bundleToSend);
                    nextSend = DateTime.UtcNow.AddMilliseconds(minimumTimeBetweenBundles);
                }
            }

            FlushPackets();
        }

        /// <summary>
        /// This is called from ConnectionListener.OnDataReceieve()->Session.HandlePacket()->This
        /// </summary>
        public void HandlePacket(ClientPacket packet)
        {
            if (packet.Header.Sequence <= lastReceivedSequence && packet.Header.Sequence != 0)
            {
                log.DebugFormat("Packet {0} received again for account {1}", packet.Header.Sequence, session.Account);
                return;
            }

            if (packet.Header.Sequence > lastReceivedSequence + 1)
            {
                log.DebugFormat("Packet {0} received out of order for account {1}", packet.Header.Sequence, session.Account);
                lock (outOfOrderPackets)
                {
                    if (!outOfOrderPackets.ContainsKey(packet.Header.Sequence))
                        outOfOrderPackets.Add(packet.Header.Sequence, packet);
                }
                return;
            }

            ProcessPacket(packet);
        }

        private void ProcessPacket(ClientPacket packet)
        {
            if (packet.Header.HasFlag(PacketHeaderFlags.EchoRequest))
                FlagEcho(packet.HeaderOptional.ClientTime);

            if (packet.Header.HasFlag(PacketHeaderFlags.AckSequence))
                AcknowledgeSequence(packet.HeaderOptional.Sequence);

            if (packet.Header.HasFlag(PacketHeaderFlags.TimeSynch))
            {
                // Do something with this...
                // Based on network traces these are not 1:1.  Server seems to send them every 20 seconds per port.
                // Client seems to send them alternatingly every 2 or 4 seconds per port.
                // We will send this at a 20 second time interval.  I don't know what to do with these when we receive them at this point.
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.RequestRetransmit))
            {
                foreach (uint sequence in packet.HeaderOptional.RetransmitData)
                    Retransmit(sequence);
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest))
            {
                AuthenticationHandler.HandleLoginRequest(packet, session);
                return;
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.ConnectResponse))
            {
                sendResync = true;
                AuthenticationHandler.HandleConnectResponse(packet, session);
                return;
            }

            foreach (ClientPacketFragment fragment in packet.Fragments)
                ProcessFragment(fragment);

            if (packet.Header.Sequence != 0)
                lastReceivedSequence = packet.Header.Sequence;
        }

        private void ProcessFragment(ClientPacketFragment fragment)
        {
            if (fragment.Header.Count != 1)
            {
                lock (partialFragments)
                {
                    MessageBuffer buffer = null;
                    if (partialFragments.TryGetValue(fragment.Header.Sequence, out buffer))
                    {
                        buffer.AddFragment(fragment);
                        if (buffer.Complete)
                        {
                            ClientMessage message = buffer.GetMessage();
                            InboundMessageManager.HandleClientMessage(message, session);
                            partialFragments.Remove(fragment.Header.Sequence);
                        }
                    }
                    else
                    {
                        partialFragments.Add(fragment.Header.Sequence, new MessageBuffer(fragment.Header.Count));
                    }
                }
            }
            else
            {
                ClientMessage message = new ClientMessage(fragment.Data);
                InboundMessageManager.HandleClientMessage(message, session);
            }
        }

        private void FlagEcho(float clientTime)
        {
            lock (currentBundle)
            {
                // Debug.Assert(clientTime == -1f, "Multiple EchoRequests before Flush, potential issue with network logic!");
                currentBundle.ClientTime = clientTime;
                currentBundle.EncryptedChecksum = true;
            }
        }

        private void AcknowledgeSequence(uint sequence)
        {
            // TODO Sending Acks seems to cause some issues.  Needs further research.
            // if (!sendAck)
            //    sendAck = true;

            var removalList = cachedPackets.Where(x => x.Key < sequence);

            foreach (var item in removalList)
            {
                ServerPacket removedPacket;
                cachedPackets.TryRemove(item.Key, out removedPacket);
            }
        }

        private void Retransmit(uint sequence)
        {
            ServerPacket cachedPacket;

            if (cachedPackets.TryGetValue(sequence, out cachedPacket))
            {
                Console.WriteLine("Retransmit " + sequence);

                if (!cachedPacket.Header.HasFlag(PacketHeaderFlags.Retransmission))
                    cachedPacket.Header.Flags |= PacketHeaderFlags.Retransmission;

                SendPacketRaw(cachedPacket);
            }
        }

        private void FlushPackets()
        {
            while (packetQueue.Count > 0)
            {
                ServerPacket packet = packetQueue.Dequeue();

                if (packet.Header.HasFlag(PacketHeaderFlags.EncryptedChecksum) && ConnectionData.PacketSequence < 2)
                    ConnectionData.PacketSequence = 2;

                packet.Header.Sequence = ConnectionData.PacketSequence++;
                packet.Header.Id = ServerId;
                packet.Header.Table = 0x14;
                packet.Header.Time = (ushort)ConnectionData.ServerTime;

                if (packet.Header.Sequence >= 2u)
                    cachedPackets.TryAdd(packet.Header.Sequence, packet);

                SendPacket(packet);
            }
        }

        private void SendPacket(ServerPacket packet)
        {
            if (packet.Header.HasFlag(PacketHeaderFlags.EncryptedChecksum))
            {
                uint issacXor = session.GetIssacValue(PacketDirection.Server);
                packet.IssacXor = issacXor;
            }

            SendPacketRaw(packet);
        }

        private void SendPacketRaw(ServerPacket packet)
        {
            Socket socket = SocketManager.GetSocket();
            byte[] payload = packet.GetPayload();

            if (packetLog.IsDebugEnabled)
            {
                System.Net.IPEndPoint listenerEndpoint = (System.Net.IPEndPoint)socket.LocalEndPoint;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(String.Format("Sending Packet (Len: {0}) [{1}:{2}=>{3}:{4}]", payload.Length, listenerEndpoint.Address, listenerEndpoint.Port, session.EndPoint.Address, session.EndPoint.Port));
                sb.AppendLine(payload.BuildPacketString());
                packetLog.Debug(sb.ToString());
            }
            socket.SendTo(payload, session.EndPoint);
        }

        /// <summary>
        /// This function handles turning a bundle of messages (representing all messages accrued in a timeslice),
        /// into 1 or more packets, combining multiple messages into one packet or spliting large message across
        /// several packets as needed.
        ///
        /// 1 Create Packet
        /// 2 If first packet for this bundle, fill any optional headers
        /// 3 Append messages that will fit
        /// 4 If we have more messages, create additional packets.
        /// 5 If any packet is greater than the max packet size, split it across two fragments
        /// </summary>
        /// <param name="bundle"></param>
        private void SendBundle(NetworkBundle bundle)
        {
            bool firstPacket = true;

            MessageFragment carryOverMessage = null;

            while (firstPacket || carryOverMessage != null || bundle.Messages.Count > 0)
            {
                ServerPacket packet = new ServerPacket();
                PacketHeader packetHeader = packet.Header;

                if (bundle.EncryptedChecksum)
                    packetHeader.Flags |= PacketHeaderFlags.EncryptedChecksum;

                uint availableSpace = Packet.MaxPacketDataSize;

                if (firstPacket)
                {
                    firstPacket = false;

                    if (bundle.SendAck) // 0x4000
                    {
                        packetHeader.Flags |= PacketHeaderFlags.AckSequence;
                        packet.BodyWriter.Write(lastReceivedSequence);
                    }

                    if (bundle.TimeSync) // 0x1000000
                    {
                        packetHeader.Flags |= PacketHeaderFlags.TimeSynch;
                        packet.BodyWriter.Write(ConnectionData.ServerTime);
                    }

                    if (bundle.ClientTime != -1f) // 0x4000000
                    {
                        packetHeader.Flags |= PacketHeaderFlags.EchoResponse;
                        packet.BodyWriter.Write(bundle.ClientTime);
                        packet.BodyWriter.Write((float)ConnectionData.ServerTime - bundle.ClientTime);
                    }

                    availableSpace -= (uint)packet.Data.Length;
                }

                if (carryOverMessage != null || bundle.Messages.Count > 0)
                {
                    packetHeader.Flags |= PacketHeaderFlags.BlobFragments;
                    int fragmentCount = 0;
                    while (carryOverMessage != null || bundle.Messages.Count > 0)
                    {
                        MessageFragment currentMessageFragment;

                        if (carryOverMessage != null) // If we have a carryOverMessage, use that
                        {
                            currentMessageFragment = carryOverMessage;
                            carryOverMessage = null;
                        }
                        else // If we don't have a carryOverMessage, go ahead and dequeue next message from the bundle
                        {
                            currentMessageFragment = new MessageFragment(bundle.Messages.Dequeue());
                        }

                        var currentGameMessage = currentMessageFragment.message;

                        availableSpace -= PacketFragmentHeader.HeaderSize; // Account for fragment header

                        // Compute amount of data to send based on the total length and current position
                        uint dataToSend = (uint)currentGameMessage.Data.Length - currentMessageFragment.position;

                        if (dataToSend > availableSpace) // Message is too large to fit in packet
                        {
                            carryOverMessage = currentMessageFragment;
                            if (fragmentCount == 0) // If this is first message in packet, this is just a really large message, so proceed with splitting it
                                dataToSend = availableSpace;
                            else // Otherwise there are other messages already, so we'll break and come back and see if the message will fit
                                break;
                        }

                        if (currentMessageFragment.count == 0) // Compute number of fragments if we have not already
                        {
                            uint remainingData = (uint)currentGameMessage.Data.Length - dataToSend;
                            currentMessageFragment.count = (ushort)(Math.Ceiling((double)remainingData / PacketFragment.MaxFragmentDataSize) + 1);
                        }

                        // Set sequence, if new, pull next sequence from ConnectionData, if it is a carryOver, reuse that sequence
                        currentMessageFragment.sequence = currentMessageFragment.sequence == 0 ? ConnectionData.FragmentSequence++ : currentMessageFragment.sequence;


                        // Read data starting at current position reading dataToSend bytes
                        currentGameMessage.Data.Seek(currentMessageFragment.position, SeekOrigin.Begin);
                        byte[] data = new byte[dataToSend];
                        currentGameMessage.Data.Read(data, 0, (int)dataToSend);

                        // Build ServerPacketFragment structure
                        ServerPacketFragment fragment = new ServerPacketFragment(data);
                        fragment.Header.Sequence = currentMessageFragment.sequence;
                        fragment.Header.Id = 0x80000000;
                        fragment.Header.Count = currentMessageFragment.count;
                        fragment.Header.Index = currentMessageFragment.index;
                        fragment.Header.Group = (ushort)currentMessageFragment.message.Group;

                        // Increment position and index
                        currentMessageFragment.position = currentMessageFragment.position + dataToSend;
                        currentMessageFragment.index++;

                        // Add fragment to packet
                        packet.AddFragment(fragment);
                        fragmentCount++;

                        // Deduct consumed space
                        availableSpace -= dataToSend;

                        // Smallest message I am aware of requires HeaderSize + 4 bytes, so if we have less space then that, go ahead and break
                        if (availableSpace <= PacketFragmentHeader.HeaderSize + 4)
                            break;
                    }
                }

                packetQueue.Enqueue(packet);
            }
        }

        private class MessageBuffer
        {
            private List<ClientPacketFragment> fragments = new List<ClientPacketFragment>();

            public uint TotalFragments { get; }

            public bool Complete { get { return fragments.Count == TotalFragments; } }

            public MessageBuffer(uint totalFragments)
            {
                TotalFragments = totalFragments;
            }

            public void AddFragment(ClientPacketFragment fragment)
            {
                lock (fragments)
                {
                    if (!Complete && !fragments.Any(x => x.Header.Id == fragment.Header.Id))
                        fragments.Add(fragment);
                }
            }

            public ClientMessage GetMessage()
            {
                fragments.Sort(delegate(ClientPacketFragment x, ClientPacketFragment y) { return (int)x.Header.Id - (int)y.Header.Id; });
                MemoryStream stream = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(stream);
                foreach (ClientPacketFragment fragment in fragments)
                {
                    writer.Write(fragment.Data);
                }
                return new ClientMessage(stream);
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
