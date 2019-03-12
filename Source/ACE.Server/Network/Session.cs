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


        public uint AccountId { get; private set; }

        public string Account { get; private set; }

        public string LoggingIdentifier { get; private set; } = "Unverified";

        public AccessLevel AccessLevel { get; private set; }

        public List<Character> Characters { get; } = new List<Character>();

        public Player Player { get; private set; }

        public readonly ActionQueue InboundGameActionQueue = new ActionQueue();


        private DateTime logOffRequestTime;

        private bool bootSession;

        public string BootSessionReason { get; private set; }


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

        public void ProcessPacket(ClientPacket packet)
        {
            if (!CheckState(packet))
                return;

            Network.ProcessPacket(packet);

            if (packet.Header.HasFlag(PacketHeaderFlags.Disconnect))
                DropSession("PacketHeader Disconnect");
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
            if (State == SessionState.NetworkTimeout)
                return;

            // Checks if the session has stopped responding.
            if (DateTime.UtcNow.Ticks >= Network.TimeoutTick)
            {
                // Change the state to show that the Session has reached a timeout.
                State = SessionState.NetworkTimeout;
                return;
            }

            Network.Update();

            // Live server seemed to take about 6 seconds. 4 seconds is nice because it has smooth animation, and saves the user 2 seconds every logoff
            // This could be made 0 for instant logoffs.
            if (logOffRequestTime != DateTime.MinValue && logOffRequestTime.AddSeconds(6) <= DateTime.UtcNow)
                SendFinalLogOffMessages();

            // Check if the player has been booted
            if (bootSession)
                State = SessionState.NetworkTimeout;
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
                return;

            logOffRequestTime = DateTime.MinValue;

            // It's possible for a character change to happen from a GameActionSetCharacterOptions message.
            // This message can be received/processed by the server AFTER LogOfPlayer has been called.
            // What that means is, we could end up with Character changes after the Character has been saved from the initial LogOff request.
            // To make sure we commit these additional changes (if any), we check again here
            if (Player.CharacterChangesDetected)
                Player.SaveCharacterToDatabase();

            Player = null;

            Network.EnqueueSend(new GameMessageCharacterLogOff());

            CheckCharactersForDeletion();

            Network.EnqueueSend(new GameMessageCharacterList(Characters, this));

            GameMessageServerName serverNameMessage = new GameMessageServerName(ConfigManager.Config.Server.WorldName);
            Network.EnqueueSend(serverNameMessage);

            State = SessionState.AuthConnected;
        }

        public void BootSession(string reason = "", params GameMessages.GameMessage[] messages)
        {
            Network.EnqueueSend(messages);

            if (!string.IsNullOrEmpty(reason))
                BootSessionReason = reason;

            bootSession = true;
        }

        public void DropSession(string reason)
        {
            log.Info($"Session dropped. Account: {Account}, Player: {Player?.Name}, Reason: {reason}");

            if (Player != null)
            {
                LogOffPlayer();

                // We don't want to set the player to null here. Because the player is still on the network, it may still enqueue work onto it's session.
                // Some network message objects will reference session.Player in their construction. If we set Player to null here, we'll throw exceptions in those cases.

                // At this point, if the player was on a landblock, they'll still exist on that landblock until the logout animation completes (~6s).
            }

            WorldManager.RemoveSession(this);
        }


        public void SendCharacterError(CharacterError error)
        {
            Network.EnqueueSend(new GameMessageCharacterError(error));
        }

        /// <summary>
        /// Sends a broadcast message to the player
        /// </summary>
        public void WorldBroadcast(string broadcastMessage)
        {
            var worldBroadcastMessage = new GameMessageSystemChat(broadcastMessage, ChatMessageType.Broadcast);
            Network.EnqueueSend(worldBroadcastMessage);
        }
    }
}
