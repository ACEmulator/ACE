using System.Collections.Generic;
using ACE.DatLoader.Entity;

namespace ACE.Server.Entity
{
    public enum ModelMeshType
    {
        Building,
        LandObject,
        Scenery,
        Weenie
    };

    /// <summary>
    /// An instanced static mesh
    /// </summary>
    public class ModelMesh: Mesh
    {
        /// <summary>
        /// The static mesh
        /// </summary>
        public StaticMesh StaticMesh;

        /// <summary>
        /// The position and orientation
        /// </summary>
        public Frame Frame;

        /// <summary>
        /// The list of polygons comprising the mesh
        /// </summary>
        public List<Polygon> Polygons;

        /// <summary>
        /// Constructs a static mesh instance from a model and frame
        /// </summary>
        public ModelMesh(uint modelId, Frame frame)
        {
            GetMesh(modelId);
            Frame = frame;
            BuildPolygons();
        }

        /// <summary>
        /// Constructs a new model mesh for a static land object
        /// </summary>
        public ModelMesh(Stab stab)
        {
            GetMesh(stab.Id);
            Frame = stab.Frame;
            BuildPolygons();
        }

        /// <summary>
        /// Constructs a new model mesh for a building
        /// </summary>
        public ModelMesh(BuildInfo buildInfo)
        {
            GetMesh(buildInfo.ModelId);
            Frame = buildInfo.Frame;
            BuildPolygons();
        }

        /// <summary>
        /// Returns a pointer to the static mesh
        /// </summary>
        public void GetMesh(uint modelId)
        {
            StaticMesh = StaticMeshCache.GetMesh(modelId);
        }

        /// <summary>
        /// Builds the polygons for this model mesh
        /// into world space
        /// </summary>
        public void BuildPolygons()
        {
            Polygons = new List<Polygon>();

            foreach (var polygon in StaticMesh.Polygons)
            {
                Polygons.Add(new Polygon(polygon, Frame));
            }
        }
    }
}
