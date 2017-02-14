using System.Diagnostics;

namespace ACE.Network
{
    public abstract class GameMessage
    {
        public GameMessageOpcode Opcode { get; private set; }

        //TODO: Session is not needed in every message, perhaps should be pulled up and only used where needed.
        protected Session session;
        protected System.IO.BinaryWriter writer;
        private System.IO.MemoryStream data;

        public GameMessage(Session target, GameMessageOpcode opCode)
        {
            Debug.Assert(target.Player != null);
            session = target;
            Opcode = opCode;
        }

        public System.IO.MemoryStream Data
        {
            get
            {
                data = new System.IO.MemoryStream();
                writer = new System.IO.BinaryWriter(data);
                if (Opcode != GameMessageOpcode.None)
                    writer.Write((uint)Opcode);
                WriteBody();
                return data;
            }
        }

        public void Send()
        {
            session.SendWorldFragmentPacket(this);
        }

        protected virtual void WriteBody() { }
    }
}
