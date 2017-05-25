using ACE.Network.Enum;
using System.IO;

namespace ACE.Entity
{
    public class MovementData
    {
        public MovementStateFlag MovementStateFlag { get; private set; } = 0;

        private ushort currentStyle = 0;
        public ushort CurrentStyle
        {
            get
            {
                return currentStyle;
            }
            set
            {
                currentStyle = value;
                MovementStateFlag |= MovementStateFlag.CurrentStyle;
            }
        }

        private ushort forwardCommand = 0;
        public ushort ForwardCommand
        {
            get
            {
                return forwardCommand;
            }
            set
            {
                forwardCommand = value;
                MovementStateFlag |= MovementStateFlag.ForwardCommand;
            }
        }

        private ushort sideStepCommand = 0;
        public ushort SideStepCommand
        {
            get
            {
                return sideStepCommand;
            }
            set
            {
                sideStepCommand = value;
                MovementStateFlag |= MovementStateFlag.SideStepCommand;
            }
        }

        private ushort turnCommand = 0;
        public ushort TurnCommand
        {
            get
            {
                return turnCommand;
            }
            set
            {
                turnCommand = value;
                MovementStateFlag |= MovementStateFlag.TurnCommand;
            }
        }

        private float turnSpeed = 0f;
        public float TurnSpeed
        {
            get
            {
                return turnSpeed;
            }
            set
            {
                turnSpeed = value;
                MovementStateFlag |= MovementStateFlag.TurnSpeed;
            }
        }

        private float forwardSpeed = 0f;
        public float ForwardSpeed
        {
            get
            {
                return forwardSpeed;
            }
            set
            {
                forwardSpeed = value;
                MovementStateFlag |= MovementStateFlag.ForwardSpeed;
            }
        }

        private float sideStepSpeed = 0f;
        public float SideStepSpeed
        {
            get
            {
                return sideStepSpeed;
            }
            set
            {
                sideStepSpeed = value;
                MovementStateFlag |= MovementStateFlag.SideStepSpeed;
            }
        }

        public void Serialize(BinaryWriter writer)
        {
            if ((MovementStateFlag & MovementStateFlag.CurrentStyle) != 0)
                writer.Write((uint)CurrentStyle);

            if ((MovementStateFlag & MovementStateFlag.ForwardCommand) != 0)
                writer.Write((uint)ForwardCommand);

            if ((MovementStateFlag & MovementStateFlag.ForwardSpeed) != 0)
                writer.Write((float)ForwardSpeed);

            if ((MovementStateFlag & MovementStateFlag.SideStepCommand) != 0)
                writer.Write((uint)SideStepCommand);

            if ((MovementStateFlag & MovementStateFlag.SideStepSpeed) != 0)
                writer.Write((float)SideStepSpeed);

            if ((MovementStateFlag & MovementStateFlag.TurnCommand) != 0)
                writer.Write((uint)TurnCommand);

            if ((MovementStateFlag & MovementStateFlag.TurnSpeed) != 0)
                writer.Write((float)TurnSpeed);
        }
    }
}