using ACE.Entity.Enum;

namespace ACE.Server.Physics.Command
{
    public class CmdStruct
    {
        public uint[] Args = new uint[64];
        public uint Size;
        public uint Curr;
        public MotionCommand Command;
    }
}
