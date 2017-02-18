using ACE.Network.Fragments;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network
{
    public class ServerPacketFragment : PacketFragment
    {
        public BinaryWriter Payload { get; }
        public FragmentOpcode Opcode { get; }

        public ServerPacketFragment(ushort group, FragmentOpcode opcode = FragmentOpcode.None)
        {
            Opcode = opcode;

            Data = new MemoryStream((int)MaxFragmentDataSize);
            Payload = new BinaryWriter(Data);
            Header = new PacketFragmentHeader()
            {
                Group = group
            };

            if (opcode != FragmentOpcode.None)
                Payload.Write((uint)opcode);
        }
    }
}
