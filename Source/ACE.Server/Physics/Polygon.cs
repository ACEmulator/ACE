using System.Collections.Generic;
using System.Numerics;

namespace ACE.Server.Physics
{
    public enum SimplePolygonType
    {
        SimplePolygon   = 0x0,
        PathPolygon     = 0x1,
        PlanarPolygon   = 0x2
    };

    public class Polygon
    {
        public List<Vector3> Vertices;
        public List<int> VertexIDs;
        public List<Vector2> Screen;
        public short PolyID;
        public byte NumPts;
        public byte Stippling;
        public int SidesType;
        public List<byte> PosUVIndices;
        public List<byte> NegUVIndices;
        public short PosSurface;
        public short NegSurface;
        public Plane Plane;
    }
}
