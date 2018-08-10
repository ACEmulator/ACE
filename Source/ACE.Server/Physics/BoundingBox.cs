using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Entity;

namespace ACE.Server.Physics
{
    /// <summary>
    /// A bounding box for collision detection
    /// </summary>
    public class BoundingBox
    {
        /// <summary>
        /// The model this bounding box encompasses
        /// </summary>
        public ModelMesh Model;

        /// <summary>
        /// The minimum values for each dimension
        /// </summary>
        public Vector3 Min;

        /// <summary>
        /// The maximum values for each dimension
        /// </summary>
        public Vector3 Max;

        /// <summary>
        /// The center of the bounding box
        /// </summary>
        public Vector3 Center;

        /// <summary>
        /// The size of the bounding box
        /// </summary>
        public Vector3 Size;

        /// <summary>
        /// Empty constructor
        /// </summary>
        public BoundingBox()
        {
        }
        
        /// <summary>
        /// Constructs a new bounding box
        /// for a model
        /// </summary>
        public BoundingBox(ModelMesh model)
        {
            Model = model;
            BuildBox(model);
        }
        
        /// <summary>
        /// Builds a bounding box for a model
        /// </summary>
        public void BuildBox(ModelMesh model)
        {
            Min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // build the transformation matrix
            var transform = GetTransform(model);

            foreach (var gfxObj in model.StaticMesh.GfxObjs)
            {
                foreach (var v in gfxObj.VertexArray.Vertices.Values)
                {
                    var vertex = Vector3.Transform(v.Origin, transform);

                    if (vertex.X < Min.X) Min.X = vertex.X;
                    if (vertex.Y < Min.Y) Min.Y = vertex.Y;
                    if (vertex.Z < Min.Z) Min.Z = vertex.Z;

                    if (vertex.X > Max.X) Max.X = vertex.X;
                    if (vertex.Y > Max.Y) Max.Y = vertex.Y;
                    if (vertex.Z > Max.Z) Max.Z = vertex.Z;
                }
            }
            CalcSize();
        }

        /// <summary>
        /// Calculates the size and center properties
        /// </summary>
        public void CalcSize()
        {
            Size = new Vector3(Max.X - Min.X, Max.Y - Min.Y, Max.Z - Min.Z);
            Center = new Vector3(Min.X + Size.X / 2, Min.Y + Size.Y / 2, Min.Z + Size.Z / 2);
        }

        /// <summary>
        /// Builds the matrix transformation
        /// </summary>
        public Matrix4x4 GetTransform(ModelMesh model)
        {
            var scale = Matrix4x4.CreateScale(model.Scale);
            var rotate = Matrix4x4.CreateFromQuaternion(new Quaternion(model.Frame.Orientation.X, model.Frame.Orientation.Y, model.Frame.Orientation.Z, model.Frame.Orientation.W));
            var cellTranslate = Matrix4x4.CreateTranslation(new Vector3(model.Cell.X * LandblockMesh.CellSize, model.Cell.Y * LandblockMesh.CellSize, 0));
            var cellTranslateInner = Matrix4x4.CreateTranslation(new Vector3(model.Position.X, model.Position.Y, model.Position.Z));

            var transform = scale * rotate * cellTranslate * cellTranslateInner;

            return transform;
        }

        /// <summary>
        /// Returns TRUE if point is inside box
        /// on the XY plane
        /// </summary>
        public bool Contains2D(Vector3 point)
        {
            return (point.X >= Min.X && point.X <= Max.X) &&
                   (point.Y >= Min.Y && point.Y <= Max.Y);
        }

        /// <summary>
        /// Returns TRUE if bounding boxes are touching
        /// on the XY plane
        /// </summary>
        public bool Intersect2D(BoundingBox b)
        {
            return (Min.X <= b.Max.X && Max.X >= b.Min.X) &&
                   (Min.Y <= b.Max.Y && Max.Y >= b.Min.Y);
        }

        /// <summary>
        /// Returns TRUE if point is inside box
        /// </summary>
        public bool Contains(Vector3 point)
        {
            return (point.X >= Min.X && point.X <= Max.X) &&
                   (point.Y >= Min.Y && point.Y <= Max.Y) &&
                   (point.Z >= Min.Z && point.Z <= Max.Z);
        }


        /// <summary>
        /// Returns TRUE if bounding boxes are touching
        /// </summary>
        public bool Intersect(BoundingBox b)
        {
            return (Min.X <= b.Max.X && Max.X >= b.Min.X) &&
                   (Min.Y <= b.Max.Y && Max.Y >= b.Min.Y) &&
                   (Min.Z <= b.Max.Z && Max.Z >= b.Min.Z);
        }

        public void ConvertToGlobal()
        {

        }

        /// <summary>
        /// Returns 8 corner points of the bounding box
        /// </summary>
        public List<Vector3> GetCornerPoints()
        {
            // lower corner points
            var lowerNW = new Vector3(Min.X, Max.Y, Min.Z);
            var lowerNE = new Vector3(Max.X, Max.Y, Min.Z);
            var lowerSW = new Vector3(Min.X, Min.Y, Min.Z);
            var lowerSE = new Vector3(Max.X, Min.Y, Min.Z);

            // upper corner points
            var upperNW = new Vector3(Min.X, Max.Y, Max.Z);
            var upperNE = new Vector3(Max.X, Max.Y, Max.Z);
            var upperSW = new Vector3(Min.X, Min.Y, Max.Z);
            var upperSE = new Vector3(Max.X, Min.Y, Max.Z);

            return new List<Vector3>() { lowerNW, lowerNE, lowerSW, lowerSE, upperNW, upperNE, upperSW, upperSE };
        }
    }
}
