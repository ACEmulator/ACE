using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ACE.Common.Cryptography;

namespace ACE.Network.Packets
{
    public class PacketOutboundServerSwitch : ServerPacket
    {
        public PacketOutboundServerSwitch() : base()
        {
            this.Header.Flags = PacketHeaderFlags.EncryptedChecksum | PacketHeaderFlags.ServerSwitch;
            BodyWriter.Write((uint)0x18); // This value is currently the hard coded Server ID. It can be something different...
            BodyWriter.Write((uint)0x00);
        }
    }
}
