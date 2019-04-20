using System.Collections.Generic;
using System.Numerics;
using ACE.DatLoader.Entity;

namespace ACE.Server.Entity
{
    /// <summary>
    /// A polygon in world space
    /// </summary>
    public class Polygon
    {
        /// <summary>
        /// The list of polygon vertices
        /// transformed into world space
        /// </summary>
        public List<Vector3> Vertices;

        /// <summary>
        /// Constructs a new polygon from a position, rotation, and scale
        /// </summary>
        public Polygon(ModelPolygon _poly, Vector3 position, Quaternion orientation, float scale = 1.0f)
        {
            BuildPolygon(_poly, position, orientation, scale);
        }

        /// <summary>
        /// Constructs a new polygon from a frame and scale
        /// </summary>
        public Polygon(ModelPolygon _poly, Frame frame, float scale = 1.0f)
        {
            BuildPolygon(_poly, frame.Origin, frame.Orientation, scale);
        }

        /// <summary>
        /// Transforms the model from local to world space
        /// </summary>
        public void BuildPolygon(ModelPolygon _poly, Vector3 position, Quaternion orientation, float scalar = 1.0f)
        {
            Vertices = new List<Vector3>();

            // build the matrix transform
            var transform = Matrix4x4.CreateScale(scalar) * Matrix4x4.CreateFromQuaternion(orientation) * Matrix4x4.CreateTranslation(position);

            // transform the vertices
            // from local object to world space
            foreach (var vertex in _poly.Vertices)
            {
                Vertices.Add(Vector3.Transform(vertex.Origin, transform));
            }
        }
    }
}
