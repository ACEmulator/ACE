using ACE.Network;
using ACE.Network.Enum;
using System.IO;

namespace ACE.Entity
{
    public class MotionData
    {
        public ushort CurrentStyle;
        public ushort ForwardCommand;
        public ushort SideStepCommand;
        public ushort TurnCommand;
        public float  TurnSpeed;
        public float ForwardSpeed;
        public float SideStepSpeed;
        public uint Bitfield;
        public MotionStateFlag MotionStateFlag;
        public MotionData motionData = new MotionData();

        public void Initialize()
        {
            
        }


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
