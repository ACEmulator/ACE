using ACE.Common.Cryptography;
using System.IO;

namespace ACE.Network
{
    public class ClientPacketFragment : PacketFragment
    {
        public ClientPacketFragment(BinaryReader payload)
        {
            Header = new PacketFragmentHeader(payload);
            Data = payload.ReadBytes((int)(Header.Size - PacketFragmentHeader.HeaderSize));
        }

        public uint CalculateHash32()
        {
            byte[] fragmentHeaderBytes = Header.GetRaw();
            uint fragmentChecksum = Hash32.Calculate(fragmentHeaderBytes, fragmentHeaderBytes.Length) + Hash32.Calculate(Data, Data.Length);
            return fragmentChecksum;
        }
    }
}
