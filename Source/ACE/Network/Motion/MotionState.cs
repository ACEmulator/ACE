using ACE.Entity;

namespace ACE.Network.Motion
{
    public abstract class MotionState
    {
        public bool IsAutonomous { get; set; }

        public virtual byte[] GetPayload(WorldObject animationTarget, float distanceFromObject = 0.6f, float heading = 0.00f)
        {
            return null;
        }
    }
}
