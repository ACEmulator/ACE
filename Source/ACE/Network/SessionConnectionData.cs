using ACE.Common.Cryptography;
using ACE.Managers;

namespace ACE.Network
{
    public class SessionConnectionData
    {
        public uint PacketSequence { get; set; }
        public uint FragmentSequence { get; set; }
        public ISAAC IssacClient { get; }
        public ISAAC IssacServer { get; }

        public double ServerTime { get; set; }

        public SessionConnectionData()
        {
            IssacClient = new ISAAC(ISAAC.ClientSeed);
            IssacServer = new ISAAC(ISAAC.ServerSeed);

            ServerTime = WorldManager.PortalYearTicks;
        }
    }
}