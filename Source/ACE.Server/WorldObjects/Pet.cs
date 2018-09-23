using System;
using System.Linq;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Pet AI functions
    /// </summary>
    partial class Creature
    {
        public bool IsPet = false;
        public DateTime petCreationTime;

        public void PetFindTarget(float range = RadiusAwareness)
        {
            var distSq = range * range;

            var landblocks = CurrentLandblock?.GetLandblocksInRange(Location, range);

            foreach (var landblock in landblocks)
            {
                var targets = landblock.worldObjects.Values.OfType<Creature>().ToList();
                foreach (var target in targets)
                {
                    if (this == target || target is Player) continue;

                    if (Location.SquaredDistanceTo(target.Location) < distSq)
                    {
                        AttackTarget = target;
                        WakeUp();
                    }
                }
            }
        }

    }
}
