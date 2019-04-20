using ACE.Entity.Enum;

namespace ACE.Server.Physics.Animation
{
    public class MotionNode
    {
        public int ContextID;
        public uint Motion;
        public WeenieError JumpErrorCode;

        public MotionNode() { }

        public MotionNode(int contextID, uint motion, WeenieError jumpErrorCode)
        {
            ContextID = contextID;
            Motion = motion;
            JumpErrorCode = jumpErrorCode;
        }
    }
}
