using System;
using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.Common;
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
        public uint ContactPlaneCellID;
        public uint LastKnownContactPlaneCellID;
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

        // custom for server
        public bool VerifiedRestrictions;

        public CollisionInfo()
        {
            Init();
        }

        public void Init()
        {
            CollideObject = new List<PhysicsObj>();
        }

        public void SetContactPlane(Plane plane, bool isWater)
        {
            ContactPlaneValid = true;
            ContactPlane = new Plane(plane.Normal, plane.D);
            ContactPlaneIsWater = isWater;
        }

        public void SetCollisionNormal(Vector3 normal)
        {
            CollisionNormalValid = true;
            CollisionNormal = normal;   // use original?
            if (Vec.NormalizeCheckSmall(ref normal))
                CollisionNormal = Vector3.Zero;
        }

        public void SetSlidingNormal(Vector3 normal)
        {
            SlidingNormalValid = true;
            SlidingNormal = new Vector3(normal.X, normal.Y, 0.0f);
            if (Vec.NormalizeCheckSmall(ref normal))
                SlidingNormal = Vector3.Zero;
        }

        public void AddObject(PhysicsObj obj, TransitionState state)
        {
            if (CollideObject.Contains(obj))
                return;

            CollideObject.Add(obj);
            NumCollideObject++;

            if (state != TransitionState.OK)
                LastCollidedObject = obj;
        }
    }
}
