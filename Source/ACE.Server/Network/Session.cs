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

        public readonly ActionQueue InboundGameActionQueue = new ActionQueue();


        private DateTime logOffRequestTime;

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

            log.Info($"client {Account} disconnected");

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
        /// This will process all inbound GameActions.
        /// </summary>
        public void TickInbound()
        {
            if (Player != null)
                InboundGameActionQueue.RunActions();
        }

        /// <summary>
        /// This will send outgoing packets as well as the final logoff message.
        /// </summary>
        public void TickOutbound()
        {
            // Checks if the session has stopped responding.
            if (DateTime.UtcNow.Ticks >= Network.TimeoutTick)
            {
                // Change the state to show that the Session has reached a timeout.
                State = SessionState.NetworkTimeout;
            }

            Network.Update();

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

                    DatabaseManager.Shard.SaveCharacter(Characters[i], new ReaderWriterLockSlim(), null);

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
            // It's possible for a character change to happen from a GameActionSetCharacterOptions message.
            // This message can be received/processed by the server AFTER LogOfPlayer has been called.
            // What that means is, we could end up with Character changes after the Character has been saved from the initial LogOff request.
            // To make sure we commit these additional changes (if any), we check again here
            if (Player.CharacterChangesDetected)
                Player.SaveCharacterToDatabase();

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
