using System.Numerics;
using ACE.Server.Physics.Animation;

namespace ACE.Server.Physics.Common
{
    public class Position
    {
        public int ObjCellID;
        public AFrame Frame;

        public Position()
        {
            Frame = new AFrame();    // cache frames?
        }

        public Position(int objCellID)
        {
            ObjCellID = objCellID;
            Frame = new AFrame();
        }

        public Vector3 LocalToGlobal(Position p, Vector3 v)
        {
            return v;
        }

        public Vector3 LocalToGlobalVec(Vector3 v)
        {
            return v;
        }

        public double Distance(Position pos)
        {
            return -1;
        }

        public double CylinderDistance(float curRadius, float curHeight, float radius, float height, Position pos)
        {
            return -1;
        }

        public Vector3 GlobalToLocalVec(Vector3 global)
        {
            return global;
        }

        public Vector3 GetOffset(Position pos)
        {
            return Vector3.Zero;
        }

        public int DetermineQuadrant(float height, Position pos)
        {
            return -1;
        }

        public void adjust_to_outside()
        {

        }
    }
}
