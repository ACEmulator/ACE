using ACE.Common.Cryptography;
using ACE.Server.Entity;
using ACE.Server.Network.Sequence;

namespace ACE.Server.Network
{
    public class SessionConnectionData
    {
        public UIntSequence PacketSequence { get; set; }
        public uint FragmentSequence { get; set; }
        public ISAAC IssacClient { get; }
        public ISAAC IssacServer { get; }

        /// <summary>
        /// This is just a wrapper around Timers.PortalYearTicks.<para />
        /// In the future, we may want to consider removing this and referencing Timers.PortalYearTicks directly.
        /// </summary>
        public double ServerTime => Timers.PortalYearTicks;

        public SessionConnectionData()
        {
            IssacClient = new ISAAC(ISAAC.ClientSeed);
            IssacServer = new ISAAC(ISAAC.ServerSeed);
            PacketSequence = new UIntSequence(false);
        }
    }
}
