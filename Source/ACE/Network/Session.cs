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

namespace ACE.Network
{
    public class Session
    {
        public uint Id { get; private set; }
        public string Account { get; private set; }
        public AccessLevel AccessLevel { get; private set; }
        public SessionState State { get; set; }

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

        public ConcurrentDictionary<uint /*seq*/, CachedPacket> CachedPackets { get; } = new ConcurrentDictionary<uint, CachedPacket>();

        public NetworkBuffer LoginBuffer { get; private set; }
        public NetworkBuffer WorldBuffer { get; private set; }

        public Session(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
            LoginBuffer = new NetworkBuffer(this, ConnectionType.Login);
            WorldBuffer = new NetworkBuffer(this, ConnectionType.World);
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
            if (WorldConnection != null)
                WorldConnection.ServerTime += lastTick;
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
    }
}
