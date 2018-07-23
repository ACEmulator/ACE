using System.Collections.Generic;
using System.Linq;
using ACE.Entity.Enum;

namespace ACE.Server.Physics.Animation
{
    public class RawMotionState
    {
        public List<ActionNode> Actions;
        public HoldKey CurrentHoldKey;
        public uint CurrentStyle;
        public uint ForwardCommand;
        public HoldKey ForwardHoldKey;
        public float ForwardSpeed;
        public uint SideStepCommand;
        public HoldKey SideStepHoldKey;
        public float SideStepSpeed;
        public uint TurnCommand;
        public HoldKey TurnHoldKey;
        public float TurnSpeed;

        public RawMotionState()
        {
            InitDefaults();
        }

        public void AddAction(uint action, float speed, int stamp, bool autonomous)
        {
            Actions.Add(new ActionNode(action, speed, stamp, autonomous));
        }

        public void ApplyMotion(uint motion, MovementParameters movementParams)
        {
            switch (motion)
            {
                case (uint)MotionCommand.TurnRight:
                case (uint)MotionCommand.TurnLeft:
                    TurnCommand = motion;
                    if (movementParams.SetHoldKey)
                    {
                        TurnHoldKey = HoldKey.Invalid;
                        TurnSpeed = movementParams.Speed;
                    }
                    else
                    {
                        TurnHoldKey = movementParams.HoldKeyToApply;
                        TurnSpeed = movementParams.Speed;
                    }
                    break;

                case (uint)MotionCommand.SideStepRight:
                case (uint)MotionCommand.SideStepLeft:
                    SideStepCommand = motion;
                    if (movementParams.SetHoldKey)
                    {
                        SideStepHoldKey = HoldKey.Invalid;
                        SideStepSpeed = movementParams.Speed;
                    }
                    else
                    {
                        SideStepHoldKey = movementParams.HoldKeyToApply;
                        SideStepSpeed = movementParams.Speed;
                    }
                    break;

                default:
                    if ((motion & 0x40000000) != 0)
                    {
                        if (motion != (uint)MotionCommand.RunForward)
                        {
                            ForwardCommand = motion;
                            if (movementParams.SetHoldKey)
                            {
                                ForwardHoldKey = HoldKey.Invalid;
                                ForwardSpeed = movementParams.Speed;
                            }
                            else
                            {
                                ForwardHoldKey = movementParams.HoldKeyToApply;
                                ForwardSpeed = movementParams.Speed;
                            }
                        }
                    }
                    else if ((motion & 0x80000000) != 0)
                    {
                        if (CurrentStyle != motion)
                        {
                            ForwardCommand = (uint)MotionCommand.Ready;
                            CurrentStyle = motion;
                        }
                    }
                    else if ((motion & 0x10000000) != 0)
                    {
                        AddAction(motion, movementParams.Speed, movementParams.ActionStamp, movementParams.Autonomous);
                    }
                    break;
            }
        }

        public void InitDefaults()
        {
            Actions = new List<ActionNode>();

            CurrentHoldKey = HoldKey.None;
            CurrentStyle = (uint)MotionCommand.NonCombat;
            ForwardCommand = (uint)MotionCommand.Ready;
            ForwardHoldKey = HoldKey.Invalid;
            ForwardSpeed = 1.0f;
            SideStepCommand = 0;
            SideStepHoldKey = HoldKey.Invalid;
            SideStepSpeed = 1.0f;
            TurnCommand = 0;
            TurnHoldKey = HoldKey.Invalid;
            TurnSpeed = 1.0f;
        }

        public uint RemoveAction()
        {
            if (Actions.Count == 0)
                return 0;

            var action = Actions.First();
            Actions.RemoveAt(0);
            return action.Action;
        }

        public void RemoveMotion(uint motion)
        {
            switch (motion)
            {
                case (uint)MotionCommand.TurnRight:
                case (uint)MotionCommand.TurnLeft:
                    TurnCommand = 0;
                    break;

                case (uint)MotionCommand.SideStepRight:
                case (uint)MotionCommand.SideStepLeft:
                    SideStepCommand = 0;
                    break;

                default:
                    if ((motion & 0x40000000) != 0)
                    {
                        if (motion == ForwardCommand)
                        {
                            ForwardCommand = (uint)MotionCommand.Ready;
                            ForwardSpeed = 1.0f;
                        }
                    }
                    else if ((motion & 0x80000000) != 0)
                    {
                        if (motion == CurrentStyle)
                            CurrentStyle = (uint)MotionCommand.NonCombat;
                    }
                    break;
            }
        }
    }
}
