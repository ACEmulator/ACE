using System;
using System.IO;

using ACE.Common.Cryptography;

namespace ACE.Server.Network
{
    public class ServerPacketFragment : PacketFragment
    {
        public ServerPacketFragment(byte[] data)
        {
            Header = new PacketFragmentHeader();
            Data = data;
        }

        public uint GetPayload(BinaryWriter writer)
        {
            Header.Size = (ushort)(PacketFragmentHeader.HeaderSize + Data.Length);
            byte[] fragmentHeaderBytes = Header.GetRaw();
            uint fragmentChecksum = Hash32.Calculate(fragmentHeaderBytes, fragmentHeaderBytes.Length) + Hash32.Calculate(Data, Data.Length);
            writer.Write(fragmentHeaderBytes);
            writer.Write(Data);
            return fragmentChecksum;
        }

        public uint AddPayloadToBuffer(byte[] buffer, ref int offset)
        {
            Header.Size = (ushort)(PacketFragmentHeader.HeaderSize + Data.Length);

            byte[] fragmentHeaderBytes = Header.GetRaw();

            Buffer.BlockCopy(fragmentHeaderBytes, 0, buffer, offset, fragmentHeaderBytes.Length);
            offset += fragmentHeaderBytes.Length;

            Buffer.BlockCopy(Data, 0, buffer, offset, Data.Length);
            offset += Data.Length;

            return Hash32.Calculate(fragmentHeaderBytes, fragmentHeaderBytes.Length) + Hash32.Calculate(Data, Data.Length);
        }
    }
}
