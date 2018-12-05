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

        public WorldObject PetFindTarget(float rangeSquared = RadiusAwarenessSquared)
        {
            var visibleObjs = PhysicsObj.ObjMaint.VisibleObjectTable.Values;

            //Console.WriteLine($"{Name} searching {visibleObjs.Count} visible objects for target");

            foreach (var obj in visibleObjs)
            {
                if (PhysicsObj == obj) continue;

                var target = obj.WeenieObj.WorldObject as Creature;

                if (target == null || target is Player) continue;

                if (Location.SquaredDistanceTo(target.Location) < rangeSquared)
                {
                    //Console.WriteLine($"{Name} found target {target.Name}");
                    AttackTarget = target;
                    WakeUp();
                    return AttackTarget;
                }
            }
            return null;
        }
    }
}
