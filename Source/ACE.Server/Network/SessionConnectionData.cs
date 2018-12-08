using ACE.Common.Cryptography;
using ACE.Server.Entity;
using ACE.Server.Network.Sequence;

namespace ACE.Server.Network
{
    public class SessionConnectionData
    {
        /// <summary>
        /// random shared secret
        /// for final phase of login and connect handshake
        /// clear text transmitted S2C and C2S during three-way handshake
        /// </summary>
        public ulong ConnectionCookie { get; set; }

        /// <summary>
        /// random shared secret
        /// initialization vector for C2S CRC stream cipher
        /// 64 bit keyspace. this is the starting point of that wheel
        /// clear text transmitted S2C during during three-way handshake
        /// </summary>
        public byte[] ClientSeed { get; set; }
        /// <summary>
        /// random shared secret
        /// initialization vector for S2C CRC stream cipher
        /// 64 bit keyspace. this is the starting point of that wheel
        /// clear text transmitted S2C during during three-way handshake
        /// </summary>
        public byte[] ServerSeed { get; set; }

        public UIntSequence PacketSequence { get; set; }
        public uint FragmentSequence { get; set; }

#if NETDIAG
        public ISAAC IssacClient { get; set; }
#else
        public ISAAC IssacClient { get; }
#endif
        public ISAAC IssacServer { get; }

        /// <summary>
        /// This is just a wrapper around Timers.PortalYearTicks.<para />
        /// In the future, we may want to consider removing this and referencing Timers.PortalYearTicks directly.
        /// </summary>
        public double ServerTime => Timers.PortalYearTicks;

        public SessionConnectionData()
        {
            ClientSeed = new byte[4];
            ServerSeed = new byte[4];

            Physics.Common.Random.NextBytes(ClientSeed);
            Physics.Common.Random.NextBytes(ServerSeed);

            IssacClient = new ISAAC(ClientSeed);
            IssacServer = new ISAAC(ServerSeed);

            PacketSequence = new UIntSequence(false);
        }
    }
}
