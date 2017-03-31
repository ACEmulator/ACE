using ACE.Network;
using ACE.Network.Enum;
using System.IO;

namespace ACE.Entity
{
    public class MovementData
    {
        public ushort CurrentStyle = 0;
        public ushort ForwardCommand = 0;
        public ushort SideStepCommand = 0;
        public ushort TurnCommand = 0;
        public float TurnSpeed = 0.00f;
        public float ForwardSpeed = 0.00f;
        public float SideStepSpeed = 0.00f;
        public MovementStateFlag MovementStateFlag = 0;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((uint)this.MovementStateFlag);

            if ((this.MovementStateFlag & MovementStateFlag.CurrentStyle) != 0)
                writer.Write((uint)this.CurrentStyle);

            if ((this.MovementStateFlag & MovementStateFlag.ForwardCommand) != 0)
                writer.Write((uint)this.ForwardCommand);

            if ((this.MovementStateFlag & MovementStateFlag.ForwardSpeed) != 0)
                writer.Write((float)this.ForwardSpeed);

            if ((this.MovementStateFlag & MovementStateFlag.SideStepCommand) != 0)
                writer.Write((uint)this.SideStepCommand);

            if ((this.MovementStateFlag & MovementStateFlag.SideStepSpeed) != 0)
                writer.Write((float)this.SideStepSpeed);

            if ((this.MovementStateFlag & MovementStateFlag.TurnCommand) != 0)
                writer.Write((uint)this.TurnCommand);

            if ((this.MovementStateFlag & MovementStateFlag.TurnSpeed) != 0)
                writer.Write((float)this.TurnSpeed);
            writer.Align();
        }
    }
}