using ACE.Server.Entity;

namespace ACE.Server.Physics
{
    public class PhysicsTimer
    {
        public double CurrentTime;

        public PhysicsTimer()
        {
            CurrentTime = Timer.CurrentTime;
        }
    }
}
