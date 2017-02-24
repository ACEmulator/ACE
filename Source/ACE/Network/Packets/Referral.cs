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
    public class Referral : ServerPacket2
    {
        private ulong worldConnectionKey;

        public Referral(ulong worldConnectionKey) : base()
        {
            this.Header.Flags = PacketHeaderFlags.EncryptedChecksum | PacketHeaderFlags.Referral;
            this.worldConnectionKey = worldConnectionKey;
        }

        protected override void WriteBody()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(worldConnectionKey);
                    writer.Write((ushort)2);
                    writer.WriteUInt16BE((ushort)ConfigManager.Config.Server.Network.WorldPort);
                    writer.Write(ConfigManager.Host);
                    writer.Write(0ul);
                    writer.Write((ushort)0x18);
                    writer.Write((ushort)0);
                    writer.Write(0u);
                    SetBody(stream.ToArray());
                }
            }
        }
    }
}
