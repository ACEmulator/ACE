using System;
using System.Collections.Generic;
using System.Net;

using ACE.Common;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.Network.GameMessages.Messages;
using ACE.Managers;
using ACE.Network.Managers;
using log4net;

namespace ACE.Network
{
    public class Session : NetworkSession
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DateTime logOffRequestTime;
        private DateTime lastSaveTime;
        private DateTime lastAgeIntUpdateTime;
        private DateTime lastSendAgeIntUpdateTime;

        public List<CachedCharacter> AccountCharacters { get; } = new List<CachedCharacter>();
        public CachedCharacter CharacterRequested { get; set; }
        public Player Player { get; set; }

        public uint GameEventSequence { get; set; }

        public Session(IPEndPoint endpoint, ushort clientId, ushort serverId)
            : base(endpoint, clientId, serverId)
        {
            base.StateChanged += Session_StateChanged;
            Network = networkSession;
            Network.StateChanged += Network_StateChanged;
            Network.ClientMessageReceived += Network_ClientMessageReceived;
            DefineMessageHandlers();
        }

        public override void Update(double lastTick)
        {
            base.Update(lastTick);

            // Live server seemed to take about 6 seconds. 4 seconds is nice because it has smooth animation, and saves the user 2 seconds every logoff
            // This could be made 0 for instant logoffs.
            if (logOffRequestTime != DateTime.MinValue && logOffRequestTime.AddSeconds(6) <= DateTime.UtcNow)
            {
                logOffRequestTime = DateTime.MinValue;
                SendFinalLogOffMessages();
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

                if (Player.WaitingForDelayedTeleport && DateTime.UtcNow >= Player.DelayedTeleportTime)
                {
                    // TODO: Check for movement from position at which player started the delayed teleport
                    //       if wandered to far, cancel teleport and send error msg to player.

                    Player.WaitingForDelayedTeleport = false;
                    Player.Teleport(Player.DelayedTeleportDestination);
                    Player.ClearDelayedTeleport();
                }
            }
        }

        public void SaveSession()
        {
            if (this.Player != null)
            {
                this.Player.SaveOptions();
                this.Player.SaveCharacter();
            }
        }

        protected override void ClientMessageReceived(ClientMessage message)
        {
            InboundMessageManager.HandleClientMessage(message, this);
        }

        private void Session_StateChanged(object sender, SessionStateChangedEventArgs e)
        {
            if (e.NewState == SessionState.AuthConnected)
            {
                NetworkConnected();
            }
            else if (e.NewState == SessionState.Terminated)
            {
                NetworkDisconnected();
            }
        }

        private async void NetworkConnected()
        {
            log.DebugFormat("[{0}] Network Connected", AccountName);
            var result = await DatabaseManager.Character.GetByAccount(AccountId);

            UpdateCachedCharacters(result);

            GameMessageCharacterList characterListMessage = new GameMessageCharacterList(result, AccountName);
            GameMessageServerName serverNameMessage = new GameMessageServerName(ConfigManager.Config.Server.WorldName);
            GameMessageDDDInterrogation dddInterrogation = new GameMessageDDDInterrogation();

            EnqueueSend(characterListMessage, serverNameMessage, dddInterrogation);
        }

        private void NetworkDisconnected()
        {
            log.DebugFormat("[{0}] Network Disconnected", AccountName);
            if (Player != null)
            {
                SaveSession();
                Player.Logout(true);
            }
        }

        public void UpdateCachedCharacters(IEnumerable<CachedCharacter> characters)
        {
            AccountCharacters.Clear();
            foreach (var character in characters)
            {
                AccountCharacters.Add(character);
            }
        }

        public void InitSessionForWorldLogin()
        {
            Player = new Player(this);
            CharacterRequested = null;

            lastSaveTime = DateTime.MinValue;
            lastAgeIntUpdateTime = DateTime.MinValue;
            lastSendAgeIntUpdateTime = DateTime.MinValue;

            GameEventSequence = 0;
        }

        private async void SendFinalLogOffMessages()
        {
            EnqueueSend(new GameMessageCharacterLogOff());

            var result = await DatabaseManager.Character.GetByAccount(AccountId);
            UpdateCachedCharacters(result);
            EnqueueSend(new GameMessageCharacterList(result, AccountName));

            GameMessageServerName serverNameMessage = new GameMessageServerName(ConfigManager.Config.Server.WorldName);
            EnqueueSend(serverNameMessage);

            State = SessionState.AuthConnected;

            Player = null;
        }
    }
}
