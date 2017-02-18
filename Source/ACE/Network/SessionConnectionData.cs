using ACE.Common.Cryptography;
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

        public SessionConnectionData(ConnectionType type)
        {
            IssacClient = new ISAAC(type == ConnectionType.Login ? ISAAC.ClientSeed : ISAAC.WorldClientSeed);
            IssacServer = new ISAAC(type == ConnectionType.Login ? ISAAC.ServerSeed : ISAAC.WorldServerSeed);
        }
    }
}
