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
