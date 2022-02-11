using System;
using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Collision;
using ACE.Server.Physics.Extensions;

namespace ACE.Server.Physics.BSP
{
    public enum BSPTreeType
    {
        Drawing = 0x0,
        Physics = 0x1,
        Cell = 0x2
    };

    public class BSPTree: IEquatable<BSPTree>
    {
        public BSPNode RootNode;

        public BSPTree() { }

        public BSPTree(BSPNode rootNode)
        {
            RootNode = rootNode;
        }

        public BSPTree(DatLoader.Entity.BSPTree bsp, Dictionary<ushort, DatLoader.Entity.Polygon> polys, DatLoader.Entity.CVertexArray vertexArray)
        {
            RootNode = new BSPNode(bsp.RootNode, polys, vertexArray);
        }

        public Sphere GetSphere()
        {
            return RootNode.Sphere;
        }

        public void RemoveNonPortalNodes()
        {
            RootNode.LinkPortals(RootNode.PurgePortals());
        }

        public bool adjust_to_plane(Sphere checkPos, Vector3 curPos, Polygon hitPoly, Vector3 contactPoint)
        {
            var movement = checkPos.Center - curPos;
            var lowerTime = 0.0;
            var upperTime = 1.0;

            var maxIterations = 15;    // hardcoded
            for (var i = 0; i < maxIterations; i++)
            {
                var touchTime = hitPoly.adjust_sphere_to_poly(checkPos, curPos, movement);
                if (touchTime == 1.0f)
                {
                    checkPos.Center = curPos + movement * (float)touchTime;

                    if (!RootNode.sphere_intersects_poly(checkPos, movement, ref hitPoly, ref contactPoint))
                    {
                        lowerTime = touchTime;
                        break;
                    }
                    upperTime = touchTime;
                }

                if (i == maxIterations - 1)
                    return false;
            }

            for (var j = 0; j < maxIterations; j++)
            {
                var averageTime = (lowerTime + upperTime) * 0.5f;

                checkPos.Center = curPos + movement * (float)averageTime;

                if (!RootNode.sphere_intersects_poly(checkPos, movement, ref hitPoly, ref contactPoint))
                    upperTime = (lowerTime + upperTime) * 0.5f;
                else
                    lowerTime = (lowerTime + upperTime) * 0.5f;

                if (upperTime - lowerTime < 0.02)
                    return false;
            }

            return true;
        }

        public bool box_intersects_cell_bsp(BBox box)
        {
            return RootNode.box_intersects_cell_bsp(box);
        }

        public void build_draw_portals_only(int portalPolyOrPortalContents)
        {
            // for rendering
        }

        public TransitionState check_walkable(SpherePath path, Sphere checkPos, float scale)
        {
            var validPos = new Sphere(checkPos);
            return RootNode.hits_walkable(path, validPos, path.LocalSpaceZ) ? TransitionState.Collided : TransitionState.OK;
        }

        public TransitionState collide_with_pt(Transition transition, Sphere checkPos, Vector3 curPos, Polygon hitPoly, Vector3 contactPoint, float scale)
        {
            var obj = transition.ObjectInfo;
            var path = transition.SpherePath;
            var collisions = transition.CollisionInfo;
            var collisionNormal = path.LocalSpacePos.LocalToGlobalVec(hitPoly.Plane.Normal);

            if (!obj.State.HasFlag(ObjectInfoState.PerfectClip))
            {
                collisions.SetCollisionNormal(collisionNormal);
                return TransitionState.Collided;
            }

            var validPos = new Sphere(checkPos);

            if (!adjust_to_plane(validPos, curPos, hitPoly, contactPoint))
                return TransitionState.Collided;

            collisions.SetCollisionNormal(collisionNormal);

            var adjusted = validPos.Center - checkPos.Center;
            var offset = path.LocalSpacePos.LocalToGlobalVec(adjusted) * scale;

            path.AddOffsetToCheckPos(offset);

            return TransitionState.Adjusted;
        }

        public TransitionState find_collisions(Transition transition, float scale)
        {
            var path = transition.SpherePath;
            var collisions = transition.CollisionInfo;

            var center = path.LocalSpaceCurrCenter[0].Center;
            var localSphere = path.LocalSpaceSphere[0];

            var localSphere_ = path.NumSphere > 1 ? path.LocalSpaceSphere[1] : null;

            var movement = localSphere.Center - center;

            if (path.InsertType == InsertType.Placement || path.ObstructionEthereal)
            {
                var clearCell = true;
                if (path.BuildingCheck)
                    clearCell = !path.HitsInteriorCell;

                if (RootNode.sphere_intersects_solid(localSphere, clearCell) || path.NumSphere > 1 && RootNode.sphere_intersects_solid(localSphere_, clearCell))
                    return TransitionState.Collided;
                else
                    return TransitionState.OK;
            }

            if (path.CheckWalkable)
                return check_walkable(path, localSphere, scale);

            if (path.StepDown)
                return step_sphere_down(transition, localSphere, scale);

            Polygon hitPoly = null;

            if (path.Collide)
            {
                var changed = false;
                var validPos = new Sphere(localSphere);

                RootNode.find_walkable(path, validPos, ref hitPoly, movement, path.LocalSpaceZ, ref changed);

                if (changed)
                {
                    var offset = validPos.Center - localSphere.Center;
                    var collisionNormal = path.LocalSpacePos.LocalToGlobalVec(offset) * scale;
                    path.AddOffsetToCheckPos(collisionNormal);

                    var contactPlane = hitPoly.Plane.LocalToGlobal(path.CheckPos, path.LocalSpacePos, hitPoly.Plane);
                    contactPlane.D *= scale;

                    collisions.SetContactPlane(contactPlane, false);
                    collisions.ContactPlaneCellID = path.CheckPos.ObjCellID;
                    path.SetWalkable(validPos, hitPoly, path.LocalSpaceZ, path.LocalSpacePos, scale);

                    return TransitionState.Adjusted;
                }
                else
                    return TransitionState.OK;
            }

            var contactPoint = Vector3.Zero;
            var obj = transition.ObjectInfo;

            if (obj.State.HasFlag(ObjectInfoState.Contact))
            {
                if (RootNode.sphere_intersects_poly(localSphere, movement, ref hitPoly, ref contactPoint))
                    return step_sphere_up(transition, hitPoly.Plane.Normal);

                if (path.NumSphere > 1)
                {
                    Polygon hitPoly_ = null;

                    if (RootNode.sphere_intersects_poly(localSphere_, movement, ref hitPoly_, ref contactPoint))
                        return slide_sphere(transition, hitPoly_.Plane.Normal);

                    if (hitPoly_ != null) return NegPolyHit(path, hitPoly_, false);
                    if (hitPoly  != null) return NegPolyHit(path, hitPoly,  true);
                }
                return TransitionState.OK;
            }
            
            if (RootNode.sphere_intersects_poly(localSphere, movement, ref hitPoly, ref contactPoint) || hitPoly != null)
            {
                if (obj.State.HasFlag(ObjectInfoState.PathClipped))
                    return collide_with_pt(transition, localSphere, center, hitPoly, contactPoint, scale);

                var collisionNormal = path.LocalSpacePos.LocalToGlobalVec(hitPoly.Plane.Normal);
                path.WalkableAllowance = PhysicsGlobals.LandingZ;
                path.SetCollide(collisionNormal);

                return TransitionState.Adjusted;
            }
            else if (path.NumSphere > 1)
            {
                if (RootNode.sphere_intersects_poly(localSphere_, movement, ref hitPoly, ref contactPoint) || hitPoly != null)
                {
                    var collisionNormal = path.LocalSpacePos.LocalToGlobalVec(hitPoly.Plane.Normal);
                    collisions.SetCollisionNormal(collisionNormal);

                    return TransitionState.Collided;
                }
            }
            return TransitionState.OK;
        }

        public TransitionState NegPolyHit(SpherePath path, Polygon hitPoly, bool stepUp)
        {
            var collisionNormal = path.LocalSpacePos.LocalToGlobalVec(hitPoly.Plane.Normal);
            path.SetNegPolyHit(stepUp, collisionNormal);

            return TransitionState.OK;
        }

        public TransitionState placement_insert(Transition transition)
        {
            var path = transition.SpherePath;

            var validPos = new Sphere(path.LocalSpaceSphere[0]);
            var rad = validPos.Radius;

            Sphere validPos_ = null;
            if (path.NumSphere > 1)
                validPos_ = new Sphere(path.LocalSpaceSphere[1]);

            var clearCell = true;
            if (path.BuildingCheck)
                clearCell = !path.HitsInteriorCell;

            Polygon hitPoly = null;

            var maxIterations = 20;   // hardcoded
            for (var i = 0; i < maxIterations; i++)
            {
                var centerSolid = false;
                if (RootNode.sphere_intersects_solid_poly(validPos, rad, ref centerSolid, ref hitPoly, clearCell))
                {
                    if (hitPoly != null)
                    {
                        hitPoly.adjust_to_placement_poly(validPos, validPos_, rad, centerSolid, clearCell);
                        continue;
                    }
                }
                else
                {
                    if (path.NumSphere >= 2)
                    {
                        if (RootNode.sphere_intersects_solid_poly(validPos_, rad, ref centerSolid, ref hitPoly, clearCell))
                        {
                            if (hitPoly != null)
                            {
                                hitPoly.adjust_to_placement_poly(validPos_, validPos, rad, centerSolid, clearCell);
                                continue;
                            }
                        }
                        else
                            return placement_insert_inner(validPos, path, i);
                    }
                    else
                        return placement_insert_inner(validPos, path, i);
                }
                rad *= 2;
            }
            return TransitionState.Collided;
        }

        public TransitionState placement_insert_inner(Sphere validPos, SpherePath path, int i)
        {
            if (i == 0)
                return TransitionState.OK;

            var adjust = validPos.Center - path.LocalSpaceSphere[0].Center;
            var offset = path.LocalSpacePos.LocalToGlobalVec(adjust);
            path.AddOffsetToCheckPos(offset);
            return TransitionState.Adjusted;
        }

        public bool point_inside_cell_bsp(Vector3 origin)
        {
            return RootNode.point_inside_cell_bsp(origin);
        }

        public TransitionState slide_sphere(Transition transition, Vector3 collisionNormal)
        {
            // modifies collision normal?
            var path = transition.SpherePath;
            var globNormal = path.LocalSpacePos.LocalToGlobalVec(collisionNormal);
            return path.GlobalSphere[0].SlideSphere(transition, ref globNormal, path.GlobalCurrCenter[0].Center);
        }

        public BoundingType sphere_intersects_cell_bsp(Sphere sphere)
        {
            return RootNode.sphere_intersects_cell_bsp(sphere);
        }

        public TransitionState step_sphere_down(Transition transition, Sphere checkPos, float scale)
        {
            var path = transition.SpherePath;
            var collisions = transition.CollisionInfo;

            var step_down_amount = -(path.StepDownAmt * path.WalkInterp);

            var movement = path.LocalSpaceZ * step_down_amount * (1.0f / scale);
            var validPos = new Sphere(checkPos);
            var changed = false;
            Polygon polyHit = null;

            RootNode.find_walkable(path, validPos, ref polyHit, movement, path.LocalSpaceZ, ref changed);

            if (changed)
            {
                var adjusted = validPos.Center - checkPos.Center;
                var offset = path.LocalSpacePos.LocalToGlobalVec(adjusted) * scale;
                path.CheckPos.Frame.Origin += offset;
                path.CacheGlobalSphere(offset);

                var contactPlane = polyHit.Plane.LocalToGlobal(path.CheckPos, path.LocalSpacePos, polyHit.Plane);

                collisions.ContactPlaneValid = true;
                collisions.ContactPlane.Normal = contactPlane.Normal;
                collisions.ContactPlane.D = contactPlane.D * scale;

                collisions.ContactPlaneIsWater = false;
                collisions.ContactPlaneCellID = path.CheckPos.ObjCellID;

                path.SetWalkable(validPos, polyHit, path.LocalSpaceZ, path.LocalSpacePos, scale);

                return TransitionState.Adjusted;
            }

            return TransitionState.OK;
        }

        public TransitionState step_sphere_up(Transition transition, Vector3 collisionNormal)
        {
            var path = transition.SpherePath;
            var globNormal = path.LocalSpacePos.LocalToGlobalVec(collisionNormal);

            if (transition.StepUp(globNormal))
                return TransitionState.OK;
            else
                return path.StepUpSlide(transition);
        }

        public bool Equals(BSPTree bspTree)
        {
            if (bspTree == null) return false;
            return RootNode.Equals(bspTree.RootNode);
        }

        public override int GetHashCode()
        {
            return RootNode.GetHashCode();
        }
    }
}
