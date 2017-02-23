using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;

using ACE.Common.Cryptography;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Managers;
using System;
using ACE.Managers;
using ACE.Network.Handlers;

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
                StateChanged(value);
            }
        }

        public List<CachedCharacter> CachedCharacters { get; } = new List<CachedCharacter>();
        public CachedCharacter CharacterRequested { get; set; }
        public Player Player { get; set; }

        // connection related
        public IPEndPoint EndPoint { get; }
        public SessionConnectionData LoginConnection { get; } = new SessionConnectionData(ConnectionType.Login);
        public SessionConnectionData WorldConnection { get; set; }
        public ulong WorldConnectionKey { get; set; }
        public uint GameEventSequence { get; set; }
        public byte UpdateAttributeSequence { get; set; } = 0x0;
        public byte UpdateSkillSequence { get; set; } = 0x0;
        public byte UpdatePropertyInt64Sequence { get; set; } = 0x0;
        public byte UpdatePropertyIntSequence { get; set; } = 0x0;
        public byte UpdatePropertyStringSequence { get; set; } = 0x0;
        public byte UpdatePropertyBoolSequence { get; set; } = 0x0;
        public byte UpdatePropertyDoubleSequence { get; set; } = 0x0;

        public SessionNetworkManager LoginBuffer { get; set; }
        public SessionNetworkManager WorldBuffer { get; set; }

        public Session(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
            LoginBuffer = new SessionNetworkManager(this, ConnectionType.Login);
            WorldBuffer = new SessionNetworkManager(this, ConnectionType.World);
        }

        private void StateChanged(SessionState newState)
        {
            Console.WriteLine("New State " + newState);
            switch(newState)
            {
                case SessionState.AuthLoginRequest:
                    
                    break;
                case SessionState.AuthConnectResponse:
                    
                    break;
                case SessionState.AuthConnected:
                    LoginBuffer.SetTimers();
                    LoginBuffer.StartResync();
                    break;
                case SessionState.WorldLoginRequest:
                    
                    break;
                case SessionState.WorldConnectResponse:
                    
                    break;
                case SessionState.WorldConnected:
                    WorldBuffer.SetTimers();
                    WorldBuffer.StartResync();
                    break;
            }
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
            LoginConnection.ServerTime += lastTick;
            LoginBuffer.Update();
            if (WorldConnection != null)
            {
                WorldConnection.ServerTime += lastTick;
                WorldBuffer.Update();
            }
        }

        public uint GetIssacValue(PacketDirection direction, ConnectionType type)
        {
            var connectionData = (type == ConnectionType.Login ? LoginConnection : WorldConnection);
            return (direction == PacketDirection.Client ? connectionData.IssacClient.GetOffset() : connectionData.IssacServer.GetOffset());
        }

        public void SendCharacterError(CharacterError error)
        {
            var characterError         = new ServerPacket(0x0B, PacketHeaderFlags.EncryptedChecksum);
            var characterErrorFragment = new ServerPacketFragment(0x09, GameMessageOpcode.CharacterError);
            characterErrorFragment.Payload.Write((uint)error);
            characterError.Fragments.Add(characterErrorFragment);

            NetworkManager.SendPacket(ConnectionType.Login, characterError, this);
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
                if (DateTime.Now < WorldManager.WorldStartTime.AddSeconds(30d))
                    SendCharacterError(CharacterError.ServerCrash);
                return;
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.Disconnect))
                HandleDisconnectResponse(packet);

            var buffer = (type == ConnectionType.Login) ? LoginBuffer : WorldBuffer;

            buffer.HandlePacket(packet);
        }

        private void HandleDisconnectResponse(ClientPacket packet)
        {
            if (Player != null)
                Player.Logout();

            WorldManager.Remove(this);
        }
    }
}
