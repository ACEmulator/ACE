using System.Collections.Generic;
using System.Numerics;

namespace ACE.Server.Physics.Collision
{
    public class CollisionInfo
    {
        public int LastKnownContactPlaneValid;
        public Plane LastKnownContactPlane;
        public int LastKnownContactPlaneIsWater;
        public int ContactPlaneValid;
        public Plane ContactPlane;
        public int ContactPlaneCellID;
        public int LastKnownContactPlaneCellID;
        public int ContactPlaneIsWater;
        public int SlidingNormalValid;
        public Vector3 SlidingNormal;
        public int CollisionNormalValid;
        public Vector3 CollisionNormal;
        public Vector3 AdjustOffset;
        public int NumCollideObject;
        public List<PhysicsObj> CollideObject;
        public PhysicsObj LastCollidedObject;
        public int CollidedWithEnvironment;
        public int FramesStationaryFall;
    }
}
