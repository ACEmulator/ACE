using ACE.Entity;
using System.Numerics;

namespace ACE.Server.Physics.Common
{
    public class Position
    {
        public int ObjCellID;
        public Frame Frame;

        public Position()
        {
            Frame = new Frame();    // cache frames?
        }

        public Vector3 LocalToGlobal(Position p, Vector3 v)
        {
            return v;
        }

        public Vector3 LocalToGlobalVec(Vector3 v)
        {
            return v;
        }
    }
}
