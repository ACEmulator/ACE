using ACE.Network.Messages;
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
        public GameMessageOpcode Opcode { get; }

        public ServerPacketFragment(ushort group, GameMessageOpcode opcode = GameMessageOpcode.None)
        {
            Opcode = opcode;

            Data = new MemoryStream((int)MaxFragmentDataSize);
            Payload = new BinaryWriter(Data);
            Header = new PacketFragmentHeader()
            {
                Group = group
            };

            if (opcode != GameMessageOpcode.None)
                Payload.Write((uint)opcode);
        }

        public ServerPacketFragment(ushort group, GameMessage message)
        {
            Opcode = message.Opcode;

            Data = message.Data;
            Payload = new BinaryWriter(Data);
            Header = new PacketFragmentHeader()
            {
                Group = group
            };
        }
    }
}
