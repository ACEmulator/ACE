using System.Collections.Generic;
using System.Numerics;

namespace ACE.Server.Physics.Common
{
    public class Vertex
    {
        public Vector3 Origin;
        public ushort Index;
        public List<VertexUV> UVs;
        public Vector3 Normal;
        public float Unknown1;
        public float Unknown2;

        public Vertex() { }

        public Vertex(ushort index, List<VertexUV> uvs)
        {
            Index = index;
            UVs = uvs;  // copy?
        }

        public static Vector3 operator+ (Vertex a, Vertex b)
        {
            return a.Origin + b.Origin;
        }

        public static Vector3 operator- (Vertex a, Vertex b)
        {
            return a.Origin - b.Origin;
        }

        public static Vector3 operator* (Vertex a, Vertex b)
        {
            return a.Origin * b.Origin;
        }

        public static Vector3 operator/ (Vertex a, Vertex b)
        {
            return a.Origin / b.Origin;
        }
    }
}
