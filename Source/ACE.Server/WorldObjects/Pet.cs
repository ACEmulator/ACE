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

        public void PetFindTarget(float rangeSquared = RadiusAwarenessSquared)
        {
            var visibleObjs = PhysicsObj.ObjMaint.VisibleObjectTable.Values;

            foreach (var obj in visibleObjs)
            {
                if (PhysicsObj == obj) continue;

                var target = obj.WeenieObj.WorldObject as Creature;

                if (target == null || target is Player) continue;

                if (Location.SquaredDistanceTo(target.Location) < rangeSquared)
                {
                    AttackTarget = target;
                    WakeUp();
                }
            }
        }
    }
}
