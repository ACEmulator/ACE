using System.Collections.Generic;

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

        public void ApplyMotion(int motion, MovementParameters movementParams)
        {

        }

        public int GetNumActions()
        {
            return Actions.Count;
        }

        public void RemoveAction()
        {

        }

        public void RemoveMotion(int motion)
        {

        }
    }
}
