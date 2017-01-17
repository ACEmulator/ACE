using ACE.Cryptography;
using System.Net;

namespace ACE.Network
{
    public class Session
    {
        public IPEndPoint EndPoint { get; }
        public uint PacketSequence { get; set; }
        public uint FragmentSequence { get; set; }

        public double ServerTime { get; private set; }

        private ISAAC issacClient;
        private ISAAC issacServer;

        public Session(IPEndPoint endPoint)
        {
            EndPoint    = endPoint;
            issacClient = new ISAAC(ISAAC.ClientSeed);
            issacServer = new ISAAC(ISAAC.ServerSeed);
        }

        public void Update(double lastTick)
        {
            ServerTime += lastTick;
        }

        public uint GetIssacValue(PacketDirection direction) { return (direction == PacketDirection.Client ? issacClient.GetOffset() : issacServer.GetOffset()); }
    }
}
