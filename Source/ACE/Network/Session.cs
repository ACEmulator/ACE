using System;
using System.Collections.Generic;
using System.Net;

using ACE.Common;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using ACE.Managers;

namespace ACE.Network
{
    public class Session
    {
        public uint Id { get; private set; }
        public string Account { get; private set; }
        public AccessLevel AccessLevel { get; private set; }
        private SessionState state;
        public SessionState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                if (state == SessionState.WorldLoginRequest)
                    this.WorldLoginRequested();
            }
        }

        public List<CachedCharacter> CachedCharacters { get; } = new List<CachedCharacter>();
        public CachedCharacter CharacterRequested { get; set; }
        public Player Player { get; set; }

        private DateTime logOffRequestTime;

        // connection related
        public IPEndPoint EndPoint { get; }
        public ulong WorldConnectionKey { get; set; }
        public uint GameEventSequence { get; set; }
        public byte UpdateAttributeSequence { get; set; }
        public byte UpdateSkillSequence { get; set; }
        public byte UpdatePropertyInt64Sequence { get; set; }
        public byte UpdatePropertyIntSequence { get; set; }
        public byte UpdatePropertyStringSequence { get; set; }
        public byte UpdatePropertyBoolSequence { get; set; }
        public byte UpdatePropertyDoubleSequence { get; set; } 

        public NetworkSession LoginSession { get; set; }
        public NetworkSession WorldSession { get; set; }

        public Session(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
            LoginSession = new NetworkSession(this, ConnectionType.Login);
        }

        private void WorldLoginRequested()
        {
            WorldSession = new NetworkSession(this, ConnectionType.World);
            Player = new Player(this);
            CharacterRequested = null;

            GameEventSequence = 0;
            UpdateAttributeSequence = 0;
            UpdateSkillSequence = 0;
            UpdatePropertyInt64Sequence = 0;
            UpdatePropertyIntSequence = 0;
            UpdatePropertyStringSequence = 0;
            UpdatePropertyBoolSequence = 0;
            UpdatePropertyDoubleSequence = 0;
        }

        public void SetAccount(uint accountId, string account, AccessLevel accountAccesslevel)
        {
            Id      = accountId;
            Account = account;
            AccessLevel = accountAccesslevel;
        }

        public void UpdateCachedCharacters(IEnumerable<CachedCharacter> characters)
        {
            CachedCharacters.Clear();
            foreach (var character in characters)
            {
                CachedCharacters.Add(character);
            }
        }

        public void Update(double lastTick)
        {
            LoginSession.Update(lastTick);
            if (WorldSession != null)
            {
                WorldSession.Update(lastTick);
            }

            // Live server seemed to take about 6 seconds. 4 seconds is nice because it has smooth animation, and saves the user 2 seconds every logoff
            // This could be made 0 for instant logoffs.

            if (logOffRequestTime != DateTime.MinValue && logOffRequestTime.AddSeconds(6) <= DateTime.UtcNow)
            {
                logOffRequestTime = DateTime.MinValue;
                SendFinalLogOffMessages();
            }
        }

        public uint GetIssacValue(PacketDirection direction, ConnectionType type)
        {
            var session = (type == ConnectionType.Login ? LoginSession : WorldSession);
            return (direction == PacketDirection.Client ? session.ConnectionData.IssacClient.GetOffset() : session.ConnectionData.IssacServer.GetOffset());
        }

        public void SendCharacterError(CharacterError error)
        {
            LoginSession.EnqueueSend(new GameMessageCharacterError(error));
        }

        private bool CheckState(ClientPacket packet)
        {
            if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest) && State != SessionState.AuthLoginRequest)
                return false;

            if (packet.Header.HasFlag(PacketHeaderFlags.ConnectResponse) && State != SessionState.AuthConnectResponse && State != SessionState.WorldConnectResponse)
                return false;

            if (packet.Header.HasFlag(PacketHeaderFlags.AckSequence | PacketHeaderFlags.TimeSynch | PacketHeaderFlags.EchoRequest | PacketHeaderFlags.Flow) && State == SessionState.AuthLoginRequest)
                return false;

            if (packet.Header.HasFlag(PacketHeaderFlags.WorldLoginRequest) && State != SessionState.WorldLoginRequest)
                return false;

            return true;
        }

        public void HandlePacket(ConnectionType type, ClientPacket packet)
        {
            if (!CheckState(packet))
            {
                // server treats all packets sent during the first 30 seconds as invalid packets due to server crash, this will move clients to the disconnect screen
                if (DateTime.UtcNow < WorldManager.WorldStartTime.AddSeconds(30d))
                    SendCharacterError(CharacterError.ServerCrash);
                return;
            }

            var buffer = (type == ConnectionType.Login) ? LoginSession : WorldSession;
            //Prevent crash when world is not initialized yet.  Need to look at this closer as I think there are some changes needed to state handling/transitions.
            if(buffer != null)
                buffer.HandlePacket(packet);

            if (packet.Header.HasFlag(PacketHeaderFlags.Disconnect))
                HandleDisconnectResponse();
        }

        private void HandleDisconnectResponse()
        {
            if (Player != null)
                Player.Logout(true);

            WorldManager.Remove(this);
        }

        public void LogOffPlayer()
        {
            Player.Logout();

            logOffRequestTime = DateTime.UtcNow;
        }

        private async void SendFinalLogOffMessages()
        {
            WorldSession.EnqueueSend(new GameMessageCharacterLogOff());

            var result = await DatabaseManager.Character.GetByAccount(Id);
            UpdateCachedCharacters(result);
            WorldSession.EnqueueSend(new GameMessageCharacterList(result, Account));

            GameMessageServerName serverNameMessage = new GameMessageServerName(ConfigManager.Config.Server.WorldName);
            WorldSession.EnqueueSend(serverNameMessage);

            WorldSession.Flush();
            WorldSession = null;

            State = SessionState.AuthConnected;

            Player = null;
        }
    }
}
