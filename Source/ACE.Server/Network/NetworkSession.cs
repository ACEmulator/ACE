using ACE.Common;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Handlers;
using ACE.Server.Network.Managers;
using ACE.Server.WorldObjects;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace ACE.Server.Network
{
    public class Session
    {
        private const int minimumTimeBetweenBundles = 5; // 5ms 
        private const int timeBetweenTimeSync = 20000; // 20s
        private const int timeBetweenAck = 2000; // 2s
        private readonly object[] currentBundleLocks = new object[(int)GameMessageGroup.QueueMax];
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
        private readonly bool sendAck = true;
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
        private readonly ConcurrentQueue<ServerPacket> packetQueue = new ConcurrentQueue<ServerPacket>();
        public readonly SessionConnectionData ConnectionData = new SessionConnectionData();
        private readonly OutboundPacketQueue OutboundQueue = null;
        /// <summary>
        /// Stores the tick value for the when an active session will timeout. If this value is in the past, the session is dead/inactive.
        /// </summary>
        public long TimeoutTick { get; set; }
        public ushort ClientId { get; }
        public ushort ServerId { get; }
        public IPEndPoint EndPoint { get; }
        public uint GameEventSequence { get; set; }
        public SessionState State { get; set; }
        public uint AccountId { get; private set; }
        public string Account { get; private set; }
        public string LoggingIdentifier { get; private set; } = "Unverified";
        public AccessLevel AccessLevel { get; private set; }
        public List<Character> Characters { get; } = new List<Character>();
        public Player Player { get; private set; }
        private DateTime logOffRequestTime;
        public SessionTerminationDetails PendingTermination { get; set; } = null;

        public Session(IPEndPoint endPoint, ushort clientId, ushort serverId)
        {
            EndPoint = endPoint;
            ClientId = clientId;
            ServerId = serverId;
            OutboundQueue = NetworkManager.OutboundQueue;
            // New network auth session timeouts will always be low.
            TimeoutTick = DateTime.UtcNow.AddSeconds(AuthenticationHandler.DefaultAuthTimeout).Ticks;
            for (int i = 0; i < currentBundles.Length; i++)
            {
                currentBundleLocks[i] = new object();
                currentBundles[i] = new NetworkBundle();
            }
            ConnectionData.CryptoClient.OnCryptoSystemCatastrophicFailure += (sender, e) =>
            {
                Terminate(SessionTerminationReason.ClientConnectionFailure);
            };
        }

        private bool CheckState(ClientPacket packet)
        {
            if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest) && State != SessionState.AuthLoginRequest)
            {
                return false;
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.ConnectResponse) && State != SessionState.AuthConnectResponse)
            {
                return false;
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.AckSequence | PacketHeaderFlags.TimeSync | PacketHeaderFlags.EchoRequest | PacketHeaderFlags.Flow) && State == SessionState.AuthLoginRequest)
            {
                return false;
            }

            return true;
        }
        public void ProcessSessionPacket(ClientPacket packet)
        {
            if (!CheckState(packet))
            {
                return;
            }

            ProcessPacket(packet);
        }
        /// <summary>
        /// This will send outgoing packets as well as the final logoff message.
        /// </summary>
        public void TickOutbound()
        {
            // Check if the player has been booted
            if (PendingTermination != null)
            {
                if (PendingTermination.TerminationStatus == SessionTerminationPhase.Initialized)
                {
                    State = SessionState.TerminationStarted;
                    Update(); // boot messages may need sending
                    PendingTermination.TerminationStatus = SessionTerminationPhase.SessionWorkCompleted;
                }
                return;
            }
            if (State == SessionState.TerminationStarted)
            {
                return;
            }
            // Checks if the session has stopped responding.
            if (DateTime.UtcNow.Ticks >= TimeoutTick)
            {
                // The Session has reached a timeout.  Send the client the error disconnect signal, and then drop the session
                Terminate(SessionTerminationReason.NetworkTimeout);
                return;
            }
            Update();
            // Live server seemed to take about 6 seconds. 4 seconds is nice because it has smooth animation, and saves the user 2 seconds every logoff
            // This could be made 0 for instant logoffs.
            if (logOffRequestTime != DateTime.MinValue && logOffRequestTime.AddSeconds(6) <= DateTime.UtcNow)
            {
                SendFinalLogOffMessages();
            }
        }
        public void SetAccount(uint accountId, string account, AccessLevel accountAccesslevel)
        {
            AccountId = accountId;
            Account = account;
            AccessLevel = accountAccesslevel;
        }
        public void UpdateCharacters(IEnumerable<Character> characters)
        {
            Characters.Clear();
            Characters.AddRange(characters);
            CheckCharactersForDeletion();
        }
        public void CheckCharactersForDeletion()
        {
            for (int i = Characters.Count - 1; i >= 0; i--)
            {
                if (Characters[i].DeleteTime > 0 && Time.GetUnixTime() > Characters[i].DeleteTime)
                {
                    Characters[i].IsDeleted = true;
                    DatabaseManager.Shard.SaveCharacter(Characters[i], new ReaderWriterLockSlim(), null);
                    PlayerManager.ProcessDeletedPlayer(Characters[i].Id);
                    Characters.RemoveAt(i);
                }
            }
        }
        public void InitSessionForWorldLogin()
        {
            GameEventSequence = 1;
        }
        public void SetAccessLevel(AccessLevel accountAccesslevel)
        {
            AccessLevel = accountAccesslevel;
        }
        public void SetPlayer(Player player)
        {
            Player = player;
        }
        /// <summary>
        /// Log off the player normally
        /// </summary>
        public void LogOffPlayer()
        {
            if (logOffRequestTime == DateTime.MinValue)
            {
                Player.LogOut();
                logOffRequestTime = DateTime.UtcNow;
            }
        }
        private void SendFinalLogOffMessages()
        {
            // If we still exist on a landblock, we can't exit yet.
            if (Player.CurrentLandblock != null)
            {
                return;
            }

            logOffRequestTime = DateTime.MinValue;
            // It's possible for a character change to happen from a GameActionSetCharacterOptions message.
            // This message can be received/processed by the server AFTER LogOfPlayer has been called.
            // What that means is, we could end up with Character changes after the Character has been saved from the initial LogOff request.
            // To make sure we commit these additional changes (if any), we check again here
            if (Player.CharacterChangesDetected)
            {
                Player.SaveCharacterToDatabase();
            }

            Player = null;
            EnqueueSend(new GameMessageCharacterLogOff());
            CheckCharactersForDeletion();
            EnqueueSend(new GameMessageCharacterList(Characters, this));
            GameMessageServerName serverNameMessage = new GameMessageServerName(ConfigManager.Config.Server.WorldName, PlayerManager.GetAllOnline().Count, (int)ConfigManager.Config.Server.Network.MaximumAllowedSessions);
            EnqueueSend(serverNameMessage);
            State = SessionState.AuthConnected;
        }
        public void Terminate(SessionTerminationReason reason, GameMessage message = null, ServerPacket packet = null, string extraReason = "")
        {
            if (packet != null)
            {
                EnqueueSend(packet);
            }
            if (message != null)
            {
                EnqueueSend(message);
            }
            PendingTermination = new SessionTerminationDetails()
            {
                ExtraReason = extraReason,
                Reason = reason
            };
        }
        public void DropSession()
        {
            if (PendingTermination == null || PendingTermination.TerminationStatus != SessionTerminationPhase.SessionWorkCompleted)
            {
                return;
            }

            if (PendingTermination.Reason != SessionTerminationReason.PongSentClosingConnection)
            {
                SessionTerminationReason reason = PendingTermination.Reason;
                string reas = (reason != SessionTerminationReason.None) ? $", Reason: {reason.GetDescription()}" : "";
                if (PendingTermination.ExtraReason != null)
                {
                    reas = reas + ", " + PendingTermination.ExtraReason;
                }
            }
            if (Player != null)
            {
                LogOffPlayer();
            }
            NetworkManager.RemoveSession(this);
        }
        public void SendCharacterError(CharacterError error)
        {
            EnqueueSend(new GameMessageCharacterError(error));
        }
        public void WorldBroadcast(string broadcastMessage)
        {
            GameMessageSystemChat worldBroadcastMessage = new GameMessageSystemChat(broadcastMessage, ChatMessageType.Broadcast);
            EnqueueSend(worldBroadcastMessage);
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
                GameMessageGroup grp = k.First().Group;
                object currentBundleLock = currentBundleLocks[(int)grp];
                lock (currentBundleLock)
                {
                    NetworkBundle currentBundle = currentBundles[(int)grp];
                    foreach (GameMessage msg in k)
                    {
                        currentBundle.EncryptedChecksum = true;
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
            foreach (ServerPacket packet in packets)
            {
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
                GameMessageGroup group = (GameMessageGroup)i;
                object currentBundleLock = currentBundleLocks[i];
                lock (currentBundleLock)
                {
                    NetworkBundle currentBundle = currentBundles[i];
                    if (group == GameMessageGroup.InvalidQueue)
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
                        if (currentBundle.NeedsSending && DateTime.UtcNow >= nextSend)
                        {
                            // Swap out bundle so we can process it
                            bundleToSend = currentBundle;
                            currentBundles[i] = new NetworkBundle();
                        }
                    }
                    else
                    {
                        if (currentBundle.NeedsSending && DateTime.UtcNow >= nextSend)
                        {
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
            try
            {
                FlushPackets();
            }
            catch (Exception)
            {
            }
        }
        // This is called from ConnectionListener.OnDataReceieve()->Session.ProcessPacket()->This
        /// <summary>
        /// Processes and incoming packet from a client.
        /// </summary>
        /// <param name="packet">The ClientPacket to process.</param>
        public void ProcessPacket(ClientPacket packet)
        {
            NetworkStatistics.C2S_Packets_Aggregate_Increment();
            #region order-insensitive "half-processing"
            if (packet.Header.HasFlag(PacketHeaderFlags.Disconnect))
            {
                Terminate(SessionTerminationReason.PacketHeaderDisconnect);
                return;
            }
            if (packet.Header.HasFlag(PacketHeaderFlags.NetErrorDisconnect))
            {
                Terminate(SessionTerminationReason.ClientSentNetworkErrorDisconnect);
                return;
            }
            // If the client is requesting a retransmission process it immediately
            if (packet.Header.HasFlag(PacketHeaderFlags.RequestRetransmit))
            {
                foreach (uint sequence in packet.HeaderOptional.RetransmitData)
                {
                    Retransmit(sequence);
                }
                NetworkStatistics.C2S_RequestsForRetransmit_Aggregate_Increment();
            }
            // depending on the current session state:
            // Set the next timeout tick value, to compare against in the WorldManager
            // Sessions that have gone past the AuthLoginRequest step will stay active for a longer period of time (exposed via configuration) 
            // Sessions that in the AuthLoginRequest will have a short timeout, as set in the AuthenticationHandler.DefaultAuthTimeout.
            // Example: Applications that check uptime will stay in the AuthLoginRequest state.
            TimeoutTick = (State == SessionState.AuthLoginRequest) ?
                DateTime.UtcNow.AddSeconds(NetworkManager.DefaultSessionTimeout).Ticks :
                DateTime.UtcNow.AddSeconds(AuthenticationHandler.DefaultAuthTimeout).Ticks;
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
                return;
            }
            // Check if this packet's sequence is greater then the next one we should be getting.
            // If true we must store it to replay once we have caught up.
            uint desiredSeq = lastReceivedPacketSequence + 1;
            if (packet.Header.Sequence > desiredSeq)
            {
                if (!outOfOrderPackets.ContainsKey(packet.Header.Sequence))
                {
                    outOfOrderPackets.TryAdd(packet.Header.Sequence, packet);
                }
                if (desiredSeq + 2 <= packet.Header.Sequence && DateTime.Now - LastRequestForRetransmitTime > new TimeSpan(0, 0, 1))
                {
                    DoRequestForRetransmission(packet.Header.Sequence);
                }
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
        /// <summary>
        /// request retransmission of lost sequences
        /// </summary>
        /// <param name="rcvdSeq">the sequence of the packet that was just received.</param>
        private void DoRequestForRetransmission(uint rcvdSeq)
        {
            uint desiredSeq = lastReceivedPacketSequence + 1;
            List<uint> needSeq = new List<uint>
            {
                desiredSeq
            };
            uint bottom = desiredSeq + 1;
            for (uint a = bottom; a < rcvdSeq; a++)
            {
                if (!outOfOrderPackets.ContainsKey(a))
                {
                    needSeq.Add(a);
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
            LastRequestForRetransmitTime = DateTime.Now;
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
            // If we have an EchoRequest flag, we should flag to respond with an echo response on next send.
            if (packet.Header.HasFlag(PacketHeaderFlags.EchoRequest))
            {
                FlagEcho(packet.HeaderOptional.EchoRequestClientTime);
            }
            // If we have an AcknowledgeSequence flag, we can clear our cached packet buffer up to that sequence.
            if (packet.Header.HasFlag(PacketHeaderFlags.AckSequence))
            {
                AcknowledgeSequence(packet.HeaderOptional.AckSequence);
            }
            if (packet.Header.HasFlag(PacketHeaderFlags.TimeSync))
            {
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
                AuthenticationHandler.HandleLoginRequest(packet, this);
                return;
            }
            // Process all fragments out of the packet
            foreach (ClientPacketFragment fragment in packet.Fragments)
            {
                ProcessFragment(fragment);
            }
            // Update the last received sequence.
            if (packet.Header.Sequence != 0 && packet.Header.Flags != PacketHeaderFlags.AckSequence)
            {
                lastReceivedPacketSequence = packet.Header.Sequence;
            }
        }
        /// <summary>
        /// Processes a fragment, combining split fragments as needed, then handling them
        /// </summary>
        /// <param name="fragment">ClientPacketFragment to process</param>
        private void ProcessFragment(ClientPacketFragment fragment)
        {
            ClientMessage message = null;
            // Check if this fragment is split
            if (fragment.Header.Count != 1)
            {
                // Packet is split
                if (partialFragments.TryGetValue(fragment.Header.Sequence, out MessageBuffer buffer))
                {
                    // Existing buffer, add this to it and check if we are finally complete.
                    buffer.AddFragment(fragment);
                    if (buffer.Complete)
                    {
                        // The buffer is complete, so we can go ahead and handle
                        message = buffer.GetMessage();
                        partialFragments.TryRemove(fragment.Header.Sequence, out MessageBuffer removed);
                    }
                }
                else
                {
                    // No existing buffer, so add a new one for this fragment sequence.
                    MessageBuffer newBuffer = new MessageBuffer(fragment.Header.Sequence, fragment.Header.Count);
                    newBuffer.AddFragment(fragment);
                    partialFragments.TryAdd(fragment.Header.Sequence, newBuffer);
                }
            }
            else
            {
                // Packet is not split, proceed with handling it.
                message = new ClientMessage(fragment.Data);
            }
            // If message is not null, we have a complete message to handle
            if (message != null)
            {
                // First check if this message is the next sequence, if it is not, add it to our outOfOrderFragments
                if (fragment.Header.Sequence == lastReceivedFragmentSequence + 1)
                {
                    HandleFragment(message);
                }
                else
                {
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
            InboundMessageManager.HandleClientMessage(message, this);
            lastReceivedFragmentSequence++;
        }
        /// <summary>
        /// Checks if we now have packets queued out of order which should be processed as the next sequence.
        /// </summary>
        private void CheckOutOfOrderPackets()
        {
            while (outOfOrderPackets.TryRemove(lastReceivedPacketSequence + 1, out ClientPacket packet))
            {
                HandleOrderedPacket(packet);
            }
        }
        /// <summary>
        /// Checks if we now have fragments queued out of order which should be handled as the next sequence.
        /// </summary>
        private void CheckOutOfOrderFragments()
        {
            while (outOfOrderFragments.TryRemove(lastReceivedFragmentSequence + 1, out ClientMessage message))
            {
                HandleFragment(message);
            }
        }
        //is this special channel
        private void FlagEcho(float clientTime)
        {
            object currentBundleLock = currentBundleLocks[(int)GameMessageGroup.InvalidQueue];
            lock (currentBundleLock)
            {
                NetworkBundle currentBundle = currentBundles[(int)GameMessageGroup.InvalidQueue];
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
            IEnumerable<KeyValuePair<uint, ServerPacket>> removalList = cachedPackets.Where(x => x.Key < sequence);
            foreach (KeyValuePair<uint, ServerPacket> item in removalList)
            {
                cachedPackets.TryRemove(item.Key, out ServerPacket removedPacket);
                if (removedPacket.Data != null)
                {
                    removedPacket.Data.Dispose();
                }
            }
        }
        private void Retransmit(uint sequence)
        {
            if (cachedPackets.TryGetValue(sequence, out ServerPacket cachedPacket))
            {
                if (!cachedPacket.Header.HasFlag(PacketHeaderFlags.Retransmission))
                {
                    cachedPacket.Header.Flags |= PacketHeaderFlags.Retransmission;
                }
                SendPacketRaw(cachedPacket);
            }
            else
            {
            }
        }
        private void FlushPackets()
        {
            ServerPacket packet = null;
            while (packetQueue.TryDequeue(out packet))
            {
                if (packet.Header.HasFlag(PacketHeaderFlags.EncryptedChecksum) && ConnectionData.PacketSequence.CurrentValue == 0)
                {
                    ConnectionData.PacketSequence = new Sequence.UIntSequence(1);
                }
                // If we are only ACKing, then we don't seem to have to increment the sequence
                if (packet.Header.Flags == PacketHeaderFlags.AckSequence || packet.Header.Flags.HasFlag(PacketHeaderFlags.RequestRetransmit))
                {
                    packet.Header.Sequence = ConnectionData.PacketSequence.CurrentValue;
                }
                else
                {
                    packet.Header.Sequence = ConnectionData.PacketSequence.NextValue;
                }
                packet.Header.Id = ServerId;
                packet.Header.Iteration = 0x14;
                packet.Header.Time = (ushort)ConnectionData.ServerTime;
                if (packet.Header.Sequence >= 2u)
                {
                    cachedPackets.TryAdd(packet.Header.Sequence, packet);
                }
                SendPacket(packet);
            }
        }
        private void SendPacket(ServerPacket packet)
        {
            NetworkStatistics.S2C_Packets_Aggregate_Increment();
            if (packet.Header.HasFlag(PacketHeaderFlags.EncryptedChecksum))
            {
                uint issacXor = ConnectionData.IssacServer.GetOffset();
                packet.IssacXor = issacXor;
            }
            SendPacketRaw(packet);
        }
        private void SendPacketRaw(ServerPacket packet)
        {
            byte[] buffer = ArrayPool<byte>.Shared.Rent((int)(PacketHeader.HeaderSize + (packet.Data?.Length ?? 0) + (packet.Fragments.Count * PacketFragment.MaxFragementSize)));
            try
            {
                packet.CreateReadyToSendPacket(buffer, out int size);
                byte[] data = new byte[size];
                Buffer.BlockCopy(buffer, 0, data, 0, size);
                data = NetworkSyntheticTesting.SyntheticCorruption_S2C(data);
                OutboundQueue.Enqueue(new OutboundPacketQueue.RawOutboundPacket() { Packet = data, Session = this, Them = EndPoint });
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
            bool writeOptionalHeaders = true;
            List<MessageFragment> fragments = new List<MessageFragment>();
            // Pull all messages out and create MessageFragment objects
            while (bundle.HasMoreMessages)
            {
                GameMessage message = bundle.Dequeue();
                MessageFragment fragment = new MessageFragment(message, ConnectionData.FragmentSequence++);
                fragments.Add(fragment);
            }
            // Loop through while we have fragements
            while (fragments.Count > 0 || writeOptionalHeaders)
            {
                ServerPacket packet = new ServerPacket();
                PacketHeader packetHeader = packet.Header;
                if (fragments.Count > 0)
                {
                    packetHeader.Flags |= PacketHeaderFlags.BlobFragments;
                }
                if (bundle.EncryptedChecksum)
                {
                    packetHeader.Flags |= PacketHeaderFlags.EncryptedChecksum;
                }
                int availableSpace = ServerPacket.MaxPacketSize;
                // Pull first message and see if it is a large one
                MessageFragment firstMessage = fragments.FirstOrDefault();
                if (firstMessage != null)
                {
                    // If a large message send only this one, filling the whole packet
                    if (firstMessage.DataRemaining >= availableSpace)
                    {
                        ServerPacketFragment spf = firstMessage.GetNextFragment();
                        packet.Fragments.Add(spf);
                        availableSpace -= spf.Length;
                        if (firstMessage.DataRemaining <= 0)
                        {
                            fragments.Remove(firstMessage);
                        }
                    }
                    // Otherwise we'll write any optional headers and process any small messages that will fit
                    else
                    {
                        if (writeOptionalHeaders)
                        {
                            writeOptionalHeaders = false;
                            WriteOptionalHeaders(bundle, packet);
                            if (packet.Data != null)
                            {
                                availableSpace -= (int)packet.Data.Length;
                            }
                        }
                        // Create a list to remove completed messages after iterator
                        List<MessageFragment> removeList = new List<MessageFragment>();
                        foreach (MessageFragment fragment in fragments)
                        {
                            // Is this a large fragment and does it have a tail that needs sending?
                            if (!fragment.TailSent && availableSpace >= fragment.TailSize)
                            {
                                ServerPacketFragment spf = fragment.GetTailFragment();
                                packet.Fragments.Add(spf);
                                availableSpace -= spf.Length;
                            }
                            // Otherwise will this message fit in the remaining space?
                            else if (availableSpace >= fragment.NextSize)
                            {
                                ServerPacketFragment spf = fragment.GetNextFragment();
                                packet.Fragments.Add(spf);
                                availableSpace -= spf.Length;
                            }
                            // If message is out of data, set to remove it
                            if (fragment.DataRemaining <= 0)
                            {
                                removeList.Add(fragment);
                            }
                        }
                        // Remove all completed messages
                        fragments.RemoveAll(x => removeList.Contains(x));
                    }
                }
                // If no messages, write optional headers
                else
                {
                    if (writeOptionalHeaders)
                    {
                        writeOptionalHeaders = false;
                        WriteOptionalHeaders(bundle, packet);
                        if (packet.Data != null)
                        {
                            availableSpace -= (int)packet.Data.Length;
                        }
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
                packet.InitializeBodyWriter();
                packet.BodyWriter.Write(lastReceivedPacketSequence);
            }
            if (bundle.TimeSync) // 0x1000000
            {
                packetHeader.Flags |= PacketHeaderFlags.TimeSync;
                packet.InitializeBodyWriter();
                packet.BodyWriter.Write(ConnectionData.ServerTime);
            }
            if (bundle.ClientTime != -1f) // 0x4000000
            {
                packetHeader.Flags |= PacketHeaderFlags.EchoResponse;
                packet.InitializeBodyWriter();
                packet.BodyWriter.Write(bundle.ClientTime);
                packet.BodyWriter.Write((float)ConnectionData.ServerTime - bundle.ClientTime);
            }
        }
    }
}
