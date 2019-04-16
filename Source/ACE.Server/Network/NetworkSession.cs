using ACE.Common;
using ACE.Common.Cryptography;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Handlers;
using ACE.Server.Network.Managers;
using ACE.Server.Network.Sequence;
using ACE.Server.WorldObjects;
using log4net;
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
        #region Members
        private const int minimumTimeBetweenBundles = 5;
        private const int timeBetweenTimeSync = 20000;
        private const int timeBetweenAck = 2000;

        public bool sendResync;
        private readonly bool sendAck = true;

        public ushort ClientId { get; private set; }
        private ushort ServerId { get; }

        public uint GameEventSequence { get; set; }
        public uint AccountId { get; private set; }
        private uint lastReceivedPacketSequence = 1;
        private uint lastReceivedFragmentSequence;
        private uint FragmentSequence { get; set; }
        private readonly uint DefaultSessionTimeout = ConfigManager.Config.Server.Network.DefaultSessionTimeout;

        public ulong ConnectionCookie { get; private set; }
        private long TimeoutTick { get; set; }

        public double ServerTime => Timers.PortalYearTicks;

        private DateTime nextSend = DateTime.UtcNow;
        private DateTime nextResync = DateTime.UtcNow;
        private DateTime nextAck = DateTime.UtcNow.AddMilliseconds(timeBetweenAck);
        private DateTime logOffRequestTime;
        private DateTime LatestTimeOf_S2C_NAK = DateTime.MinValue;

        public string Account { get; private set; }

        public SessionState State { get; set; }
        public AccessLevel AccessLevel { get; private set; }

        public IPEndPoint EndPoint { get; }
        public SessionTerminationDetails PendingTermination { get; private set; } = null;
        private UIntSequence PacketSequence { get; set; }
        public readonly CryptoSystem CryptoClient = null;
        private readonly ISAAC IssacServer = null;

        public Player Player { get; private set; }
        public List<Character> Characters { get; private set; } = new List<Character>();
        public byte[] ClientSeed { get; private set; }
        public byte[] ServerSeed { get; private set; }

        private readonly object[] currentBundleLocks = new object[(int)GameMessageGroup.QueueMax];
        private readonly NetworkBundle[] currentBundles = new NetworkBundle[(int)GameMessageGroup.QueueMax];
        private readonly ConcurrentDictionary<uint, ClientPacket> outOfOrderPackets = new ConcurrentDictionary<uint, ClientPacket>();
        private readonly ConcurrentDictionary<uint, MessageBuffer> partialFragments = new ConcurrentDictionary<uint, MessageBuffer>();
        private readonly ConcurrentDictionary<uint, ClientMessage> outOfOrderFragments = new ConcurrentDictionary<uint, ClientMessage>();
        private readonly ConcurrentDictionary<uint, ServerPacket> cachedPackets = new ConcurrentDictionary<uint, ServerPacket>();
        private readonly ConcurrentQueue<ServerPacket> packetQueue = new ConcurrentQueue<ServerPacket>();
        private readonly OutboundPacketQueue OutboundQueue = null;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        public Session(IPEndPoint endPoint, ushort clientId, ushort serverId)
        {
            EndPoint = endPoint;
            ClientId = clientId;
            ServerId = serverId;
            OutboundQueue = NetworkManager.OutboundQueue;
            TimeoutTick = DateTime.UtcNow.AddSeconds(AuthenticationHandler.DefaultAuthTimeout).Ticks;
            for (int i = 0; i < currentBundles.Length; i++)
            {
                currentBundleLocks[i] = new object();
                currentBundles[i] = new NetworkBundle();
            }
            Random rand = new Random();
            ClientSeed = new byte[4];
            ServerSeed = new byte[4];
            rand.NextBytes(ClientSeed);
            rand.NextBytes(ServerSeed);
            CryptoClient = new CryptoSystem(ClientSeed);
            IssacServer = new ISAAC(ServerSeed);
            byte[] bytes = new byte[8];
            rand.NextBytes(bytes);
            ConnectionCookie = BitConverter.ToUInt64(bytes, 0);
            PacketSequence = new UIntSequence(false);
            CryptoClient.OnCryptoSystemCatastrophicFailure += (sender, e) =>
            {
                Terminate(SessionTerminationReason.ClientConnectionFailure);
            };
        }
        public void DiscardSeeds()
        {
            ClientSeed = null;
            ServerSeed = null;
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
        public void LogOffPlayer()
        {
            if (logOffRequestTime == DateTime.MinValue)
            {
                Player.LogOut();
                logOffRequestTime = DateTime.UtcNow;
            }
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
        public void SendCharacterError(CharacterError error)
        {
            EnqueueSend(new GameMessageCharacterError(error));
        }
        public void WorldBroadcast(string broadcastMessage)
        {
            GameMessageSystemChat worldBroadcastMessage = new GameMessageSystemChat(broadcastMessage, ChatMessageType.Broadcast);
            EnqueueSend(worldBroadcastMessage);
        }
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
        public void EnqueueSend(params ServerPacket[] packets)
        {
            foreach (ServerPacket packet in packets)
            {
                packetQueue.Enqueue(packet);
            }
        }
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
                            bundleToSend = currentBundle;
                            currentBundles[i] = new NetworkBundle();
                        }
                    }
                    else
                    {
                        if (currentBundle.NeedsSending && DateTime.UtcNow >= nextSend)
                        {
                            bundleToSend = currentBundle;
                            currentBundles[i] = new NetworkBundle();
                        }
                    }
                }
                if (bundleToSend != null)
                {
                    SendBundle(bundleToSend, group);
                    nextSend = DateTime.UtcNow.AddMilliseconds(minimumTimeBetweenBundles);
                }
            }
            FlushPackets();
        }
        public void ProcessSessionPacket(ClientPacket packet)
        {
            if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest) && State != SessionState.AuthLoginRequest)
            {
                return;
            }
            if (packet.Header.HasFlag(PacketHeaderFlags.ConnectResponse) && State != SessionState.AuthConnectResponse)
            {
                return;
            }
            if (packet.Header.HasFlag(PacketHeaderFlags.AckSequence | PacketHeaderFlags.TimeSync | PacketHeaderFlags.EchoRequest | PacketHeaderFlags.Flow) && State == SessionState.AuthLoginRequest)
            {
                return;
            }
            NetworkStatistics.C2S_Packets_Aggregate_Increment();
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
            if (packet.Header.HasFlag(PacketHeaderFlags.RequestRetransmit))
            {
                foreach (uint sequence in packet.HeaderOptional.RetransmitData)
                {
                    Retransmit(sequence);
                }
                NetworkStatistics.C2S_RequestsForRetransmit_Aggregate_Increment();
            }
            TimeoutTick = (State == SessionState.AuthLoginRequest) ?
                DateTime.UtcNow.AddSeconds(AuthenticationHandler.DefaultAuthTimeout).Ticks :
                DateTime.UtcNow.AddSeconds(DefaultSessionTimeout).Ticks;
            if (packet.Header.Sequence <= lastReceivedPacketSequence && packet.Header.Sequence != 0 &&
                !(packet.Header.Flags == PacketHeaderFlags.AckSequence && packet.Header.Sequence == lastReceivedPacketSequence))
            {
                return;
            }
            uint desiredSeq = lastReceivedPacketSequence + 1;
            if (packet.Header.Sequence > desiredSeq)
            {
                if (!outOfOrderPackets.ContainsKey(packet.Header.Sequence))
                {
                    outOfOrderPackets.TryAdd(packet.Header.Sequence, packet);
                }
                if (desiredSeq + 2 <= packet.Header.Sequence && DateTime.Now - LatestTimeOf_S2C_NAK > new TimeSpan(0, 0, 1))
                {
                    DoRequestForRetransmission(packet.Header.Sequence);
                }
                return;
            }
            HandleOrderedPacket(packet);
            CheckOutOfOrderPackets();
            CheckOutOfOrderFragments();
        }
        public override string ToString()
        {
            string plr = (Player != null) ? $", Player: {Player.Name}" : "";
            return $"{ClientId}\\{EndPoint} Account: {Account}{plr}";
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
                if (!string.IsNullOrWhiteSpace(PendingTermination.ExtraReason))
                {
                    reas = reas + ", " + PendingTermination.ExtraReason;
                }
                log.Info($"session {ToString()} dropped{reas}");
            }
            if (Player != null)
            {
                LogOffPlayer();
            }
            NetworkManager.RemoveSession(this);
        }
        public void TickOutbound()
        {
            if (PendingTermination != null)
            {
                if (PendingTermination.TerminationStatus == SessionTerminationPhase.Initialized)
                {
                    State = SessionState.TerminationStarted;
                    Update();
                    PendingTermination.TerminationStatus = SessionTerminationPhase.SessionWorkCompleted;
                }
                return;
            }
            if (State == SessionState.TerminationStarted)
            {
                return;
            }
            if (DateTime.UtcNow.Ticks >= TimeoutTick)
            {
                Terminate(SessionTerminationReason.NetworkTimeout);
                return;
            }
            Update();
            if (logOffRequestTime != DateTime.MinValue && logOffRequestTime.AddSeconds(6) <= DateTime.UtcNow)
            {
                SendFinalLogOffMessages();
            }
        }
        private void CheckCharactersForDeletion()
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
        private void SendFinalLogOffMessages()
        {
            if (Player.CurrentLandblock != null)
            {
                return;
            }
            logOffRequestTime = DateTime.MinValue;
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
            LatestTimeOf_S2C_NAK = DateTime.Now;
            NetworkStatistics.S2C_RequestsForRetransmit_Aggregate_Increment();
        }
        private void HandleOrderedPacket(ClientPacket packet)
        {
            if (packet.Header.HasFlag(PacketHeaderFlags.EchoRequest))
            {
                FlagEcho(packet.HeaderOptional.EchoRequestClientTime);
            }
            if (packet.Header.HasFlag(PacketHeaderFlags.AckSequence))
            {
                AcknowledgeSequence(packet.HeaderOptional.AckSequence);
            }
            if (packet.Header.HasFlag(PacketHeaderFlags.TimeSync))
            {
            }
            if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest))
            {
                AuthenticationHandler.HandleLoginRequest(packet, this);
                return;
            }
            foreach (ClientPacketFragment fragment in packet.Fragments)
            {
                ProcessFragment(fragment);
            }
            if (packet.Header.Sequence != 0 && packet.Header.Flags != PacketHeaderFlags.AckSequence)
            {
                lastReceivedPacketSequence = packet.Header.Sequence;
            }
        }
        private void ProcessFragment(ClientPacketFragment fragment)
        {
            ClientMessage message = null;
            if (fragment.Header.Count != 1)
            {
                if (partialFragments.TryGetValue(fragment.Header.Sequence, out MessageBuffer buffer))
                {
                    buffer.AddFragment(fragment);
                    if (buffer.Complete)
                    {
                        message = buffer.GetMessage();
                        partialFragments.TryRemove(fragment.Header.Sequence, out MessageBuffer removed);
                    }
                }
                else
                {
                    MessageBuffer newBuffer = new MessageBuffer(fragment.Header.Sequence, fragment.Header.Count);
                    newBuffer.AddFragment(fragment);
                    partialFragments.TryAdd(fragment.Header.Sequence, newBuffer);
                }
            }
            else
            {
                message = new ClientMessage(fragment.Data);
            }
            if (message != null)
            {
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
        private void HandleFragment(ClientMessage message)
        {
            InboundMessageManager.HandleClientMessage(message, this);
            lastReceivedFragmentSequence++;
        }
        private void CheckOutOfOrderPackets()
        {
            while (outOfOrderPackets.TryRemove(lastReceivedPacketSequence + 1, out ClientPacket packet))
            {
                HandleOrderedPacket(packet);
            }
        }
        private void CheckOutOfOrderFragments()
        {
            while (outOfOrderFragments.TryRemove(lastReceivedFragmentSequence + 1, out ClientMessage message))
            {
                HandleFragment(message);
            }
        }
        private void FlagEcho(float clientTime)
        {
            object currentBundleLock = currentBundleLocks[(int)GameMessageGroup.InvalidQueue];
            lock (currentBundleLock)
            {
                NetworkBundle currentBundle = currentBundles[(int)GameMessageGroup.InvalidQueue];
                currentBundle.ClientTime = clientTime;
                currentBundle.EncryptedChecksum = true;
            }
        }
        private void AcknowledgeSequence(uint sequence)
        {
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
                if (packet.Header.HasFlag(PacketHeaderFlags.EncryptedChecksum) && PacketSequence.CurrentValue == 0)
                {
                    PacketSequence = new Sequence.UIntSequence(1);
                }
                if (packet.Header.Flags == PacketHeaderFlags.AckSequence || packet.Header.Flags.HasFlag(PacketHeaderFlags.RequestRetransmit))
                {
                    packet.Header.Sequence = PacketSequence.CurrentValue;
                }
                else
                {
                    packet.Header.Sequence = PacketSequence.NextValue;
                }
                packet.Header.Id = ServerId;
                packet.Header.Iteration = 0x14;
                packet.Header.Time = (ushort)ServerTime;
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
                uint issacXor = IssacServer.GetOffset();
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
        private void SendBundle(NetworkBundle bundle, GameMessageGroup group)
        {
            bool writeOptionalHeaders = true;
            List<MessageFragment> fragments = new List<MessageFragment>();
            while (bundle.HasMoreMessages)
            {
                GameMessage message = bundle.Dequeue();
                MessageFragment fragment = new MessageFragment(message, FragmentSequence++);
                fragments.Add(fragment);
            }
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
                MessageFragment firstMessage = fragments.FirstOrDefault();
                if (firstMessage != null)
                {
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
                        List<MessageFragment> removeList = new List<MessageFragment>();
                        foreach (MessageFragment fragment in fragments)
                        {
                            if (!fragment.TailSent && availableSpace >= fragment.TailSize)
                            {
                                ServerPacketFragment spf = fragment.GetTailFragment();
                                packet.Fragments.Add(spf);
                                availableSpace -= spf.Length;
                            }
                            else if (availableSpace >= fragment.NextSize)
                            {
                                ServerPacketFragment spf = fragment.GetNextFragment();
                                packet.Fragments.Add(spf);
                                availableSpace -= spf.Length;
                            }
                            if (fragment.DataRemaining <= 0)
                            {
                                removeList.Add(fragment);
                            }
                        }
                        fragments.RemoveAll(x => removeList.Contains(x));
                    }
                }
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
            if (bundle.SendAck)
            {
                packetHeader.Flags |= PacketHeaderFlags.AckSequence;
                packet.InitializeBodyWriter();
                packet.BodyWriter.Write(lastReceivedPacketSequence);
            }
            if (bundle.TimeSync)
            {
                packetHeader.Flags |= PacketHeaderFlags.TimeSync;
                packet.InitializeBodyWriter();
                packet.BodyWriter.Write(ServerTime);
            }
            if (bundle.ClientTime != -1f)
            {
                packetHeader.Flags |= PacketHeaderFlags.EchoResponse;
                packet.InitializeBodyWriter();
                packet.BodyWriter.Write(bundle.ClientTime);
                packet.BodyWriter.Write((float)ServerTime - bundle.ClientTime);
            }
        }
    }
}
