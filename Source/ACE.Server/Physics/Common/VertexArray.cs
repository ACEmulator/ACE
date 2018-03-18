using System.Collections.Generic;

namespace ACE.Server.Physics.Common
{
    public class VertexArray
    {
        public List<Vertex> Vertices;
        public int Type;

        public VertexArray()
        {
            Vertices = new List<Vertex>();
        }

        public void Allocate(int numVertices, int type)
        {
            Vertices = new List<Vertex>(numVertices);
            Type = type;
        }

        public void DeleteUVs()
        {
            foreach (var vertex in Vertices)
                vertex.UVs = null;
        }

        public void DestroyVertex()
        {
            Vertices = null;    // verify
        }
    }
}
