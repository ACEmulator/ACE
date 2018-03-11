using System.Collections.Generic;
using System.Numerics;
using ACE.Entity;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Collision
{
    public class GfxObj
    {
        public uint ID;
        //public Material Material;
        public int NumSurfaces;
        //public List<Surface> Surfaces;
        public List<uint> SurfaceIDs;
        public CVertexArray VertexArray;
        public int NumPhysicsPolygons;
        public Dictionary<ushort, Polygon> PhysicsPolygons;
        //public MeshBuffer ConstructedMesh;
        public bool UseBuiltMesh;
        public Sphere PhysicsSphere;
        public BSP.BSPTree PhysicsBSP;
        public Vector3 SortCenter;
        public int NumPolygons;
        public Dictionary<ushort, Polygon> Polygons;
        public Sphere DrawingSphere;
        //public BSPTree DrawingBSP;

        // is this useful for collision detection,
        // or only for drawing?
        public BBox GfxBoundBox;

        public GfxObj() { }

        public GfxObj(DatLoader.FileTypes.GfxObj gfxObj)
        {
            if (gfxObj == null) return;

            ID = gfxObj.Id;
            SurfaceIDs = gfxObj.Surfaces;
            NumSurfaces = SurfaceIDs != null ? SurfaceIDs.Count : 0;
            VertexArray = CreateMutable(gfxObj.VertexArray);
            NumPhysicsPolygons = gfxObj.PhysicsPolygons.Count;
            PhysicsPolygons = new Dictionary<ushort, Polygon>();
            foreach (var kvp in gfxObj.PhysicsPolygons)
                PhysicsPolygons.Add(kvp.Key, CreateMutable(kvp.Value));
            // usebuiltmesh
            // physicssphere
            PhysicsBSP = CreateMutable(gfxObj.PhysicsBSP);  // not mutable,
            // but still has non-unpack methods associated with it
            SortCenter = gfxObj.SortCenter;
            NumPolygons = gfxObj.Polygons.Count;
            Polygons = new Dictionary<ushort, Polygon>();
            foreach (var kvp in gfxObj.Polygons)
                Polygons.Add(kvp.Key, CreateMutable(kvp.Value));
            // drawing sphere/bsp
        }

        public TransitionState FindObjCollisions(GfxObj gfxObj, Transition transition, float scaleZ)
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

        public static CVertexArray CreateMutable(DatLoader.Entity.CVertexArray _vertexArray)
        {
            var vertexArray = new CVertexArray();
            vertexArray.VertexType = _vertexArray.VertexType;
            vertexArray.Vertices = new Dictionary<ushort, SWVertex>();
            foreach (var kvp in _vertexArray.Vertices)
                vertexArray.Vertices.Add(kvp.Key, CreateMutable(kvp.Value));
            return vertexArray;
        }

        public static SWVertex CreateMutable(DatLoader.Entity.SWVertex _vertex)
        {
            var vertex = new SWVertex();
            vertex.NormalX = _vertex.NormalX;
            vertex.NormalY = _vertex.NormalY;
            vertex.NormalZ = _vertex.NormalZ;
            // ignore texture coordinate u/v
            vertex.X = _vertex.X;
            vertex.Y = _vertex.Y;
            vertex.Z = _vertex.Z;
            return vertex;
        }

        public static Polygon CreateMutable(DatLoader.Entity.Polygon _polygon)
        {
            var polygon = new Polygon();
            // clone lists?
            polygon.NegSurface = _polygon.NegSurface;
            polygon.NegUVIndices = _polygon.NegUVIndices;
            polygon.NumPoints = _polygon.NumPts;
            polygon.PosSurface = _polygon.PosSurface;
            polygon.PosUVIndices = _polygon.PosUVIndices;
            polygon.SidesType = (Sidedness)_polygon.SidesType;
            polygon.Stippling = _polygon.Stippling;
            polygon.VertexIDs = _polygon.VertexIds;
            // build vertices
            return polygon;
        }

        public static BSP.BSPTree CreateMutable(DatLoader.Entity.BSPTree _bspTree)
        {
            return null;
        }
    }
}
