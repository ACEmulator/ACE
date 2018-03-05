using System.Collections.Generic;
using System.Linq;

namespace ACE.Server.Physics.Animation
{
    public class RawMotionState
    {
        public List<ActionNode> Actions;
        public HoldKey CurrentHoldKey;
        public int CurrentStyle;
        public int ForwardCommand;
        public HoldKey ForwardHoldKey;
        public float ForwardSpeed;
        public int SideStepCommand;
        public HoldKey SideStepHoldKey;
        public float SideStepSpeed;
        public int TurnCommand;
        public HoldKey TurnHoldKey;
        public float TurnSpeed;

        public RawMotionState()
        {
            ForwardSpeed = 1.0f;
            SideStepSpeed = 1.0f;
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
                case 0x6500000E:
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

                case 0x6500000F:
                case 0x65000010:
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
                        if (motion != 0x44000007)
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
                            ForwardCommand = 0x41000003;
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
                case 0x6500000E:
                    TurnCommand = 0;
                    break;

                case 0x6500000F:
                case 0x65000010:
                    SideStepCommand = 0;
                    break;

                default:
                    if ((motion & 0x40000000) != 0)
                    {
                        if (motion == ForwardCommand)
                        {
                            ForwardCommand = 0x41000003;
                            ForwardSpeed = 1.0f;
                        }
                    }
                    else if ((motion & 0x80000000) != 0)
                    {
                        if (motion == CurrentStyle)
                            CurrentStyle = -2147483587;
                    }
                    break;
            }
        }
    }
}
