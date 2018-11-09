using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.Animation;

namespace ACE.Server.Physics.Common
{
    public class ShadowPart
    {
        public int NumPlanes;
        public List<int> Planes;  // ClipPlaneList
        public AFrame Frame;
        public PhysicsPart Part;

        public ShadowPart() { }

        public ShadowPart(int numPlanes, List<int> planes, AFrame frame, PhysicsPart part)
        {
            NumPlanes = numPlanes;
            Planes = planes;
            Frame = frame;
            Part = part;
        }

        public ShadowPart(List<int> planes, AFrame frame, PhysicsPart part)
        {
            NumPlanes = planes != null ? planes.Count : 0;
            Planes = planes;
            Frame = frame;
            Part = part;
        }
    }
}
