using System.Numerics;

namespace ACE.Server.Physics.Animation
{
    public class ConstraintManager
    {
        public PhysicsObj PhysicsObj;
        public int IsConstrained;
        public float ConstraintPosOffset;
        public Vector3 ConstraintPos;
        public float ConstraintDistanceStart;
        public float ConstraintDistanceMax;
    }
}
