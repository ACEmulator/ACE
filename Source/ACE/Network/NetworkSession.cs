using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using log4net;
using ACE.Network.Enum;
using ACE.Network.Packets;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Managers;
using System.Diagnostics;
using System.Net;
using ACE.Entity.Enum;

namespace ACE.Network
{
    public class NetworkSession
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog packetLog = LogManager.GetLogger("Packets");

        private const int minimumTimeBetweenBundles = 5; // 5ms
        private const int timeBetweenTimeSync = 20000; // 20s
        private const int timeBetweenAck = 2000; // 2s

        private readonly Object currentBundleLock = new Object();
        private NetworkBundle currentBundle = new NetworkBundle();

        private ConcurrentDictionary<uint, ClientPacket> outOfOrderPackets = new ConcurrentDictionary<uint, ClientPacket>();
        private ConcurrentDictionary<uint, MessageBuffer> partialFragments = new ConcurrentDictionary<uint, MessageBuffer>();
        private ConcurrentDictionary<uint, ClientMessage> outOfOrderFragments = new ConcurrentDictionary<uint, ClientMessage>();

        private DateTime nextResync = DateTime.UtcNow;
        private DateTime nextAck = DateTime.UtcNow;
        private DateTime nextSend = DateTime.UtcNow;
        private DateTime nextIdleOut = DateTime.MaxValue;
        private bool sendAck = false;
        private bool sendResync = false;
        private uint lastReceivedPacketSequence = 1;
        private uint lastReceivedFragmentSequence = 0;

        private SessionState state = SessionState.AuthConnecting;
        public SessionState State
        {
            get { return state; }
            set
            {
                state = value;
                StateChanged?.Invoke(this, new SessionStateChangedEventArgs(value));
            }
        }

        public IPEndPoint EndPoint { get; }
        public uint AccountId { get; private set; }
        public string AccountName { get; private set; }
        public AccessLevel AccessLevel { get; private set; }
        public event EventHandler<SessionStateChangedEventArgs> StateChanged;

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

        protected NetworkSession(IPEndPoint endpoint, ushort clientId, ushort serverId)
        {
            log.InfoFormat("NetworkSession created with clientId {0} for connection {1}", clientId, endpoint);
            EndPoint = endpoint;
            ClientId = clientId;
            ServerId = serverId;
        }

        protected virtual void ClientMessageReceived(ClientMessage message)
        {
        }

        public void SetAccount(uint accountId, string account, AccessLevel accountAccesslevel)
        {
            log.InfoFormat("Setting session account to {0} with name {1} and access {2}, network client id {3}", accountId, account, accountAccesslevel, ClientId);
            AccountId = accountId;
            AccountName = account;
            AccessLevel = accountAccesslevel;
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
                    currentBundle.Enqueue(message);
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
                packetQueue.Enqueue(packet);
        }

        public void SendCharacterError(CharacterError error)
        {
           EnqueueSend(new GameMessageCharacterError(error));
        }

        // It is assumed that this will only be called from a single thread.WorldManager.UpdateWorld()->Session.Update(lastTick)->This
        /// <summary>
        /// Checks if we should send the current bundle and then flushes all pending packets.
        /// </summary>
        /// <param name="lastTick">Amount of time that has passed for the last cycle.</param>
        public virtual void Update(double lastTick)
        {
            ConnectionData.ServerTime += lastTick;

            if (State == SessionState.Terminated)
                return;

            if (DateTime.Now > nextIdleOut)
            {
                log.WarnFormat("Idle out NetworkSession with clientId {0} for connection {1}", ClientId, EndPoint);
                Terminate(CharacterError.ServerCrash);
            }

            NetworkBundle bundleToSend = null;
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
            log.DebugFormat("[{0}] Processing packet {1}", ClientId, packet.Header.Sequence);

            if (State == SessionState.Terminated)
                return;

            nextIdleOut = DateTime.Now.AddSeconds(30);

            // Check if this packet's sequence is a sequence which we have already processed.
            // There are some exceptions:
            // Sequence 0 as we have several Seq 0 packets during connect.  This also cathes a case where it seems CICMDCommand arrives at any point with 0 sequence value too.
            // If the only header on the packet is AckSequence. It seems AckSequence can come in with the same sequence value sometimes.
            if (packet.Header.Sequence <= lastReceivedPacketSequence && packet.Header.Sequence != 0 &&
                !(packet.Header.Flags == PacketHeaderFlags.AckSequence && packet.Header.Sequence == lastReceivedPacketSequence))
            {
                log.WarnFormat("[{0}] Packet {1} received again", ClientId, packet.Header.Sequence);
                return;
            }

            // Check if this packet's sequence is greater then the next one we should be getting.
            // If true we must store it to replay once we have caught up.
            if (packet.Header.Sequence > lastReceivedPacketSequence + 1)
            {
                log.WarnFormat("[{0}] Packet {1} received out of order", ClientId, packet.Header.Sequence);
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

        private bool CheckState(ClientPacket packet)
        {
            if (packet.Header.HasFlag(PacketHeaderFlags.ConnectResponse) && State != SessionState.AuthConnecting)
                return false;

            if (packet.Header.HasFlag(PacketHeaderFlags.AckSequence | PacketHeaderFlags.TimeSynch | PacketHeaderFlags.EchoRequest | PacketHeaderFlags.Flow) && State == SessionState.AuthConnecting)
                return false;

            return true;
        }

        /// <summary>
        /// Handles a packet, reading the flags and processing all fragments.
        /// </summary>
        /// <param name="packet">ClientPacket to handle</param>
        private void HandlePacket(ClientPacket packet)
        {
            log.DebugFormat("[{0}] Handling packet {1}", ClientId, packet.Header.Sequence);

            if (!CheckState(packet))
                return;

            uint issacXor = packet.Header.HasFlag(PacketHeaderFlags.EncryptedChecksum) ? ConnectionData.IssacClient.GetOffset() : 0;
            if (!packet.VerifyChecksum(issacXor))
            {
                log.WarnFormat("[{0}] Packet {1} has invalid checksum", ClientId, packet.Header.Sequence);
            }

            // If we have an EchoRequest flag, we should flag to respond with an echo response on next send.
            if (packet.Header.HasFlag(PacketHeaderFlags.EchoRequest))
                FlagEcho(packet.HeaderOptional.EchoRequestClientTime);

            // If we have an AcknowledgeSequence flag, we can clear our cached packet buffer up to that sequence.
            if (packet.Header.HasFlag(PacketHeaderFlags.AckSequence))
                AcknowledgeSequence(packet.HeaderOptional.Sequence);

            if (packet.Header.HasFlag(PacketHeaderFlags.TimeSynch))
            {
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

            if (packet.Header.HasFlag(PacketHeaderFlags.Disconnect))
            {
                log.DebugFormat("[{0}] Disconnecting", ClientId);
                State = SessionState.Terminating;
                State = SessionState.Terminated;
            }

            // This should be set on the first packet to the server indicating the client is logging in.
            // This is the start of a three-way handshake between the client and server (LoginRequest, ConnectRequest, ConnectResponse)
            // Note this would be sent to each server a client would connect too (Login and each world).
            // In our current implimenation we handle all roles in this one server.
            if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest))
            {
                HandleLoginRequest(packet);
                return;
            }

            // This should be set on the second packet to the server from the client.
            // This completes the three-way handshake.
            if (packet.Header.HasFlag(PacketHeaderFlags.ConnectResponse))
            {
                sendResync = true;
                HandleConnectResponse(packet);
                return;
            }

            // Process all fragments out of the packet
            foreach (ClientPacketFragment fragment in packet.Fragments)
                ProcessFragment(fragment);

            // Update the last received sequence.
            if (packet.Header.Sequence != 0)
                lastReceivedPacketSequence = packet.Header.Sequence;
        }

        public void Terminate(CharacterError error = CharacterError.Undefined)
        {
            log.DebugFormat("[{0}] Terminating", AccountName);
            State = SessionState.Terminating;
            if (error != CharacterError.Undefined)
                SendCharacterError(error);
            State = SessionState.Terminated;
        }

        public async void HandleLoginRequest(ClientPacket packet)
        {
            PacketInboundLoginRequest loginRequest = new PacketInboundLoginRequest(packet);
            try
            {
                var result = await DatabaseManager.Authentication.GetAccountByName(loginRequest.Account);
                AccountSelectCallback(result);
            }
            catch (IndexOutOfRangeException)
            {
                AccountSelectCallback(null);
            }
        }

        private void AccountSelectCallback(Account account)
        {
            var connectRequest = new PacketOutboundConnectRequest(ConnectionData.ServerTime, 0, ClientId, ISAAC.ServerSeed, ISAAC.ClientSeed);
            EnqueueSend(connectRequest);

            if (account == null)
            {
                Terminate(CharacterError.AccountDoesntExist);
                return;
            }

            if (WorldManager.Find(account.Name) != null)
            {
                Terminate(CharacterError.AccountInUse);
                return;
            }

            /*if (glsTicket != digest)
            {
            }*/

            // TODO: check for account bans

            SetAccount(account.AccountId, account.Name, account.AccessLevel);
        }

        private void HandleConnectResponse(ClientPacket packet)
        {
            PacketInboundConnectResponse connectResponse = new PacketInboundConnectResponse(packet);
            State = SessionState.AuthConnected;
        }

        /// <summary>
        /// Processes a fragment, combining split fragments as needed, then handling them
        /// </summary>
        /// <param name="fragment">ClientPacketFragment to process</param>
        private void ProcessFragment(ClientPacketFragment fragment)
        {
            log.DebugFormat("[{0}] Processing fragment {1}", ClientId, fragment.Header.Sequence);
            ClientMessage message = null;
            // Check if this fragment is split
            if (fragment.Header.Count != 1)
            {
                // Packet is split
                log.DebugFormat("[{0}] Fragment {1} is split, this index {2} of {3} fragments", ClientId, fragment.Header.Sequence, fragment.Header.Index, fragment.Header.Count);

                MessageBuffer buffer = null;
                if (partialFragments.TryGetValue(fragment.Header.Sequence, out buffer))
                {
                    // Existing buffer, add this to it and check if we are finally complete.
                    buffer.AddFragment(fragment);
                    log.DebugFormat("[{0}] Added fragment {1} to existing buffer. Buffer at {2} of {3}", ClientId, fragment.Header.Sequence, buffer.Count, buffer.TotalFragments);
                    if (buffer.Complete)
                    {
                        // The buffer is complete, so we can go ahead and handle
                        log.DebugFormat("[{0}] Buffer {1} is complete", ClientId, buffer.Sequence);
                        message = buffer.GetMessage();
                        MessageBuffer removed = null;
                        partialFragments.TryRemove(fragment.Header.Sequence, out removed);
                    }
                }
                else
                {
                    // No existing buffer, so add a new one for this fragment sequence.
                    log.DebugFormat("[{0}] Creating new buffer {1} for this split fragment", ClientId, fragment.Header.Sequence);
                    var newBuffer = new MessageBuffer(fragment.Header.Sequence, fragment.Header.Count);
                    newBuffer.AddFragment(fragment);

                    log.DebugFormat("[{0}] Added fragment {1} to the new buffer. Buffer at {2} of {3}", ClientId, fragment.Header.Sequence, newBuffer.Count, newBuffer.TotalFragments);
                    partialFragments.TryAdd(fragment.Header.Sequence, newBuffer);
                }
            }
            else
            {
                // Packet is not split, proceed with handling it.
                log.DebugFormat("[{0}] Fragment {1} is not split", ClientId, fragment.Header.Sequence);
                message = new ClientMessage(fragment.Data);
            }

            // If message is not null, we have a complete message to handle
            if (message != null)
            {
                // First check if this message is the next sequence, if it is not, add it to our outOfOrderFragments
                if (fragment.Header.Sequence == lastReceivedFragmentSequence + 1)
                {
                    log.DebugFormat("[{0}] Handling fragment {1}", ClientId, fragment.Header.Sequence);
                    HandleClientMessage(message);
                }
                else
                {
                    log.DebugFormat("[{0}] Fragment {1} is early, lastReceivedFragmentSequence = {2}", ClientId, fragment.Header.Sequence, lastReceivedFragmentSequence);
                    outOfOrderFragments.TryAdd(fragment.Header.Sequence, message);
                }
            }
        }

        /// <summary>
        /// Handles a ClientMessage
        /// </summary>
        /// <param name="message">ClientMessage to process</param>
        private void HandleClientMessage(ClientMessage message)
        {
            ClientMessageReceived(message);
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
                log.DebugFormat("[{0}] Ready to handle out-of-order packet {1}", ClientId, packet.Header.Sequence);
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
                log.DebugFormat("[{0}] Ready to handle out of order fragment {1}", ClientId, lastReceivedFragmentSequence + 1);
                HandleClientMessage(message);
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
                log.DebugFormat("[{0}] Retransmit {1}", ClientId, sequence);

                if (!cachedPacket.Header.HasFlag(PacketHeaderFlags.Retransmission))
                    cachedPacket.Header.Flags |= PacketHeaderFlags.Retransmission;

                SendPacketRaw(cachedPacket);
            }
        }

        private uint GetIssacValue(PacketDirection direction)
        {
            return (direction == PacketDirection.Client ? ConnectionData.IssacClient.GetOffset() : ConnectionData.IssacServer.GetOffset());
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
                uint issacXor = GetIssacValue(PacketDirection.Server);
                packet.IssacXor = issacXor;
            }

            SendPacketRaw(packet);
        }

        private void SendPacketRaw(ServerPacket packet)
        {
            Socket socket;
            if (packet.Header.Sequence == 0)
                socket = WorldManager.GetSocket(0);
            else
                socket = WorldManager.GetSocket();

            byte[] payload = packet.GetPayload();

            if (packetLog.IsDebugEnabled)
            {
                System.Net.IPEndPoint listenerEndpoint = (System.Net.IPEndPoint)socket.LocalEndPoint;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(String.Format("[{5}] Sending Packet (Len: {0}) [{1}:{2}=>{3}:{4}]", payload.Length, listenerEndpoint.Address, listenerEndpoint.Port, EndPoint.Address, EndPoint.Port, ClientId));
                sb.AppendLine(payload.BuildPacketString());
                packetLog.Debug(sb.ToString());
            }
            socket.SendTo(payload, EndPoint);
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

            while (firstPacket || carryOverMessage != null || bundle.HasMoreMessages)
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
                        packet.BodyWriter.Write(lastReceivedPacketSequence);
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

                if (carryOverMessage != null || bundle.HasMoreMessages)
                {
                    packetHeader.Flags |= PacketHeaderFlags.BlobFragments;
                    int fragmentCount = 0;
                    while (carryOverMessage != null || bundle.HasMoreMessages)
                    {
                        MessageFragment currentMessageFragment = null;

                        if (carryOverMessage != null) // If we have a carryOverMessage, use that
                        {
                            currentMessageFragment = carryOverMessage;
                            carryOverMessage = null;
                        }
                        else // If we don't have a carryOverMessage, go ahead and dequeue next message from the bundle
                        {
                            currentMessageFragment = new MessageFragment(bundle.Dequeue());
                        }

                        var currentGameMessage = currentMessageFragment.Message;

                        availableSpace -= PacketFragmentHeader.HeaderSize; // Account for fragment header

                        // Compute amount of data to send based on the total length and current position
                        uint dataToSend = (uint)currentGameMessage.Data.Length - currentMessageFragment.Position;

                        if (dataToSend > availableSpace) // Message is too large to fit in packet
                        {
                            carryOverMessage = currentMessageFragment;
                            if (fragmentCount == 0) // If this is first message in packet, this is just a really large message, so proceed with splitting it
                                dataToSend = availableSpace;
                            else // Otherwise there are other messages already, so we'll break and come back and see if the message will fit
                                break;
                        }

                        if (currentMessageFragment.Count == 0) // Compute number of fragments if we have not already
                        {
                            uint remainingData = (uint)currentGameMessage.Data.Length - dataToSend;
                            currentMessageFragment.Count = (ushort)(Math.Ceiling((double)remainingData / PacketFragment.MaxFragmentDataSize) + 1);
                        }

                        // Set sequence, if new, pull next sequence from ConnectionData, if it is a carryOver, reuse that sequence
                        currentMessageFragment.Sequence = currentMessageFragment.Sequence == 0 ? ConnectionData.FragmentSequence++ : currentMessageFragment.Sequence;

                        // Read data starting at current position reading dataToSend bytes
                        currentGameMessage.Data.Seek(currentMessageFragment.Position, SeekOrigin.Begin);
                        byte[] data = new byte[dataToSend];
                        currentGameMessage.Data.Read(data, 0, (int)dataToSend);

                        // Build ServerPacketFragment structure
                        ServerPacketFragment fragment = new ServerPacketFragment(data);
                        fragment.Header.Sequence = currentMessageFragment.Sequence;
                        fragment.Header.Id = 0x80000000;
                        fragment.Header.Count = currentMessageFragment.Count;
                        fragment.Header.Index = currentMessageFragment.Index;
                        fragment.Header.Group = (ushort)currentMessageFragment.Message.Group;

                        // Increment position and index
                        currentMessageFragment.Position = currentMessageFragment.Position + dataToSend;
                        currentMessageFragment.Index++;

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

        public class NetworkSessionStateChangedEventArgs : EventArgs
        {
            public NetworkSessionState NewState { get; }

            public NetworkSessionStateChangedEventArgs(NetworkSessionState newState)
            {
                NewState = newState;
            }
        }

        public class SessionStateChangedEventArgs : EventArgs
        {
            public SessionState NewState { get; }

            public SessionStateChangedEventArgs(SessionState newState)
            {
                NewState = newState;
            }
        }

        public class ClientMessageReceivedEventArgs : EventArgs
        {
            public ClientMessage Message { get; }

            public ClientMessageReceivedEventArgs(ClientMessage message)
            {
                Message = message;
            }
        }

        private class MessageFragment
        {
            public GameMessage Message { get; private set; }

            public uint Position { get; set; }

            public uint Sequence { get; set; }

            public ushort Index { get; set; }

            public ushort Count { get; set; }

            public MessageFragment(GameMessage message)
            {
                this.Message = message;
                Index = 0;
                Count = 0;
                Position = 0;
                Sequence = 0;
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
