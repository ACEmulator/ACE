using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using ACE.DatLoader.Entity;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Entity;

namespace ACE.Server.Physics.Collision
{
    public class GfxObj
    {
        public uint ID;
        public CVertexArray VertexArray;
        /// <summary>
        /// Only populated if !PhysicsEngine.Instance.Server
        /// </summary>
        public Dictionary<ushort, Polygon> Polygons;
        /// <summary>
        /// Only populated if !PhysicsEngine.Instance.Server
        /// </summary>
        public Dictionary<ushort, Polygon> PhysicsPolygons;
        public Sphere PhysicsSphere;
        public BSP.BSPTree PhysicsBSP;
        /// <summary>
        /// Only populated if !PhysicsEngine.Instance.Server
        /// </summary>
        public Vector3 SortCenter;
        public Sphere DrawingSphere;
        public BSP.BSPTree DrawingBSP;
        public BBox GfxBoundBox;
        //public Material Material;     // unused
        //public List<Surface> Surfaces;    // unused
        //public List<uint> SurfaceIDs;     // unused

        public GfxObj() { }

        public GfxObj(DatLoader.FileTypes.GfxObj gfxObj)
        {
            if (gfxObj == null) return;

            ID = gfxObj.Id;
            VertexArray = gfxObj.VertexArray;

            if (!PhysicsEngine.Instance.Server)
            {
                Polygons = new Dictionary<ushort, Polygon>();
                foreach (var kvp in gfxObj.Polygons)
                    Polygons.Add(kvp.Key, PolygonCache.Get(kvp.Value, gfxObj.VertexArray));
            }

            if (gfxObj.PhysicsPolygons.Count > 0)
            {
                if (!PhysicsEngine.Instance.Server)
                {
                    PhysicsPolygons = new Dictionary<ushort, Polygon>();
                    foreach (var kvp in gfxObj.PhysicsPolygons)
                        PhysicsPolygons.Add(kvp.Key, PolygonCache.Get(kvp.Value, gfxObj.VertexArray));
                }

                PhysicsBSP = BSPCache.Get(gfxObj.PhysicsBSP, gfxObj.PhysicsPolygons, gfxObj.VertexArray);
                PhysicsSphere = PhysicsBSP.GetSphere();
            }

            if (!PhysicsEngine.Instance.Server)
                SortCenter = gfxObj.SortCenter;
            DrawingBSP = BSPCache.Get(gfxObj.DrawingBSP, gfxObj.Polygons, gfxObj.VertexArray);
            DrawingSphere = DrawingBSP.GetSphere();

            Init();
        }

        public TransitionState FindObjCollisions(Transition transition, float scaleZ)
        {
            var path = transition.SpherePath;

            foreach (var localSpaceSphere in path.LocalSpaceSphere)
            {
                var offset = PhysicsSphere.Center - localSpaceSphere.Center;
                var radsum = PhysicsSphere.Radius + localSpaceSphere.Radius;

                if (offset.LengthSquared() - radsum * radsum < PhysicsGlobals.EPSILON)
                {
                    if (path.InsertType == InsertType.InitialPlacement)
                        return PhysicsBSP.placement_insert(transition);
                    else
                        return PhysicsBSP.find_collisions(transition, scaleZ);
                }
            }
            return TransitionState.OK;
        }

        public void Init()
        {
            GfxBoundBox = new BBox();

            if (VertexArray.Vertices != null && VertexArray.Vertices.Count > 0)
            {
                var v = VertexArray.Vertices.Values.First();
                GfxBoundBox.Min = v.Origin; // ref?
                GfxBoundBox.Max = v.Origin;

                foreach (var vertex in VertexArray.Vertices.Values)
                    GfxBoundBox.AdjustBBox(vertex.Origin);
            }
            else
            {
                GfxBoundBox.Min = Vector3.Zero;
                GfxBoundBox.Max = Vector3.Zero;
            }
        }
    }
}
