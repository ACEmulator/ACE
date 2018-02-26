namespace ACE.Server.Physics.Animation
{
    public class PositionManager
    {
        public InterpolationManager InterpolationManager;
        public StickyManager StickyManager;
        public ConstraintManager ConstraintManager;
        public PhysicsObj PhysicsObj;

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
    }
}
