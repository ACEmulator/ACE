using System.Collections.Generic;
using System.Linq;
using ACE.Entity.Enum;

namespace ACE.Server.Physics.Animation
{
    public class InterpretedMotionState
    {
        public uint CurrentStyle;
        public uint ForwardCommand;
        public float ForwardSpeed;
        public uint SideStepCommand;
        public float SideStepSpeed;
        public uint TurnCommand;
        public float TurnSpeed;
        public List<ActionNode> Actions;

        public InterpretedMotionState()
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
                    TurnCommand = (uint)MotionCommand.TurnRight;
                    TurnSpeed = movementParams.Speed;
                    break;

                case (uint)MotionCommand.SideStepRight:
                    SideStepCommand = (uint)MotionCommand.SideStepRight;
                    SideStepSpeed = movementParams.Speed;
                    break;

                default:
                    if ((motion & (uint)CommandMask.SubState) != 0)
                    {
                        ForwardCommand = motion;
                        ForwardSpeed = movementParams.Speed;
                    }
                    else if ((motion & (uint)CommandMask.Style) != 0)
                    {
                        ForwardCommand = (uint)MotionCommand.Ready;
                        CurrentStyle = motion;
                    }
                    else if ((motion & (uint)CommandMask.Action) != 0)
                    {
                        AddAction(motion, movementParams.Speed, movementParams.ActionStamp, movementParams.Autonomous);
                    }
                    break;
            }
        }

        public void copy_movement_from(InterpretedMotionState state)
        {
            CurrentStyle = state.CurrentStyle;
            ForwardCommand = state.ForwardCommand;
            ForwardSpeed = state.ForwardSpeed;
            SideStepCommand = state.SideStepCommand;
            SideStepSpeed = state.SideStepSpeed;
            TurnCommand = state.TurnCommand;
            TurnSpeed = state.TurnSpeed;
        }

        public void InitDefaults()
        {
            Actions = new List<ActionNode>();
            CurrentStyle = (uint)MotionCommand.NonCombat;
            ForwardCommand = (uint)MotionCommand.Ready;
            ForwardSpeed = 1.0f;
            SideStepCommand = 0;
            SideStepSpeed = 1.0f;
            TurnCommand = 0;
            TurnSpeed = 1.0f;
        }

        public int GetNumActions()
        {
            return Actions.Count;
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
                    TurnCommand = 0;
                    break;
                case (uint)MotionCommand.SideStepRight:
                    SideStepCommand = 0;
                    break;

                default:
                    if ((motion & (uint)CommandMask.SubState) != 0)
                    {
                        if (ForwardCommand == motion)
                        {
                            ForwardCommand = (uint)MotionCommand.Ready;
                            ForwardSpeed = 1.0f;
                        }
                    }
                    else if ((motion & (uint)CommandMask.Style) != 0)
                    {
                        if (CurrentStyle == motion)
                            CurrentStyle = (uint)MotionCommand.NonCombat;
                    }
                    break;
            }
        }

        public bool HasCommands()
        {
            //return ForwardCommand != 0 && ForwardCommand != (uint)MotionCommand.Ready || SideStepCommand != 0 || TurnCommand != 0;
            return SideStepCommand != 0 || TurnCommand != 0;
        }
    }
}
