using System.Collections.Generic;
using System.Linq;

namespace ACE.Server.Physics.Animation
{
    public class InterpretedMotionState
    {
        public int CurrentStyle;
        public int ForwardCommand;
        public float ForwardSpeed;
        public int SideStepCommand;
        public float SideStepSpeed;
        public int TurnCommand;
        public float TurnSpeed;
        public List<ActionNode> Actions;

        public InterpretedMotionState()
        {
            CurrentStyle = -2147483587;
            ForwardCommand = 0x41000003;
            ForwardSpeed = 1.0f;
            SideStepSpeed = 1.0f;
            TurnSpeed = 1.0f;
        }

        public void AddAction(int action, float speed, int stamp, bool autonomous)
        {
            Actions.Add(new ActionNode(action, speed, stamp, autonomous));
        }

        public void ApplyMotion(int motion, MovementParameters movementParams)
        {
            switch (motion)
            {
                case 0x6500000D:
                    TurnCommand = 0x6500000D;
                    TurnSpeed = movementParams.Speed;
                    break;

                case 0x6500000F:
                    SideStepCommand = 0x6500000F;
                    SideStepSpeed = movementParams.Speed;
                    break;

                default:
                    if ((motion & 0x40000000) != 0)
                    {
                        ForwardCommand = motion;
                        ForwardSpeed = movementParams.Speed;
                    }
                    else if ((motion & 0x80000000) != 0)
                    {
                        ForwardCommand = 0x41000003;
                        CurrentStyle = motion;
                    }
                    else if ((motion & 0x10000000) != 0)
                    {
                        AddAction(motion, movementParams.Speed, movementParams.ActionStamp, movementParams.Autonomous);
                    }
                    break;
            }
        }

        public int GetNumActions()
        {
            return Actions.Count;
        }

        public int RemoveAction()
        {
            if (Actions.Count == 0)
                return 0;

            var action = Actions.First();
            Actions.RemoveAt(0);
            return action.Action;
        }

        public void RemoveMotion(int motion)
        {
            switch (motion)
            {
                case 0x6500000D:
                    TurnCommand = 0;
                    break;
                case 0x6500000F:
                    SideStepCommand = 0;
                    break;

                default:
                    if ((motion & 0x40000000) != 0)
                    {
                        if (ForwardCommand == motion)
                        {
                            ForwardCommand = 0x41000003;
                            ForwardSpeed = 1.0f;
                        }
                    }
                    else if ((motion & 0x80000000) != 0 )
                    {
                        if (CurrentStyle == motion)
                            CurrentStyle = -2147483587;
                    }
                    break;
            }
        }
    }
}
