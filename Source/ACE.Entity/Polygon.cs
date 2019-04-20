using System.Collections.Generic;

namespace ACE.Entity
{
    public class Polygon
    {
        public byte NumPts { get; set; }
        public byte Stippling { get; set; } // Whether it has that textured/bumpiness to it

        public int SidesType { get; set; }
        public short PosSurface { get; set; }
        public short NegSurface { get; set; }

        public List<short> VertexIds { get; set;  }

        public List<byte> PosUVIndices { get; set; }
        public List<byte> NegUVIndices { get; set; }
    }
}
