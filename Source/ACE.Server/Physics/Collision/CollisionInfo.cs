using System;
using System.Collections.Generic;
using System.Numerics;

namespace ACE.Server.Physics.Collision
{
    public class CollisionInfo
    {
        /// <summary>
        /// The walkable allowance when landing on the ground
        /// </summary>
        public static readonly float LandingZ = 0.0871557f;

        public static readonly float DefaultFriction = 0.94999999f;

        public static readonly float DefaultElasticity = 0.050000001f;

        public static readonly float DefaultTranslucency = 0.0f;

        public static readonly float DummySphereRadius = 0.1f;

        public bool LastKnownContactPlaneValid;
        public Plane LastKnownContactPlane;
        public bool LastKnownContactPlaneIsWater;
        public bool ContactPlaneValid;
        public Plane ContactPlane;
        public int ContactPlaneCellID;
        public int LastKnownContactPlaneCellID;
        public bool ContactPlaneIsWater;
        public int SlidingNormalValid;
        public Vector3 SlidingNormal;
        public bool CollisionNormalValid;
        public Vector3 CollisionNormal;
        public Vector3 AdjustOffset;
        public int NumCollideObject;
        public List<PhysicsObj> CollideObject;
        public PhysicsObj LastCollidedObject;
        public int CollidedWithEnvironment;
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
    }
}
