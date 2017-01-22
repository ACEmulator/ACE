using ACE.Cryptography;
using System.Diagnostics;
using System.Net;

namespace ACE.Network
{
    public class Session
    {
        public uint Id { get; private set; }
        public string Account { get; private set; }
        public bool Authenticated { get; private set; }

        public double ServerTime { get; private set; }

        public IPEndPoint EndPoint { get; }
        public uint PacketSequence { get; set; }
        public uint FragmentSequence { get; set; }

        private ISAAC issacClient;
        private ISAAC issacServer;

        public Session(IPEndPoint endPoint)
        {
            EndPoint    = endPoint;
            issacClient = new ISAAC(ISAAC.ClientSeed);
            issacServer = new ISAAC(ISAAC.ServerSeed);
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
            ServerTime += lastTick;
        }

        public uint GetIssacValue(PacketDirection direction) { return (direction == PacketDirection.Client ? issacClient.GetOffset() : issacServer.GetOffset()); }

        public void SendCharacterError(CharacterError error)
        {
            var characterError         = new ServerPacket(0x0B, PacketHeaderFlags.EncryptedChecksum);
            var characterErrorFragment = new ServerPacketFragment(9, FragmentOpcode.CharacterError);
            characterErrorFragment.Payload.Write((uint)error);
            characterError.Fragments.Add(characterErrorFragment);

            NetworkManager.SendLoginPacket(characterError, this);
        }
    }
}
