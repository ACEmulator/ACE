using ACE.Common.Cryptography;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network
{
    public class ServerPacketFragment2
    {
        public PacketFragmentHeader Header { get; private set; }

        public byte[] Body { get; set; }

        public ServerPacketFragment2()
        {
            Header = new PacketFragmentHeader();
        }

        public uint GetPayload(BinaryWriter writer)
        {
            Header.Size = (ushort)(PacketFragmentHeader.HeaderSize + Body.Length);
            byte[] fragmentHeaderBytes = Header.GetRaw();
            uint fragmentChecksum = Hash32.Calculate(fragmentHeaderBytes, fragmentHeaderBytes.Length) + Hash32.Calculate(Body, Body.Length);
            writer.Write(fragmentHeaderBytes);
            writer.Write(Body);
            return fragmentChecksum;
        }
    }
}
