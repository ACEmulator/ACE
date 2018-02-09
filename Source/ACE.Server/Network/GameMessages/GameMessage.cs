namespace ACE.Server.Network.GameMessages
{
    public abstract class GameMessage
    {
        public GameMessageOpcode Opcode { get; }

        public System.IO.MemoryStream Data { get; }

        public GameMessageGroup Group { get; }

        protected System.IO.BinaryWriter Writer { get; }

        protected GameMessage(GameMessageOpcode opCode, GameMessageGroup group)
        {
            Opcode = opCode;

            Group = group;

            Data = new System.IO.MemoryStream();

            Writer = new System.IO.BinaryWriter(Data);

            if (Opcode != GameMessageOpcode.None)
                Writer.Write((uint)Opcode);
        }
    }
}
