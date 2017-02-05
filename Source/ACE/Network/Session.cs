using ACE.Cryptography;
using ACE.Entity;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;

namespace ACE.Network
{
    public class SessionConnectionData
    {
        public uint PacketSequence { get; set; }
        public uint FragmentSequence { get; set; }
        public ISAAC IssacClient { get; }
        public ISAAC IssacServer { get; }

        public double ServerTime { get; set; }

        public SessionConnectionData(ConnectionType type)
        {
            IssacClient = new ISAAC(type == ConnectionType.Login ? ISAAC.ClientSeed : ISAAC.WorldClientSeed);
            IssacServer = new ISAAC(type == ConnectionType.Login ? ISAAC.ServerSeed : ISAAC.WorldServerSeed);
        }
    }

    public class CachedCharacter
    {
        public uint LowGuid { get; }
        public byte SlotId { get; }
        public string Name { get; }
        public ulong DeleteTime { get; }

        public CachedCharacter(uint lowGuid, byte slotId, string name, ulong deleteTime)
        {
            LowGuid = lowGuid;
            SlotId  = slotId;
            Name    = name;
            DeleteTime = deleteTime;
        }
    }

    public class Session
    {
        public uint Id { get; private set; }
        public string Account { get; private set; }
        public SessionState State { get; set; }

        public List<CachedCharacter> CachedCharacters { get; } = new List<CachedCharacter>();
        public CachedCharacter CharacterRequested { get; set; }
        public Player Character { get; set; }

        // connection related
        public IPEndPoint EndPoint { get; }
        public SessionConnectionData LoginConnection { get; } = new SessionConnectionData(ConnectionType.Login);
        public SessionConnectionData WorldConnection { get; set; }
        public ulong WorldConnectionKey { get; set; }
        public uint GameEventSequence { get; set; }

        public ConcurrentDictionary<uint /*seq*/, CachedPacket> CachedPackets { get; } = new ConcurrentDictionary<uint, CachedPacket>();

        public Session(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
        }

        public void SetAccount(uint accountId, string account)
        {
            Id      = accountId;
            Account = account;
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
            var characterErrorFragment = new ServerPacketFragment(0x09, FragmentOpcode.CharacterError);
            characterErrorFragment.Payload.Write((uint)error);
            characterError.Fragments.Add(characterErrorFragment);

            NetworkManager.SendPacket(ConnectionType.Login, characterError, this);
        }
    }
}
