using System.Collections.Generic;

namespace ACE.Server.Physics.Animation
{
    public class RawMotionState
    {
        public List<ActionNode> Actions;
        public int CurrentHoldKey;
        public int CurrentStyle;
        public int ForwardCommand;
        public int ForwardHoldKey;
        public float ForwardSpeed;
        public int SideStepCommand;
        public int SideStepHoldKey;
        public float SideStepSpeed;
        public int TurnCommand;
        public int TurnHoldKey;
        public float TurnSpeed;
    }
}
