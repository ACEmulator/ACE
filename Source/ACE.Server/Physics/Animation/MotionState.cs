namespace ACE.Server.Physics.Animation
{
    public class MotionState
    {
        public int Style;
        public int Substate;
        public float SubstateMod;
        public MotionList ModifierHead;
        public MotionList ActionHead;
        public MotionList ActionTail;
    }
}
