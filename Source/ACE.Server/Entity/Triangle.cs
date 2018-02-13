using System.Collections.Generic;
using System.Numerics;

namespace ACE.Server.Entity
{
    /// <summary>
    /// Represents a 3D triangle for meshes,
    /// with some methods that operate in 2-space
    /// </summary>
    public class Triangle
    {
        /// <summary>
        /// To avoid storing many redundant vertices,
        /// only indices into the mesh vertices
        /// </summary>
        public int[] Indices;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Triangle()
        {
            Indices = new int[3];
        }

        /// <summary>
        /// Constructs a new triangle from 3 vertex indices
        /// </summary>
        public Triangle(int a, int b, int c)
        {
            Indices = new int[3] { a, b, c };
        }

        /// <summary>
        /// Returns the vertices of the triangle
        /// </summary>
        public Vector3[] GetVertices(List<Vector3> vertices)
        {
            // TODO: out-of-bounds exception
            return new Vector3[]
            {
                vertices[Indices[0]], vertices[Indices[1]], vertices[Indices[2]]
            };
        }

        /// <summary>
        /// Returns TRUE if point is contained within triangle
        /// </summary>
        public bool Contains(Vector2 point, List<Vector3> vertices)
        {
            var p1 = vertices[Indices[0]];
            var p2 = vertices[Indices[1]];
            var p3 = vertices[Indices[2]];

            // TODO: further optimizations listed
            // https://stackoverflow.com/questions/2049582/how-to-determine-if-a-point-is-in-a-2d-triangle
            var area = Area(p1, p2, p3);

            var s = 1.0f / (2 * area) * (p1.Y * p3.X - p1.X * p3.Y + (p3.Y - p1.Y) * point.X + (p1.X - p3.X) * point.Y);
            if (s < 0) return false;

            var t = 1.0f / (2 * area) * (p1.X * p2.Y - p1.Y * p2.X + (p1.Y - p2.Y) * point.X + (p2.X - p1.X) * point.Y);
            if (t < 0 || 1 - s - t < 0) return false;

            return true;
        }

        /// <summary>
        /// Returns the area of the 2D triangle
        /// </summary>
        public static float Area(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            return 0.5f * (-p2.Y * p3.X + p1.Y * (-p2.X + p3.X) + p1.X * (p2.Y - p3.Y) + p2.X * p3.Y);
        }

        /// <summary>
        /// Consider the triangle as a plane,
        /// find the Z-coordinate for a 2D coordinate on the plane
        /// </summary>
        public float GetZ(List<Vector3> vertices, Vector2 point)
        {
            // Reference:
            // https://social.msdn.microsoft.com/Forums/en-US/1b32dc40-f84d-4365-a677-b59e49d41eb0/how-to-calculate-a-point-on-a-plane-based-on-a-plane-from-3-points

            Vector3 v1 = new Vector3();
            Vector3 v2 = new Vector3();
            Vector3 abc = new Vector3();

            var p1 = vertices[Indices[0]];
            var p2 = vertices[Indices[1]];
            var p3 = vertices[Indices[2]];

            v1.X = p1.X - p3.X;
            v1.Y = p1.Y - p3.Y;
            v1.Z = p1.Z - p3.Z;

            v2.X = p2.X - p3.X;
            v2.Y = p2.Y - p3.Y;
            v2.Z = p2.Z - p3.Z;

            abc.X = (v1.Y * v2.Z) - (v1.Z * v2.Y);
            abc.Y = (v1.Z * v2.X) - (v1.X * v2.Z);
            abc.Z = (v1.X * v2.Y) - (v1.Y * v2.X);

            float d = (abc.X * p3.X) + (abc.Y * p3.Y) + (abc.Z * p3.Z);

            float z = (d - (abc.X * point.X) - (abc.Y * point.Y)) / abc.Z;

            return z;
        }
    }
}
