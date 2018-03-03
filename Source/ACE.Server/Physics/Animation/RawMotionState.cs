using System.Collections.Generic;

namespace ACE.Server.Physics.Animation
{
    public class RawMotionState
    {
        public List<ActionNode> Actions;        // 0
        public int CurrentHoldKey;              // 4
        public int CurrentStyle;                // 8
        public bool ForwardCommand;             // 12
        public bool ForwardHoldKey;             // 13
        public float ForwardSpeed;              // 14
        public bool SideStepCommand;            // 18
        public bool SideStepHoldKey;            // 19
        public float SideStepSpeed;             // 20
        public int TurnCommand;                 // 24
        public int TurnHoldKey;                 // 28
        public float TurnSpeed;                 // 32

        public RawMotionState()
        {
            ForwardSpeed = 1.0f;
            SideStepSpeed = 1.0f;
        }
    }
}
