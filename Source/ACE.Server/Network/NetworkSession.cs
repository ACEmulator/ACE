using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.Handlers;
using ACE.Server.Network.Managers;
using log4net;

namespace ACE.Server.Network
{
    public class NetworkSession
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog packetLog = LogManager.GetLogger(System.Reflection.Assembly.GetEntryAssembly(), "Packets");

        private const int minimumTimeBetweenBundles = 5; // 5ms
        private const int timeBetweenTimeSync = 20000; // 20s
        private const int timeBetweenAck = 2000; // 2s

        private readonly Session session;

        //private readonly Object currentBundleLock = new Object();
        private ConcurrentDictionary<GameMessageGroup, Object> currentBundleLocks = new ConcurrentDictionary<GameMessageGroup, Object>();
        private ConcurrentDictionary<GameMessageGroup, NetworkBundle> currentBundles = new ConcurrentDictionary<GameMessageGroup, NetworkBundle>();
        //private NetworkBundle currentBundle = new NetworkBundle();

        private ConcurrentDictionary<uint, ClientPacket> outOfOrderPackets = new ConcurrentDictionary<uint, ClientPacket>();
        private ConcurrentDictionary<uint, MessageBuffer> partialFragments = new ConcurrentDictionary<uint, MessageBuffer>();
        private ConcurrentDictionary<uint, ClientMessage> outOfOrderFragments = new ConcurrentDictionary<uint, ClientMessage>();

        private DateTime nextSend = DateTime.UtcNow;

        // Resync will be started after ConnectResponse, and should immediately be sent then, so no delay here.
        // Fun fact: even though we send the server time in the ConnectRequest, client doesn't seem to use it?  Therefore we must TimeSync early so client doesn't see a skew when we send it later.
        private bool sendResync;
        private DateTime nextResync = DateTime.UtcNow;

        // Ack should be sent after a 2 second delay, so start enabled with the delay.
        // Sending this too early seems to cause issues with clients disconnecting.
        private bool sendAck = true;
        private DateTime nextAck = DateTime.UtcNow.AddMilliseconds(timeBetweenAck);

        private uint lastReceivedPacketSequence = 1;
        private uint lastReceivedFragmentSequence;

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

            foreach (var gmg in System.Enum.GetValues(typeof(GameMessageGroup)))
            {
                var group = (GameMessageGroup)gmg;
                currentBundleLocks[group] = new object();
                currentBundles[group] = new NetworkBundle();
            }
        }

        /// <summary>
        /// Enequeues a GameMessage for sending to this client.
        /// This may be called from many threads.
        /// </summary>
        /// <param name="messages">One or more GameMessages to send</param>
        public void EnqueueSend(params GameMessage[] messages)
        {
            messages.GroupBy(k => k.Group).ToList().ForEach(k =>
            {
                var grp = k.First().Group;
                var currentBundleLock = currentBundleLocks[grp];
                lock (currentBundleLock)
                {
                    foreach (var msg in k)
                    {
                        var currentBundle = currentBundles[msg.Group];
                        currentBundle.EncryptedChecksum = true;
                        log.DebugFormat("[{0}] Enqueuing Message {1}", session.LoggingIdentifier, msg.Opcode);
                        currentBundle.Enqueue(msg);
                    }
                }
            });
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
                log.DebugFormat("[{0}] Enqueuing Packet {1}", session.LoggingIdentifier, packet.GetHashCode());
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

            currentBundles.Keys.ToList().ForEach(group =>
            {
                var currentBundleLock = currentBundleLocks[group];
                var currentBundle = currentBundles[group];

                NetworkBundle bundleToSend = null;
                lock (currentBundleLock)
                {
                    if (group == GameMessageGroup.InvalidQueue)
                    {
                        if (sendResync && !currentBundle.TimeSync && DateTime.UtcNow > nextResync)
                        {
                            log.DebugFormat("[{0}] Setting to send TimeSync packet", session.LoggingIdentifier);
                            currentBundle.TimeSync = true;
                            currentBundle.EncryptedChecksum = true;
                            nextResync = DateTime.UtcNow.AddMilliseconds(timeBetweenTimeSync);
                        }

                        if (sendAck && !currentBundle.SendAck && DateTime.UtcNow > nextAck)
                        {
                            log.DebugFormat("[{0}] Setting to send ACK packet", session.LoggingIdentifier);
                            currentBundle.SendAck = true;
                            nextAck = DateTime.UtcNow.AddMilliseconds(timeBetweenAck);
                        }

                        if (currentBundle.NeedsSending && DateTime.UtcNow > nextSend)
                        {
                            log.DebugFormat("[{0}] Swaping bundle", session.LoggingIdentifier);
                            // Swap out bundle so we can process it
                            bundleToSend = currentBundle;
                            currentBundles[group] = new NetworkBundle();
                        }
                    }
                    else
                    {
                        if (currentBundle.NeedsSending && DateTime.UtcNow > nextSend)
                        {
                            log.DebugFormat("[{0}] Swaping bundle", session.LoggingIdentifier);
                            // Swap out bundle so we can process it
                            bundleToSend = currentBundle;
                            currentBundles[group] = new NetworkBundle();
                        }
                    }
                }

                // Send our bundle if we have one
                // We should be able to execute this outside the lock as Sending is single threaded
                // and all future writes from other threads will go to the new bundle
                if (bundleToSend != null)
                {
                    SendBundle(bundleToSend, group);
                    nextSend = DateTime.UtcNow.AddMilliseconds(minimumTimeBetweenBundles);
                }
            });
            FlushPackets();
        }

        // This is called from ConnectionListener.OnDataReceieve()->Session.ProcessPacket()->This
        /// <summary>
        /// Processes and incoming packet from a client.
        /// </summary>
        /// <param name="packet">The ClientPacket to process.</param>
        public void ProcessPacket(ClientPacket packet)
        {
            log.DebugFormat("[{0}] Processing packet {1}", session.LoggingIdentifier, packet.Header.Sequence);
            // Check if this packet's sequence is a sequence which we have already processed.
            // There are some exceptions:
            // Sequence 0 as we have several Seq 0 packets during connect.  This also cathes a case where it seems CICMDCommand arrives at any point with 0 sequence value too.
            // If the only header on the packet is AckSequence. It seems AckSequence can come in with the same sequence value sometimes.
            if (packet.Header.Sequence <= lastReceivedPacketSequence && packet.Header.Sequence != 0 &&
                !(packet.Header.Flags == PacketHeaderFlags.AckSequence && packet.Header.Sequence == lastReceivedPacketSequence))
            {
                log.WarnFormat("[{0}] Packet {1} received again", session.LoggingIdentifier, packet.Header.Sequence);
                return;
            }

            // Check if this packet's sequence is greater then the next one we should be getting.
            // If true we must store it to replay once we have caught up.
            var desiredSeq = lastReceivedPacketSequence + 1;
            if (packet.Header.Sequence > desiredSeq)
            {
                log.WarnFormat("[{0}] Packet {1} received out of order", session.LoggingIdentifier, packet.Header.Sequence);
                if (!outOfOrderPackets.ContainsKey(packet.Header.Sequence))
                    outOfOrderPackets.TryAdd(packet.Header.Sequence, packet);

                if (desiredSeq + 2 <= packet.Header.Sequence && DateTime.Now - LastRequestForRetransmitTime > new TimeSpan(0, 0, 1))
                    DoRequestForRetransmission(packet.Header.Sequence);

                return;
            }

            // If we reach here, this is a packet we should proceed with processing.
            HandlePacket(packet);

            // Finally check if we have any out of order packets or fragments we need to process;
            CheckOutOfOrderPackets();
            CheckOutOfOrderFragments();
        }
        /// <summary>
        /// request retransmission of lost sequences
        /// </summary>
        /// <param name="rcvdSeq">the sequence of the packet that was just received.</param>
        private void DoRequestForRetransmission(uint rcvdSeq)
        {
            var desiredSeq = lastReceivedPacketSequence + 1;
            List<uint> needSeq = new List<uint>();
            needSeq.Add(desiredSeq);
            uint bottom = desiredSeq + 1;
            for (uint a = bottom; a < rcvdSeq; a++)
                if (!outOfOrderPackets.ContainsKey(a))
                    needSeq.Add(a);

            ServerPacket reqPacket = new ServerPacket();
            byte[] reqData = new byte[4 + (needSeq.Count * 4)];
            MemoryStream msReqData = new MemoryStream(reqData);
            msReqData.Write(BitConverter.GetBytes((uint)needSeq.Count), 0, 4);
            needSeq.ForEach(k => msReqData.Write(BitConverter.GetBytes(k), 0, 4));
            reqPacket.Data = msReqData;
            reqPacket.Header.Flags = PacketHeaderFlags.RequestRetransmit;
            //reqPacket.Header.Checksum = reqPacket.Header.CalculateHash32();

            EnqueueSend(reqPacket);

            // TODO: calculate session packet loss
            RequestsForRetransmit += (uint)needSeq.Count;
            LastRequestForRetransmitTime = DateTime.Now;
            log.WarnFormat("[{0}] Requested retransmit of {1}", session.LoggingIdentifier, needSeq.Select(k => k.ToString()).Aggregate((a, b) => a + ", " + b));
        }
        private DateTime LastRequestForRetransmitTime = DateTime.MinValue;
        private uint RequestsForRetransmit = 0;

        /// <summary>
        /// Handles a packet, reading the flags and processing all fragments.
        /// </summary>
        /// <param name="packet">ClientPacket to handle</param>
        private void HandlePacket(ClientPacket packet)
        {
            log.DebugFormat("[{0}] Handling packet {1}", session.LoggingIdentifier, packet.Header.Sequence);


            // Upon a client's request of packet retransmit the session CRC salt/offset becomes out of sync somehow.
            // This hack recovers the correct offset and makes WAN client sessions at least reliable enough to test with.
            // TODO: figure out why
            uint issacXor = !packet.Header.HasFlag(PacketHeaderFlags.RequestRetransmit) && packet.Header.HasFlag(PacketHeaderFlags.EncryptedChecksum) ? ConnectionData.IssacClient.GetOffset() : 0;
            if (!packet.VerifyChecksum(issacXor))
            {
                if (issacXor != 0)
                {
                    issacXor = ConnectionData.IssacClient.GetOffset();
                    log.WarnFormat("[{0}] Packet {1} has invalid checksum, trying the next offset", session.LoggingIdentifier, packet.Header.Sequence);

                    bool verified = packet.VerifyChecksum(issacXor);
                    log.WarnFormat("[{0}] Packet {1} improvised offset checksum result: {2}", session.LoggingIdentifier, packet.Header.Sequence, (verified) ? "successful" : "failed");
                }
                else
                    log.WarnFormat("[{0}] Packet {1} has invalid checksum", session.LoggingIdentifier, packet.Header.Sequence);
            }
            // TODO: drop corrupted packets?

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

            if (packet.Header.HasFlag(PacketHeaderFlags.TimeSync))
            {
                log.DebugFormat("[{0}] Incoming TimeSync TS: {1}", session.LoggingIdentifier, packet.HeaderOptional.TimeSynch);
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
                // TODO: calculate packet loss
            }

            // This should be set on the first packet to the server indicating the client is logging in.
            // This is the start of a three-way handshake between the client and server (LoginRequest, ConnectRequest, ConnectResponse)
            // Note this would be sent to each server a client would connect too (Login and each world).
            // In our current implimenation we handle all roles in this one server.
            if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest))
            {
                log.Info($"[{session.LoggingIdentifier}] LoginRequest");
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
            log.DebugFormat("[{0}] Processing fragment {1}", session.LoggingIdentifier, fragment.Header.Sequence);
            ClientMessage message = null;
            // Check if this fragment is split
            if (fragment.Header.Count != 1)
            {
                // Packet is split
                log.DebugFormat("[{0}] Fragment {1} is split, this index {2} of {3} fragments", session.LoggingIdentifier, fragment.Header.Sequence, fragment.Header.Index, fragment.Header.Count);

                if (partialFragments.TryGetValue(fragment.Header.Sequence, out var buffer))
                {
                    // Existing buffer, add this to it and check if we are finally complete.
                    buffer.AddFragment(fragment);
                    log.DebugFormat("[{0}] Added fragment {1} to existing buffer. Buffer at {2} of {3}", session.LoggingIdentifier, fragment.Header.Sequence, buffer.Count, buffer.TotalFragments);
                    if (buffer.Complete)
                    {
                        // The buffer is complete, so we can go ahead and handle
                        log.DebugFormat("[{0}] Buffer {1} is complete", session.LoggingIdentifier, buffer.Sequence);
                        message = buffer.GetMessage();
                        MessageBuffer removed = null;
                        partialFragments.TryRemove(fragment.Header.Sequence, out removed);
                    }
                }
                else
                {
                    // No existing buffer, so add a new one for this fragment sequence.
                    log.DebugFormat("[{0}] Creating new buffer {1} for this split fragment", session.LoggingIdentifier, fragment.Header.Sequence);
                    var newBuffer = new MessageBuffer(fragment.Header.Sequence, fragment.Header.Count);
                    newBuffer.AddFragment(fragment);

                    log.DebugFormat("[{0}] Added fragment {1} to the new buffer. Buffer at {2} of {3}", session.LoggingIdentifier, fragment.Header.Sequence, newBuffer.Count, newBuffer.TotalFragments);
                    partialFragments.TryAdd(fragment.Header.Sequence, newBuffer);
                }
            }
            else
            {
                // Packet is not split, proceed with handling it.
                log.DebugFormat("[{0}] Fragment {1} is not split", session.LoggingIdentifier, fragment.Header.Sequence);
                message = new ClientMessage(fragment.Data);
            }

            // If message is not null, we have a complete message to handle
            if (message != null)
            {
                // First check if this message is the next sequence, if it is not, add it to our outOfOrderFragments
                if (fragment.Header.Sequence == lastReceivedFragmentSequence + 1)
                {
                    log.DebugFormat("[{0}] Handling fragment {1}", session.LoggingIdentifier, fragment.Header.Sequence);
                    HandleFragment(message);
                }
                else
                {
                    log.DebugFormat("[{0}] Fragment {1} is early, lastReceivedFragmentSequence = {2}", session.LoggingIdentifier, fragment.Header.Sequence, lastReceivedFragmentSequence);
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
            while (outOfOrderPackets.TryRemove(lastReceivedPacketSequence + 1, out var packet))
            {
                log.DebugFormat("[{0}] Ready to handle out-of-order packet {1}", session.LoggingIdentifier, packet.Header.Sequence);
                HandlePacket(packet);
            }
        }

        /// <summary>
        /// Checks if we now have fragments queued out of order which should be handled as the next sequence.
        /// </summary>
        private void CheckOutOfOrderFragments()
        {
            while (outOfOrderFragments.TryRemove(lastReceivedFragmentSequence + 1, out var message))
            {
                log.DebugFormat("[{0}] Ready to handle out of order fragment {1}", session.LoggingIdentifier, lastReceivedFragmentSequence + 1);
                HandleFragment(message);
            }
        }

        //is this special channel
        private void FlagEcho(float clientTime)
        {
            var currentBundle = currentBundles[GameMessageGroup.InvalidQueue];
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
                if (removedPacket.Data != null)
                    removedPacket.Data.Dispose();
            }
        }

        private void Retransmit(uint sequence)
        {
            if (cachedPackets.TryGetValue(sequence, out var cachedPacket))
            {
                log.DebugFormat("[{0}] Retransmit {1}", session.LoggingIdentifier, sequence);

                if (!cachedPacket.Header.HasFlag(PacketHeaderFlags.Retransmission))
                    cachedPacket.Header.Flags |= PacketHeaderFlags.Retransmission;

                SendPacketRaw(cachedPacket);
            }
        }

        private void FlushPackets()
        {
            while (packetQueue.Count > 0)
            {
                log.DebugFormat("[{0}] Flushing packets, count {1}", session.LoggingIdentifier, packetQueue.Count);
                ServerPacket packet = packetQueue.Dequeue();

                if (packet.Header.HasFlag(PacketHeaderFlags.EncryptedChecksum) && ConnectionData.PacketSequence.CurrentValue == 0)
                    ConnectionData.PacketSequence = new Sequence.UIntSequence(1);

                // If we are only ACKing, then we don't seem to have to increment the sequence
                if (packet.Header.Flags == PacketHeaderFlags.AckSequence || packet.Header.Flags.HasFlag(PacketHeaderFlags.RequestRetransmit))
                    packet.Header.Sequence = ConnectionData.PacketSequence.CurrentValue;
                else
                    packet.Header.Sequence = ConnectionData.PacketSequence.NextValue;
                packet.Header.Id = ServerId;
                packet.Header.Iteration = 0x14;
                packet.Header.Time = (ushort)ConnectionData.ServerTime;


                if (packet.Header.Sequence >= 2u)
                    cachedPackets.TryAdd(packet.Header.Sequence, packet);

                SendPacket(packet);
            }
        }

        private void SendPacket(ServerPacket packet)
        {
            log.DebugFormat("[{0}] Sending packet {1}", session.LoggingIdentifier, packet.GetHashCode());
            if (packet.Header.HasFlag(PacketHeaderFlags.EncryptedChecksum))
            {
                uint issacXor = session.GetIssacValue(PacketDirection.Server);
                log.DebugFormat("[{0}] Setting Issac for packet {1} to {2}", session.LoggingIdentifier, packet.GetHashCode(), issacXor);
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
                sb.AppendLine(String.Format("[{5}] Sending Packet (Len: {0}) [{1}:{2}=>{3}:{4}]", payload.Length, listenerEndpoint.Address, listenerEndpoint.Port, session.EndPoint.Address, session.EndPoint.Port, session.Id));
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
        private void SendBundle(NetworkBundle bundle, GameMessageGroup group)
        {
            log.DebugFormat("[{0}] Sending Bundle", session.LoggingIdentifier);
            bool writeOptionalHeaders = true;

            List<MessageFragment> fragments = new List<MessageFragment>();

            // Pull all messages out and create MessageFragment objects
            while (bundle.HasMoreMessages)
            {
                var message = bundle.Dequeue();

                var fragment = new MessageFragment(message, ConnectionData.FragmentSequence++);
                fragments.Add(fragment);
            }

            log.DebugFormat("[{0}] Bundle Fragment Count: {1}", session.LoggingIdentifier, fragments.Count);

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
                        log.DebugFormat("[{0}] Sending large fragment", session.LoggingIdentifier);
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
                                log.DebugFormat("[{0}] Sending tail fragment", session.LoggingIdentifier);
                                ServerPacketFragment spf = fragment.GetTailFragment();
                                packet.Fragments.Add(spf);
                                availableSpace -= (uint)spf.Length;
                            }
                            // Otherwise will this message fit in the remaining space?
                            else if (availableSpace >= fragment.NextSize)
                            {
                                log.DebugFormat("[{0}] Sending small message", session.LoggingIdentifier);
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
                    log.DebugFormat("[{0}] No messages, just sending optional headers", session.LoggingIdentifier);
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
                log.DebugFormat("[{0}] Outgoing AckSeq: {1}", session.LoggingIdentifier, lastReceivedPacketSequence);
                packet.BodyWriter.Write(lastReceivedPacketSequence);
            }

            if (bundle.TimeSync) // 0x1000000
            {
                packetHeader.Flags |= PacketHeaderFlags.TimeSync;
                log.DebugFormat("[{0}] Outgoing TimeSync TS: {1}", session.LoggingIdentifier, ConnectionData.ServerTime);
                packet.BodyWriter.Write(ConnectionData.ServerTime);
            }

            if (bundle.ClientTime != -1f) // 0x4000000
            {
                packetHeader.Flags |= PacketHeaderFlags.EchoResponse;
                log.DebugFormat("[{0}] Outgoing EchoResponse: {1}", session.LoggingIdentifier, bundle.ClientTime);
                packet.BodyWriter.Write(bundle.ClientTime);
                packet.BodyWriter.Write((float)ConnectionData.ServerTime - bundle.ClientTime);
            }
        }

        private class MessageBuffer
        {
            private List<ClientPacketFragment> fragments = new List<ClientPacketFragment>();

            public uint Sequence { get; }
            public int Count => fragments.Count;
            public uint TotalFragments { get; }
            public bool Complete => fragments.Count == TotalFragments;

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
                fragments.Sort(delegate (ClientPacketFragment x, ClientPacketFragment y) { return (int)x.Header.Index - (int)y.Header.Index; });
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

        private class NetworkBundle
        {
            private bool propChanged;

            public bool NeedsSending => propChanged || messages.Count > 0;

            public bool HasMoreMessages => messages.Count > 0;

            private Queue<GameMessage> messages = new Queue<GameMessage>();

            private float clientTime = -1f;
            public float ClientTime
            {
                get => clientTime;
                set
                {
                    clientTime = value;
                    propChanged = true;
                }
            }

            private bool timeSync;
            public bool TimeSync
            {
                get => timeSync;
                set
                {
                    timeSync = value;
                    propChanged = true;
                }
            }

            private bool ackSeq;
            public bool SendAck
            {
                get => ackSeq;
                set
                {
                    ackSeq = value;
                    propChanged = true;
                }
            }

            public bool EncryptedChecksum { get; set; }

            public int CurrentSize { get; private set; }

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
