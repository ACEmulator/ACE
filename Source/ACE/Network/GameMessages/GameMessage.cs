
namespace ACE.Network.GameMessages
{
    public abstract class GameMessage
    {
        public GameMessageOpcode Opcode { get; private set; }

        public System.IO.MemoryStream Data { get; private set; }

        protected System.IO.BinaryWriter Writer { get; private set; }

        protected GameMessage(GameMessageOpcode opCode)
        {
            Opcode = opCode;

            Data = new System.IO.MemoryStream();

            Writer = new System.IO.BinaryWriter(Data);

            if (Opcode != GameMessageOpcode.None)
                Writer.Write((uint)Opcode);
        }
    }
}