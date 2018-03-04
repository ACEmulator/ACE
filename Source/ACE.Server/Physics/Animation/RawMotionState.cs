using System.Collections.Generic;

namespace ACE.Server.Physics.Animation
{
    public class RawMotionState
    {
        public List<ActionNode> Actions;    // 0
        public HoldKey CurrentHoldKey;      // 4
        public int CurrentStyle;            // 8
        public int ForwardCommand;          // 12
        public HoldKey ForwardHoldKey;      // 16
        public float ForwardSpeed;          // 20
        public int SideStepCommand;         // 24
        public HoldKey SideStepHoldKey;     // 28
        public float SideStepSpeed;         // 32
        public int TurnCommand;             // 36
        public HoldKey TurnHoldKey;         // 40
        public float TurnSpeed;             // 44

        public RawMotionState()
        {
            ForwardSpeed = 1.0f;
            SideStepSpeed = 1.0f;
        }

        public void ApplyMotion(int motion, MovementParameters movementParams)
        {

        }

        public void RemoveAction()
        {

        }

        public void RemoveMotion(int motion)
        {

        }
    }
}
