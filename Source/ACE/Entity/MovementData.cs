using ACE.Network;
using ACE.Network.Enum;
using System.IO;

using log4net;

namespace ACE.Entity
{
    public class MovementData
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

        /// <summary>
        /// This guy is nasty!  The movement commands input by the client are not ACCEPTED by the client!
        /// Only forward and right motions are accepted -- any left or reverse motions are not!
        /// To fix, we need to use negative speeds with right motions when the client requests a left motion.
        /// FIXME: Need to dig through client to figure out how to calculate value passed to client based on run
        /// </summary>
        /// <param name="holdKey"></param>
        /// <returns></returns>
        public MovementData ConvertToClientAccepted(uint holdKey, CreatureSkill run)
        {
            MovementData md = new MovementData();
            // FIXME(ddevec): -- This is hacky!  I mostly reverse engineered it from old network logs
            //   WARNING: this is ugly stuffs -- 
            //      I'm basically just converting based on analyzing packet stuffs, no idea where the magic #'s come from
            if (holdKey != 0 && holdKey != 2)
            {
                log.WarnFormat("Unexpected hold key: {0}", holdKey.ToString("X"));
            }

            if ((MovementStateFlag & MovementStateFlag.CurrentStyle) != 0) {
                md.CurrentStyle = CurrentStyle;
            }

            float baseTurnSpeed = 1;
            float baseSidestepSpeed = 1;
            float baseSpeed = 1;
            if (holdKey == 2)
            {
                // THis is just a random number that looks close to what the game expects
                if (run.ActiveValue >= 800)
                {
                    baseSpeed = 18f / 4f;
                }
                else
                {
                    // FIXME(ddevec): Is burden accounted for externally, or as part of the skill?
                    baseSpeed = (((float)run.ActiveValue / (run.ActiveValue + 200f) * 11f) + 4.0f) / 4.0f;
                }

                baseTurnSpeed = 1.5f;
                baseSidestepSpeed = 1.5f;
            }

            if (ForwardCommand != 0)
            {
                if (ForwardCommand == (ushort)MotionCommand.WalkForward)
                {
                    if (holdKey == 2)
                    {
                        md.ForwardCommand = (ushort)MotionCommand.RunForward;
                        if (baseSpeed > 4f)
                        {
                            baseSpeed = 4f;
                        }
                    }
                    else
                    {
                        if (baseSpeed > 3.11999f)
                        {
                            baseSpeed = 3.2f;
                        }

                        if (baseSpeed < -3.11999f)
                        {
                            baseSpeed = -3.2f;
                        }
                        md.ForwardCommand = (ushort)MotionCommand.WalkForward;
                    }
                    md.ForwardSpeed = baseSpeed;
                }
                else if (ForwardCommand == (ushort)MotionCommand.WalkBackwards)
                {
                    md.ForwardCommand = (ushort)MotionCommand.WalkForward;
                    md.ForwardSpeed = -1 * baseSpeed;
                }
                // Emote -- some are put here, others are sent externally -- ugh
                //   This relates to if you can move (e.g. sidestep) while emoting -- some you can some you cant
                else
                {
                    md.ForwardCommand = ForwardCommand;
                }
            }

            if (SideStepCommand != 0)
            {
                if (SideStepCommand == (ushort)MotionCommand.SideStepRight)
                {
                    md.SideStepCommand = (ushort)MotionCommand.SideStepRight;
                    md.SideStepSpeed = baseSidestepSpeed;
                }
                else if (SideStepCommand == (ushort)MotionCommand.SideStepLeft)
                {
                    md.SideStepCommand = (ushort)MotionCommand.SideStepRight;
                    md.SideStepSpeed = -1 * baseSidestepSpeed;
                }
                // Unknown turn command?
                else
                {
                    log.WarnFormat("Unexpected SideStep command: {0}", SideStepCommand.ToString("X"));
                }
            }

            if (TurnCommand != 0)
            {
                if (TurnCommand == (ushort)MotionCommand.TurnRight)
                {
                    md.TurnCommand = (ushort)MotionCommand.TurnRight;
                    md.TurnSpeed = baseTurnSpeed;
                }
                else if (TurnCommand == (ushort)MotionCommand.TurnLeft)
                {
                    md.TurnCommand = (ushort)MotionCommand.TurnRight;
                    md.TurnSpeed = -1 * baseTurnSpeed;
                }
                // Unknown turn command?
                else
                {
                    log.WarnFormat("Unexpected turn command: {0}", TurnCommand.ToString("X"));
                }
            }

            return md;
        }

        public void Serialize(BinaryWriter writer)
        {
            if ((this.MovementStateFlag & MovementStateFlag.CurrentStyle) != 0)
                writer.Write((ushort)this.CurrentStyle);

            if ((this.MovementStateFlag & MovementStateFlag.ForwardCommand) != 0)
                // writer.Write((uint)this.ForwardCommand);
                writer.Write((ushort)this.ForwardCommand);

            if ((this.MovementStateFlag & MovementStateFlag.SideStepCommand) != 0)
                // writer.Write((uint)this.SideStepCommand);
                writer.Write((ushort)this.SideStepCommand);

            if ((this.MovementStateFlag & MovementStateFlag.TurnCommand) != 0)
                // writer.Write((uint)this.TurnCommand);
                writer.Write((ushort)this.TurnCommand);

            if ((this.MovementStateFlag & MovementStateFlag.ForwardSpeed) != 0)
                writer.Write((float)this.ForwardSpeed);

            if ((this.MovementStateFlag & MovementStateFlag.SideStepSpeed) != 0)
                writer.Write((float)this.SideStepSpeed);

            if ((this.MovementStateFlag & MovementStateFlag.TurnSpeed) != 0)
                writer.Write((float)this.TurnSpeed);
        }
    }
}