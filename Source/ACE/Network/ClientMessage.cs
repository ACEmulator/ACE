using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network
{
    public class ClientMessage
    {
        public BinaryReader Payload { get; }
        public MemoryStream Data { get; }
        public uint Opcode { get; }

        public ClientMessage(MemoryStream stream)
        {
            Data = stream;
            Payload = new BinaryReader(Data);
            Opcode = Payload.ReadUInt32();
        }

        public ClientMessage(byte[] data)
        {
            Data = new MemoryStream(data);
            Payload = new BinaryReader(Data);
            Opcode = Payload.ReadUInt32();
        }
    }
}
