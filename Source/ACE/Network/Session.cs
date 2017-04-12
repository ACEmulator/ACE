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
    public class Session
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DateTime logOffRequestTime;
        private DateTime lastSaveTime;
        private DateTime lastAgeIntUpdateTime;
        private DateTime lastSendAgeIntUpdateTime;

        public uint AccountId { get; private set; }
        public string AccountName { get; private set; }
        public AccessLevel AccessLevel { get; private set; }

        private SessionState state = SessionState.Idle;
        public SessionState State
        {
            get { return state; }
            set
            {
                state = value;
                StateChanged?.Invoke(this, new SessionStateChangedEventArgs(state));
            }
        }

        public List<CachedCharacter> AccountCharacters { get; } = new List<CachedCharacter>();

        public CachedCharacter CharacterRequested { get; set; }

        public Player Player { get; set; }

        public uint GameEventSequence { get; set; }

        public NetworkSession Network { get; set; }

        public event EventHandler<SessionStateChangedEventArgs> StateChanged;

        public Session(NetworkSession networkSession)
        {
            Network = networkSession;
            Network.StateChanged += Network_StateChanged;
            Network.ClientMessageReceived += Network_ClientMessageReceived;
        }

        private void Network_ClientMessageReceived(object sender, NetworkSession.ClientMessageReceivedEventArgs e)
        {
            InboundMessageManager.HandleClientMessage(e.Message, this);
        }

        private void Network_StateChanged(object sender, NetworkSession.NetworkSessionStateChangedEventArgs e)
        {
            log.DebugFormat("[{0}] Network State Changed to {1}", AccountName, e.NewState);
            if (e.NewState == NetworkSessionState.Connecting)
            {
                State = SessionState.AuthConnecting;
            }
            else if (e.NewState == NetworkSessionState.Connected)
            {
                State = SessionState.AuthConnected;
                NetworkConnected();
            }
            else if (e.NewState == NetworkSessionState.Disconnected)
            {
                NetworkDisconnected();
                State = SessionState.Terminated;
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

            Network.EnqueueSend(characterListMessage, serverNameMessage, dddInterrogation);
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

        public void InitSessionForWorldLogin()
        {
            Player = new Player(this);
            CharacterRequested = null;

            lastSaveTime = DateTime.MinValue;
            lastAgeIntUpdateTime = DateTime.MinValue;
            lastSendAgeIntUpdateTime = DateTime.MinValue;

            GameEventSequence = 0;
        }

        public void SetAccount(uint accountId, string account, AccessLevel accountAccesslevel)
        {
            log.InfoFormat("Setting session account to {0} with name {1} and access {2}, network client id {3}", accountId, account, accountAccesslevel, Network.ClientId);
            AccountId = accountId;
            AccountName = account;
            AccessLevel = accountAccesslevel;
        }

        public void UpdateCachedCharacters(IEnumerable<CachedCharacter> characters)
        {
            AccountCharacters.Clear();
            foreach (var character in characters)
            {
                AccountCharacters.Add(character);
            }
        }

        public void Update(double lastTick)
        {
            Network.Update(lastTick);

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

        public void SendCharacterError(CharacterError error)
        {
            Network.SendCharacterError(error);
        }

        public void Terminate(CharacterError error = CharacterError.Undefined)
        {
            log.DebugFormat("[{0}] Terminating", AccountName);
            State = SessionState.Terminating;
            Network.Terminate(error);
            State = SessionState.Terminated;
        }

        public void LogOffPlayer()
        {
            log.DebugFormat("[{0}] Logging off", AccountName);
            SaveSession();
            Player.Logout();
            logOffRequestTime = DateTime.UtcNow;
        }

        private async void SendFinalLogOffMessages()
        {
            Network.EnqueueSend(new GameMessageCharacterLogOff());

            var result = await DatabaseManager.Character.GetByAccount(AccountId);
            UpdateCachedCharacters(result);
            Network.EnqueueSend(new GameMessageCharacterList(result, AccountName));

            GameMessageServerName serverNameMessage = new GameMessageServerName(ConfigManager.Config.Server.WorldName);
            Network.EnqueueSend(serverNameMessage);

            State = SessionState.AuthConnected;

            Player = null;
        }

        public class SessionStateChangedEventArgs : EventArgs
        {
            public SessionState NewState { get; }

            public SessionStateChangedEventArgs(SessionState newState)
            {
                NewState = newState;
            }
        }
    }
}
