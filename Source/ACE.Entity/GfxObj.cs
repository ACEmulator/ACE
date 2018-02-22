using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace ACE.Entity
{
    public class GfxObj
    {
        public List<uint> Surfaces { get; set;  }
        public List<uint> RGSurfaces { get; set; }
        public CVertexArray VertexArray { get; set;  }
        public Dictionary<ushort, Polygon> PhysicsPolygons { get; set; }
        public BSPTree PhysicsBSP { get; set; }
        public Vector3 SortCenter { get; set; }
        public Dictionary<ushort, Polygon> Polygons { get; set; }
        public BSPTree DrawingBSP { get; set; }
        public uint DIDDegrade { get; set; }
    }
}
