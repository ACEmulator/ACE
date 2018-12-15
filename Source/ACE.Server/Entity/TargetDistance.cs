using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class TargetDistance
    {
        public Creature Target;
        public float Distance;

        public TargetDistance(Creature target, float distance)
        {
            Target = target;
            Distance = distance;
        }
    }
}
