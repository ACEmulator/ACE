using System.Collections.Generic;

namespace ACE.Entity
{
    public class BSPNode
    {
        public uint Type { get; set; }

        public Plane SplittingPlane { get; set; }

        public BSPNode PosNode { get; set; }
        public BSPNode NegNode { get; set; }

        public Sphere Sphere { get; set; }

        public List<ushort> InPolys { get; set; } // List of PolygonIds
    }
}
