using System;
using System.Collections.Generic;
using System.Net;
using log4net;
using ACE.Common;
using ACE.Database;
using ACE.Entity;
using ACE.Network.Enum;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Sequence;

namespace ACE.Network
{
    public partial class Session : NetworkSession
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DateTime logOffRequestTime;
        private DateTime lastSaveTime;
        private DateTime lastAgeIntUpdateTime;
        private DateTime lastSendAgeIntUpdateTime;

        private List<CachedCharacter> AccountCharacters { get; } = new List<CachedCharacter>();
        public Player Player { get; set; }

        public UIntSequence GameEventSequence { get; private set; }

        public Session(IPEndPoint endpoint, ushort clientId, ushort serverId)
            : base(endpoint, clientId, serverId)
        {
            base.StateChanged += Session_StateChanged;
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
            HandleClientMessage(message);
        }

        private void Session_StateChanged(object sender, SessionStateChangedEventArgs e)
        {
            if (e.NewState == SessionState.AuthConnected)
            {
                LoginConnected();
            }
            else if (e.NewState == SessionState.Terminated)
            {
                Terminated();
            }
        }

        private async void LoginConnected()
        {
            log.DebugFormat("[{0}] Login Connected", AccountName);

            Player = null;

            var result = await DatabaseManager.Character.GetByAccount(AccountId);

            UpdateCachedCharacters(result);

            GameMessageCharacterList characterListMessage = new GameMessageCharacterList(result, AccountName);
            GameMessageServerName serverNameMessage = new GameMessageServerName(ConfigManager.Config.Server.WorldName);

            EnqueueSend(characterListMessage, serverNameMessage);
        }

        private void Terminated()
        {
            log.DebugFormat("[{0}] Session Terminated", AccountName);
            if (Player != null)
            {
                SaveSession();
                Player.Logout(true);
                Player = null;
            }
        }

        private void UpdateCachedCharacters(IEnumerable<CachedCharacter> characters)
        {
            AccountCharacters.Clear();
            foreach (var character in characters)
            {
                AccountCharacters.Add(character);
            }
        }

        private void InitSessionForWorldLogin(CachedCharacter character)
        {
            Player = new Player(this, character);

            lastSaveTime = DateTime.MinValue;
            lastAgeIntUpdateTime = DateTime.MinValue;
            lastSendAgeIntUpdateTime = DateTime.MinValue;

            GameEventSequence = new UIntSequence(false);
        }

        private void SendFinalLogOffMessages()
        {
            EnqueueSend(new GameMessageCharacterLogOff());
            State = SessionState.AuthConnected;
        }
    }
}
