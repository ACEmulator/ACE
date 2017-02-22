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

        public NetworkBuffer LoginBuffer { get; set; }
        public NetworkBuffer WorldBuffer { get; set; }

        public Session(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
            LoginBuffer = new NetworkBuffer(this, ConnectionType.Login);
            WorldBuffer = new NetworkBuffer(this, ConnectionType.World);
        }

        private void StateChanged(SessionState newState)
        {
            Console.WriteLine("New State " + newState);
            if(newState == SessionState.AuthConnected)
            {
                LoginBuffer.SetTimers();
            }
            if (newState == SessionState.AuthConnectResponse)
            {
                LoginBuffer.StartResync();
            }
            if (newState == SessionState.WorldConnected)
            {
                //WorldBuffer.StartTimers();
            }
        }

        public void SetAccount(uint accountId, string account, AccessLevel accountAccesslevel)
        {
            Id      = accountId;
            Account = account;
            AccessLevel = accountAccesslevel;
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

            var buffer = (type == ConnectionType.Login) ? LoginBuffer : WorldBuffer;

            buffer.LastReceivedSequence = packet.Header.Sequence;

            if (packet.Header.HasFlag(PacketHeaderFlags.EchoRequest))
            {
                buffer.FlagEcho(packet.HeaderOptional.ClientTime);
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.AckSequence))
            {
                buffer.AcknowledgeSequence(packet.HeaderOptional.Sequence);
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.TimeSynch))
            {
                //Do something with this...
                //Based on network traces these are not 1:1.  Server seems to send them every 20 seconds per port.
                //Client seems to send them alternatingly every 2 or 4 seconds per port.
                //NetworkBuffer will send this at a 20 second time interval.  I don't know what to do with these when we receive them at this point.
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.RequestRetransmit))
            {
                foreach (uint sequence in packet.HeaderOptional.RetransmitData)
                {
                    buffer.Retransmit(sequence);
                }
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.LoginRequest))
            {
                AuthenticationHandler.HandleLoginRequest(packet, this);
                return;
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.WorldLoginRequest))
            {
                AuthenticationHandler.HandleWorldLoginRequest(packet, this);
                return;
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.ConnectResponse))
            {
                if (type == ConnectionType.Login)
                {
                    AuthenticationHandler.HandleConnectResponse(packet, this);
                    return;
                }

                AuthenticationHandler.HandleWorldConnectResponse(packet, this);
                return;
            }

            if (packet.Header.HasFlag(PacketHeaderFlags.Disconnect))
                HandleDisconnectResponse(packet);

            foreach (ClientPacketFragment fragment in packet.Fragments)
            {
                PacketManager.HandleClientFragment(fragment, this);
            }
        }

        private void HandleDisconnectResponse(ClientPacket packet)
        {
            if (Player != null)
                Player.Logout();

            WorldManager.Remove(this);
        }
    }
}
