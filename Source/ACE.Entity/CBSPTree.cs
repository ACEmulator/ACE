using ACE.Entity;
using ACE.Entity.Enum;

public class CBSPTree : System.IDisposable
{

    private CBSPNode rootNode;
    private readonly BSPType treeType;



    // BSP:
    // General: There are BSPNode and BSPLeaf objects, 
    //          each node can have a frontChild and/or a backChild 
    //          On the engine side, there's also an encompassing BSPTree that's used for top-level querying.
    // Inheritance: Leaf inherits from Node, although logically we can merge everything and just have leaf nodes
    //              be identified as those that don't have children.
    // Triangle indicies: These refer to the trifan index in their respective category 
    //                    (drawing BSPs have a list of drawing trifans, physics BSPs are the same) 
    //


    // ===============
    // BSPTree
    // ===============
    public CBSPTree(BSPType type)
    {
        this.treeType = type;
        this.rootNode = null;
    }
    public void Dispose()
    {
        if (this.rootNode != null)
        {
            this.rootNode.Dispose();
        }
    }

    public bool Parse(cByteStream pBS)
    {
        CBSPNode tempNode = new CBSPNode();
        this.rootNode = tempNode.ParseNode(pBS, this.treeType);
        return true;
    }
    public bool SkipOver(cByteStream pBS)
    {
        CBSPNode tempNode = new CBSPNode();
        tempNode.ParseNode(pBS, this.treeType);
        return true;
    }

    // Note: most of the spherepath/ctransition related functions not present.
    // we mostly just need the range/bound checks on the nodes and leaves.

    public AceMath3dUtil.SPHERE_DEF GetSphere()
    {
        return (this.rootNode.GetSphere());
    }
    public CBSPNode GetRootNode()
    {
        return (this.rootNode);
    }

    public void InitPolygonRefs(CPolygon[] poly_refs, uint32_t poly_ref_count)
    {
        this.rootNode.InitPolygonRefs(poly_refs, poly_ref_count);
    }


    // This is rather important function that does the collision compensation 
    // for a solid sphere against a physics BSP, provided an initial colliding polygon (hit_poly). 
    //
    // check_pos is the sphere's position in the next time quanta, as if it were unhindered by 
    // collision adjustment.  curr_pos is the sphere's current center position.
    // NOTE: This function modifies check_pos!!
    //
    // If you need 'hit_poly', call SphereIntersectsPoly or SphereIntersectsSolidPoly first.
    public bool AdjustToPlane(AceMath3dUtil.SPHERE_DEF check_pos, vec3t curr_pos, CPolygon hit_poly, vec3t contact_pt)
    {
        // 'iterCountMax' limits the number of collision adjustments/total iterations in one call,
        // set to 15 to match AC's implementation.
        //
        // Essentially, adjusting a sphere's position to compensate for collision
        // may cause it to hit other polygons, which necessitates a retest after each adjustment.
        // The terminating condition, of course, would be to have adjusted the sphere so that it no longer
        // collides with any polygon in the BSP nor with 'hit_poly'.  However, this is not always possible.
        const uint32_t iterCountMax = 15;
        const float convergeThreshold = 0.02f;
        uint32_t count = new uint32_t();
        // initial time ranges set to max (1.0) and min (0.0)
        float utime = 1.0f;
        float ltime = 0.0f;
        float avgtime;
        float time_touch;
        vec3t movement = new vec3t();
        vec3t adjusted = new vec3t();

        // movement = check_pos - curr_pos
        Vec3_Copy(movement, check_pos.pos);
        Vec3_Sub(movement, curr_pos);

        // loop 1 - repeatedly adjust the sphere until no collisions left, or until limit reached
        count = 0;
        while (count++ < iterCountMax != null)
        {
            // AdjustSphereToPoly returns 1.0 if the sphere is not intersecting the polygon.
            time_touch = hit_poly.AdjustSphereToPoly(check_pos, curr_pos, movement);
            if (time_touch == 1.0f)
            {
                break;
            }

            // calcuate new adjusted position
            // adjusted = (movement * time_touch) + curr_pos
            Vec3_Copy(adjusted, movement);
            Vec3_Scale(adjusted, time_touch);
            Vec3_Add(adjusted, curr_pos);
            // new check_pos->pos to test = adjusted
            Vec3_Copy(check_pos.pos, adjusted);

            // test adjusted check_pos against the BSP
            // note: SphereIntersectsPoly will modify 'hit_poly' if a collision is found, pointing to
            //       a polygon in the BSP that is hit.
            if (this.rootNode.SphereIntersectsPoly(check_pos, movement, hit_poly, contact_pt) == false)
            {
                // found an adjusted position where it's not colliding with anything anymore.
                ltime = time_touch;
                break;
            }
            else
            {
                utime = time_touch;
            }
        }
        // reached max, didn't successfully adjust.
        if (count >= iterCountMax)
        {
            return false;
        }

        // loop 2
        // note, we carry over the previous counter value such that we make at most 'iterCountMax'
        // iterations across both loops.
        while (count++ < iterCountMax != null)
        {
            // move sphere along path, converging on the point of contact.
            // this is also to test collisions against the BSP along the way.
            avgtime = (ltime + utime) * 0.5;

            // adjusted = (avgtime * movement) + curr_pos
            Vec3_Copy(adjusted, movement);
            Vec3_Scale(adjusted, avgtime);
            Vec3_Add(adjusted, curr_pos);
            // new check_pos->pos = adjusted
            Vec3_Copy(check_pos.pos, adjusted);

            // test against BSP
            if (this.rootNode.SphereIntersectsPoly(check_pos, movement, hit_poly, contact_pt) == true)
            {
                // collision (we stepped too far forward), move back some.
                utime = (ltime + utime) * 0.5;
            }
            else
            {
                // step forward
                ltime = (ltime + utime) * 0.5;
            }

            // convergence condition
            if ((utime - ltime) < convergeThreshold)
            {
                break;
            }
        }

        // get the position just before contact.
        // adjusted = (ltime * movement) + curr_pos
        Vec3_Copy(adjusted, movement);
        Vec3_Scale(adjusted, ltime);
        Vec3_Add(adjusted, curr_pos);
        // set final adjusted sphere position = adjusted
        Vec3_Copy(check_pos.pos, adjusted);

        return true;
    }
}
