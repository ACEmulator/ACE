using System.IO;

namespace ACE.Server.Network
{
    public class ClientMessage
    {
        public MemoryStream Data { get; }

        public BinaryReader Payload { get; }

        public uint Opcode { get; }

        /// <exception cref="EndOfStreamException">stream must be at least 4 bytes in length remaining to read</exception>
        public ClientMessage(MemoryStream stream)
        {
            Data = stream;
            Payload = new BinaryReader(Data);
            Opcode = Payload.ReadUInt32();
        }

        /// <exception cref="EndOfStreamException">data must be at least 4 bytes in length</exception>
        public ClientMessage(byte[] data)
        {
            Data = new MemoryStream(data);
            Payload = new BinaryReader(Data);
            Opcode = Payload.ReadUInt32();
        }
    }
}
