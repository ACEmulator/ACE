using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

using ACE.Common.Cryptography;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Handlers;
using ACE.Server.Network.Managers;
using ACE.Server.Network.Packets;

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
        private readonly ConnectionListener connectionListener;

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

        private static readonly TimeSpan cachedPacketPruneInterval = TimeSpan.FromSeconds(5);
        private DateTime lastCachedPacketPruneTime;
        /// <summary>
        /// Number of seconds to retain cachedPackets
        /// </summary>
        private const int cachedPacketRetentionTime = 120;

        /// <summary>
        /// This is referenced by multiple thread:<para />
        /// [ConnectionListener Thread + 0] WorldManager.ProcessPacket()->SendLoginRequestReject()<para />
        /// [ConnectionListener Thread + 0] WorldManager.ProcessPacket()->Session.ProcessPacket()->NetworkSession.ProcessPacket()->DoRequestForRetransmission()<para />
        /// [ConnectionListener Thread + 1] WorldManager.ProcessPacket()->Session.ProcessPacket()->NetworkSession.ProcessPacket()-> ... AuthenticationHandler<para />
        /// [World Manager Thread] WorldManager.UpdateWorld()->Session.Update(lastTick)->This.Update(lastTick)<para />
        /// </summary>
        private readonly ConcurrentQueue<ServerPacket> packetQueue = new ConcurrentQueue<ServerPacket>();

        public readonly SessionConnectionData ConnectionData = new SessionConnectionData();

        /// <summary>
        /// Stores the tick value for the when an active session will timeout. If this value is in the past, the session is dead/inactive.
        /// </summary>
        public long TimeoutTick { get; set; }

        public ushort ClientId { get; }
        public ushort ServerId { get; }

        public NetworkSession(Session session, ConnectionListener connectionListener, ushort clientId, ushort serverId)
        {
            this.session = session;
            this.connectionListener = connectionListener;

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
            if (isReleased) // Session has been removed
                return;

            foreach (var message in messages)
            {
                var grp = message.Group;
                var currentBundleLock = currentBundleLocks[(int) grp];
                lock (currentBundleLock)
                {
                    var currentBundle = currentBundles[(int) grp];
                    currentBundle.EncryptedChecksum = true;
                    packetLog.DebugFormat("[{0}] Enqueuing Message {1}", session.LoggingIdentifier, message.Opcode);
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
            if (isReleased) // Session has been removed
                return;

            foreach (var packet in packets)
            {
                packetLog.DebugFormat("[{0}] Enqueuing Packet {1}", session.LoggingIdentifier, packet.GetHashCode());
                packetQueue.Enqueue(packet);
            }
        }

        /// <summary>
        /// Prunes the cachedPackets dictionary
        /// Checks if we should send the current bundle and then flushes all pending packets.
        /// </summary>
        public void Update()
        {
            if (isReleased) // Session has been removed
                return;

            if (DateTime.UtcNow - lastCachedPacketPruneTime > cachedPacketPruneInterval)
                PruneCachedPackets();

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

        private void PruneCachedPackets()
        {
            lastCachedPacketPruneTime = DateTime.UtcNow;

            var currentTime = (ushort)Timers.PortalYearTicks;

            // Make sure our comparison still works when ushort wraps every 18.2 hours.
            var removalList = cachedPackets.Values.Where(x => (currentTime >= x.Header.Time ? currentTime : currentTime + ushort.MaxValue) - x.Header.Time > cachedPacketRetentionTime);

            foreach (var packet in removalList)
                cachedPackets.TryRemove(packet.Header.Sequence, out _);
        }

        // This is called from ConnectionListener.OnDataReceieve()->Session.ProcessPacket()->This
        /// <summary>
        /// Processes and incoming packet from a client.
        /// </summary>
        /// <param name="packet">The ClientPacket to process.</param>
        public void ProcessPacket(ClientPacket packet)
        {
            if (isReleased) // Session has been removed
                return;

            packetLog.DebugFormat("[{0}] Processing packet {1}", session.LoggingIdentifier, packet.Header.Sequence);
            NetworkStatistics.C2S_Packets_Aggregate_Increment();

            if (!packet.VerifyCRC(ConnectionData.CryptoClient))
            {
                return;
            }

            // If the client sent a NAK with a cleartext CRC then process it
            if ((packet.Header.Flags & PacketHeaderFlags.RequestRetransmit) == PacketHeaderFlags.RequestRetransmit
                && !((packet.Header.Flags & PacketHeaderFlags.EncryptedChecksum) == PacketHeaderFlags.EncryptedChecksum))
            {
                List<uint> uncached = null;

                foreach (uint sequence in packet.HeaderOptional.RetransmitData)
                {
                    if (!Retransmit(sequence))
                    {
                        if (uncached == null)
                            uncached = new List<uint>();

                        uncached.Add(sequence);
                    }
                }

                if (uncached != null)
                {
                    // Sends a response packet w/ PacketHeader.RejectRetransmit
                    var packetRejectRetransmit = new PacketRejectRetransmit(uncached);
                    EnqueueSend(packetRejectRetransmit);
                }

                NetworkStatistics.C2S_RequestsForRetransmit_Aggregate_Increment();
                return; //cleartext crc NAK is never accompanied by additional data needed by the rest of the pipeline
            }

            #region order-insensitive "half-processing"

            if (packet.Header.HasFlag(PacketHeaderFlags.Disconnect))
            {
                session.Terminate(SessionTerminationReason.PacketHeaderDisconnect);
                return;
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.NetErrorDisconnect))
            {
                session.Terminate(SessionTerminationReason.ClientSentNetworkErrorDisconnect);
                return;
            }

            // depending on the current session state:
            // Set the next timeout tick value, to compare against in the WorldManager
            // Sessions that have gone past the AuthLoginRequest step will stay active for a longer period of time (exposed via configuration) 
            // Sessions that in the AuthLoginRequest will have a short timeout, as set in the AuthenticationHandler.DefaultAuthTimeout.
            // Example: Applications that check uptime will stay in the AuthLoginRequest state.
            session.Network.TimeoutTick = (session.State == SessionState.AuthLoginRequest) ?
                DateTime.UtcNow.AddSeconds(AuthenticationHandler.DefaultAuthTimeout).Ticks : // Default is 15s
                DateTime.UtcNow.AddSeconds(NetworkManager.DefaultSessionTimeout).Ticks; // Default is 60s

            #endregion

            #region Reordering stage

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

                if (desiredSeq + 2 <= packet.Header.Sequence && DateTime.UtcNow - LastRequestForRetransmitTime > new TimeSpan(0, 0, 1))
                    DoRequestForRetransmission(packet.Header.Sequence);

                return;
            }

            #endregion

            #region Final processing stage

            // Processing stage
            // If we reach here, this is a packet we should proceed with processing.
            HandleOrderedPacket(packet);
        
            // Process data now in sequence
            // Finally check if we have any out of order packets or fragments we need to process;
            CheckOutOfOrderPackets();
            CheckOutOfOrderFragments();

            #endregion
        }

        const uint MaxNumNakSeqIds = 115; //464 + header = 484;  (464 - 4) / 4

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
            if (rcvdSeq < bottom || rcvdSeq - bottom > CryptoSystem.MaximumEffortLevel)
            {
                session.Terminate(SessionTerminationReason.AbnormalSequenceReceived);
                return;
            }
            uint seqIdCount = 1;
            for (uint a = bottom; a < rcvdSeq; a++)
            {
                if (!outOfOrderPackets.ContainsKey(a))
                {
                    needSeq.Add(a);
                    seqIdCount++;
                    if (seqIdCount >= MaxNumNakSeqIds)
                    {
                        break;
                    }
                }
            }

            ServerPacket reqPacket = new ServerPacket();
            byte[] reqData = new byte[4 + (needSeq.Count * 4)];
            MemoryStream msReqData = new MemoryStream(reqData, 0, reqData.Length, true, true);
            msReqData.Write(BitConverter.GetBytes((uint)needSeq.Count), 0, 4);
            needSeq.ForEach(k => msReqData.Write(BitConverter.GetBytes(k), 0, 4));
            reqPacket.Data = msReqData;
            reqPacket.Header.Flags = PacketHeaderFlags.RequestRetransmit;

            EnqueueSend(reqPacket);

            LastRequestForRetransmitTime = DateTime.UtcNow;
            packetLog.DebugFormat("[{0}] Requested retransmit of {1}", session.LoggingIdentifier, needSeq.Select(k => k.ToString()).Aggregate((a, b) => a + ", " + b));
            NetworkStatistics.S2C_RequestsForRetransmit_Aggregate_Increment();
        }

        private DateTime LastRequestForRetransmitTime = DateTime.MinValue;

        /// <summary>
        /// Handles a packet<para />
        /// Packets at this stage are already verified, "half processed", and reordered
        /// </summary>
        /// <param name="packet">ClientPacket to handle</param>
        private void HandleOrderedPacket(ClientPacket packet)
        {
            packetLog.DebugFormat("[{0}] Handling packet {1}", session.LoggingIdentifier, packet.Header.Sequence);

            // If we have an EchoRequest flag, we should flag to respond with an echo response on next send.
            if (packet.Header.HasFlag(PacketHeaderFlags.EchoRequest))
            {
                FlagEcho(packet.HeaderOptional.EchoRequestClientTime);
                VerifyEcho(packet.HeaderOptional.EchoRequestClientTime);
            }

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
                HandleOrderedPacket(packet);
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

        //private List<EchoStamp> EchoStamps = new List<EchoStamp>();

        private static int EchoLogInterval = 5;
        private static int EchoInterval = 10;
        private static float EchoThreshold = 2.0f;
        private static float DiffThreshold = 0.01f;

        private float lastClientTime;
        private DateTime lastServerTime;

        private double lastDiff;
        private int echoDiff;

        private void VerifyEcho(float clientTime)
        {
            if (session.Player == null || session.logOffRequestTime != DateTime.MinValue)
                return;

            var serverTime = DateTime.UtcNow;

            if (lastClientTime == 0)
            {
                lastClientTime = clientTime;
                lastServerTime = serverTime;
                return;
            }

            var serverTimeDiff = serverTime - lastServerTime;
            var clientTimeDiff = clientTime - lastClientTime;

            var diff = Math.Abs(serverTimeDiff.TotalSeconds - clientTimeDiff);

            if (diff > EchoThreshold && diff - lastDiff > DiffThreshold)
            {
                lastDiff = diff;
                echoDiff++;

                if (echoDiff >= EchoLogInterval)
                    log.Warn($"{session.Player.Name} - TimeSync error: {echoDiff} (diff: {diff})");

                if (echoDiff >= EchoInterval)
                {
                    log.Error($"{session.Player.Name} - disconnected for speedhacking");

                    var actionChain = new ActionChain();
                    actionChain.AddAction(session.Player, () =>
                    {
                        //session.Network.EnqueueSend(new GameMessageBootAccount(session));
                        session.Network.EnqueueSend(new GameMessageSystemChat($"TimeSync: client speed error", ChatMessageType.Broadcast));
                        session.LogOffPlayer();

                        echoDiff = 0;
                        lastDiff = 0;
                        lastClientTime = 0;

                    });
                    actionChain.EnqueueChain();
                }
            }
            else if (echoDiff > 0)
            {
                if (echoDiff > EchoLogInterval)
                    log.Warn($"{session.Player.Name} - Diff: {diff}");

                lastDiff = 0;
                echoDiff = 0;
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

            var removalList = cachedPackets.Keys.Where(x => x < sequence);

            foreach (var key in removalList)
                cachedPackets.TryRemove(key, out _);
        }

        private bool Retransmit(uint sequence)
        {
            if (cachedPackets.TryGetValue(sequence, out var cachedPacket))
            {
                packetLog.DebugFormat("[{0}] Retransmit {1}", session.LoggingIdentifier, sequence);

                if (!cachedPacket.Header.HasFlag(PacketHeaderFlags.Retransmission))
                    cachedPacket.Header.Flags |= PacketHeaderFlags.Retransmission;

                SendPacketRaw(cachedPacket);

                return true;
            }

            if (cachedPackets.Count > 0)
            {
                // This is to catch a race condition between .Count and .Min() and .Max()
                try
                {
                    log.Error($"Session {session.Network?.ClientId}\\{session.EndPoint} ({session.Account}:{session.Player?.Name}) retransmit requested packet {sequence} not in cache. Cache range {cachedPackets.Keys.Min()} - {cachedPackets.Keys.Max()}.");
                }
                catch
                {
                    log.Error($"Session {session.Network?.ClientId}\\{session.EndPoint} ({session.Account}:{session.Player?.Name}) retransmit requested packet {sequence} not in cache. Cache is empty. Race condition threw exception.");
                }
            }
            else
                log.Error($"Session {session.Network?.ClientId}\\{session.EndPoint} ({session.Account}:{session.Player?.Name}) retransmit requested packet {sequence} not in cache. Cache is empty.");

            return false;
        }

        private void FlushPackets()
        {
            while (packetQueue.TryDequeue(out var packet))
            {
                packetLog.DebugFormat("[{0}] Flushing packets, count {1}", session.LoggingIdentifier, packetQueue.Count);

                if (packet.Header.HasFlag(PacketHeaderFlags.EncryptedChecksum) && ConnectionData.PacketSequence.CurrentValue == 0)
                    ConnectionData.PacketSequence = new Sequence.UIntSequence(1);

                bool isNak = packet.Header.Flags.HasFlag(PacketHeaderFlags.RequestRetransmit);

                // If we are only ACKing, then we don't seem to have to increment the sequence
                if (packet.Header.Flags == PacketHeaderFlags.AckSequence || isNak)
                    packet.Header.Sequence = ConnectionData.PacketSequence.CurrentValue;
                else
                    packet.Header.Sequence = ConnectionData.PacketSequence.NextValue;
                packet.Header.Id = ServerId;
                packet.Header.Iteration = 0x14;
                packet.Header.Time = (ushort)Timers.PortalYearTicks;

                if (packet.Header.Sequence >= 2u && !isNak)
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
                uint issacXor = ConnectionData.IssacServer.Next();
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
                var socket = connectionListener.Socket;

                packet.CreateReadyToSendPacket(buffer, out var size);

                packetLog.Debug(packet.ToString());

                if (packetLog.IsDebugEnabled)
                {
                    var listenerEndpoint = (System.Net.IPEndPoint)socket.LocalEndPoint;
                    var sb = new StringBuilder();
                    sb.AppendLine(String.Format("[{5}] Sending Packet (Len: {0}) [{1}:{2}=>{3}:{4}]", size, listenerEndpoint.Address, listenerEndpoint.Port, session.EndPoint.Address, session.EndPoint.Port, session.Network.ClientId));
                    sb.AppendLine(buffer.BuildPacketString(0, size));
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

                    session.Terminate(SessionTerminationReason.SendToSocketException, null, null, ex.Message);
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
                            bool fragmentSkipped = false;

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
                            else
                                fragmentSkipped = true;

                            // If message is out of data, set to remove it
                            if (fragment.DataRemaining <= 0)
                                removeList.Add(fragment);

                            // UIQueue messages must go out in order. Otherwise, you might see an NPC's tells in an order that doesn't match their defined emotes.
                            if (fragmentSkipped && group == GameMessageGroup.UIQueue)
                                break;
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
                packet.InitializeDataWriter();
                packet.DataWriter.Write(lastReceivedPacketSequence);
            }

            if (bundle.TimeSync) // 0x1000000
            {
                packetHeader.Flags |= PacketHeaderFlags.TimeSync;
                packetLog.DebugFormat("[{0}] Outgoing TimeSync TS: {1}", session.LoggingIdentifier, Timers.PortalYearTicks);
                packet.InitializeDataWriter();
                packet.DataWriter.Write(Timers.PortalYearTicks);
            }

            if (bundle.ClientTime != -1f) // 0x4000000
            {
                packetHeader.Flags |= PacketHeaderFlags.EchoResponse;
                packetLog.DebugFormat("[{0}] Outgoing EchoResponse: {1}", session.LoggingIdentifier, bundle.ClientTime);
                packet.InitializeDataWriter();
                packet.DataWriter.Write(bundle.ClientTime);
                packet.DataWriter.Write((float)Timers.PortalYearTicks - bundle.ClientTime);
            }
        }


        private bool isReleased;

        /// <summary>
        /// This will empty out arrays, collections and dictionaries, and mark the object as released.
        /// Any further work assigned to this object will be ignored.
        /// </summary>
        public void ReleaseResources()
        {
            isReleased = true;

            for (int i = 0; i < currentBundles.Length; i++)
                currentBundles[i] = null;

            outOfOrderPackets.Clear();
            partialFragments.Clear();
            outOfOrderFragments.Clear();

            cachedPackets.Clear();

            packetQueue.Clear();

            ConnectionData.CryptoClient.ReleaseResources();
        }
    }
}
