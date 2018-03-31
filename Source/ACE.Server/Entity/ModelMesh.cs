using System.Collections.Generic;
using System.Numerics;
using ACE.DatLoader.Entity;
using ACE.Server.Physics;

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
        /// from the original data
        /// </summary>
        public Frame Frame;

        /// <summary>
        /// The position backing store
        /// </summary>
        private Vector3? _position;

        /// <summary>
        /// The rotation backing store
        /// </summary>
        private Quaternion? _rotation;

        /// <summary>
        /// The position of the model instance
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return _position == null ? Frame.Origin : _position.Value;
            }
            set
            {
                _position = value;
            }
        }

        /// <summary>
        /// The rotation of the model instance
        /// </summary>
        public Quaternion Rotation
        {
            get
            {
                return _rotation == null ? Frame.Orientation : _rotation.Value;
            }
            set
            {
                _rotation = value;
            }
        }

        /// <summary>
        /// The cell offsets within the landblock
        /// </summary>
        public Vector2 Cell = Vector2.Zero;

        /// <summary>
        /// The scale of the model
        /// </summary>
        public float Scale = 1.0f;

        /// <summary>
        /// For scenery object types
        /// </summary>
        public ObjectDesc ObjectDesc;

        /// <summary>
        /// The list of polygons comprising the mesh
        /// </summary>
        public List<Polygon> Polygons;

        /// <summary>
        /// The bounding box for collision detection
        /// </summary>
        public BoundingBox BoundingBox;

        /// <summary>
        /// Constructs a static mesh instance from a model and frame
        /// </summary>
        public ModelMesh(uint modelId, Frame frame)
        {
            Init(modelId, frame);
        }

        /// <summary>
        /// Constructs a new model mesh for a static land object
        /// </summary>
        public ModelMesh(Stab stab)
        {
            Init(stab.Id, stab.Frame);
        }

        /// <summary>
        /// Constructs a new model mesh for a building
        /// </summary>
        public ModelMesh(BuildInfo buildInfo)
        {
            Init(buildInfo.ModelId, buildInfo.Frame);
        }

        /// <summary>
        /// Initializes a new mesh instance
        /// </summary>
        /// <param name="modelId">The modelID of the mesh to load</param>
        /// <param name="frame">The position/orientation info</param>
        public void Init(uint modelId, Frame frame)
        {
            GetMesh(modelId);
            Frame = frame;

            BuildPolygons();
            BuildBoundingBox();
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
                Polygons.Add(new Polygon(polygon, Position, Rotation, Scale));
            }
        }

        /// <summary>
        /// Builds a bounding box for the model mesh
        /// </summary>
        public void BuildBoundingBox()
        {
            BoundingBox = new BoundingBox(this);
        }
    }
}
