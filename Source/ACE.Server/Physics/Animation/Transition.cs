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
        public List<ObjCell> CellArray;
        public ObjCell NewCellPtr;

        public Transition()
        {
            ObjectInfo = new ObjectInfo();
            SpherePath = new SpherePath();
            CollisionInfo = new CollisionInfo();
            CellArray = new List<ObjCell>();
            NewCellPtr = new ObjCell();
        }

        public void CacheLocalSpaceSphere(Position pos, float scaleZ)
        {

        }

        public bool StepUp(Vector3 collisionNormal)
        {
            return true;
        }
    }
}
