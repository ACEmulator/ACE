using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.Collision;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    public enum TransitionState
    {
        Invalid   = 0x0,
        OK        = 0x1,
        Collided  = 0x2,
        Adjusted  = 0x3,
        Slid      = 0x4
    };
    public class Transition
    {
        public ObjectInfo ObjectInfo;
        public SpherePath SpherePath;
        public CollisionInfo CollisionInfo;
        public CellArray CellArray;
        public ObjCell NewCellPtr;

        public Transition()
        {
            ObjectInfo = new ObjectInfo();
            SpherePath = new SpherePath();
            CollisionInfo = new CollisionInfo();
            CellArray = new CellArray();
            NewCellPtr = new ObjCell();
        }

        public static Transition MakeTransition()
        {
            return null;
        }

        public void InitObject(PhysicsObj obj, ObjectInfoState objectState)
        {
            ObjectInfo.Init(obj, objectState);
        }

        public void CacheLocalSpaceSphere(Position pos, float scaleZ)
        {

        }

        public bool StepUp(Vector3 collisionNormal)
        {
            return true;
        }

        public void InitSphere(int numSphere, Sphere sphere, float scale)
        {
            SpherePath.InitSphere(numSphere, new List<Sphere>() { sphere }, scale);
        }

        public void CleanupTransition()
        {

        }

        public void InitContactPlane(int cellID, Plane contactPlane, bool isWater)
        {

        }

        public void InitLastKnownContactPlane(int cellID, Plane contactPlane, bool isWater)
        {

        }

        public void InitSlidingNormal(Vector3 normal)
        {

        }

        public void InitPath(ObjCell cell, Position oldPos, Position newPos)
        {

        }

        public bool FindValidPosition()
        {
            return false;
        }
    }
}
