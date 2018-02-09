using System.IO;
using log4net;

namespace ACE.Server.Network
{
    public class ClientPacket : Packet
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public BinaryReader Payload { get; }
        public PacketHeaderOptional HeaderOptional { get; private set; }

        public ClientPacket(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(stream))
                {
                    Header = new PacketHeader(reader);
                    Data = new MemoryStream(reader.ReadBytes(Header.Size), 0, Header.Size, false, true);
                    Payload = new BinaryReader(Data);
                    HeaderOptional = new PacketHeaderOptional(Payload, Header);
                }
            }

            ReadFragments();
        }

        private void ReadFragments()
        {
            if (Header.HasFlag(PacketHeaderFlags.BlobFragments))
            {
                while (Payload.BaseStream.Position != Payload.BaseStream.Length)
                    Fragments.Add(new ClientPacketFragment(Payload));
            }
        }

        public bool VerifyChecksum(uint issacXor)
        {
            uint fragmentChecksum = 0u;
            foreach (ClientPacketFragment fragment in Fragments)
            {
                fragmentChecksum += fragment.CalculateHash32();
            }

            uint payloadChecksum = HeaderOptional.CalculateHash32() + fragmentChecksum;

            uint finalChecksum = Header.CalculateHash32() + (payloadChecksum ^ issacXor);
            log.DebugFormat("Checksum is calculated as {0} and is {1} in header", finalChecksum, Header.Checksum);
            return finalChecksum == Header.Checksum;
        }
    }
}
