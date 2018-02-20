using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    public class StickyManager
    {
        public int TargetID;
        public float TargetRadius;
        public Position TargetPosition;
        public PhysicsObj PhysicsObj;
        public int Initialized;
        public double StickyTimeoutTime;
    }
}
