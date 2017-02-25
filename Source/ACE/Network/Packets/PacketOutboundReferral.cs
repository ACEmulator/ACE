using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ACE.Common.Cryptography;
using ACE.Common;

namespace ACE.Network.Packets
{
    public class PacketOutboundReferral : ServerPacket
    {
        private ulong worldConnectionKey;

        public PacketOutboundReferral(ulong worldConnectionKey) : base()
        {
            this.Header.Flags = PacketHeaderFlags.EncryptedChecksum | PacketHeaderFlags.Referral;
            this.worldConnectionKey = worldConnectionKey;
            BodyWriter.Write(worldConnectionKey);
            BodyWriter.Write((ushort)2);
            BodyWriter.WriteUInt16BE((ushort)ConfigManager.Config.Server.Network.WorldPort);
            BodyWriter.Write(ConfigManager.Host);
            BodyWriter.Write(0ul);
            BodyWriter.Write((ushort)0x18);
            BodyWriter.Write((ushort)0);
            BodyWriter.Write(0u);
        }
    }
}
