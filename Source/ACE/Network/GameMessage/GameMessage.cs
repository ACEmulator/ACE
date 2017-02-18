using System.Diagnostics;

namespace ACE.Network
{
    public abstract class GameMessage
    {
        public GameMessageOpcode Opcode { get; private set; }

        protected System.IO.BinaryWriter writer;
        private System.IO.MemoryStream data;

        protected GameMessage(GameMessageOpcode opCode)
        {
            Opcode = opCode;
            data = new System.IO.MemoryStream();
            writer = new System.IO.BinaryWriter(data);
            if (Opcode != GameMessageOpcode.None)
                writer.Write((uint)Opcode);
        }

        public System.IO.MemoryStream Data
        {
            get
            {
                return data;
            }
        }
    }
}