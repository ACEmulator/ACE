using ACE.Entity.Enum;
using System;
using System.Numerics;
using ACE.Entity;
using static ACE.Entity.AceVector3;

// Plane described in normal-distance form (Ax+By+Cz+D=0).
// Basically you can visualize it like this:
// Given a unit-length normal vector N and a distance from (local) origin = abs(d),
// the plane orientation (rotation) is given by N
// and the point on the plane that is closest to the origin is given by (-d*N)
// if d = 0, the plane intersects the origin.
//
// For sidedness, positions that are facing away from the normal
// (beyond -90 to 90 deg relative angle) are considered negative.
//
// To flip the plane "sidedness" without changing the plane orientation/position itself,
// just flip the signs of N and d (-d*N == d*-N).
//
// x intercept = -d/a  (-d/N.x)
// y intercept = -d/b
// z intercept = -d/c
/*typedef struct
{
    Vector3 N;    // normal
    float d;    // dist
} PLANE_DEF;*/

public class CPlane
{
    // going to make plane params public, since they'll be referenced a lot.
    public CFrameRef m_FrameRef = new CFrameRef();
    public Vector3 N = new Vector3();
    public float d;

    // nothing - default XY plane @ Z=0.
    public CPlane()
    {
        Vector3 default_normal = new Vector3(0,0,1);
        SetParams(default_normal, 0.0f);
    }
    // given plane normal + any point on plane
    public CPlane(Vector3 normal, Vector3 pt)
    {
        // d = -(pt DOT n)
        SetParams(normal, -(Vector3.Dot(pt, N));
    }
    // plane normal + d
    public CPlane(Vector3 normal, float dist)
    {
        SetParams(normal, dist);
    }
    // from triangle vertices (doesn't check for degenerate cases)
    public CPlane(Vector3 a, Vector3 b, Vector3 c)
    {
        InitFromTriangle(a, b, c, true);
    }

    public void InitFromTriangle(Vector3 a, Vector3 b, Vector3 c, bool fNormalizeN = true)
    {
        Vector3[] t = Arrays.InitializeWithDefaultInstances<Vector3>(2);
        Vector3 n = new Vector3();
        float d;

        // n = (b-a) CROSS (c-a)
        t[0] = b;
        Vector3.Subtract(t[0], a);
        t[1] = c;
        Vector3.Subtract(t[1], a);
        n = Vector3.Cross(t[0], t[1]);
        if (fNormalizeN)
        {
            Vector3.Normalize(n);
        }

        // d = -(a DOT n)
        d = -(Vec3_Dot(a, n));

        SetParams(n, d);
    }

    public float PlanePointDistance(Vector3 pt)
    {
        return (Vec3_Dot(N, pt) + d);
    }

    // basically cos(Z_angle)
    // 0.0 if vertical (XZ/YZ aligned)
    // 1.0 if horizontal (XY aligned)
    // can return negative values depending on where the plane normal is facing.
    public float GetSlopeZ()
    {
        return (N.z);
    }

    // used for verticality check.
    public bool IsVerticalZ()
    {
        return ((Math.Abs(N.z) <= (0.0002f)));
    }

    // flips the "sidedness" of the plane without changing the plane's orientation.
    public void FlipSidedness()
    {
        Vec3_Neg(N);
        d = -(d);
    }

    // plane/ray

    // ray = point + direction
    // direction vector expected to be normalized.
    public bool RayIntersectionTime(Vector3 ray_pt, Vector3 ray_dir, ref float time)
    {
        float dir;

        // relative direction (derived from angle)
        dir = Vec3_Dot(ray_dir, N);
        if ((Math.Abs(dir) <= (0.0002f)))
        {
            return false; // non-intersecting (parallel)
        }

        time = Vec3_Dot(ray_pt, N) / (-dir);
        if (time < 0.0f)
        {
            return false; // direction faces away from plane
        }

        return true;
    }
    public bool RayIntersectionPoint(Vector3 ray_pt, Vector3 ray_dir, Vector3 pt)
    {
        float t;

        if (RayIntersectionTime(ray_pt, ray_dir, ref t) == false)
        {
            return false;
        }

        // intersect_point = ray_pt + (ray_dir * time)
        Vec3_Copy(pt, ray_dir);
        Vec3_Scale(pt, t);
        Vec3_Add(pt, ray_pt);

        return true;
    }

    // side classification

    // determine which side of the plane point pt is wrt plane
    // for most uses, bias = 0.
    public PlaneSide ClassifySidePoint(Vector3 p, float bias)
    {
        float dist;

        dist = PlanePointDistance(p) + bias;
        if ((Math.Abs(dist) <= (0.0002f)))
        {
            return (PlaneSide.kSideIN_PLANE); // point in plane (within tolerance and bias)
        }

        return ((dist > 0.0f) ? PlaneSide.kSidePOSITIVE : PlaneSide.kSideNEGATIVE);
    }

    // prfDistR (returned dist_r value) can be a NULL pointer.
    public PlaneSide ClassifySideSphere(AceMath3dUtil.SPHERE_DEF sphere, ref float prfDistR)
    {
        PlaneSide side;
        float dist;
        float dist_r;

        dist = PlanePointDistance(sphere.pos);
        if ((Math.Abs(dist) <= (0.0002f)))
        {
            return (PlaneSide.kSideIN_PLANE);
        }

        // based on the sign of dist, we know which side the sphere center is
        // Just have to find out now if the sphere's extent crosses the plane.
        if (dist < 0.0f)
        {
            // center on negative side of sphere
            dist_r = dist + sphere.radius;
            side = (dist_r > 0.0f) ? PlaneSide.kSideCROSSING : PlaneSide.kSideNEGATIVE;
        }
        else // if (dist postive)
        {
            dist_r = dist - sphere.radius;
            side = (dist_r < 0.0f) ? PlaneSide.kSideCROSSING : PlaneSide.kSidePOSITIVE;
        }

        // For crossing spheres,
        // the actual contact point (or nearest contact point) can be given by:
        // = sphere->pos + (-dist_r * plane.N)

        if (prfDistR != null)
        {
            prfDistR = dist_r;
        }

        return (side);
    }

    // returns whether it's crossing or pos/neg, used for BSP AABB bounding volumes.
    // If one or more points is IN the plane, it's considered crossing.
    // technically the logic should also work for OBBs
    public PlaneSide ClassifySideAABB(AceMath3dUtil.AABB_DEF box)
    {
        Vector3 vMin = new Vector3();
        Vector3 vMax = new Vector3();
        Vector3 p = new Vector3();
        uint32_t r = new uint32_t();
        uint32_t[] result = { 0 };
        uint32_t index = new uint32_t();

        // "sort" the box pts where p0 contains the min extents, and p1 the max.
        // vMin is left/bottom (smallest x,y,z value)
        // vMax is right/top (largest x,y,z values)
        vMin.x = ((box.p0.x) < (box.p1.x) != 0F ? (box.p0.x) : (box.p1.x));
        vMin.y = ((box.p0.y) < (box.p1.y) != 0F ? (box.p0.y) : (box.p1.y));
        vMin.z = ((box.p0.z) < (box.p1.z) != 0F ? (box.p0.z) : (box.p1.z));

        vMax.x = ((box.p0.x) > (box.p1.x) ? (box.p0.x) : (box.p1.x));
        vMax.y = ((box.p0.y) > (box.p1.y) ? (box.p0.y) : (box.p1.y));
        vMax.z = ((box.p0.z) > (box.p1.z) ? (box.p0.z) : (box.p1.z));

        // Note that the logic differs from AC client, but should arrive at the same results.

        // permute and classify all 8 points
        for (index = 0; index < 8; index++)
        {
            GlobalMembers.iPermuteBoxPoint(p, vMin, vMax, index);
            r = ClassifySidePoint(p, 0.0f);
            // accumulate results.
            result[r]++;
        }

        // crossing (both pos and neg sides) OR any in-plane result.
        if ((result[(int)PlaneSide.kSidePOSITIVE] > 0 && result[(int)PlaneSide.kSideNEGATIVE] > 0) || result[(int)PlaneSide.kSideIN_PLANE] > 0)
        {
            return (PlaneSide.kSideCROSSING);
        }

        // else, its either only kSidePOSITIVE or kSideNEGATIVE.
        if (result[(int)PlaneSide.kSidePOSITIVE] > 0)
        {
            return (PlaneSide.kSidePOSITIVE);
        }
        else // result[kSideNEGATIVE] > 0
        {
            return (PlaneSide.kSideNEGATIVE);
        }
    }

    // space conversions
    // New N = L-to-G Normal transformed by m_FrameRef->mBW
    // pNew = plane origin transformed by LocalToGlobal call.
    // New Dist = -(pNew DOT plane.N)
    //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
    //        void LocalToGlobal(CPlane result);

    // sphere adjustment (stuff useful for collision response / walkability checks)

    // Note: Modifies 'sphere->pos'
    //
    // can also be used to adjust spheres to general coplanar polygons
    // (provided you check first that the sphere actually intersects the polygon)
    //
    // Also supports solid volumes - pass in fSolid and set solid_side to whichever side is supposed
    // to be solid (which for AC should be kSideNEGATIVE).
    //
    // returns true if sphere position adjusted.
    public bool AdjustSphere(AceMath3dUtil.SPHERE_DEF sphere, bool fSolid, PlaneSide solid_side)
    {
        PlaneSide side;
        PlaneSide center_side;
        Vector3 adjust = new Vector3();
        float dist;
        float dist_r;

        dist = PlanePointDistance(sphere.pos);
        side = ClassifySideSphere(sphere, ref dist_r);
        // non-intersecting non-solid cases don't need adjusting.
        // (although if you wanted to 'snap' a sphere to a polygon, you can still use the same logic)
        if (fSolid == false && side != PlaneSide.kSideCROSSING)
        {
            return false;
        }

        // if center lies within the solid side, the adjustment will need to be more.
        if (fSolid == true)
        {
            // classify where the sphere center lies relative to plane.
            center_side = (dist > 0.0f) ? PlaneSide.kSidePOSITIVE : PlaneSide.kSideNEGATIVE;

            if (center_side == solid_side)
            {
                // negative side solid, have to adjust sphere to to positive side.
                if (solid_side == PlaneSide.kSideNEGATIVE)
                {
                    dist_r = -dist + sphere.radius;
                }
                // positive side solid, have to adjust sphere to negative side.
                else
                {
                    dist_r = -dist - sphere.radius;
                }
            }
            else
            {
                // we can reject cases where the sphere is wholly in the non-solid side.
                //
                // otherwise, for solid-but-sphere-center-not-in-solid cases, they're handled
                // just the same as non-solids (which is to "snap" to the closest side of the plane)
                if (Math.Abs(dist) > sphere.radius)
                {
                    return false;
                }
            }
        }

        // adjust in the direction of plane normal N.
        // = sphere->pos + (-dist_r * plane.N)
        Vec3_Copy(adjust, N);
        Vec3_Scale(adjust, -dist_r);
        Vec3_Add(sphere.pos, adjust);

        return true;
    }

    // get Z (height) of a given x,y; returns false on failure (vertically-aligned plane)
    public bool GetZ(float x, float y, ref float outZ)
    {
        float numer;

        // For any plane Ax + By + Cz + D = 0, solve for z, given A,B,C,x,y.
        // (recall that A = N.x, B = N.y, C = N.z)
        // z = (-D - Ax - By) / C

        // check for plane verticality (C == 0)
        if (IsVerticalZ())
        {
            return false;
        }

        numer = -d - (x * N.x) - (y * N.y);
        outZ = (numer / N.z);

        return true;
    }

    // aligns a vector to a plane, given x and y, becoming parallel or conicident to it.
    // Note: modifies offset->z
    public bool SnapToPlane(Vector3 offset)
    {
        float numer;

        if (IsVerticalZ())
        {
            return false;
        }

        // newZ = GetZ() - (-d / N.z)
        //      = ((-D - Ax - By) / C) + (D/C)
        //      = (-Ax - By) / C
        numer = -(offset.x * N.x) - (offset.y * N.y);
        offset.z = (numer / N.z);

        return true;
    }

    // N expected to be normalized.
    public void SetParams(Vector3 N, float d)
    {
        Vec3_Copy(this.N, N);
        this.d = d;
    }
    public void SetFrameRef(CFrameRef frameref)
    {
        m_FrameRef.CopyFrom(frameref);
    }
}
