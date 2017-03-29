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
    }
}
