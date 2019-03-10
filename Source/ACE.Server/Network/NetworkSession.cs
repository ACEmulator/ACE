using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

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

        private readonly Object[] currentBundleLocks = new Object[(int)GameMessageGroup.QueueMax];
        private readonly NetworkBundle[] currentBundles = new NetworkBundle[(int)GameMessageGroup.QueueMax];

        private ConcurrentDictionary<uint, ClientPacket> outOfOrderPackets = new ConcurrentDictionary<uint, ClientPacket>();
        private ConcurrentDictionary<uint, MessageBuffer> partialFragments = new ConcurrentDictionary<uint, MessageBuffer>();
        private ConcurrentDictionary<uint, ClientMessage> outOfOrderFragments = new ConcurrentDictionary<uint, ClientMessage>();

        private DateTime nextSend = DateTime.UtcNow;

        // Resync will be started after ConnectResponse, and should immediately be sent then, so no delay here.
        // Fun fact: even though we send the server time in the ConnectRequest, client doesn't seem to use it?  Therefore we must TimeSync early so client doesn't see a skew when we send it later.
        public bool sendResync;
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

            for (int i = 0; i < currentBundles.Length; i++)
            {
                currentBundleLocks[i] = new object();
                currentBundles[i] = new NetworkBundle();
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
                var currentBundleLock = currentBundleLocks[(int)grp];
                lock (currentBundleLock)
                {
                    var currentBundle = currentBundles[(int)grp];

                    foreach (var msg in k)
                    {
                        currentBundle.EncryptedChecksum = true;
                        packetLog.DebugFormat("[{0}] Enqueuing Message {1}", session.LoggingIdentifier, msg.Opcode);
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
                packetLog.DebugFormat("[{0}] Enqueuing Packet {1}", session.LoggingIdentifier, packet.GetHashCode());
                packetQueue.Enqueue(packet);
            }
        }

        /// <summary>
        /// Checks if we should send the current bundle and then flushes all pending packets.
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < currentBundles.Length; i++)
            {
                NetworkBundle bundleToSend = null;

                var group = (GameMessageGroup)i;

                var currentBundleLock = currentBundleLocks[i];
                lock (currentBundleLock)
                {
                    var currentBundle = currentBundles[i];

                    if (group == GameMessageGroup.InvalidQueue)
                    {
                        if (sendResync && !currentBundle.TimeSync && DateTime.UtcNow > nextResync)
                        {
                            packetLog.DebugFormat("[{0}] Setting to send TimeSync packet", session.LoggingIdentifier);
                            currentBundle.TimeSync = true;
                            currentBundle.EncryptedChecksum = true;
                            nextResync = DateTime.UtcNow.AddMilliseconds(timeBetweenTimeSync);
                        }

                        if (sendAck && !currentBundle.SendAck && DateTime.UtcNow > nextAck)
                        {
                            packetLog.DebugFormat("[{0}] Setting to send ACK packet", session.LoggingIdentifier);
                            currentBundle.SendAck = true;
                            nextAck = DateTime.UtcNow.AddMilliseconds(timeBetweenAck);
                        }

                        if (currentBundle.NeedsSending && DateTime.UtcNow >= nextSend)
                        {
                            packetLog.DebugFormat("[{0}] Swapping bundle", session.LoggingIdentifier);
                            // Swap out bundle so we can process it
                            bundleToSend = currentBundle;
                            currentBundles[i] = new NetworkBundle();
                        }
                    }
                    else
                    {
                        if (currentBundle.NeedsSending && DateTime.UtcNow >= nextSend)
                        {
                            packetLog.DebugFormat("[{0}] Swapping bundle", session.LoggingIdentifier);
                            // Swap out bundle so we can process it
                            bundleToSend = currentBundle;
                            currentBundles[i] = new NetworkBundle();
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
            packetLog.DebugFormat("[{0}] Processing packet {1}", session.LoggingIdentifier, packet.Header.Sequence);
            NetworkStatistics.C2S_Packets_Aggregate_Increment();

            // If the client is requesting a retransmission, verify the CRC and process it immediately
            // TO-DO: Theory: Would it be possible to verify all unencrypted CRCs here as well?
            if (packet.Header.HasFlag(PacketHeaderFlags.RequestRetransmit))
            {
                if (!packet.Header.HasFlag(PacketHeaderFlags.EncryptedChecksum))
                {
                    // discard retransmission request with cleartext CRC
                    // client sends one encrypted version and one non encrypted version of each retransmission request
                    // honoring these causes client to drop because it's only expecting one of the two retransmission requests to be honored
                    // and it's more secure to only accept the trusted version
                    return;
                }
                if (packet.VerifyCRC(ConnectionData.CryptoClient, false))
                {
                    packet.CRCVerified = true;
                    foreach (uint sequence in packet.HeaderOptional.RetransmitData)
                    {
                        Retransmit(sequence);
                    }
                    NetworkStatistics.C2S_RequestsForRetransmit_Aggregate_Increment();
                }
                else
                {
                    return;
                }
            }

            // Reordering stage
            // Check if this packet's sequence is a sequence which we have already processed.
            // There are some exceptions:
            // Sequence 0 as we have several Seq 0 packets during connect.  This also cathes a case where it seems CICMDCommand arrives at any point with 0 sequence value too.
            // If the only header on the packet is AckSequence. It seems AckSequence can come in with the same sequence value sometimes.
            if (packet.Header.Sequence <= lastReceivedPacketSequence && packet.Header.Sequence != 0 &&
                !(packet.Header.Flags == PacketHeaderFlags.AckSequence && packet.Header.Sequence == lastReceivedPacketSequence))
            {
                packetLog.WarnFormat("[{0}] Packet {1} received again", session.LoggingIdentifier, packet.Header.Sequence);
                return;
            }

            // Check if this packet's sequence is greater then the next one we should be getting.
            // If true we must store it to replay once we have caught up.
            var desiredSeq = lastReceivedPacketSequence + 1;
            if (packet.Header.Sequence > desiredSeq)
            {
                packetLog.DebugFormat("[{0}] Packet {1} received out of order", session.LoggingIdentifier, packet.Header.Sequence);

                if (!outOfOrderPackets.ContainsKey(packet.Header.Sequence))
                    outOfOrderPackets.TryAdd(packet.Header.Sequence, packet);

                if (desiredSeq + 2 <= packet.Header.Sequence && DateTime.Now - LastRequestForRetransmitTime > new TimeSpan(0, 0, 1))
                    DoRequestForRetransmission(packet.Header.Sequence);

                return;
            }

            // Processing stage
            // If we reach here, this is a packet we should proceed with processing.
            HandlePacket(packet);

            // Process data now in sequence
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

            EnqueueSend(reqPacket);

            LastRequestForRetransmitTime = DateTime.Now;
            packetLog.DebugFormat("[{0}] Requested retransmit of {1}", session.LoggingIdentifier, needSeq.Select(k => k.ToString()).Aggregate((a, b) => a + ", " + b));
            NetworkStatistics.S2C_RequestsForRetransmit_Aggregate_Increment();
        }
        private DateTime LastRequestForRetransmitTime = DateTime.MinValue;

        /// <summary>
        /// Handles a packet<para />
        /// Packets at this stage are already reordered
        /// </summary>
        /// <param name="packet">ClientPacket to handle</param>
        private void HandlePacket(ClientPacket packet)
        {
            packetLog.DebugFormat("[{0}] Handling packet {1}", session.LoggingIdentifier, packet.Header.Sequence);

            if (!packet.CRCVerified)
            {
                if (packet.VerifyCRC(ConnectionData.CryptoClient, true))
                {
                    packet.CRCVerified = true; 
                }
                else
                {
                    return;
                }
            }
            else
            {
                // Packet includes a request for retransmit and was already verified and half processed without advancing, so advance now
                ConnectionData.CryptoClient.RangeAdvance();
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.NetErrorDisconnect))
            {
                session.State = Enum.SessionState.ClientSentNetErrorDisconnect;
                return;
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
                AcknowledgeSequence(packet.HeaderOptional.AckSequence);

            if (packet.Header.HasFlag(PacketHeaderFlags.TimeSync))
            {
                packetLog.DebugFormat("[{0}] Incoming TimeSync TS: {1}", session.LoggingIdentifier, packet.HeaderOptional.TimeSynch);
                // Do something with this...
                // Based on network traces these are not 1:1.  Server seems to send them every 20 seconds per port.
                // Client seems to send them alternatingly every 2 or 4 seconds per port.
                // We will send this at a 20 second time interval.  I don't know what to do with these when we receive them at this point.
            }

            // This should be set on the first packet to the server indicating the client is logging in.
            // This is the start of a three-way handshake between the client and server (LoginRequest, ConnectRequest, ConnectResponse)
            // Note this would be sent to each server a client would connect too (Login and each world).
            // In our current implimenation we handle all roles in this one server.
            if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest))
            {
                packetLog.Debug($"[{session.LoggingIdentifier}] LoginRequest");
                AuthenticationHandler.HandleLoginRequest(packet, session);
                return;
            }



            // Process all fragments out of the packet
            foreach (ClientPacketFragment fragment in packet.Fragments)
                ProcessFragment(fragment);

            // Update the last received sequence.
            if (packet.Header.Sequence != 0 && packet.Header.Flags != PacketHeaderFlags.AckSequence)
                lastReceivedPacketSequence = packet.Header.Sequence;
        }

        /// <summary>
        /// Processes a fragment, combining split fragments as needed, then handling them
        /// </summary>
        /// <param name="fragment">ClientPacketFragment to process</param>
        private void ProcessFragment(ClientPacketFragment fragment)
        {
            packetLog.DebugFormat("[{0}] Processing fragment {1}", session.LoggingIdentifier, fragment.Header.Sequence);

            ClientMessage message = null;

            // Check if this fragment is split
            if (fragment.Header.Count != 1)
            {
                // Packet is split
                packetLog.DebugFormat("[{0}] Fragment {1} is split, this index {2} of {3} fragments", session.LoggingIdentifier, fragment.Header.Sequence, fragment.Header.Index, fragment.Header.Count);

                if (partialFragments.TryGetValue(fragment.Header.Sequence, out var buffer))
                {
                    // Existing buffer, add this to it and check if we are finally complete.
                    buffer.AddFragment(fragment);
                    packetLog.DebugFormat("[{0}] Added fragment {1} to existing buffer. Buffer at {2} of {3}", session.LoggingIdentifier, fragment.Header.Sequence, buffer.Count, buffer.TotalFragments);
                    if (buffer.Complete)
                    {
                        // The buffer is complete, so we can go ahead and handle
                        packetLog.DebugFormat("[{0}] Buffer {1} is complete", session.LoggingIdentifier, buffer.Sequence);
                        message = buffer.GetMessage();
                        MessageBuffer removed = null;
                        partialFragments.TryRemove(fragment.Header.Sequence, out removed);
                    }
                }
                else
                {
                    // No existing buffer, so add a new one for this fragment sequence.
                    packetLog.DebugFormat("[{0}] Creating new buffer {1} for this split fragment", session.LoggingIdentifier, fragment.Header.Sequence);
                    var newBuffer = new MessageBuffer(fragment.Header.Sequence, fragment.Header.Count);
                    newBuffer.AddFragment(fragment);

                    packetLog.DebugFormat("[{0}] Added fragment {1} to the new buffer. Buffer at {2} of {3}", session.LoggingIdentifier, fragment.Header.Sequence, newBuffer.Count, newBuffer.TotalFragments);
                    partialFragments.TryAdd(fragment.Header.Sequence, newBuffer);
                }
            }
            else
            {
                // Packet is not split, proceed with handling it.
                packetLog.DebugFormat("[{0}] Fragment {1} is not split", session.LoggingIdentifier, fragment.Header.Sequence);
                message = new ClientMessage(fragment.Data);
            }

            // If message is not null, we have a complete message to handle
            if (message != null)
            {
                // First check if this message is the next sequence, if it is not, add it to our outOfOrderFragments
                if (fragment.Header.Sequence == lastReceivedFragmentSequence + 1)
                {
                    packetLog.DebugFormat("[{0}] Handling fragment {1}", session.LoggingIdentifier, fragment.Header.Sequence);
                    HandleFragment(message);
                }
                else
                {
                    packetLog.DebugFormat("[{0}] Fragment {1} is early, lastReceivedFragmentSequence = {2}", session.LoggingIdentifier, fragment.Header.Sequence, lastReceivedFragmentSequence);
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
                packetLog.DebugFormat("[{0}] Ready to handle out-of-order packet {1}", session.LoggingIdentifier, packet.Header.Sequence);
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
                packetLog.DebugFormat("[{0}] Ready to handle out of order fragment {1}", session.LoggingIdentifier, lastReceivedFragmentSequence + 1);
                HandleFragment(message);
            }
        }

        //is this special channel
        private void FlagEcho(float clientTime)
        {
            var currentBundleLock = currentBundleLocks[(int)GameMessageGroup.InvalidQueue];
            lock (currentBundleLock)
            {
                var currentBundle = currentBundles[(int)GameMessageGroup.InvalidQueue];

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
                packetLog.DebugFormat("[{0}] Retransmit {1}", session.LoggingIdentifier, sequence);

                if (!cachedPacket.Header.HasFlag(PacketHeaderFlags.Retransmission))
                    cachedPacket.Header.Flags |= PacketHeaderFlags.Retransmission;

                SendPacketRaw(cachedPacket);
            }
            else
            {
                log.Error($"retransmit requested packet {sequence} not in cache.");
            }
        }

        private void FlushPackets()
        {
            while (packetQueue.Count > 0)
            {
                packetLog.DebugFormat("[{0}] Flushing packets, count {1}", session.LoggingIdentifier, packetQueue.Count);

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
            packetLog.DebugFormat("[{0}] Sending packet {1}", session.LoggingIdentifier, packet.GetHashCode());
            NetworkStatistics.S2C_Packets_Aggregate_Increment();

            if (packet.Header.HasFlag(PacketHeaderFlags.EncryptedChecksum))
            {
                uint issacXor = ConnectionData.IssacServer.GetOffset();
                packetLog.DebugFormat("[{0}] Setting Issac for packet {1} to {2}", session.LoggingIdentifier, packet.GetHashCode(), issacXor);
                packet.IssacXor = issacXor;
            }

            SendPacketRaw(packet);
        }

        private void SendPacketRaw(ServerPacket packet)
        {
            byte[] buffer = ArrayPool<byte>.Shared.Rent((int)(PacketHeader.HeaderSize + (packet.Data?.Length ?? 0) + (packet.Fragments.Count * PacketFragment.MaxFragementSize)));

            try
            {
                Socket socket = SocketManager.GetMainSocket();

                packet.CreateReadyToSendPacket(buffer, out var size);

                packetLog.Debug(packet.ToString());

                buffer = NetworkSyntheticTesting.SyntheticCorruption_S2C(buffer);

                if (packetLog.IsDebugEnabled)
                {
                    var listenerEndpoint = (System.Net.IPEndPoint)socket.LocalEndPoint;
                    var sb = new StringBuilder();
                    sb.AppendLine(String.Format("[{5}] Sending Packet (Len: {0}) [{1}:{2}=>{3}:{4}]", buffer.Length, listenerEndpoint.Address, listenerEndpoint.Port, session.EndPoint.Address, session.EndPoint.Port, session.Network.ClientId));
                    sb.AppendLine(buffer.BuildPacketString());
                    packetLog.Debug(sb.ToString());
                }

                try
                {
                    socket.SendTo(buffer, size, SocketFlags.None, session.EndPoint);
                }
                catch (SocketException ex)
                {
                    // Unhandled Exception: System.Net.Sockets.SocketException: A message sent on a datagram socket was larger than the internal message buffer or some other network limit, or the buffer used to receive a datagram into was smaller than the datagram itself
                    // at System.Net.Sockets.Socket.UpdateStatusAfterSocketErrorAndThrowException(SocketError error, String callerName)
                    // at System.Net.Sockets.Socket.SendTo(Byte[] buffer, Int32 offset, Int32 size, SocketFlags socketFlags, EndPoint remoteEP)

                    var listenerEndpoint = (System.Net.IPEndPoint)socket.LocalEndPoint;
                    var sb = new StringBuilder();
                    sb.AppendLine(ex.ToString());
                    sb.AppendLine(String.Format("[{5}] Sending Packet (Len: {0}) [{1}:{2}=>{3}:{4}]", buffer.Length, listenerEndpoint.Address, listenerEndpoint.Port, session.EndPoint.Address, session.EndPoint.Port, session.Network.ClientId));
                    log.Error(sb.ToString());

                    session.State = Enum.SessionState.NetworkTimeout; // This will force WorldManager to drop the session
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer, true);
            }
        }

        /// <summary>
        /// This function handles turning a bundle of messages (representing all messages accrued in a timeslice),
        /// into 1 or more packets, combining multiple messages into one packet or spliting large message across
        /// several packets as needed.
        /// </summary>
        /// <param name="bundle"></param>
        private void SendBundle(NetworkBundle bundle, GameMessageGroup group)
        {
            packetLog.DebugFormat("[{0}] Sending Bundle", session.LoggingIdentifier);

            bool writeOptionalHeaders = true;

            List<MessageFragment> fragments = new List<MessageFragment>();

            // Pull all messages out and create MessageFragment objects
            while (bundle.HasMoreMessages)
            {
                var message = bundle.Dequeue();

                var fragment = new MessageFragment(message, ConnectionData.FragmentSequence++);
                fragments.Add(fragment);
            }

            packetLog.DebugFormat("[{0}] Bundle Fragment Count: {1}", session.LoggingIdentifier, fragments.Count);

            // Loop through while we have fragements
            while (fragments.Count > 0 || writeOptionalHeaders)
            {
                ServerPacket packet = new ServerPacket();
                PacketHeader packetHeader = packet.Header;

                if (fragments.Count > 0)
                    packetHeader.Flags |= PacketHeaderFlags.BlobFragments;

                if (bundle.EncryptedChecksum)
                    packetHeader.Flags |= PacketHeaderFlags.EncryptedChecksum;

                int availableSpace = ServerPacket.MaxPacketSize;

                // Pull first message and see if it is a large one
                var firstMessage = fragments.FirstOrDefault();
                if (firstMessage != null)
                {
                    // If a large message send only this one, filling the whole packet
                    if (firstMessage.DataRemaining >= availableSpace)
                    {
                        packetLog.DebugFormat("[{0}] Sending large fragment", session.LoggingIdentifier);
                        ServerPacketFragment spf = firstMessage.GetNextFragment();
                        packet.Fragments.Add(spf);
                        availableSpace -= spf.Length;
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
                            if (packet.Data != null)
                                availableSpace -= (int)packet.Data.Length;
                        }

                        // Create a list to remove completed messages after iterator
                        List<MessageFragment> removeList = new List<MessageFragment>();

                        foreach (MessageFragment fragment in fragments)
                        {
                            // Is this a large fragment and does it have a tail that needs sending?
                            if (!fragment.TailSent && availableSpace >= fragment.TailSize)
                            {
                                packetLog.DebugFormat("[{0}] Sending tail fragment", session.LoggingIdentifier);
                                ServerPacketFragment spf = fragment.GetTailFragment();
                                packet.Fragments.Add(spf);
                                availableSpace -= spf.Length;
                            }
                            // Otherwise will this message fit in the remaining space?
                            else if (availableSpace >= fragment.NextSize)
                            {
                                packetLog.DebugFormat("[{0}] Sending small message", session.LoggingIdentifier);
                                ServerPacketFragment spf = fragment.GetNextFragment();
                                packet.Fragments.Add(spf);
                                availableSpace -= spf.Length;
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
                    packetLog.DebugFormat("[{0}] No messages, just sending optional headers", session.LoggingIdentifier);
                    if (writeOptionalHeaders)
                    {
                        writeOptionalHeaders = false;
                        WriteOptionalHeaders(bundle, packet);
                        if (packet.Data != null)
                            availableSpace -= (int)packet.Data.Length;
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
                packetLog.DebugFormat("[{0}] Outgoing AckSeq: {1}", session.LoggingIdentifier, lastReceivedPacketSequence);
                packet.InitializeBodyWriter();
                packet.BodyWriter.Write(lastReceivedPacketSequence);
            }

            if (bundle.TimeSync) // 0x1000000
            {
                packetHeader.Flags |= PacketHeaderFlags.TimeSync;
                packetLog.DebugFormat("[{0}] Outgoing TimeSync TS: {1}", session.LoggingIdentifier, ConnectionData.ServerTime);
                packet.InitializeBodyWriter();
                packet.BodyWriter.Write(ConnectionData.ServerTime);
            }

            if (bundle.ClientTime != -1f) // 0x4000000
            {
                packetHeader.Flags |= PacketHeaderFlags.EchoResponse;
                packetLog.DebugFormat("[{0}] Outgoing EchoResponse: {1}", session.LoggingIdentifier, bundle.ClientTime);
                packet.InitializeBodyWriter();
                packet.BodyWriter.Write(bundle.ClientTime);
                packet.BodyWriter.Write((float)ConnectionData.ServerTime - bundle.ClientTime);
            }
        }
    }
}
