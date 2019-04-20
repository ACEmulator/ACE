using System.Collections.Generic;
using System.Numerics;

namespace ACE.Server.Entity
{
    /// <summary>
    /// A 3D mesh of vertices and triangles
    /// Used for collision detection and physics simulation
    /// </summary>
    public class Mesh
    {
        /// <summary>
        /// The list of vertices comprising the mesh
        /// </summary>
        public List<Vector3> Vertices;

        /// <summary>
        /// The list of triangles comprising the mesh
        /// </summary>
        public List<Triangle> Triangles;
    }
}
