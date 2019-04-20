using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Server.Entity
{
    /// <summary>
    /// Maintains a single copy of each static mesh
    /// </summary>
    public class StaticMeshCache
    {
        /// <summary>
        /// The static mesh cache
        /// </summary>
        public static Dictionary<uint, StaticMesh> Meshes;

        /// <summary>
        /// Static constructor
        /// </summary>
        static StaticMeshCache()
        {
            Meshes = new Dictionary<uint, StaticMesh>();
        }

        /// <summary>
        /// Returns true if mesh is already cached
        /// </summary>
        /// <param name="id">The model id</param>
        public static bool Contains(uint id)
        {
            return Meshes.ContainsKey(id);
        }

        /// <summary>
        /// Adds a static mesh to the cache
        /// </summary>
        public static void Add(uint id, StaticMesh mesh)
        {
            Meshes.Add(id, mesh);
        }

        /// <summary>
        /// Performs a cache lookup,
        /// adds a new static mesh if not found
        /// </summary>
        /// <param name="id">The model id to fetch the static mesh for</param>
        public static StaticMesh GetMesh(uint id)
        {
            StaticMesh mesh = null;
            Meshes.TryGetValue(id, out mesh);
            if (mesh == null)
            {
                mesh = new StaticMesh(id);
                Add(id, mesh);
            }
            return mesh;
        }

        // TODO: different caching strategies
        // besides global cache
    }
}
