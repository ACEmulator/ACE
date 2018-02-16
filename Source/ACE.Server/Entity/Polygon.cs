using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using ACE.DatLoader.Entity;

namespace ACE.Server.Entity
{
    /// <summary>
    /// A polygon in world space
    /// </summary>
    public class Polygon
    {
        public List<Vector3> Vertices;

        public Polygon(ModelPolygon _poly, Frame frame)
        {
            Vertices = new List<Vector3>();

            // transform the vertices
            // from local object to world space
            foreach (var vertex in _poly.Vertices)
            {
                // move to position within the landblock
                var translate = Matrix4x4.CreateTranslation(frame.Origin);

                // rotate
                var rotate = Matrix4x4.CreateFromQuaternion(frame.Orientation);

                Vertices.Add(Vector3.Transform(vertex.ToVector(), rotate * translate));
            }
        }
    }
}
