using ACE.Network;
using ACE.Network.Enum;
using System.IO;

namespace ACE.Entity
{
    public class MotionData
    {
        public ushort CurrentStyle = 0;
        public ushort ForwardCommand = 0;
        public ushort SideStepCommand = 0;
        public ushort TurnCommand = 0;
        public float  TurnSpeed = 0.00f;
        public float ForwardSpeed = 0.00f;
        public float SideStepSpeed = 0.00f;
        public uint Bitfield = 0;
        public MotionStateFlag MotionStateFlag = 0;

        public void Serialize(BinaryWriter writer)
        {

            if ((this.MotionStateFlag & MotionStateFlag.CurrentStyle) != 0)
                writer.Write((uint)this.CurrentStyle);

            if ((this.MotionStateFlag & MotionStateFlag.ForwardCommand) != 0)
                writer.Write((uint)this.ForwardCommand);

            if ((this.MotionStateFlag & MotionStateFlag.ForwardSpeed) != 0)
                writer.Write((float)this.ForwardSpeed);

            if ((this.MotionStateFlag & MotionStateFlag.SideStepCommand) != 0)
                writer.Write((uint)this.SideStepCommand);

            if ((this.MotionStateFlag & MotionStateFlag.SideStepSpeed) != 0)
                writer.Write((float)this.SideStepSpeed);

            if ((this.MotionStateFlag & MotionStateFlag.CurrentStyle) != 0)
                writer.Write((uint)this.CurrentStyle);

            if ((this.MotionStateFlag & MotionStateFlag.TurnCommand) != 0)
                writer.Write((uint)this.TurnCommand);

            if ((this.MotionStateFlag & MotionStateFlag.TurnSpeed) != 0)
                writer.Write((float)this.TurnSpeed);
            writer.Align();
        }
    }
}
