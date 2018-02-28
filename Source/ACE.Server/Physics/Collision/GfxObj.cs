using System.Collections.Generic;
using System.Numerics;
using ACE.Entity;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Collision
{
    public class GfxObj
    {
        public int ID;
        //public Material Material;
        public int NumSurfaces;
        //public List<Surface> Surfaces;
        public CVertexArray VertexArray;
        public int NumPhysicsPolygons;
        public List<Polygon> PhysicsPolygons;
        //public MeshBuffer ConstructedMesh;
        public bool UseBuiltMesh;
        public Sphere PhysicsSphere;
        public BSPTree PhysicsBSP;
        public Vector3 SortCenter;
        public int NumPolygons;
        public List<Polygon> Polygons;
        public Sphere DrawingSphere;
        //public BSPTree DrawingBSP;

        // is this useful for collision detection,
        // or only for drawing?
        public BBox GfxBoundBox;

        public static TransitionState FindObjCollisions(GfxObj gfxObj, Transition transition, float scaleZ)
        {
            return TransitionState.OK;
        }

        public static GfxObj Get(int gfxObjID)
        {
            return (GfxObj)DBObj.Get(new QualifiedDataID(6, gfxObjID));
        }
    }
}
