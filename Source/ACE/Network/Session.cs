using System;
using System.Threading;
using System.Collections.Generic;
using System.Net;

using ACE.Common;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Actions;
using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.Network.GameMessages.Messages;
using ACE.Managers;
using log4net;

namespace ACE.Network
{
    public class Session : IActor
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public uint SubscriptionId { get; private set; }

        public string ClientAccountString { get; private set; }

        public string LoggingIdentifier { get; private set; } = "Unverified";

        public AccessLevel AccessLevel { get; private set; }

        public SessionState State { get; set; }

        public List<CachedCharacter> AccountCharacters { get; } = new List<CachedCharacter>();

        public CachedCharacter CharacterRequested { get; set; }

        public Player Player { get; private set; }

        private DateTime logOffRequestTime;

        private DateTime lastSaveTime;

        private DateTime lastAgeIntUpdateTime;
        private DateTime lastSendAgeIntUpdateTime;
        private bool bootSession = false;

        private ReaderWriterLockSlim playerWaitLock = new ReaderWriterLockSlim();
        private object playerSync = new object();

        // connection related
        public IPEndPoint EndPoint { get; }

        public uint GameEventSequence { get; set; }

        public NetworkSession Network { get; set; }

        /// <summary>
        /// This actionQueue forces network packets on to the main thread off the network thread, to avoid concurrency errors
        /// </summary>
        private NestedActionQueue actionQueue = new NestedActionQueue();

        public Session(IPEndPoint endPoint, ushort clientId, ushort serverId)
        {
            EndPoint = endPoint;
            Network = new NetworkSession(this, clientId, serverId);
            actionQueue.SetParent(WorldManager.ActionQueue);
        }

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
            // NOTE(ddevec): Once again -- no reader-writer lock and Monitor support in c# -- ventring frustration now --  asa;gklkfj;kl
            //  -- This should be a rare operation, so we don't really care about the stupid double locking, as long as its done right for no deadlocks
            lock (playerSync)
            {
                Monitor.PulseAll(playerSync);
            }
            playerWaitLock.ExitWriteLock();
        }

        public void InitSessionForWorldLogin()
        {
            CharacterRequested = null;

            lastSaveTime = DateTime.MinValue;
            lastAgeIntUpdateTime = DateTime.MinValue;
            lastSendAgeIntUpdateTime = DateTime.MinValue;

            GameEventSequence = 0;
        }

        public void SetSubscription(Subscription sub, string clientAccountString, string loggingIdentifier)
        {
            log.Info($"setting subscription information for {sub.SubscriptionGuid}, clientAccountString {clientAccountString}");
            SubscriptionId = sub.SubscriptionId;
            ClientAccountString = clientAccountString;
            LoggingIdentifier = loggingIdentifier;
            AccessLevel = sub.AccessLevel;
        }

        public void UpdateCachedCharacters(IEnumerable<CachedCharacter> characters)
        {
            AccountCharacters.Clear();
            byte slot = 0;
            foreach (var character in characters)
            {
                if (character.DeleteTime > 0)
                {
                    if (Time.GetUnixTime() > character.DeleteTime)
                    {
                        character.Deleted = true;
                        DatabaseManager.Shard.DeleteCharacter(character.Guid.Full, deleteSuccess =>
                        {
                            if (deleteSuccess)
                            {
                                log.Info($"Character {character.Guid.Full:X} successfully marked as deleted");
                            }
                            else
                            {
                                log.Error($"Unable to mark character {character.Guid.Full:X} as deleted");
                            }
                        });
                        continue;
                    }
                }
                character.SlotId = slot;
                AccountCharacters.Add(character);
                slot++;
            }
        }

        public void Update(double lastTick, long currentTimeTick)
        {
            // Checks if the session has stopped responding.
            if (currentTimeTick >= Network.TimeoutTick)
            {
                // Change the state to show that the Session has reached a timeout.
                State = SessionState.NetworkTimeout;
            }

            Network.Update(lastTick);

            // FIXME(ddevec): Most of the following work can probably be integrated into the player's action queue, or an action queue strucutre

            // Live server seemed to take about 6 seconds. 4 seconds is nice because it has smooth animation, and saves the user 2 seconds every logoff
            // This could be made 0 for instant logoffs.
            if (logOffRequestTime != DateTime.MinValue && logOffRequestTime.AddSeconds(6) <= DateTime.UtcNow)
            {
                logOffRequestTime = DateTime.MinValue;
                SendFinalLogOffMessages();
            }

            // Check if the player has been booted
            if (bootSession != false)
            {
                SendFinalBoot();
            }

            if (Player != null)
            {
                if (lastSaveTime == DateTime.MinValue)
                    lastSaveTime = DateTime.UtcNow;
                if (lastSaveTime != DateTime.MinValue && lastSaveTime.AddMinutes(5) <= DateTime.UtcNow)
                {
                    SaveSession();
                    lastSaveTime = DateTime.UtcNow;
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

        public void SaveSession()
        {
            if (this.Player != null)
            {
                this.Player.HandleActionSaveCharacter();
            }
        }

        public uint GetIssacValue(PacketDirection direction)
        {
            return (direction == PacketDirection.Client ? Network.ConnectionData.IssacClient.GetOffset() : Network.ConnectionData.IssacServer.GetOffset());
        }

        public void SendCharacterError(CharacterError error)
        {
            Network.EnqueueSend(new GameMessageCharacterError(error));
        }

        private bool CheckState(ClientPacket packet)
        {
            if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest) && State != SessionState.AuthLoginRequest)
                return false;

            if (packet.Header.HasFlag(PacketHeaderFlags.ConnectResponse) && State != SessionState.AuthConnectResponse)
                return false;

            if (packet.Header.HasFlag(PacketHeaderFlags.AckSequence | PacketHeaderFlags.TimeSynch | PacketHeaderFlags.EchoRequest | PacketHeaderFlags.Flow) && State == SessionState.AuthLoginRequest)
                return false;

            return true;
        }

        public void ProcessPacket(ClientPacket packet)
        {
            if (!CheckState(packet))
            {
                return;
            }

            // Prevent crash when world is not initialized yet.  Need to look at this closer as I think there are some changes needed to state handling/transitions.
            if (Network != null)
                Network.ProcessPacket(packet);

            if (packet.Header.HasFlag(PacketHeaderFlags.Disconnect))
                HandleDisconnectResponse();
        }

        private void HandleDisconnectResponse()
        {
            if (Player != null)
            {
                SaveSession();
                Player.HandleActionLogout(true);
            }

            WorldManager.RemoveSession(this);
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

        public void BootPlayer()
        {
            bootSession = true;
        }

        private void SendFinalLogOffMessages()
        {
            Network.EnqueueSend(new GameMessageCharacterLogOff());

            DatabaseManager.Shard.GetCharacters(SubscriptionId, ((List<CachedCharacter> result) =>
            {
                UpdateCachedCharacters(result);
                Network.EnqueueSend(new GameMessageCharacterList(result, ClientAccountString));

                GameMessageServerName serverNameMessage = new GameMessageServerName(ConfigManager.Config.Server.WorldName);
                Network.EnqueueSend(serverNameMessage);

                State = SessionState.AuthConnected;

                Player = null;
            }));
        }

        private void SendFinalBoot()
        {
            // Note that: Currently, if a player is able to block this specific message
            // then they will not be booted from the server, this was noticed in practice and test.
            // TODO: Hook in a player disconnect function and prevent the LogOffPlayer() function from firing after this diconnect has occurred.
            Network.EnqueueSend(new GameMessageBootAccount(this));
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

        /// Boilerplate Action/Actor stuff
        public LinkedListNode<IAction> EnqueueAction(IAction act)
        {
            return actionQueue.EnqueueAction(act);
        }

        public void RunActions()
        {
            actionQueue.RunActions();
        }

        public void DequeueAction(LinkedListNode<IAction> node)
        {
            actionQueue.DequeueAction(node);
        }
    }
}
