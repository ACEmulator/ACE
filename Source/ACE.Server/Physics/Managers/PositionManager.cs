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

        public PositionManager() { }

        public PositionManager(PhysicsObj obj)
        {
            SetPhysicsObject(obj);
        }

        public void AdjustOffset(AFrame frame, double quantum)
        {
            if (InterpolationManager != null)
                InterpolationManager.adjust_offset(frame, quantum);
            if (StickyManager != null)
                StickyManager.adjust_offset(frame, quantum);
            if (ConstraintManager != null)
                ConstraintManager.adjust_offset(frame, quantum);
        }

        public void ConstrainTo(Position position, float startDistance, float maxDistance)
        {
            if (ConstraintManager == null)
                ConstraintManager = ConstraintManager.Create(PhysicsObj);

            ConstraintManager.ConstrainTo(position, startDistance, maxDistance);
        }

        public static PositionManager Create(PhysicsObj physicsObj)
        {
            return new PositionManager(physicsObj);
        }

        public uint GetStickyObjectID()
        {
            if (StickyManager == null) return 0;
            return StickyManager.TargetID;
        }

        public void HandleUpdateTarget(TargetInfo targetInfo)
        {
            if (StickyManager != null)
                StickyManager.HandleUpdateTarget(targetInfo);
        }

        public void InterpolateTo(Position position, bool keepHeading)
        {
            if (InterpolationManager == null)
                InterpolationManager = InterpolationManager.Create(PhysicsObj);

            InterpolationManager.InterpolateTo(position, keepHeading);
        }

        public bool IsFullyConstrained()
        {
            if (ConstraintManager == null)
                return false;
            else
                return ConstraintManager.IsFullyConstrained();
        }

        public bool IsInterpolating()
        {
            return InterpolationManager != null && InterpolationManager.IsInterpolating();
        }

        public void MakeStickyManager()
        {
            if (StickyManager == null)
                StickyManager = StickyManager.Create(PhysicsObj);
        }

        public void SetPhysicsObject(PhysicsObj obj)
        {
            PhysicsObj = obj;
            if (InterpolationManager != null)
                InterpolationManager.SetPhysicsObject(obj);
            if (StickyManager != null)
                StickyManager.SetPhysicsObject(obj);
            if (ConstraintManager != null)
                ConstraintManager.SetPhysicsObject(obj);
        }

        public void StickTo(uint objectID, float radius, float height)
        {
            if (StickyManager == null)
                MakeStickyManager();

            StickyManager.StickTo(objectID, radius, height);
        }

        public void StopInterpolating()
        {
            if (InterpolationManager != null)
                InterpolationManager.StopInterpolating();
        }

        public void Unconstrain()
        {
            if (ConstraintManager != null)
                ConstraintManager.Unconstrain();
        }

        public void Unstick()
        {
            if (StickyManager != null)
                StickyManager.HandleExitWorld();
        }

        public void UseTime()
        {
            if (InterpolationManager != null)
                InterpolationManager.UseTime();

            if (StickyManager != null)
                StickyManager.UseTime();

            if (ConstraintManager != null)
                ConstraintManager.UseTime();
        }
    }
}
