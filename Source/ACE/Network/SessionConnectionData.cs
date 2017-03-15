using ACE.Common.Cryptography;
using ACE.Common;
using ACE.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
