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
using ACE.Managers;

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

        private ConcurrentDictionary<uint, ClientPacket> outOfOrderPackets = new ConcurrentDictionary<uint, ClientPacket>();
        private ConcurrentDictionary<uint, MessageBuffer> partialFragments = new ConcurrentDictionary<uint, MessageBuffer>();
        private ConcurrentDictionary<uint, ClientMessage> outOfOrderFragments = new ConcurrentDictionary<uint, ClientMessage>();

        private DateTime nextSend = DateTime.UtcNow;

        // Resync will be started after ConnectResponse, and should immediately be sent then, so no delay here.
        // Fun fact: even though we send the server time in the ConnectRequest, client doesn't seem to use it?  Therefore we must TimeSync early so client doesn't see a skew when we send it later.
        private bool sendResync = false;
        private DateTime nextResync = DateTime.UtcNow;

        // Ack should be sent after a 2 second delay, so start enabled with the delay.
        // Sending this too early seems to cause issues with clients disconnecting.
        private bool sendAck = true;
        private DateTime nextAck = DateTime.UtcNow.AddMilliseconds(timeBetweenAck);
        
        private uint lastReceivedPacketSequence = 1;
        private uint lastReceivedFragmentSequence = 0;

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

        /// <summary>
        /// Stores the tick value for the when an active session will timeout. If this value is in the past, the session is dead/inactive.
        /// </summary>
        public long TimeoutTick { get; set; }

        public ushort ClientId { get; }
        public ushort ServerId { get; }

        public NetworkSession(Session session, ushort clientId, ushort serverId)
        {
            this.session = session;
            ClientId = clientId;
            ServerId = serverId;
            // New network auth session timeouts will always be low.
            TimeoutTick = DateTime.UtcNow.AddSeconds(AuthenticationHandler.DefaultAuthTimeout).Ticks;
        }

        /// <summary>
        /// Enequeues a GameMessage for sending to this client.
        /// This may be called from many threads.
        /// </summary>
        /// <param name="messages">One or more GameMessages to send</param>
        public void EnqueueSend(params GameMessage[] messages)
        {
            lock (currentBundleLock)
            {
                currentBundle.EncryptedChecksum = true;

                foreach (var message in messages)
                {
                    log.DebugFormat("[{0}] Enqueuing Message {1}", session.ClientAccountString, message.Opcode);
                    currentBundle.Enqueue(message);
                }
            }
        }

        /// <summary>
        /// Enqueues a ServerPacket for sending to this client.
        /// Currently this is only used publicly once during login.  If that changes it's thread safety should be re
        /// </summary>
        /// <param name="packets"></param>
        public void EnqueueSend(params ServerPacket[] packets)
        {
            foreach (var packet in packets)
            {
                log.DebugFormat("[{0}] Enqueuing Packet {1}", session.ClientAccountString, packet.GetHashCode());
                packetQueue.Enqueue(packet);
            }
        }

        // It is assumed that this will only be called from a single thread.WorldManager.UpdateWorld()->Session.Update(lastTick)->This
        /// <summary>
        /// Checks if we should send the current bundle and then flushes all pending packets.
        /// </summary>
        /// <param name="lastTick">Amount of time that has passed for the last cycle.</param>
        public void Update(double lastTick)
        {
            ConnectionData.ServerTime += lastTick;
            NetworkBundle bundleToSend = null;
            lock (currentBundleLock)
            {
                if (sendResync && !currentBundle.TimeSync && DateTime.UtcNow > nextResync)
                {
                    log.DebugFormat("[{0}] Setting to send TimeSync packet", session.ClientAccountString);
                    currentBundle.TimeSync = true;
                    currentBundle.EncryptedChecksum = true;
                    nextResync = DateTime.UtcNow.AddMilliseconds(timeBetweenTimeSync);
                }

                if (sendAck && !currentBundle.SendAck && DateTime.UtcNow > nextAck)
                {
                    log.DebugFormat("[{0}] Setting to send ACK packet", session.ClientAccountString);
                    currentBundle.SendAck = true;
                    nextAck = DateTime.UtcNow.AddMilliseconds(timeBetweenAck);
                }

                if (currentBundle.NeedsSending && DateTime.UtcNow > nextSend)
                {
                    log.DebugFormat("[{0}] Swaping bundle", session.ClientAccountString);
                    // Swap out bundle so we can process it
                    bundleToSend = currentBundle;
                    currentBundle = new NetworkBundle();
                }
            }

            // Send our bundle if we have one
            // We should be able to execute this outside the lock as Sending is single threaded
            // and all future writes from other threads will go to the new bundle
            if (bundleToSend != null)
            {
                SendBundle(bundleToSend);
                nextSend = DateTime.UtcNow.AddMilliseconds(minimumTimeBetweenBundles);
            }

            FlushPackets();
        }

        // This is called from ConnectionListener.OnDataReceieve()->Session.ProcessPacket()->This
        /// <summary>
        /// Processes and incoming packet from a client.
        /// </summary>
        /// <param name="packet">The ClientPacket to process.</param>
        public void ProcessPacket(ClientPacket packet)
        {
            log.DebugFormat("[{0}] Processing packet {1}", session.ClientAccountString, packet.Header.Sequence);
            // Check if this packet's sequence is a sequence which we have already processed.
            // There are some exceptions:
            // Sequence 0 as we have several Seq 0 packets during connect.  This also cathes a case where it seems CICMDCommand arrives at any point with 0 sequence value too.
            // If the only header on the packet is AckSequence. It seems AckSequence can come in with the same sequence value sometimes.
            if (packet.Header.Sequence <= lastReceivedPacketSequence && packet.Header.Sequence != 0 &&
                !(packet.Header.Flags == PacketHeaderFlags.AckSequence && packet.Header.Sequence == lastReceivedPacketSequence))
            {
                log.WarnFormat("[{0}] Packet {1} received again", session.ClientAccountString, packet.Header.Sequence);
                return;
            }

            // Check if this packet's sequence is greater then the next one we should be getting.
            // If true we must store it to replay once we have caught up.
            if (packet.Header.Sequence > lastReceivedPacketSequence + 1)
            {
                log.WarnFormat("[{0}] Packet {1} received out of order", session.ClientAccountString, packet.Header.Sequence);
                if (!outOfOrderPackets.ContainsKey(packet.Header.Sequence))
                    outOfOrderPackets.TryAdd(packet.Header.Sequence, packet);
                return;
            }

            // If we reach here, this is a packet we should proceed with processing.
            HandlePacket(packet);

            // Finally check if we have any out of order packets or fragments we need to process;
            CheckOutOfOrderPackets();
            CheckOutOfOrderFragments();
        }

        /// <summary>
        /// Handles a packet, reading the flags and processing all fragments.
        /// </summary>
        /// <param name="packet">ClientPacket to handle</param>
        private void HandlePacket(ClientPacket packet)
        {
            log.DebugFormat("[{0}] Handling packet {1}", session.ClientAccountString, packet.Header.Sequence);

            uint issacXor = packet.Header.HasFlag(PacketHeaderFlags.EncryptedChecksum) ? ConnectionData.IssacClient.GetOffset() : 0;
            if (!packet.VerifyChecksum(issacXor))
            {
                log.WarnFormat("[{0}] Packet {1} has invalid checksum", session.ClientAccountString, packet.Header.Sequence);
            }

            // depending on the current session state:
            // Set the next timeout tick value, to compare against in the WorldManager
            // Sessions that have gone past the AuthLoginRequest step will stay active for a longer period of time (exposed via configuration) 
            // Sessions that in the AuthLoginRequest will have a short timeout, as set in the AuthenticationHandler.DefaultAuthTimeout.
            // Example: Applications that check uptime will stay in the AuthLoginRequest state.
            session.Network.TimeoutTick = (session.State == Enum.SessionState.AuthLoginRequest) ?
                DateTime.UtcNow.AddSeconds(WorldManager.DefaultSessionTimeout).Ticks : 
                DateTime.UtcNow.AddSeconds(AuthenticationHandler.DefaultAuthTimeout).Ticks;

            // If we have an EchoRequest flag, we should flag to respond with an echo response on next send.
            if (packet.Header.HasFlag(PacketHeaderFlags.EchoRequest))
                FlagEcho(packet.HeaderOptional.EchoRequestClientTime);

            // If we have an AcknowledgeSequence flag, we can clear our cached packet buffer up to that sequence.
            if (packet.Header.HasFlag(PacketHeaderFlags.AckSequence))
                AcknowledgeSequence(packet.HeaderOptional.Sequence);

            if (packet.Header.HasFlag(PacketHeaderFlags.TimeSynch))
            {
                log.DebugFormat("[{0}] Incoming TimeSync TS: {1}", session.ClientAccountString, packet.HeaderOptional.TimeSynch);
                // Do something with this...
                // Based on network traces these are not 1:1.  Server seems to send them every 20 seconds per port.
                // Client seems to send them alternatingly every 2 or 4 seconds per port.
                // We will send this at a 20 second time interval.  I don't know what to do with these when we receive them at this point.
            }

            // If the client is requesting a retransmission, pull those packets from the queue and resend them.
            if (packet.Header.HasFlag(PacketHeaderFlags.RequestRetransmit))
            {
                foreach (uint sequence in packet.HeaderOptional.RetransmitData)
                    Retransmit(sequence);
            }

            // This should be set on the first packet to the server indicating the client is logging in.
            // This is the start of a three-way handshake between the client and server (LoginRequest, ConnectRequest, ConnectResponse)
            // Note this would be sent to each server a client would connect too (Login and each world).
            // In our current implimenation we handle all roles in this one server.
            if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest))
            {
                log.Info($"[{session.ClientAccountString}] LoginRequest");
                AuthenticationHandler.HandleLoginRequest(packet, session);
                return;
            }

            // This should be set on the second packet to the server from the client.
            // This completes the three-way handshake.
            if (packet.Header.HasFlag(PacketHeaderFlags.ConnectResponse))
            {
                sendResync = true;
                AuthenticationHandler.HandleConnectResponse(packet, session);
                return;
            }

            // Process all fragments out of the packet
            foreach (ClientPacketFragment fragment in packet.Fragments)
                ProcessFragment(fragment);

            // Update the last received sequence.
            if (packet.Header.Sequence != 0)
                lastReceivedPacketSequence = packet.Header.Sequence;
        }

        /// <summary>
        /// Processes a fragment, combining split fragments as needed, then handling them
        /// </summary>
        /// <param name="fragment">ClientPacketFragment to process</param>
        private void ProcessFragment(ClientPacketFragment fragment)
        {
            log.DebugFormat("[{0}] Processing fragment {1}", session.ClientAccountString, fragment.Header.Sequence);
            ClientMessage message = null;
            // Check if this fragment is split
            if (fragment.Header.Count != 1)
            {
                // Packet is split
                log.DebugFormat("[{0}] Fragment {1} is split, this index {2} of {3} fragments", session.ClientAccountString, fragment.Header.Sequence, fragment.Header.Index, fragment.Header.Count);

                MessageBuffer buffer = null;
                if (partialFragments.TryGetValue(fragment.Header.Sequence, out buffer))
                {
                    // Existing buffer, add this to it and check if we are finally complete.
                    buffer.AddFragment(fragment);
                    log.DebugFormat("[{0}] Added fragment {1} to existing buffer. Buffer at {2} of {3}", session.ClientAccountString, fragment.Header.Sequence, buffer.Count, buffer.TotalFragments);
                    if (buffer.Complete)
                    {
                        // The buffer is complete, so we can go ahead and handle
                        log.DebugFormat("[{0}] Buffer {1} is complete", session.ClientAccountString, buffer.Sequence);
                        message = buffer.GetMessage();
                        MessageBuffer removed = null;
                        partialFragments.TryRemove(fragment.Header.Sequence, out removed);
                    }
                }
                else
                {
                    // No existing buffer, so add a new one for this fragment sequence.
                    log.DebugFormat("[{0}] Creating new buffer {1} for this split fragment", session.ClientAccountString, fragment.Header.Sequence);
                    var newBuffer = new MessageBuffer(fragment.Header.Sequence, fragment.Header.Count);
                    newBuffer.AddFragment(fragment);

                    log.DebugFormat("[{0}] Added fragment {1} to the new buffer. Buffer at {2} of {3}", session.ClientAccountString, fragment.Header.Sequence, newBuffer.Count, newBuffer.TotalFragments);
                    partialFragments.TryAdd(fragment.Header.Sequence, newBuffer);
                }
            }
            else
            {
                // Packet is not split, proceed with handling it.
                log.DebugFormat("[{0}] Fragment {1} is not split", session.ClientAccountString, fragment.Header.Sequence);
                message = new ClientMessage(fragment.Data);
            }

            // If message is not null, we have a complete message to handle
            if (message != null)
            {
                // First check if this message is the next sequence, if it is not, add it to our outOfOrderFragments
                if (fragment.Header.Sequence == lastReceivedFragmentSequence + 1)
                {
                    log.DebugFormat("[{0}] Handling fragment {1}", session.ClientAccountString, fragment.Header.Sequence);
                    HandleFragment(message);
                }
                else
                {
                    log.DebugFormat("[{0}] Fragment {1} is early, lastReceivedFragmentSequence = {2}", session.ClientAccountString, fragment.Header.Sequence, lastReceivedFragmentSequence);
                    outOfOrderFragments.TryAdd(fragment.Header.Sequence, message);
                }
            }
        }

        /// <summary>
        /// Handles a ClientMessage by calling using InboundMessageManager
        /// </summary>
        /// <param name="message">ClientMessage to process</param>
        private void HandleFragment(ClientMessage message)
        {
            InboundMessageManager.HandleClientMessage(message, session);
            lastReceivedFragmentSequence++;
        }

        /// <summary>
        /// Checks if we now have packets queued out of order which should be processed as the next sequence.
        /// </summary>
        private void CheckOutOfOrderPackets()
        {
            ClientPacket packet = null;

            while (outOfOrderPackets.TryRemove(lastReceivedPacketSequence + 1, out packet))
            {
                log.DebugFormat("[{0}] Ready to handle out-of-order packet {1}", session.ClientAccountString, packet.Header.Sequence);
                HandlePacket(packet);
            }
        }

        /// <summary>
        /// Checks if we now have fragments queued out of order which should be handled as the next sequence.
        /// </summary>
        private void CheckOutOfOrderFragments()
        {
            ClientMessage message = null;
            while (outOfOrderFragments.TryRemove(lastReceivedFragmentSequence + 1, out message))
            {
                log.DebugFormat("[{0}] Ready to handle out of order fragment {1}", session.ClientAccountString, lastReceivedFragmentSequence + 1);
                HandleFragment(message);
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
                log.DebugFormat("[{0}] Retransmit {1}", session.ClientAccountString, sequence);

                if (!cachedPacket.Header.HasFlag(PacketHeaderFlags.Retransmission))
                    cachedPacket.Header.Flags |= PacketHeaderFlags.Retransmission;

                SendPacketRaw(cachedPacket);
            }
        }

        private void FlushPackets()
        {
            while (packetQueue.Count > 0)
            {
                log.DebugFormat("[{0}] Flushing packets, count {1}", session.ClientAccountString, packetQueue.Count);
                ServerPacket packet = packetQueue.Dequeue();

                if (packet.Header.HasFlag(PacketHeaderFlags.EncryptedChecksum) && ConnectionData.PacketSequence.CurrentValue == 0)
                    ConnectionData.PacketSequence = new Sequence.UIntSequence(1);

                // If we are only ACKing, then we don't seem to have to increment the sequence
                if (packet.Header.Flags == PacketHeaderFlags.AckSequence)
                    packet.Header.Sequence = ConnectionData.PacketSequence.CurrentValue;
                else
                    packet.Header.Sequence = ConnectionData.PacketSequence.NextValue;
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
            log.DebugFormat("[{0}] Sending packet {1}", session.ClientAccountString, packet.GetHashCode());
            if (packet.Header.HasFlag(PacketHeaderFlags.EncryptedChecksum))
            {
                uint issacXor = session.GetIssacValue(PacketDirection.Server);
                log.DebugFormat("[{0}] Setting Issac for packet {1} to {2}", session.ClientAccountString, packet.GetHashCode(), issacXor);
                packet.IssacXor = issacXor;
            }

            SendPacketRaw(packet);
        }

        private void SendPacketRaw(ServerPacket packet)
        {
            Socket socket = SocketManager.GetSocket();
            if (packet.Header.Sequence == 0)
                socket = SocketManager.GetSocket(0);

            byte[] payload = packet.GetPayload();

            if (packetLog.IsDebugEnabled)
            {
                System.Net.IPEndPoint listenerEndpoint = (System.Net.IPEndPoint)socket.LocalEndPoint;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(String.Format("[{5}] Sending Packet (Len: {0}) [{1}:{2}=>{3}:{4}]", payload.Length, listenerEndpoint.Address, listenerEndpoint.Port, session.EndPoint.Address, session.EndPoint.Port, session.SubscriptionId));
                sb.AppendLine(payload.BuildPacketString());
                packetLog.Debug(sb.ToString());
            }
            socket.SendTo(payload, session.EndPoint);
        }

        /// <summary>
        /// This function handles turning a bundle of messages (representing all messages accrued in a timeslice),
        /// into 1 or more packets, combining multiple messages into one packet or spliting large message across
        /// several packets as needed.
        /// </summary>
        /// <param name="bundle"></param>
        private void SendBundle(NetworkBundle bundle)
        {
            log.DebugFormat("[{0}] Sending Bundle", session.ClientAccountString);
            bool writeOptionalHeaders = true;

            List<MessageFragment> fragments = new List<MessageFragment>();

            // Pull all messages out and create MessageFragment objects
            while (bundle.HasMoreMessages)
            {
                var message = bundle.Dequeue();
                var fragment = new MessageFragment(message, ConnectionData.FragmentSequence++);
                fragments.Add(fragment);
            }

            log.DebugFormat("[{0}] Bundle Fragment Count: {1}", session.ClientAccountString, fragments.Count);

            // Loop through while we have fragements
            while (fragments.Count > 0 || writeOptionalHeaders)
            {
                ServerPacket packet = new ServerPacket();
                PacketHeader packetHeader = packet.Header;

                if (fragments.Count > 0)
                    packetHeader.Flags |= PacketHeaderFlags.BlobFragments;

                if (bundle.EncryptedChecksum)
                    packetHeader.Flags |= PacketHeaderFlags.EncryptedChecksum;

                uint availableSpace = Packet.MaxPacketDataSize;

                // Pull first message and see if it is a large one
                var firstMessage = fragments.FirstOrDefault();
                if (firstMessage != null)
                {
                    // If a large message send only this one, filling the whole packet
                    if (firstMessage.DataRemaining >= availableSpace)
                    {
                        log.DebugFormat("[{0}] Sending large fragment", session.ClientAccountString);
                        ServerPacketFragment spf = firstMessage.GetNextFragment();
                        packet.Fragments.Add(spf);
                        availableSpace -= (uint)spf.Length;
                        if (firstMessage.DataRemaining <= 0)
                            fragments.Remove(firstMessage);
                    }
                    // Otherwise we'll write any optional headers and process any small messages that will fit
                    else
                    {
                        if (writeOptionalHeaders)
                        {
                            writeOptionalHeaders = false;
                            WriteOptionalHeaders(bundle, packet);
                            availableSpace -= (uint)packet.Data.Length;
                        }

                        // Create a list to remove completed messages after iterator
                        List<MessageFragment> removeList = new List<MessageFragment>();

                        foreach (MessageFragment fragment in fragments)
                        {
                            // Is this a large fragment and does it have a tail that needs sending?
                            if (!fragment.TailSent && availableSpace >= fragment.TailSize)
                            {
                                log.DebugFormat("[{0}] Sending tail fragment", session.ClientAccountString);
                                ServerPacketFragment spf = fragment.GetTailFragment();
                                packet.Fragments.Add(spf);
                                availableSpace -= (uint)spf.Length;
                            }
                            // Otherwise will this message fit in the remaining space?
                            else if (availableSpace >= fragment.NextSize)
                            {
                                log.DebugFormat("[{0}] Sending small message", session.ClientAccountString);
                                ServerPacketFragment spf = fragment.GetNextFragment();
                                packet.Fragments.Add(spf);
                                availableSpace -= (uint)spf.Length;
                            }
                            // If message is out of data, set to remove it
                            if (fragment.DataRemaining <= 0)
                                removeList.Add(fragment);
                        }

                        // Remove all completed messages
                        fragments.RemoveAll(x => removeList.Contains(x));
                    }
                }
                // If no messages, write optional headers
                else
                {
                    log.DebugFormat("[{0}] No messages, just sending optional headers", session.ClientAccountString);
                    if (writeOptionalHeaders)
                    {
                        writeOptionalHeaders = false;
                        WriteOptionalHeaders(bundle, packet);
                        availableSpace -= (uint)packet.Data.Length;
                    }
                }

                EnqueueSend(packet);
            }
        }

        private void WriteOptionalHeaders(NetworkBundle bundle, ServerPacket packet)
        {
            PacketHeader packetHeader = packet.Header;
            if (bundle.SendAck) // 0x4000
            {
                packetHeader.Flags |= PacketHeaderFlags.AckSequence;
                log.DebugFormat("[{0}] Outgoing AckSeq: {1}", session.ClientAccountString, lastReceivedPacketSequence);
                packet.BodyWriter.Write(lastReceivedPacketSequence);
            }

            if (bundle.TimeSync) // 0x1000000
            {
                packetHeader.Flags |= PacketHeaderFlags.TimeSynch;
                log.DebugFormat("[{0}] Outgoing TimeSync TS: {1}", session.ClientAccountString, ConnectionData.ServerTime);
                packet.BodyWriter.Write(ConnectionData.ServerTime);
            }

            if (bundle.ClientTime != -1f) // 0x4000000
            {
                packetHeader.Flags |= PacketHeaderFlags.EchoResponse;
                log.DebugFormat("[{0}] Outgoing EchoResponse: {1}", session.ClientAccountString, bundle.ClientTime);
                packet.BodyWriter.Write(bundle.ClientTime);
                packet.BodyWriter.Write((float)ConnectionData.ServerTime - bundle.ClientTime);
            }
        }

        private class MessageBuffer
        {
            private List<ClientPacketFragment> fragments = new List<ClientPacketFragment>();

            public uint Sequence { get; }
            public int Count { get { return fragments.Count; } }
            public uint TotalFragments { get; }
            public bool Complete { get { return fragments.Count == TotalFragments; } }

            public MessageBuffer(uint sequence, uint totalFragments)
            {
                Sequence = sequence;
                TotalFragments = totalFragments;
            }

            public void AddFragment(ClientPacketFragment fragment)
            {
                lock (fragments)
                {
                    if (!Complete && !fragments.Any(x => x.Header.Index == fragment.Header.Index))
                        fragments.Add(fragment);
                }
            }

            public ClientMessage GetMessage()
            {
                fragments.Sort(delegate(ClientPacketFragment x, ClientPacketFragment y) { return (int)x.Header.Index - (int)y.Header.Index; });
                MemoryStream stream = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(stream);
                foreach (ClientPacketFragment fragment in fragments)
                {
                    writer.Write(fragment.Data);
                }
                stream.Seek(0, SeekOrigin.Begin);
                return new ClientMessage(stream);
            }
        }

        private class MessageFragment
        {
            public GameMessage Message { get; private set; }

            public uint Sequence { get; set; }

            public ushort Index { get; set; }

            public ushort Count { get; set; }

            public uint DataLength
            {
                get
                {
                    return (uint)Message.Data.Length;
                }
            }

            public uint DataRemaining { get; private set; }

            public uint NextSize
            {
                get
                {
                    var dataSize = DataRemaining;
                    if (dataSize > ServerPacketFragment.MaxFragmentDataSize)
                        dataSize = ServerPacketFragment.MaxFragmentDataSize;
                    return PacketFragmentHeader.HeaderSize + dataSize;
                }
            }

            public uint TailSize
            {
                get
                {
                    return PacketFragmentHeader.HeaderSize + (DataLength % ServerPacketFragment.MaxFragmentDataSize);
                }
            }

            public bool TailSent { get; private set; } = false;

            public MessageFragment(GameMessage message, uint sequence)
            {
                Message = message;
                DataRemaining = DataLength;
                Sequence = sequence;
                Count = (ushort)(Math.Ceiling((double)DataLength / PacketFragment.MaxFragmentDataSize));
                Index = 0;
                if (Count == 1)
                    TailSent = true;
                log.DebugFormat("Sequence {0}, count {1}, DataRemaining {2}", sequence, Count, DataRemaining);
            }

            public ServerPacketFragment GetTailFragment()
            {
                var index = (ushort)(Count - 1);
                TailSent = true;
                return CreateServerFragment(index);
            }

            public ServerPacketFragment GetNextFragment()
            {
                return CreateServerFragment(Index++);
            }

            private ServerPacketFragment CreateServerFragment(ushort index)
            {
                log.DebugFormat("Creating ServerFragment for index {0}", index);
                if (index >= Count)
                    throw new ArgumentOutOfRangeException("index", index, "Passed index is greater then computed count");

                var position = index * ServerPacketFragment.MaxFragmentDataSize;
                if (position > DataLength)
                    throw new ArgumentOutOfRangeException("index", index, "Passed index computes to invalid position size");

                if (DataRemaining <= 0)
                    throw new InvalidOperationException("There is no data remaining");

                var dataToSend = DataLength - position;
                if (dataToSend > ServerPacketFragment.MaxFragmentDataSize)
                    dataToSend = ServerPacketFragment.MaxFragmentDataSize;

                if (DataRemaining < dataToSend)
                    throw new InvalidOperationException("More data to send then data remaining!");

                // Read data starting at position reading dataToSend bytes
                Message.Data.Seek(position, SeekOrigin.Begin);
                byte[] data = new byte[dataToSend];
                Message.Data.Read(data, 0, (int)dataToSend);

                // Build ServerPacketFragment structure
                ServerPacketFragment fragment = new ServerPacketFragment(data);
                fragment.Header.Sequence = Sequence;
                fragment.Header.Id = 0x80000000;
                fragment.Header.Count = Count;
                fragment.Header.Index = index;
                fragment.Header.Group = (ushort)Message.Group;

                DataRemaining -= dataToSend;
                log.DebugFormat("Done creating ServerFragment for index {0}. After reading {1} DataRemaining {2}", index, dataToSend, DataRemaining);
                return fragment;
            }
        }

        private class NetworkBundle
        {
            private bool propChanged = false;

            public bool NeedsSending
            {
                get
                {
                    return propChanged || messages.Count > 0;
                }
            }
            public bool HasMoreMessages
            {
                get
                {
                    return messages.Count > 0;
                }
            }

            private Queue<GameMessage> messages = new Queue<GameMessage>();

            private float clientTime = -1f;
            public float ClientTime
            {
                get { return clientTime; }
                set
                {
                    clientTime = value;
                    propChanged = true;
                }
            }

            private bool timeSync = false;
            public bool TimeSync
            {
                get { return timeSync; }
                set
                {
                    timeSync = value;
                    propChanged = true;
                }
            }

            private bool ackSeq = false;
            public bool SendAck
            {
                get { return ackSeq; }
                set
                {
                    ackSeq = value;
                    propChanged = true;
                }
            }

            public bool EncryptedChecksum { get; set; } = false;

            public int CurrentSize { get; private set; }

            public NetworkBundle()
            {
            }

            public void Enqueue(GameMessage message)
            {
                CurrentSize += (int)message.Data.Length;
                messages.Enqueue(message);
            }

            public GameMessage Dequeue()
            {
                return messages.Dequeue();
            }
        }
    }
}
