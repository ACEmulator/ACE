using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network
{
    public class ClientPacket : Packet
    {
        public BinaryReader Payload { get; }
        public PacketHeaderOptional HeaderOptional { get; private set; }

        public ClientPacket(byte[] data, bool debug = false)
        {
            Direction = (debug ? PacketDirection.Server : PacketDirection.Client);

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
                while (Payload.BaseStream.Position != Payload.BaseStream.Length)
                    Fragments.Add(new ClientPacketFragment(Payload));
        }
    }
}
