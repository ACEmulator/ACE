using System;
using System.Numerics;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Collision;
using ACE.Server.Physics.Common;
using ACE.Server.Physics.Extensions;

namespace ACE.Server.Physics
{
    /// <summary>
    /// A cylinder sphere
    /// </summary>
    public class CylSphere: IEquatable<CylSphere>
    {
        /// <summary>
        /// The base of the cylinder sphere
        /// </summary>
        public Vector3 LowPoint;

        /// <summary>
        /// The height of the cylinder sphere
        /// </summary>
        public float Height;

        /// <summary>
        /// The radius of the cylinder sphere
        /// </summary>
        public float Radius;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CylSphere()
        {
            LowPoint = Vector3.Zero;
        }

        /// <summary>
        /// Constructs a cylinder sphere from components
        /// </summary>
        /// <param name="lowPoint">The base of the cylinder sphere</param>
        /// <param name="height">The height of the object</param>
        /// <param name="radius">The object radius</param>
        public CylSphere(Vector3 lowPoint, float height, float radius)
        {
            LowPoint = lowPoint;
            Height = height;
            Radius = radius;
        }

        /// <summary>
        /// Constructs a cylsphere loaded from portal.dat
        /// </summary>
        public CylSphere(DatLoader.Entity.CylSphere cylSphere)
        {
            LowPoint = cylSphere.Origin;
            Height = cylSphere.Height;
            Radius = cylSphere.Radius;
        }

        /// <summary>
        /// Constructs a cylinder sphere from components and scale
        /// </summary>
        /// <param name="lowPoint">The base of the cylinder sphere</param>
        /// <param name="height">The height of the object</param>
        /// <param name="radius">The object radius</param>
        /// <param name="scale">The scale to be applied to all components</param>
        public CylSphere(Vector3 lowPoint, float height, float radius, float scale)
        {
            LowPoint = lowPoint * scale;
            Height = height * scale;
            Radius = radius * scale;
        }

        /// <summary>
        /// Redirects a cylinder sphere to be on collision course towards a point
        /// </summary>
        /// <param name="transition">The transition information for the sphere</param>
        /// <param name="checkPos">The spherical point to redirect towards</param>
        /// <param name="disp">Used for calculating the collision normal</param>
        /// <param name="radsum">The sum of the sphere and spherical point radii</param>
        /// <param name="sphereNum">Used as an offset in path.GlobalCurrCenter to determine movement</param>
        /// <returns>The TransitionState either collided or adjusted</returns>
        public TransitionState CollideWithPoint(Transition transition, Sphere checkPos, Vector3 disp, float radsum, int sphereNum)
        {
            var obj = transition.ObjectInfo;
            var path = transition.SpherePath;
            var collisions = transition.CollisionInfo;

            Vector3 collisionNormal;
            var definate = CollisionNormal(transition, checkPos, disp, radsum, sphereNum, out collisionNormal);
            if (Vec.NormalizeCheckSmall(ref collisionNormal))
                return TransitionState.Collided;
            if (!obj.State.HasFlag(ObjectInfoState.PerfectClip))
            {
                collisions.SetCollisionNormal(collisionNormal);
                return TransitionState.Collided;
            }
            var globCenter = path.GlobalCurrCenter[0].Center;
            var movement = LandDefs.GetBlockOffset(path.CurPos.ObjCellID, path.CheckPos.ObjCellID);
            movement += checkPos.Center - globCenter;
            var old_disp = globCenter - LowPoint;
            radsum += PhysicsGlobals.EPSILON;

            // this is similar to ray/sphere intersection, could be inlined...
            var xyMoveLenSq = movement.LengthSquared2D();
            var xyDiff = -movement.Dot2D(old_disp);
            var diffSq = xyDiff * xyDiff - (old_disp.LengthSquared2D() - radsum * radsum) * xyMoveLenSq;
            var diff = (float)Math.Sqrt(diffSq);
            Vector3 scaledMovement, offset;
            float time;     // calculated below, refactor

            if (!definate)
            {
                if (Math.Abs(movement.Z) < PhysicsGlobals.EPSILON)
                    return TransitionState.Collided;
                if (movement.Z > 0.0f)
                {
                    collisionNormal = new Vector3(0, 0, -1.0f);
                    time = (movement.Z + checkPos.Radius) / movement.Z * -1.0f;
                }
                else
                {
                    collisionNormal = new Vector3(0, 0, 1.0f);
                    time = (checkPos.Radius + Height - movement.Z) / movement.Z;
                }

                scaledMovement = movement * time;
                var scaledLenSq = (scaledMovement + old_disp).LengthSquared2D();
                if (scaledLenSq >= radsum * radsum)
                {
                    if (Math.Abs(xyMoveLenSq) < PhysicsGlobals.EPSILON)
                        return TransitionState.Collided;

                    if (diffSq >= 0.0f && xyMoveLenSq > PhysicsGlobals.EPSILON)
                    {
                        if (xyDiff - diff < 0.0f)
                            time = (float)((diff - movement.Dot2D(old_disp)) / xyMoveLenSq);
                        else
                            time = (float)((xyDiff - diff) / xyMoveLenSq);

                        scaledMovement = movement * time;
                    }
                    collisionNormal = (scaledMovement + globCenter - LowPoint) / radsum;
                    collisionNormal.Z = 0.0f;
                }

                if (time < 0.0f || time > 1.0f)
                    return TransitionState.Collided;

                offset = globCenter - scaledMovement - checkPos.Center;
                path.AddOffsetToCheckPos(offset, checkPos.Radius);
                collisions.SetCollisionNormal(collisionNormal);

                return TransitionState.Adjusted;
            }

            if (collisionNormal.Z != 0.0f)
            {
                if (Math.Abs(movement.Z) < PhysicsGlobals.EPSILON)
                    return TransitionState.Collided;

                if (movement.Z > 0.0f)
                    time = -((old_disp.Z + checkPos.Radius) / movement.Z);
                else
                    time = (checkPos.Radius + Height - old_disp.Z) / movement.Z;

                scaledMovement = movement * time;

                if (time < 0.0f || time > 1.0f)
                    return TransitionState.Collided;

                offset = globCenter + scaledMovement - checkPos.Center;
                path.AddOffsetToCheckPos(offset, checkPos.Radius);
                collisions.SetCollisionNormal(collisionNormal);

                return TransitionState.Adjusted;
            }

            // duplicated from above, refactor...
            if (diffSq < 0.0f || xyMoveLenSq < PhysicsGlobals.EPSILON)
                return TransitionState.Collided;

            if (xyDiff - diff < 0.0f)
                time = (float)((diff - movement.Dot2D(old_disp)) / xyMoveLenSq);
            else
                time = (float)((xyDiff - diff) / xyMoveLenSq);

            scaledMovement = movement * time;
            if (time < 0.0f || time > 1.0f)
                return TransitionState.Collided;

            collisionNormal = (scaledMovement + globCenter - LowPoint) / radsum;
            collisionNormal.Z = 0.0f;
            offset = globCenter + scaledMovement - checkPos.Center;
            path.AddOffsetToCheckPos(offset, checkPos.Radius);
            collisions.SetCollisionNormal(collisionNormal);

            return TransitionState.Adjusted;
        }

        /// <summary>
        /// Returns true if this CylSphere collides with a sphere
        /// </summary>
        public bool CollidesWithSphere(Sphere checkPos, Vector3 disp, float radsum)
        {
            var result = false;

            if (disp.X * disp.X + disp.Y * disp.Y <= radsum * radsum)
            {
                if (checkPos.Radius - PhysicsGlobals.EPSILON + Height * 0.5f >= Math.Abs(Height * 0.5f - disp.Z))
                    result = true;
            }
            return result;
        }

        /// <summary>
        /// Determines if this CylSphere collides with anything during its transition
        /// </summary>
        /// <param name="transition">The transition path for this cylinder sphere</param>
        /// <returns>The collision result for this transition path</returns>
        public TransitionState IntersectsSphere(Transition transition)
        {
            var obj = transition.ObjectInfo;
            var path = transition.SpherePath;

            var globSphere = path.GlobalSphere[0];
            var disp = globSphere.Center - LowPoint;

            Sphere globSphere_ = null;
            Vector3 disp_ = Vector3.Zero;

            if (path.NumSphere > 1)
            {
                globSphere_ = path.GlobalSphere[1];
                disp_ = globSphere_.Center - LowPoint;
            }

            var radsum = Radius - PhysicsGlobals.EPSILON + globSphere.Radius;

            if (path.InsertType == InsertType.Placement || path.ObstructionEthereal)
            {
                if (CollidesWithSphere(globSphere, disp, radsum))
                    return TransitionState.Collided;

                if (path.NumSphere > 1 && CollidesWithSphere(globSphere_, disp_, radsum))
                    return TransitionState.Collided;

                return TransitionState.OK;
            }
            else
            {
                if (path.StepDown)
                    return StepSphereDown(transition, globSphere, disp, radsum);

                if (path.CheckWalkable)
                {
                    if (CollidesWithSphere(globSphere, disp, radsum))
                        return TransitionState.Collided;

                    if (path.NumSphere > 1)
                    {
                        if (CollidesWithSphere(globSphere_, disp_, radsum))
                            return TransitionState.Collided;
                    }
                    return TransitionState.OK;
                }

                if (!path.Collide)
                {
                    if (obj.State.HasFlag(ObjectInfoState.Contact) || obj.State.HasFlag(ObjectInfoState.OnWalkable))
                    {
                        if (CollidesWithSphere(globSphere, disp, radsum))
                            return StepSphereUp(transition, globSphere, disp, radsum);

                        if (path.NumSphere > 1)
                        {
                            if (CollidesWithSphere(globSphere_, disp_, radsum))
                                return SlideSphere(transition, globSphere_, disp, radsum, 1);
                        }
                    }
                    else if (obj.State.HasFlag(ObjectInfoState.PathClipped))
                    {
                        if (CollidesWithSphere(globSphere, disp, radsum))
                            return CollideWithPoint(transition, globSphere, disp, radsum, 0);
                    }
                    else
                    {
                        if (CollidesWithSphere(globSphere, disp, radsum))
                            return LandOnCylinder(transition, globSphere, disp, radsum);

                        if (path.NumSphere > 1)
                        {
                            if (CollidesWithSphere(globSphere_, disp_, radsum))
                                return CollideWithPoint(transition, globSphere_, disp_, radsum, 1);
                        }
                    }
                    return TransitionState.OK;
                }

                if (CollidesWithSphere(globSphere, disp, radsum) || path.NumSphere > 1 && CollidesWithSphere(globSphere_, disp_, radsum))
                {
                    var blockOffset = path.GetCurPosCheckPosBlockOffset();
                    var movement = path.GlobalCurrCenter[0].Center - globSphere.Center - blockOffset;

                    if (Math.Abs(movement.Z) < PhysicsGlobals.EPSILON)
                        return TransitionState.Collided;

                    var timecheck = (Height + globSphere.Radius - disp.Z) / movement.Z;

                    var offset = movement * timecheck;

                    var offsetDispSum = offset + disp;

                    if (radsum * radsum < offsetDispSum.Dot2D(offsetDispSum))
                        return TransitionState.OK;

                    var t = (1.0f - timecheck) * transition.SpherePath.WalkInterp;

                    if (t >= transition.SpherePath.WalkInterp || t < -0.1f)
                        return TransitionState.Collided;

                    var pDist = globSphere.Center + offset;
                    pDist.Z -= globSphere.Radius;

                    var contactPlane = new Plane(Vector3.UnitZ, -Vector3.Dot(Vector3.UnitZ, pDist));

                    transition.CollisionInfo.SetContactPlane(contactPlane, true);
                    transition.CollisionInfo.ContactPlaneCellID = transition.SpherePath.CheckPos.ObjCellID;
                    transition.SpherePath.WalkInterp = t;
                    transition.SpherePath.AddOffsetToCheckPos(offset, globSphere.Radius);

                    return TransitionState.Adjusted;
                }
            }

            return TransitionState.OK;
        }

        /// <summary>
        /// Determines if this sphere collides with anything during its transition
        /// </summary>
        /// <returns>The collision result for this transition path</returns>
        public TransitionState IntersectsSphere(Position pos, float scale, Transition transition)
        {
            var path = transition.SpherePath;

            path.CacheLocalSpaceSphere(pos, 1.0f);
            var global_cylsphere = new CylSphere(LowPoint, Height, Radius, scale);
            global_cylsphere.LowPoint = path.CheckPos.LocalToGlobal(pos, global_cylsphere.LowPoint);

            return global_cylsphere.IntersectsSphere(transition);
        }

        /// <summary>
        /// Handles the collision when an object lands on a cylinder sphere
        /// </summary>
        public TransitionState LandOnCylinder(Transition transition, Sphere checkPos, Vector3 disp, float radsum)
        {
            Vector3 collisionNormal;
            CollisionNormal(transition, checkPos, disp, radsum, 0, out collisionNormal);

            if (Vec.NormalizeCheckSmall(ref collisionNormal))
                return TransitionState.Collided;

            var path = transition.SpherePath;
            path.SetCollide(collisionNormal);
            path.WalkableAllowance = PhysicsGlobals.LandingZ;

            return TransitionState.Adjusted;
        }

        /// <summary>
        /// Attempts to slide the CylSphere from a collision
        /// </summary>
        public TransitionState SlideSphere(Transition transition, Sphere checkPos, Vector3 disp, float radsum, int sphereNum)
        {
            Vector3 collisionNormal;
            CollisionNormal(transition, checkPos, disp, radsum, sphereNum, out collisionNormal);
            if (Vec.NormalizeCheckSmall(ref collisionNormal))
                return TransitionState.Collided;

            return checkPos.SlideSphere(transition, ref collisionNormal, transition.SpherePath.GlobalCurrCenter[sphereNum].Center);
        }

        /// <summary>
        /// Attempts to move the CylSphere down from a collision
        /// </summary>
        public TransitionState StepSphereDown(Transition transition, Sphere checkPos, Vector3 disp, float radsum)
        {
            var path = transition.SpherePath;

            Sphere globSphere_ = null;
            Vector3 disp_ = Vector3.Zero;

            if (path.NumSphere > 1)
            {
                globSphere_ = path.GlobalSphere[1];
                disp_ = globSphere_.Center - LowPoint;
            }

            if (CollidesWithSphere(checkPos, disp, radsum) || path.NumSphere > 1 && CollidesWithSphere(globSphere_, disp_, radsum))
            {
                var stepScale = path.StepDownAmt * path.WalkInterp;
                if (Math.Abs(stepScale) < PhysicsGlobals.EPSILON)
                    return TransitionState.Collided;

                var deltaz = Height + checkPos.Radius - disp.Z;
                var interp = (1.0f - deltaz / stepScale) * path.WalkInterp;
                if (interp >= path.WalkInterp || interp < -0.1f)
                    return TransitionState.Collided;

                var normal = Vector3.UnitZ;
                var contactPoint = new Vector3(checkPos.Center.X, checkPos.Center.Y, checkPos.Center.Z + (deltaz - checkPos.Radius));
                var contactPlane = new Plane(normal, -Vector3.Dot(normal, contactPoint));

                var collisions = transition.CollisionInfo;
                collisions.SetContactPlane(contactPlane, true);     // is water?
                path.WalkInterp = interp;
                path.AddOffsetToCheckPos(new Vector3(0, 0, deltaz), checkPos.Radius);
                return TransitionState.Adjusted;
            }
            return TransitionState.OK;
        }


        /// <summary>
        /// Attempts to move the CylSphere up from a collision
        /// </summary>
        public TransitionState StepSphereUp(Transition transition, Sphere checkPos, Vector3 disp, float radsum)
        {
            var obj = transition.ObjectInfo;

            if (obj.StepUpHeight < checkPos.Radius + Height - disp.Z)
                return SlideSphere(transition, checkPos, disp, radsum, 0);

            Vector3 collisionNormal;
            CollisionNormal(transition, checkPos, disp, radsum, 0, out collisionNormal);
            if (Vec.NormalizeCheckSmall(ref collisionNormal))
                return TransitionState.Collided;

            var path = transition.SpherePath;
            var globalPos = path.LocalSpacePos.LocalToGlobalVec(collisionNormal);
            if (transition.StepUp(globalPos))
                return TransitionState.OK;
            else
                return path.StepUpSlide(transition);
        }

        /// <summary>
        /// Returns the collision normal for this CylSphere transition
        /// </summary>
        public bool CollisionNormal(Transition transition, Sphere checkPos, Vector3 _disp, float radsum, int sphereNum, out Vector3 normal)
        {
            var disp = transition.SpherePath.GlobalCurrCenter[sphereNum].Center - LowPoint;
            if (radsum * radsum < disp.LengthSquared2D())
            {
                normal = new Vector3(disp.X, disp.Y, 0);
                return checkPos.Radius - PhysicsGlobals.EPSILON + Height * 0.5f >= Math.Abs(Height * 0.5f - disp.Z)
                    || Math.Abs(disp.Z - _disp.Z) <= PhysicsGlobals.EPSILON;
            }
            var normZ = (_disp.Z - disp.Z <= 0.0f) ? 1 : -1;
            normal = new Vector3(0, 0, normZ);
            return true;
        }

        public bool Equals(CylSphere cylSphere)
        {
            return cylSphere != null && Height == cylSphere.Height && Radius == cylSphere.Radius && LowPoint == cylSphere.LowPoint;
        }

        public override int GetHashCode()
        {
            int hash = 0;

            hash = (hash * 397) ^ Height.GetHashCode();
            hash = (hash * 397) ^ Radius.GetHashCode();
            hash = (hash * 397) ^ LowPoint.GetHashCode();

            return hash;
        }
    }
}
