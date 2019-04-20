using System.IO;

using ACE.Common.Cryptography;

namespace ACE.Server.Network
{
    public class ClientPacketFragment : PacketFragment
    {
        public ClientPacketFragment(BinaryReader payload)
        {
            Header = new PacketFragmentHeader(payload);
            Data = payload.ReadBytes(Header.Size - PacketFragmentHeader.HeaderSize);
        }

        public uint CalculateHash32()
        {
            byte[] fragmentHeaderBytes = Header.GetRaw();
            uint fragmentChecksum = Hash32.Calculate(fragmentHeaderBytes, fragmentHeaderBytes.Length) + Hash32.Calculate(Data, Data.Length);
            return fragmentChecksum;
        }
    }
}
