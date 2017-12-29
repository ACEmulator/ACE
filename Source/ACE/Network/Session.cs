using System;
using System.Threading;
using System.Threading.Tasks;
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
    public class Session
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

        private bool bootSession = false;

        private ReaderWriterLockSlim playerWaitLock = new ReaderWriterLockSlim();
        private object playerSync = new object();

        // connection related
        public IPEndPoint EndPoint { get; }

        public uint GameEventSequence { get; set; }

        public NetworkSession Network { get; set; }

        public Session(IPEndPoint endPoint, ushort clientId, ushort serverId)
        {
            EndPoint = endPoint;
            Network = new NetworkSession(this, clientId, serverId);
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
            // NOTE(ddevec): Once again -- no reader-writer lock and Monitor support in c# -- ventring frustration now --  asa;gklkfj;kl
            //  -- This should be a rare operation, so we don't really care about the stupid double locking, as long as its done right for no deadlocks
            playerWaitLock.EnterWriteLock();
            Player = player;

            lock (playerSync)
            {
                Monitor.PulseAll(playerSync);
            }
            playerWaitLock.ExitWriteLock();

            // FIXME(ddevec): These need to be managed better, but this hack is good enough for now.
            WorldManager.StartGameTask(async () =>
            {
                // FIXME: Need to kill this task at some point :-/
                while (true)
                {
                    await Task.Delay(TimeSpan.FromMinutes(5));
                    if (Player == null)
                    {
                        break;
                    }
                    await SaveSession();
                }
            });

            WorldManager.StartGameTask(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    if (Player == null)
                    {
                        break;
                    }
                    Player.UpdateAge();
                }
            });

            WorldManager.StartGameTask(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromSeconds(7));
                    if (Player == null)
                    {
                        break;
                    }
                    Player.SendAgeInt();
                }
            });
        }

        public void InitSessionForWorldLogin()
        {
            CharacterRequested = null;

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

        public async Task UpdateCachedCharacters(IEnumerable<CachedCharacter> characters)
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
                        bool deleteSuccess = await DatabaseManager.Shard.DeleteCharacter(character.Guid.Full);
                        if (deleteSuccess)
                        {
                            log.Info($"Character {character.Guid.Full:X} successfully marked as deleted");
                        }
                        else
                        {
                            log.Error($"Unable to mark character {character.Guid.Full:X} as deleted");
                        }

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

            // FIXME(ddevec): Most of the following work can probably be integrated into the player's work structure

            // Check if the player has been booted
            if (bootSession != false)
            {
                SendFinalBoot();
            }
        }

        public async Task SaveSession()
        {
            if (this.Player != null)
            {
                await this.Player.SaveCharacter();
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

        public async Task ProcessPacket(ClientPacket packet)
        {
            if (!CheckState(packet))
            {
                return;
            }

            // Prevent crash when world is not initialized yet.  Need to look at this closer as I think there are some changes needed to state handling/transitions.
            if (Network != null)
                Network.ProcessPacket(packet);

            if (packet.Header.HasFlag(PacketHeaderFlags.Disconnect))
                await HandleDisconnectResponse();
        }

        private async Task HandleDisconnectResponse()
        {
            if (Player != null)
            {
                await SaveSession();
                await Player.Logout(true);
            }

            WorldManager.RemoveSession(this);
        }

        public async Task LogOffPlayer()
        {
            // First save, then logout
            await Player.SaveCharacter();
            await Player.Logout();

            // Live server seemed to take about 6 seconds. 4 seconds is nice because it has smooth animation, and saves the user 2 seconds every logoff
            // This could be made 0 for instant logoffs.
            await Task.Delay(TimeSpan.FromSeconds(6));

            await SendFinalLogOffMessages();
        }

        public void BootPlayer()
        {
            bootSession = true;
        }

        private async Task SendFinalLogOffMessages()
        {
            Network.EnqueueSend(new GameMessageCharacterLogOff());

            List<CachedCharacter> result = await DatabaseManager.Shard.GetCharacters(SubscriptionId);
            await UpdateCachedCharacters(result);
            Network.EnqueueSend(new GameMessageCharacterList(result, ClientAccountString));

            GameMessageServerName serverNameMessage = new GameMessageServerName(ConfigManager.Config.Server.WorldName);
            Network.EnqueueSend(serverNameMessage);

            State = SessionState.AuthConnected;

            Player = null;
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
    }
}
