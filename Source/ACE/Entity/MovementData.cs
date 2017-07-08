using ACE.Network;
using ACE.Network.Enum;
using System.IO;

using log4net;

namespace ACE.Entity
{
    using System;

    public class MovementData
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MovementStateFlag MovementStateFlag { get; private set; } = 0;

        public ushort CurrentStyle { get; set; } = 0;

        public ushort ForwardCommand { get; set; } = 0;

        public ushort SideStepCommand { get; set; } = 0;

        public ushort TurnCommand { get; set; } = 0;

        public float TurnSpeed { get; set; } = 0f;

        public float ForwardSpeed { get; set; } = 0f;

        public float SideStepSpeed { get; set; } = 0f;

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

            if ((MovementStateFlag & MovementStateFlag.CurrentStyle) != 0)
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
                        md.ForwardCommand = (ushort)MotionCommand.WalkForward;
                    }
                    md.ForwardSpeed = baseSpeed;
                }
                else if (ForwardCommand == (ushort)MotionCommand.WalkBackwards)
                {
                    md.ForwardCommand = (ushort)MotionCommand.WalkForward;
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
                if (SideStepCommand == (ushort)MotionCommand.SideStepRight)
                {
                    md.SideStepCommand = (ushort)MotionCommand.SideStepRight;
                    md.SideStepSpeed = baseSidestepSpeed * 3.12f / 1.25f * .5f;
                    if (md.SideStepSpeed > 3)
                    {
                        md.SideStepSpeed = 3;
                    }
                }
                else if (SideStepCommand == (ushort)MotionCommand.SideStepLeft)
                {
                    md.SideStepCommand = (ushort)MotionCommand.SideStepRight;
                    md.SideStepSpeed = -1 * baseSidestepSpeed * 3.12f / 1.25f * .5f;
                    if (md.SideStepSpeed < -3)
                    {
                        md.SideStepSpeed = -3;
                    }
                }
                // Unknown turn command?
                else
                {
                    log.WarnFormat("Unexpected SideStep command: {0}", SideStepCommand.ToString("X"));
                }
            }

            if (TurnCommand != 0)
            {
                if (holdKey == 2)
                {
                    baseTurnSpeed = 1.5f;
                }
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

        public void SetMovementStateFlag()
        {
            MovementStateFlag = MovementStateFlag.NoMotionState;

            if (CurrentStyle != 0)
                MovementStateFlag |= MovementStateFlag.CurrentStyle;
            if (ForwardCommand != 0)
                MovementStateFlag |= MovementStateFlag.ForwardCommand;
            if (SideStepCommand != 0)
                MovementStateFlag |= MovementStateFlag.SideStepCommand;
            if (TurnCommand != 0)
                MovementStateFlag |= MovementStateFlag.TurnCommand;
            // Floating point compare
            if (Math.Abs(ForwardSpeed) > 0.01)
                MovementStateFlag |= MovementStateFlag.ForwardSpeed;
            // Floating point compare
            if (Math.Abs(SideStepSpeed) > 0.01)
                MovementStateFlag |= MovementStateFlag.SideStepSpeed;
            // Floating point compare
            if (Math.Abs(TurnSpeed) > 0.01)
                MovementStateFlag |= MovementStateFlag.TurnSpeed;
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