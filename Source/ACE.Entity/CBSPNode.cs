using System.Diagnostics;
using System.Numerics;
using ACE.Entity;
using ACE.Entity.Enum;

public class CBSPNode : System.IDisposable
{
    protected AceMath3dUtil.SPHERE_DEF sphere = new AceMath3dUtil.SPHERE_DEF(); // bounding sphere
    protected CPlane partitionPlane = new CPlane(); // partitioning plane

    protected CBSPNode frontChild; // aka 'pos_node'; possibly NULL
    protected CBSPNode backChild; // aka 'neg_node'; possibly NULL

    // polygon references
    protected uint32_t fIndexCount = new uint32_t(); // trifan index count
    protected uint32_t[] tfIndicies; // trifan/triangle indicies (refer to parent model object)
    // actual polygon objects (note that the CBSPNode doesn't own these objects, just has refs)
    protected CPolygon[] polygons;

    protected BSPTreeType treeType;
    protected BSPNodeType nodeType;

    protected uint32_t offset = new uint32_t();

    // portal poly
    protected uint32_t portalPolyCount = new uint32_t();
    protected ACPortalPoly[] portalPolys;

    //ACModel *model;


    // ===============
    // BSPNode
    // ===============
    public CBSPNode()
    {
        this.frontChild = null;
        this.backChild = null;

        this.tfIndexCount = 0;
        this.tfIndicies = null;
        this.polygons = null;

        this.portalPolyCount = 0;
        this.portalPolys = null;
    }
    public CBSPNode(BSPType treeType_, BSPNodeType nodeType_)
    {
        this.treeType = treeType_;
        this.nodeType = nodeType_;

        this.frontChild = null;
        this.backChild = null;

        this.tfIndexCount = 0;
        this.tfIndicies = null;
        this.polygons = null;

        this.portalPolyCount = 0;
        this.portalPolys = null;
    }

    public virtual void Dispose()
    {
        if (this.frontChild != null)
        {
            this.frontChild.Dispose();
        }
        if (this.backChild != null)
        {
            this.backChild.Dispose();
        }
    }

    public CBSPNode ParseNode(cByteStream pBS, BSPType treeType_)
    {
        this.nodeType = (BSPNodeType)pBS.ReadDWORD();
        this.treeType = treeType_;

        this.offset = pBS.GetOffset();

        switch (this.nodeType)
        {
            case BSPNodeType.LEAF:
            {
                CBSPLeaf leaf = new CBSPLeaf(this.treeType);
                leaf.ParseLeaf(pBS);
                return (leaf);
            }

            case BSPNodeType.kBSPTLN_PORT:
            {
                CBSPPortal port = new CBSPPortal(this.treeType);
                port.ParsePortal(pBS);
                return (port);
            }

            default:
            {
                // anything else (Bxxx) = BSPNode
                CBSPNode node = new CBSPNode(this.treeType, this.nodeType);
                node.Parse(pBS);
                return (node);
            }
        }
    }
    public bool Parse(cByteStream pBS)
    {
        Vector3 plane_N = new Vector3();
        float plane_d;

        // partitioning plane
        plane_N.X = pBS.ReadFloat();
        plane_N.Y = pBS.ReadFloat();
        plane_N.Z = pBS.ReadFloat();
        plane_d = pBS.ReadFloat();
        // initialize plane
        this.partitionPlane.SetParams(plane_N, plane_d);

        switch (this.nodeType)
        {
            // front children only
            case BSPNodeType.BPnn:
            case BSPNodeType.BPIn:
                this.frontChild = this.ParseNode(pBS, this.treeType);
                break;

            // back children only
            case BSPNodeType.BpIN:
            case BSPNodeType.BpnN:
                this.backChild = this.ParseNode(pBS, this.treeType);
                break;

            // both
            case BSPNodeType.BPIN:
            case BSPNodeType.BPnN:
                this.frontChild = this.ParseNode(pBS, this.treeType);
                this.backChild = this.ParseNode(pBS, this.treeType);
                break;

            default:
                // this really is an error case.
                break;
        }

        // drawing and physics bsps define a bounding sphere
        if (this.treeType == BSPType.Drawing || this.treeType == BSPType.Physics)
        {
            this.sphere.pos.X = pBS.ReadFloat();
            this.sphere.pos.Y = pBS.ReadFloat();
            this.sphere.pos.Z = pBS.ReadFloat();
            this.sphere.radius = pBS.ReadFloat();
        }

        // drawing BSPs also have trifan indicies for non-leaf nodes.
        if (this.treeType == BSPType.Drawing)
        {
            this.tfIndexCount = pBS.ReadDWORD();
            if (this.tfIndexCount > 0)
            {
                this.tfIndicies = Arrays.InitializeWithDefaultInstances<uint32_t>(this.tfIndexCount);
                for (uint32_t index = 0; index < this.tfIndexCount; index++)
                {
                    this.tfIndicies[index] = pBS.ReadWORD();
                }
            }
        }

        return true;
    }

    public AceMath3dUtil.SPHERE_DEF GetSphere()
    {
        return (this.sphere);
    }
    public CPlane GetPartitionPlane()
    {
        return (this.partitionPlane);
    }


    // only call this function after BSP has been fully parsed and initialized.
    public void InitPolygonRefs(CPolygon[] poly_refs, uint32_t poly_ref_count)
    {
        // map this one first and then go init the children nodes.
        this.polygons = Arrays.InitializeWithDefaultInstances<CPolygon>(this.tfIndexCount);
        for (uint32_t ix = 0; ix < this.tfIndexCount; ix++)
        {
            Debug.Assert(this.tfIndicies[ix] <= poly_ref_count);
            this.polygons[ix] = poly_refs[this.tfIndicies[ix]];
        }

        // map child nodes
        if (this.frontChild != null)
        {
            this.frontChild.InitPolygonRefs(poly_refs, poly_ref_count);
        }
        if (this.backChild != null)
        {
            this.backChild.InitPolygonRefs(poly_refs, poly_ref_count);
        }
    }

    public virtual bool PointInsideCellBSP(vec3t origin)
    {
        CBSPNode currNode;
        PLANE_SIDE side;

        // go through the tree, if the point is within the volume, it'll be
        // on the positive facing side of all the partition planes (bounding volume assumed to be convex).
        currNode = this;
        while (true)
        {
            // classify side.
            side = currNode.partitionPlane.ClassifySidePoint(origin, 0.0f);
            if (side == PLANE_SIDE.kSideNEGATIVE)
            {
                return false;
            }

            // iterate the front-facing ('positive') nodes
            currNode = currNode.frontChild;
            if (currNode == null)
            {
                break;
            }
        }

        return true;
    }
    public virtual bool SphereIntersectsCellBSP(AceMath3dUtil.SPHERE_DEF sphere)
    {
        CBSPNode currNode = this;
        PLANE_SIDE side;

        // note: AC adds a tiny bias factor to the sphere radius (0.001),
        // but that should be insignificant so I'm skipping it.
        side = this.partitionPlane.ClassifySideSphere(sphere, null);
        if (side == PLANE_SIDE.kSideNEGATIVE)
        {
            return false;
        }

        while (true)
        {
            currNode = currNode.frontChild;
            if (side == PLANE_SIDE.kSideCROSSING)
            {
                break;
            }
            if (currNode == null)
            {
                return true;
            }

            side = currNode.partitionPlane.ClassifySideSphere(sphere, null);
            if (side == PLANE_SIDE.kSideNEGATIVE)
            {
                return false;
            }
        }

        // if we got here, then the sphere crosses one of the splitting planes.
        if (currNode != null)
        {
            return (currNode.SphereIntersectsCellBSP(sphere));
        }
        else
        {
            return true;
        }
    }

    public virtual bool PointIntersectsSolid(vec3t pt)
    {
        // recurse node based on side classification
        if (this.partitionPlane.ClassifySidePoint(pt, 0.0f) == PLANE_SIDE.kSideNEGATIVE)
        {
            return (this.backChild.PointIntersectsSolid(pt));
        }
        else
        {
            return (this.frontChild.PointIntersectsSolid(pt));
        }
    }
    public virtual bool SphereIntersectsSolid(AceMath3dUtil.SPHERE_DEF check_pos, bool fCenterCheck)
    {
        PLANE_SIDE side;

        // first test whether the sphere intersects the Node's bounding sphere
        // if not, then it's not in bounds and can't intersect.
        if (CollisionDet_Sphere_Sphere(this.sphere, check_pos) == false)
        {
            return false;
        }

        // sphere intersects bounding volume, have to figure out which side it's on.
        side = this.partitionPlane.ClassifySideSphere(check_pos, null);
        if (side == PLANE_SIDE.kSidePOSITIVE)
        {
            return (this.frontChild.SphereIntersectsSolid(check_pos, fCenterCheck));
        }
        else if (side == PLANE_SIDE.kSideNEGATIVE)
        {
            return (this.backChild.SphereIntersectsSolid(check_pos, fCenterCheck));
        }

        // if we got here, then the sphere intesects the partition plane.
        // we need to see which side the sphere center point lies.
        side = this.partitionPlane.ClassifySidePoint(check_pos.pos, 0F);
        if (side == PLANE_SIDE.kSideNEGATIVE)
        {
            // negative
            if (this.frontChild.SphereIntersectsSolid(check_pos, false) == true)
            {
                return true;
            }
            if (this.backChild.SphereIntersectsSolid(check_pos, fCenterCheck) == true)
            {
                return true;
            }
        }
        else
        {
            // positive OR in-plane
            if (this.frontChild.SphereIntersectsSolid(check_pos, fCenterCheck) == true)
            {
                return true;
            }
            if (this.backChild.SphereIntersectsSolid(check_pos, false) == true)
            {
                return true;
            }
        }

        return false;
    }
    // returns ptr to polygon hit in 'polygon' if it returns true.
    public virtual bool SphereIntersectsPoly(AceMath3dUtil.SPHERE_DEF check_pos, vec3t movement, CPolygon[] polygon, vec3t contact_pt)
    {
        PLANE_SIDE side;

        if (CollisionDet_Sphere_Sphere(this.sphere, check_pos) == false)
        {
            return false;
        }

        side = this.partitionPlane.ClassifySideSphere(check_pos, null);
        if (side == PLANE_SIDE.kSidePOSITIVE)
        {
            return (this.frontChild.SphereIntersectsPoly(check_pos, movement, polygon, contact_pt));
        }
        else if (side == PLANE_SIDE.kSideNEGATIVE)
        {
            return (this.backChild.SphereIntersectsPoly(check_pos, movement, polygon, contact_pt));
        }

        // intersects plane
        if (this.frontChild.SphereIntersectsPoly(check_pos, movement, polygon, contact_pt) == true)
        {
            return true;
        }
        if (this.backChild.SphereIntersectsPoly(check_pos, movement, polygon, contact_pt) == true)
        {
            return true;
        }

        return false;
    }
    public virtual bool SphereIntersectsSolidPoly(AceMath3dUtil.SPHERE_DEF check_pos, float radius, ref bool prfCenterSolid, CPolygon[] hit_poly, bool fCenterCheck)
    {
        PLANE_SIDE side;

        if (CollisionDet_Sphere_Sphere(this.sphere, check_pos) == false)
        {
            return false;
        }

        side = this.partitionPlane.ClassifySideSphere(check_pos, null);
        if (side == PLANE_SIDE.kSidePOSITIVE)
        {
            return (this.frontChild.SphereIntersectsSolidPoly(check_pos, radius, ref prfCenterSolid, hit_poly, fCenterCheck));
        }
        else if (side == PLANE_SIDE.kSideNEGATIVE)
        {
            return (this.backChild.SphereIntersectsSolidPoly(check_pos, radius, ref prfCenterSolid, hit_poly, fCenterCheck));
        }

        // intersects plane
        // classify sphere center point.
        side = this.partitionPlane.ClassifySidePoint(check_pos.pos, 0F);
        if (side == PLANE_SIDE.kSideNEGATIVE)
        {
            this.backChild.SphereIntersectsSolidPoly(check_pos, radius, ref prfCenterSolid, hit_poly, fCenterCheck);
            // if it didn't hit anything on the negative side, check positive side
            // (but don't do center check there since center pt isn't on that side)
            if (hit_poly[0] == null)
            {
                return (this.frontChild.SphereIntersectsSolidPoly(check_pos, radius, ref prfCenterSolid, hit_poly, false));
            }
            else
            {
                // if the hit polygon is solid, then it counts as a hit.
                return (prfCenterSolid);
            }
        }
        else
        {
            this.frontChild.SphereIntersectsSolidPoly(check_pos, radius, ref prfCenterSolid, hit_poly, fCenterCheck);
            if (hit_poly[0] == null)
            {
                return (this.backChild.SphereIntersectsSolidPoly(check_pos, radius, ref prfCenterSolid, hit_poly, false));
            }
            else
            {
                return (prfCenterSolid);
            }
        }
    }
}
