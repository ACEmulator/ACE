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
                    if ((motion & (uint)CommandMask.SubState) != 0)
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
                    else if ((motion & (uint)CommandMask.Style) != 0)
                    {
                        if (CurrentStyle != motion)
                        {
                            ForwardCommand = (uint)MotionCommand.Ready;
                            CurrentStyle = motion;
                        }
                    }
                    else if ((motion & (uint)CommandMask.Action) != 0)
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

        public void SetState(Network.Structure.RawMotionState state)
        {
            CurrentHoldKey = state.CurrentHoldKey;
            CurrentStyle = (uint)state.CurrentStyle;
            if (CurrentStyle == 0)
                CurrentStyle = (uint)MotionCommand.NonCombat;
            ForwardCommand = (uint)state.ForwardCommand;
            if (ForwardCommand == 0)
                ForwardCommand = (uint)MotionCommand.Ready;
            ForwardHoldKey = state.ForwardHoldKey;
            ForwardSpeed = state.ForwardSpeed;      // todo: verifications
            if (ForwardSpeed == 0)
                ForwardSpeed = 1.0f;
            SideStepCommand = (uint)state.SidestepCommand;
            SideStepHoldKey = state.SidestepHoldKey;
            SideStepSpeed = state.SidestepSpeed;
            if (SideStepSpeed == 0)
                SideStepSpeed = 1.0f;
            TurnCommand = (uint)state.TurnCommand;
            TurnHoldKey = state.TurnHoldKey;
            TurnSpeed = state.TurnSpeed;
            if (TurnSpeed == 0)
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
                    if ((motion & (uint)CommandMask.SubState) != 0)
                    {
                        if (motion == ForwardCommand)
                        {
                            ForwardCommand = (uint)MotionCommand.Ready;
                            ForwardSpeed = 1.0f;
                        }
                    }
                    else if ((motion & (uint)CommandMask.Style) != 0)
                    {
                        if (motion == CurrentStyle)
                            CurrentStyle = (uint)MotionCommand.NonCombat;
                    }
                    break;
            }
        }
    }
}
