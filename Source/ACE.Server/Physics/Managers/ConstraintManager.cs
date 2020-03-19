using System.Numerics;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    public class ConstraintManager
    {
        public PhysicsObj PhysicsObj;
        public bool IsConstrained;
        public float ConstraintPosOffset;
        public Position ConstraintPos;
        public float ConstraintDistanceStart;
        public float ConstraintDistanceMax;

        public ConstraintManager() { }

        public ConstraintManager(PhysicsObj obj)
        {
            SetPhysicsObject(obj);
        }

        public static ConstraintManager Create(PhysicsObj obj)
        {
            return new ConstraintManager(obj);
        }

        public void ConstrainTo(Position position, float startDistance, float maxDistance)
        {
            IsConstrained = true;

            ConstraintPos = new Position(position);
            ConstraintDistanceStart = startDistance;
            ConstraintDistanceMax = maxDistance;
            ConstraintPosOffset = position.Distance(PhysicsObj.Position);
        }

        public bool IsFullyConstrained()
        {
            return ConstraintDistanceMax * 0.9f < ConstraintPosOffset;
        }

        public void SetPhysicsObject(PhysicsObj obj)
        {
            if (PhysicsObj != null)
            {
                IsConstrained = false;
                ConstraintPosOffset = 0.0f;
            }
            PhysicsObj = obj;
        }

        public void Unconstrain()
        {
            IsConstrained = false;
        }

        public void UseTime()
        {
            // empty
        }

        public void adjust_offset(AFrame offset, double quantum)
        {
            if (PhysicsObj == null || !IsConstrained) return;

            if (PhysicsObj.TransientState.HasFlag(TransientStateFlags.Contact))
            {
                if (ConstraintPosOffset < ConstraintDistanceMax)
                {
                    if (ConstraintPosOffset > ConstraintDistanceStart)
                        offset.Origin *= (ConstraintDistanceMax - ConstraintPosOffset) / (ConstraintDistanceMax - ConstraintDistanceStart);
                }
                else
                    offset.Origin = Vector3.Zero;
            }
            ConstraintPosOffset = offset.Origin.Length();
        }
    }
}
