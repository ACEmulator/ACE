using System;
using System.Numerics;
using ACE.Entity.Enum;

namespace ACE.Server.Physics.Animation
{
    [Flags]
    public enum ObjectInfoState
    {
        Default         = 0x0,
        Contact         = 0x1,
        OnWalkable      = 0x2,
        IsViewer        = 0x4,
        PathClipped     = 0x8,
        FreeRotate      = 0x10,
        PerfectClip     = 0x40,
        IsImpenetrable  = 0x80,
        IsPlayer        = 0x100,
        EdgeSlide       = 0x200,
        IgnoreCreatures = 0x400,
        IsPK            = 0x800,
        IsPKLite        = 0x1000
    };

    public class ObjectInfo
    {
        public PhysicsObj Object;
        public ObjectInfoState State;
        public float Scale;
        public float StepUpHeight;
        public float StepDownHeight;
        public bool Ethereal;
        public bool StepDown;
        public uint TargetID;

        public float GetWalkableZ()
        {
            return Object.get_walkable_z();
        }

        public void Init(PhysicsObj obj, ObjectInfoState state)
        {
            Object = obj;
            State = state;   // copy constructor?
            Scale = Object.Scale;
            StepUpHeight = Object.GetStepUpHeight();
            StepDownHeight = Object.GetStepDownHeight();
            Ethereal = Object.State.HasFlag(PhysicsState.Ethereal);
            StepDown = !Object.State.HasFlag(PhysicsState.Missile);
            var wobj = Object.WeenieObj;
            if (wobj != null)
            {
                if (wobj.IsImpenetrable())
                    State |= ObjectInfoState.IsImpenetrable;
                if (wobj.IsPlayer())
                    State |= ObjectInfoState.IsPlayer;
                if (wobj.IsPK())
                    State |= ObjectInfoState.IsPK;
                if (wobj.IsPKLite())
                    State |= ObjectInfoState.IsPKLite;
            }
            if (obj.ProjectileTarget != null)
                TargetID = obj.ProjectileTarget.ID;
        }

        public bool IsValidWalkable(Vector3 normal)
        {
            return PhysicsObj.is_valid_walkable(normal);
        }

        public bool MissileIgnore(PhysicsObj collideObj)
        {
            // modified for 2-way
            if (collideObj.State.HasFlag(PhysicsState.Missile))
            {
                if (!Object.IsPlayer)
                    return true;

                if (collideObj.ProjectileTarget == null || collideObj.ProjectileTarget == Object)
                    return false;

                return true;
            }

            if (Object.State.HasFlag(PhysicsState.Missile))
            {
                if (collideObj.WeenieObj != null && collideObj.ID != TargetID)
                {
                    if (collideObj.State.HasFlag(PhysicsState.Ethereal) || TargetID != 0 && collideObj.WeenieObj.IsCreature())
                        return true;
                }
            }
            return false;
        }

        public void StopVelocity()
        {
            Object.set_velocity(Vector3.Zero, false);
        }

        public TransitionState ValidateWalkable(Sphere checkPos, Plane contactPlane, bool isWater, float waterDepth, Transition transition, uint landCellID)
        {
            var path = transition.SpherePath;
            var collision = transition.CollisionInfo;

            if (State.HasFlag(ObjectInfoState.IsViewer))
            {
                var dist = Vector3.Dot(checkPos.Center, contactPlane.Normal) + contactPlane.D - checkPos.Radius;
                if (dist > -PhysicsGlobals.EPSILON && path.BeginPos != null && (path.BeginPos.ObjCellID & 0xFFFF) >= 0x100)
                    return TransitionState.OK;

                var offset = checkPos.Center - path.GlobalCurrCenter[0].Center;
                var angle = dist / Vector3.Dot(offset, contactPlane.Normal);
                if ((angle <= 0.0f || angle > 1.0f) && path.BeginPos != null && (path.BeginPos.ObjCellID & 0xFFFF) >= 0x100)
                    return TransitionState.OK;

                path.AddOffsetToCheckPos(offset * -angle);
                collision.SetCollisionNormal(contactPlane.Normal);
                collision.CollidedWithEnvironment = true;
                return TransitionState.Adjusted;
            }
            else
            {
                var dist = Vector3.Dot(checkPos.Center - new Vector3(0, 0, checkPos.Radius), contactPlane.Normal) + contactPlane.D + waterDepth;
                if (dist >= -PhysicsGlobals.EPSILON)
                {
                    if (dist > PhysicsGlobals.EPSILON)
                        return TransitionState.OK;

                    if (path.StepDown || !State.HasFlag(ObjectInfoState.OnWalkable) || PhysicsObj.is_valid_walkable(contactPlane.Normal))
                    {
                        collision.SetContactPlane(contactPlane, isWater);
                        collision.ContactPlaneCellID = landCellID;
                    }
                    if (!State.HasFlag(ObjectInfoState.Contact) && !path.StepDown)
                    {
                        collision.SetCollisionNormal(contactPlane.Normal);
                        collision.CollidedWithEnvironment = true;
                    }
                    return TransitionState.OK;
                }
                else
                {
                    if (path.CheckWalkable) return TransitionState.Collided;
                    var zDist = dist / contactPlane.Normal.Z;

                    if (path.StepDown || !State.HasFlag(ObjectInfoState.OnWalkable) || PhysicsObj.is_valid_walkable(contactPlane.Normal))
                    {
                        collision.SetContactPlane(contactPlane, isWater);
                        collision.ContactPlaneCellID = landCellID;
                        if (path.StepDown)
                        {
                            var interp = (1.0f - -1.0f / (path.StepDownAmt * path.WalkInterp) * zDist) * path.WalkInterp;
                            if (interp >= path.WalkInterp || interp < -0.1f)
                                return TransitionState.Collided;

                            path.WalkInterp = interp;
                        }

                        var offset = new Vector3(0, 0, -zDist);
                        path.AddOffsetToCheckPos(offset);
                    }

                    if (!State.HasFlag(ObjectInfoState.Contact) && !path.StepDown)
                    {
                        collision.SetCollisionNormal(contactPlane.Normal);
                        collision.CollidedWithEnvironment = true;
                    }
                    return TransitionState.Adjusted;
                }
            }
        }
    }
}
