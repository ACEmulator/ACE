using System.Collections.Generic;
using ACE.DatLoader.Entity;

namespace ACE.Server.Entity
{
    /// <summary>
    /// Represents a polygon in local model space
    /// </summary>
    public class ModelPolygon
    {
        /// <summary>
        /// The list of vertices
        /// </summary>
        public List<SWVertex> Vertices;

        /// <summary>
        /// The polygon in AC data formats
        /// </summary>
        public DatLoader.Entity.Polygon Polygon;

        /// <summary>
        /// Constructs a new polygon
        /// </summary>
        public ModelPolygon(DatLoader.Entity.Polygon polygon, CVertexArray vertexArray)            
        {
            Polygon = polygon;
            LoadVertices(vertexArray);
        }

        /// <summary>
        /// Loads the vertices for a polygon
        /// </summary>
        public void LoadVertices(CVertexArray vertexArray)
        {
            Vertices = new List<SWVertex>();

            foreach (var id in Polygon.VertexIds)
                Vertices.Add(vertexArray.Vertices[(ushort)id]);
        }
    }
}
