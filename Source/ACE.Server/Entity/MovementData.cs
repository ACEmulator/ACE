using System;
using System.IO;
using ACE.Entity;
using ACE.Entity.Enum;
using log4net;

namespace ACE.Server.Entity
{
    public class MovementData
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private MovementStateFlag movementStateFlag = 0;
        public MovementStateFlag MovementStateFlag
        {
            get
            {
                SetMovementStateFlag();
                return movementStateFlag;
            }
            private set => movementStateFlag = value;
        }

        public uint CurrentStyle { get; set; }

        public uint ForwardCommand { get; set; }

        public uint SideStepCommand { get; set; }

        public uint TurnCommand { get; set; }

        public float TurnSpeed { get; set; }

        public float ForwardSpeed { get; set; }

        public float SideStepSpeed { get; set; }

        /// <summary>
        /// This guy is nasty!  The movement commands input by the client are not ACCEPTED by the client!
        /// Only forward and right motions are accepted -- any left or reverse motions are not!
        /// To fix, we need to use negative speeds with right motions when the client requests a left motion.
        /// FIXME: Need to dig through client to figure out how to calculate value passed to client based on run
        /// </summary>
        public MovementData ConvertToClientAccepted(uint holdKey, CreatureSkill run)
        {
            MovementData md = new MovementData();
            // FIXME(ddevec): -- This is hacky!  I mostly reverse engineered it from old network logs
            //   WARNING: this is ugly stuffs --
            //      I'm basically just converting based on analyzing packet stuffs, no idea where the magic #'s come from
            if (holdKey != ((uint)MotionCommand.Invalid & 0xFFFF) && holdKey != ((uint)MotionCommand.HoldSidestep & 0xFFFF))
            {
                log.WarnFormat("Unexpected hold key: {0:X}", holdKey);
            }

            if ((movementStateFlag & MovementStateFlag.CurrentStyle) != 0)
            {
                md.CurrentStyle = CurrentStyle;
            }

            float baseTurnSpeed = 1;
            float baseSpeed = 1;

            if (holdKey == 2)
            {
                if (run.ActiveValue >= 800)
                {
                    baseSpeed = 18f / 4f;
                }
                else
                {
                    // TODO(ddevec): Is burden accounted for externally, or as part of the skill?
                    baseSpeed = (((float)run.ActiveValue / (run.ActiveValue + 200f) * 11f) + 4.0f) / 4.0f;
                }
            }
            else
            {
                if (baseSpeed > 3.11999f)
                {
                    baseSpeed = 3.12f;
                }
            }

            float baseSidestepSpeed = baseSpeed;

            if (ForwardCommand != 0)
            {
                if (ForwardCommand == ((uint)MotionCommand.WalkForward & 0xFFFF))
                {
                    if (holdKey == 2)
                    {
                        md.ForwardCommand = (uint)MotionCommand.RunForward;
                        if (baseSpeed > 4f)
                        {
                            baseSpeed = 4f;
                        }
                    }
                    else
                    {
                        md.ForwardCommand = (uint)MotionCommand.WalkForward;
                    }
                    md.ForwardSpeed = baseSpeed;
                }
                else if (ForwardCommand == ((uint)MotionCommand.WalkBackwards & 0xFFFF))
                {
                    md.ForwardCommand = (uint)MotionCommand.WalkForward;
                    if (holdKey != 2)
                    {
                        baseSpeed = .65f;
                    }
                    else
                    {
                        baseSpeed = .65f * baseSpeed;
                    }
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
                if (SideStepCommand == ((uint)MotionCommand.SideStepRight & 0xFFFF))
                {
                    md.SideStepCommand = (uint)MotionCommand.SideStepRight;
                    md.SideStepSpeed = baseSidestepSpeed * 3.12f / 1.25f * .5f;
                    if (md.SideStepSpeed > 3)
                    {
                        md.SideStepSpeed = 3;
                    }
                }
                else if (SideStepCommand == ((uint)MotionCommand.SideStepLeft & 0xFFFF))
                {
                    md.SideStepCommand = (uint)MotionCommand.SideStepRight;
                    md.SideStepSpeed = -1 * baseSidestepSpeed * 3.12f / 1.25f * .5f;
                    if (md.SideStepSpeed < -3)
                    {
                        md.SideStepSpeed = -3;
                    }
                }
                // Unknown turn command?
                else
                {
                    log.WarnFormat("Unexpected SideStep command: {0:X}", SideStepCommand);
                }
            }

            if (TurnCommand != 0)
            {
                if (holdKey == 2)
                {
                    baseTurnSpeed = 1.5f;
                }
                if (TurnCommand == ((uint)MotionCommand.TurnRight & 0xFFFF))
                {
                    md.TurnCommand = (uint)MotionCommand.TurnRight;
                    md.TurnSpeed = baseTurnSpeed;
                }
                else if (TurnCommand == ((uint)MotionCommand.TurnLeft & 0xFFFF))
                {
                    md.TurnCommand = (uint)MotionCommand.TurnRight;
                    md.TurnSpeed = -1 * baseTurnSpeed;
                }
                // Unknown turn command?
                else
                {
                    log.WarnFormat("Unexpected turn command: {0:X}", TurnCommand);
                }
            }

            return md;
        }

        public void SetMovementStateFlag()
        {
            movementStateFlag = MovementStateFlag.NoMotionState;

            if (CurrentStyle != 0)
                movementStateFlag |= MovementStateFlag.CurrentStyle;
            if (ForwardCommand != 0)
                movementStateFlag |= MovementStateFlag.ForwardCommand;
            if (SideStepCommand != 0)
                movementStateFlag |= MovementStateFlag.SideStepCommand;
            if (TurnCommand != 0)
                movementStateFlag |= MovementStateFlag.TurnCommand;
            // Floating point compare
            if (Math.Abs(ForwardSpeed) > 0.01)
                movementStateFlag |= MovementStateFlag.ForwardSpeed;
            // Floating point compare
            if (Math.Abs(SideStepSpeed) > 0.01)
                movementStateFlag |= MovementStateFlag.SideStepSpeed;
            // Floating point compare
            if (Math.Abs(TurnSpeed) > 0.01)
                movementStateFlag |= MovementStateFlag.TurnSpeed;
        }

        public void Serialize(BinaryWriter writer)
        {
            if ((this.movementStateFlag & MovementStateFlag.CurrentStyle) != 0)
                writer.Write((ushort)this.CurrentStyle);

            if ((this.movementStateFlag & MovementStateFlag.ForwardCommand) != 0)
                // writer.Write((uint)this.ForwardCommand);
                writer.Write((ushort)this.ForwardCommand);

            if ((this.movementStateFlag & MovementStateFlag.SideStepCommand) != 0)
                // writer.Write((uint)this.SideStepCommand);
                writer.Write((ushort)this.SideStepCommand);

            if ((this.movementStateFlag & MovementStateFlag.TurnCommand) != 0)
                // writer.Write((uint)this.TurnCommand);
                writer.Write((ushort)this.TurnCommand);

            if ((this.movementStateFlag & MovementStateFlag.ForwardSpeed) != 0)
                writer.Write((float)this.ForwardSpeed);

            if ((this.movementStateFlag & MovementStateFlag.SideStepSpeed) != 0)
                writer.Write((float)this.SideStepSpeed);

            if ((this.movementStateFlag & MovementStateFlag.TurnSpeed) != 0)
                writer.Write((float)this.TurnSpeed);
        }
    }
}
