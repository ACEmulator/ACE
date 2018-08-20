using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

using log4net;

using ACE.Common;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.WorldObjects;
using ACE.Server.Managers;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.Network
{
    public class Session
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public IPEndPoint EndPoint { get; }

        public NetworkSession Network { get; set; }

        public uint GameEventSequence { get; set; }

        public SessionState State { get; set; }


        public uint Id { get; private set; }

        public string Account { get; private set; }

        public string LoggingIdentifier { get; private set; } = "Unverified";

        public AccessLevel AccessLevel { get; private set; }

        public List<Character> Characters { get; } = new List<Character>();

        public Player Player { get; private set; }


        private DateTime lastAutoSaveTime;
        private DateTime logOffRequestTime;
        private DateTime lastAgeIntUpdateTime;
        private DateTime lastSendAgeIntUpdateTime;

        private bool bootSession;


        // todo: Remove this when WorldManager.LoadAllPlayers() is refactored
        public bool IsOnline = true;

        // todo: Remove this when WorldManager.LoadAllPlayers() is refactored
        public Session()
        {
            IsOnline = false;
        }

        public Session(IPEndPoint endPoint, ushort clientId, ushort serverId)
        {
            EndPoint = endPoint;
            Network = new NetworkSession(this, clientId, serverId);
        }


        private bool CheckState(ClientPacket packet)
        {
            if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest) && State != SessionState.AuthLoginRequest)
                return false;

            if (packet.Header.HasFlag(PacketHeaderFlags.ConnectResponse) && State != SessionState.AuthConnectResponse)
                return false;

            if (packet.Header.HasFlag(PacketHeaderFlags.AckSequence | PacketHeaderFlags.TimeSync | PacketHeaderFlags.EchoRequest | PacketHeaderFlags.Flow) && State == SessionState.AuthLoginRequest)
                return false;

            return true;
        }

        private void HandleDisconnectResponse()
        {
            if (Player != null)
            {
                Player.EnqueueSaveChain();
                Player.HandleActionLogout(true);
            }

            WorldManager.RemoveSession(this);
        }

        public void ProcessPacket(ClientPacket packet)
        {
            if (!CheckState(packet))
                return;

            // Prevent crash when world is not initialized yet.  Need to look at this closer as I think there are some changes needed to state handling/transitions.
            if (Network != null)
                Network.ProcessPacket(packet);

            if (packet.Header.HasFlag(PacketHeaderFlags.Disconnect))
                HandleDisconnectResponse();
        }

        public uint GetIssacValue(PacketDirection direction)
        {
            return (direction == PacketDirection.Client ? Network.ConnectionData.IssacClient.GetOffset() : Network.ConnectionData.IssacServer.GetOffset());
        }


        /// <summary>
        /// This is run in parallel from our main loop.
        /// </summary>
        public void Update(double lastTick, long currentTimeTick)
        {
            // Checks if the session has stopped responding.
            if (currentTimeTick >= Network.TimeoutTick)
            {
                // Change the state to show that the Session has reached a timeout.
                State = SessionState.NetworkTimeout;
            }

            Network.Update(lastTick);

            // Live server seemed to take about 6 seconds. 4 seconds is nice because it has smooth animation, and saves the user 2 seconds every logoff
            // This could be made 0 for instant logoffs.
            if (logOffRequestTime != DateTime.MinValue && logOffRequestTime.AddSeconds(6) <= DateTime.UtcNow)
            {
                logOffRequestTime = DateTime.MinValue;
                SendFinalLogOffMessages();
            }

            // Check if the player has been booted
            if (bootSession)
            {
                SendFinalBoot();
                State = SessionState.NetworkTimeout;
            }

            // todo: I'd like to move this to a player Update() function. Mag-nus 2018-08-10
            if (Player != null)
            {
                // First, we check if the player hasn't been saved in the last 5 minutes
                if (Player.LastRequestedDatabaseSave + Player.PlayerSaveInterval <= DateTime.UtcNow)
                {
                    // Secondly, we make sure this session hasn't requested a save in the last 5 minutes.
                    // We do this because EnqueueSaveChain will queue an ActionChain that may not execute immediately. This prevents refiring while a save is pending.
                    if (lastAutoSaveTime + Player.PlayerSaveInterval <= DateTime.UtcNow)
                    {
                        lastAutoSaveTime = DateTime.UtcNow;
                        Player.EnqueueSaveChain();
                    }
                }

                if (lastAgeIntUpdateTime == DateTime.MinValue)
                    lastAgeIntUpdateTime = DateTime.UtcNow;

                if (lastAgeIntUpdateTime != DateTime.MinValue && lastAgeIntUpdateTime.AddSeconds(1) <= DateTime.UtcNow)
                {
                    Player.UpdateAge();
                    lastAgeIntUpdateTime = DateTime.UtcNow;
                }

                if (lastSendAgeIntUpdateTime == DateTime.MinValue)
                    lastSendAgeIntUpdateTime = DateTime.UtcNow;

                if (lastSendAgeIntUpdateTime != DateTime.MinValue && lastSendAgeIntUpdateTime.AddSeconds(7) <= DateTime.UtcNow)
                {
                    Player.SendAgeInt();
                    lastSendAgeIntUpdateTime = DateTime.UtcNow;
                }
            }
        }


        public void SetAccount(uint accountId, string account, AccessLevel accountAccesslevel)
        {
            Id = accountId;
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

                    var idToDelete = Characters[i].Id;

                    DatabaseManager.Shard.MarkCharacterDeleted(idToDelete, deleteSuccess =>
                    {
                        if (deleteSuccess)
                            log.Info($"Character {idToDelete:X} successfully marked as deleted");
                        else
                            log.Error($"Unable to mark character {idToDelete:X} as deleted");
                    });

                    Characters.RemoveAt(i);
                }
            }
        }

        public void InitSessionForWorldLogin()
        {
            lastAgeIntUpdateTime = DateTime.MinValue;
            lastSendAgeIntUpdateTime = DateTime.MinValue;

            GameEventSequence = 1;
        }

        public void SetAccessLevel(AccessLevel accountAccesslevel)
        {
            AccessLevel = accountAccesslevel;
        }

        private readonly ReaderWriterLockSlim playerWaitLock = new ReaderWriterLockSlim();
        private readonly object playerSync = new object();

        public void WaitForPlayer()
        {
            // NOTE(ddevec): We use a Reader-writer lock because reads are common, and writes are rare
            playerWaitLock.EnterReadLock();
            try
            {
                while (Player == null)
                {
                    // NOTE(ddevec): This slop is because monitor doesn't support releasing a reader-writer lock 
                    //     -- trust it's right, and optimial and don't touch it
                    // This should be a rare operation, so the extra locking nonsense doesn't kill us
                    lock (playerSync)
                    {
                        playerWaitLock.ExitReadLock();
                        Monitor.Wait(playerSync);
                    }
                    playerWaitLock.EnterReadLock();
                }
            }
            finally
            {
                playerWaitLock.ExitReadLock();
            }
        }

        public void SetPlayer(Player player)
        {
            playerWaitLock.EnterWriteLock();
            Player = player;
            lastAutoSaveTime = DateTime.UtcNow;
            // NOTE(ddevec): Once again -- no reader-writer lock and Monitor support in c# -- ventring frustration now --  asa;gklkfj;kl
            //  -- This should be a rare operation, so we don't really care about the stupid double locking, as long as its done right for no deadlocks
            lock (playerSync)
            {
                Monitor.PulseAll(playerSync);
            }
            playerWaitLock.ExitWriteLock();
        }

        public void LogOffPlayer()
        {
            // First save, then logout
            ActionChain logoutChain = new ActionChain();
            logoutChain.AddChain(Player.GetSaveChain());
            logoutChain.AddChain(Player.GetLogoutChain());
            logoutChain.EnqueueChain();

            logOffRequestTime = DateTime.UtcNow;
        }

        private void SendFinalLogOffMessages()
        {
            Network.EnqueueSend(new GameMessageCharacterLogOff());

            CheckCharactersForDeletion();

            Network.EnqueueSend(new GameMessageCharacterList(Characters, this));

            GameMessageServerName serverNameMessage = new GameMessageServerName(ConfigManager.Config.Server.WorldName);
            Network.EnqueueSend(serverNameMessage);

            State = SessionState.AuthConnected;

            Player = null;
        }

        public void BootPlayer()
        {
            bootSession = true;
        }

        private void SendFinalBoot()
        {
            // Note that: Currently, if a player is able to block this specific message
            // then they will not be booted from the server, this was noticed in practice and test.
            // TODO: Hook in a player disconnect function and prevent the LogOffPlayer() function from firing after this diconnect has occurred.
            Network.EnqueueSend(new GameMessageBootAccount(this));
        }


        public void SendCharacterError(CharacterError error)
        {
            Network.EnqueueSend(new GameMessageCharacterError(error));
        }

        /// <summary>
        /// Sends a broadcast message to the player
        /// </summary>
        /// <param name="broadcastMessage"></param>
        public void WorldBroadcast(string broadcastMessage)
        {
            var worldBroadcastMessage = new GameMessageSystemChat(broadcastMessage, ChatMessageType.Broadcast);
            Network.EnqueueSend(worldBroadcastMessage);
        }
    }
}
