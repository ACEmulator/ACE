using System.Collections.Generic;
using ACE.Server.Physics.Collision;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    public class Transition
    {
        public ObjectInfo ObjectInfo;
        public SpherePath SpherePath;
        public CollisionInfo CollisionInfo;
        public List<ObjCell> CellArray;
        public ObjCell NewCellPtr;
    }
}
