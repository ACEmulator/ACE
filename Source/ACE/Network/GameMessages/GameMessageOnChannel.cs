
namespace ACE.Network.GameMessages
{
    public abstract class GameMessageOnChannel
    {
        public GameMessageOpcode Opcode { get; private set; }
        public GameMessageChannel Channel { get; private set; }

        public System.IO.MemoryStream Data { get; private set; }

        protected System.IO.BinaryWriter Writer { get; private set; }

        protected GameMessageOnChannel(GameMessageOpcode opCode, GameMessageChannel channel)
        {
            Opcode = opCode;
            Channel = channel;

            Data = new System.IO.MemoryStream();

            Writer = new System.IO.BinaryWriter(Data);

            if (Opcode != GameMessageOpcode.None)
                Writer.Write((uint)Opcode);
        }
    }
}