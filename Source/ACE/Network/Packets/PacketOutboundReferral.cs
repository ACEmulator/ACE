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

        private string[] sessionIPAddress;

        public PacketOutboundReferral(ulong worldConnectionKey, string[] sessionIPAddress) : base()
        {
            this.Header.Flags = PacketHeaderFlags.EncryptedChecksum | PacketHeaderFlags.Referral;
            this.worldConnectionKey = worldConnectionKey;
            this.sessionIPAddress = sessionIPAddress;
            BodyWriter.Write(worldConnectionKey);
            BodyWriter.Write((ushort)2);
            BodyWriter.WriteUInt16BE((ushort)ConfigManager.Config.Server.Network.WorldPort);

            if (ConfigManager.Config.Server.Network.SendInternalHostOnLocalNetwork &&
                (sessionIPAddress[0] == "10"
                || (sessionIPAddress[0] == "172" && System.Convert.ToInt16(sessionIPAddress[1]) >= 16 && System.Convert.ToInt16(sessionIPAddress[1]) <= 31)
                || (sessionIPAddress[0] == "192" && sessionIPAddress[1] == "168")))
                BodyWriter.Write(ConfigManager.InternalHost);
            else
                BodyWriter.Write(ConfigManager.Host);

            BodyWriter.Write(0ul);
            BodyWriter.Write((ushort)0x18);
            BodyWriter.Write((ushort)0);
            BodyWriter.Write(0u);
        }
    }
}
