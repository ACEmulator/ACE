using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ACE.Common.Cryptography;

namespace ACE.Network.Packets
{
    public class ServerSwitch : ServerPacket2
    {
        public ServerSwitch() : base()
        {
            this.Header.Flags = PacketHeaderFlags.EncryptedChecksum | PacketHeaderFlags.ServerSwitch;
        }

        protected override void WriteBody()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write((uint)0x18);
                    writer.Write((uint)0x00);
                    SetBody(stream.ToArray());
                }
            }
        }
    }
}
