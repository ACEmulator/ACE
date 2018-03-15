namespace ACE.Server.Physics.Animation
{
    public class MotionNode
    {
        public int ContextID;
        public uint Motion;
        public int JumpErrorCode;

        public MotionNode() { }

        public MotionNode(int contextID, uint motion, int jumpErrorCode)
        {
            ContextID = contextID;
            Motion = motion;
            JumpErrorCode = jumpErrorCode;
        }
    }
}
