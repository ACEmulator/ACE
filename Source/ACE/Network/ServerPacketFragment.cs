using System.IO;

using ACE.Common.Cryptography;

namespace ACE.Network
{
    public class ServerPacketFragment : PacketFragment
    {
        //public PacketFragmentHeader Header { get; private set; }

        public byte[] Content { get; set; }

        public ServerPacketFragment()
        {
            Header = new PacketFragmentHeader();
        }

        public uint GetPayload(BinaryWriter writer)
        {
            Header.Size = (ushort)(PacketFragmentHeader.HeaderSize + Content.Length);
            byte[] fragmentHeaderBytes = Header.GetRaw();
            uint fragmentChecksum = Hash32.Calculate(fragmentHeaderBytes, fragmentHeaderBytes.Length) + Hash32.Calculate(Content, Content.Length);
            writer.Write(fragmentHeaderBytes);
            writer.Write(Content);
            return fragmentChecksum;
        }
    }
}
