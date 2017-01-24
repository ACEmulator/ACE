using ACE.Cryptography;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace ACE.Network
{
    public class SeasonConnectionData
    {
        public uint PacketSequence { get; set; }
        public uint FragmentSequence { get; set; }
        public ISAAC IssacClient { get; }
        public ISAAC IssacServer { get; }

        public double ServerTime { get; set; }

        public SeasonConnectionData(ConnectionType type)
        {
            IssacClient = new ISAAC(type == ConnectionType.Login ? ISAAC.ClientSeed : ISAAC.WorldClientSeed);
            IssacServer = new ISAAC(type == ConnectionType.Login ? ISAAC.ServerSeed : ISAAC.WorldServerSeed);
        }
    }

    public class Session
    {
        public uint Id { get; private set; }
        public string Account { get; private set; }
        public bool Authenticated { get; private set; }

        // contains references to the character guid by client slot id
        public Dictionary<byte /*slotId*/, uint /*characterGuid*/> CharacterSlots { get; } = new Dictionary<byte, uint>();

        public Dictionary<uint /*characterGuid*/, string /* characterName */> CharacterNames { get; } = new Dictionary<uint, string>();

        // connection related
        public IPEndPoint EndPoint { get; }
        public SeasonConnectionData LoginConnection { get; } = new SeasonConnectionData(ConnectionType.Login);
        public SeasonConnectionData WorldConnection { get; set; }
        public ulong WorldConnectionKey { get; set; }

        public Session(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
        }

        public void SetAccount(uint accountId, string account)
        {
            Debug.Assert(!Authenticated);

            Id            = accountId;
            Account       = account;
            Authenticated = true;
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
            var characterErrorFragment = new ServerPacketFragment(9, FragmentOpcode.CharacterError);
            characterErrorFragment.Payload.Write((uint)error);
            characterError.Fragments.Add(characterErrorFragment);

            NetworkManager.SendPacket(ConnectionType.Login, characterError, this);
        }
    }
}
