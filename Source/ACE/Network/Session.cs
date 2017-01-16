using ACE.Cryptography;
using System.Net;

namespace ACE.Network
{
    public class Session
    {
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

        public uint GetIssacValue(PacketDirection direction, uint sequence)
        {
            if (sequence >= 2u)
                return (direction == PacketDirection.Client ? issacClient.GetOffset(sequence) : issacServer.GetOffset(sequence));
            return 0u;
        }
    }
}
