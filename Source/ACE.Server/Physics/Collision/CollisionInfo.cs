using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.Animation;

namespace ACE.Server.Physics.Collision
{
    public class CollisionInfo
    {
        public bool LastKnownContactPlaneValid;
        public Plane LastKnownContactPlane;
        public bool LastKnownContactPlaneIsWater;
        public bool ContactPlaneValid;
        public Plane ContactPlane;
        public int ContactPlaneCellID;
        public int LastKnownContactPlaneCellID;
        public bool ContactPlaneIsWater;
        public bool SlidingNormalValid;
        public Vector3 SlidingNormal;
        public bool CollisionNormalValid;
        public Vector3 CollisionNormal;
        public Vector3 AdjustOffset;
        public int NumCollideObject;
        public List<PhysicsObj> CollideObject;
        public PhysicsObj LastCollidedObject;
        public bool CollidedWithEnvironment;
        public int FramesStationaryFall;

        public void SetContactPlane(Plane plane, bool isWater)
        {
            ContactPlaneValid = true;
            ContactPlane = plane;
            ContactPlaneIsWater = isWater;
        }

        public void SetCollisionNormal(Vector3 normal)
        {
            CollisionNormalValid = true;
            CollisionNormal = normal;
            if (!NormalizeCheckSmall(ref normal))
                CollisionNormal = Vector3.Zero;
        }

        public static bool NormalizeCheckSmall(ref Vector3 v)
        {
            var dist = v.Length();
            if (dist >= PhysicsGlobals.EPSILON)
            {
                v *= 1.0f / dist;
                return true;
            }
            return false;
        }

        public void AddObject(PhysicsObj obj, TransitionState state)
        {

        }
    }
}
