using System;

using ACE.Common.Cryptography;
using ACE.Server.Network.Sequence;

namespace ACE.Server.Network
{
    public class SessionConnectionData
    {
        /// <summary>
        /// random shared 64 bit secret
        /// for final phase of login and connect handshake
        /// clear text transmitted S2C and C2S during three-way handshake
        /// This gives server a high degree of confidence that whoever is replying to the connection request is the same as the one who logged in.
        /// It's assumed that in most cases only the intended recipient of the cookie, the one who logged in,
        /// knows the cookie and can prove it in a ConnectionResponse packet.
        /// </summary>
        public ulong ConnectionCookie { get; private set; }

        /// <summary>
        /// random shared 32 bit secret
        /// initialization vector for C2S CRC stream cipher
        /// The starting point of a 32 bit wheel.
        /// clear text transmitted S2C during during three-way handshake
        /// </summary>
        public byte[] ClientSeed { get; private set; }
        /// <summary>
        /// random shared 32 bit secret
        /// initialization vector for S2C CRC stream cipher
        /// The starting point of a 32 bit wheel.
        /// clear text transmitted S2C during during three-way handshake
        /// </summary>
        public byte[] ServerSeed { get; private set; }

        public UIntSequence PacketSequence { get; set; }
        public uint FragmentSequence { get; set; }

        /// <summary>
        /// Client->Server stream cipher wrapper
        /// </summary>
        public CryptoSystem CryptoClient = null;
        /// <summary>
        /// Server->Client stream cipher
        /// </summary>
        public ISAAC IssacServer = null;

        public SessionConnectionData()
        {
            // since the network processor is single threaded this can instantiate the .NET Core System.Random class without locking
            Random rand = new Random();

            // the client and server seeds determine where on the 32 bit wheel the stream cipher begins
            // by picking a random initialization vector it makes it more difficult for an adversary to forge packets
            ClientSeed = new byte[4];
            ServerSeed = new byte[4];

            rand.NextBytes(ClientSeed);
            rand.NextBytes(ServerSeed);

            CryptoClient = new CryptoSystem(ClientSeed);
            IssacServer = new ISAAC(ServerSeed);

            byte[] bytes = new byte[8];
            rand.NextBytes(bytes);
            ConnectionCookie = BitConverter.ToUInt64(bytes, 0);

            PacketSequence = new UIntSequence(false);
        }

        /// <summary>
        /// Discard the references to the byte arrays so the memory can be freed up by GC.
        /// After the seeds are sent to the client and the two ISAAC objects are constructed they are no longer needed.
        /// </summary>
        public void DiscardSeeds()
        {
            ClientSeed = null;
            ServerSeed = null;
        }

        public override string ToString()
        {
            return $"Seeds: [Client {BitConverter.ToString(ClientSeed).Replace("-","")}, Server {BitConverter.ToString(ServerSeed).Replace("-", "")}]";
        }
    }
}
