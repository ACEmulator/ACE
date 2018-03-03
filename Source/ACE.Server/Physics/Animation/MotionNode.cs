namespace ACE.Server.Physics.Animation
{
    public class MotionNode
    {
        public int ContextID;
        public int Motion;
        public int JumpErrorCode;

        public MotionNode() { }

        public MotionNode(int contextID, int motion, int jumpErrorCode)
        {
            ContextID = contextID;
            Motion = motion;
            JumpErrorCode = jumpErrorCode;
        }
    }
}
