using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ACE.Common.Cryptography;

namespace ACE.Network.Packets
{
    public class PacketOutboundConnectRequest : ServerPacket
    {
        public PacketOutboundConnectRequest(double serverTime, ulong cookie, uint clientId, byte[] isaacServerSeed, byte[] isaacClientSeed) : base()
        {
            this.Header.Flags = PacketHeaderFlags.ConnectRequest;
            BodyWriter.Write(serverTime); // CConnectHeader.ServerTime
            BodyWriter.Write(cookie); // CConnectHeader.Cookie
            BodyWriter.Write(clientId); // CConnectHeader.NetID
            BodyWriter.Write(isaacServerSeed); // CConnectHeader.OutgoingSeed
            BodyWriter.Write(isaacClientSeed); // CConnectHeader.IncomingSeed
            BodyWriter.Write(0u); // Padding for alignment?
        }
    }
}
