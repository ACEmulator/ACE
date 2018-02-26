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
    }
}
