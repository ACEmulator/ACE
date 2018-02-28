using ACE.Server.Physics.Combat;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    public class PositionManager
    {
        public InterpolationManager InterpolationManager;
        public StickyManager StickyManager;
        public ConstraintManager ConstraintManager;
        public PhysicsObj PhysicsObj;

        public PositionManager()
        {

        }

        public PositionManager(PhysicsObj physicsObj)
        {
            PhysicsObj = physicsObj;
        }

        public static PositionManager Create(PhysicsObj physicsObj)
        {
            return new PositionManager(physicsObj);
        }

        public int GetStickyObjectID()
        {
            return -1;
        }

        public void UseTime()
        {

        }

        public void AdjustOffset(AFrame frame, double quantum)
        {

        }

        public void Unstick()
        {

        }

        public void StopInterpolating()
        {

        }

        public void Unconstrain()
        {

        }

        public void StickTo(int object_id, float radius, float height)
        {

        }

        public void HandleUpdateTarget(TargetInfo targetInfo)
        {

        }

        public void InterpolateTo(Position p, bool keepHeading)
        {

        }

        public bool IsFullyConstrained()
        {
            return false;
        }

        public bool IsInterpolating()
        {
            return false;
        }

        public void ConstrainTo(Position position, float startDistance, float maxDistance)
        {

        }
    }
}
