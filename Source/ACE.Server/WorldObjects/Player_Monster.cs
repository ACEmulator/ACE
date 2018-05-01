using System;
using System.Linq;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Handles player->monster visibility checks
    /// </summary>
    partial class Player
    {
        /// <summary>
        /// Wakes up any monsters within the applicable range
        /// </summary>
        public void CheckMonsters()
        {
            if (CurrentLandblock.Id.MapScope == MapScope.Outdoors)
                GetMonstersInRange();
            else
                GetMonstersInPVS();
        }

        /// <summary>
        /// Sends alerts to monsters within 2D distance for outdoor areas
        /// </summary>
        private void GetMonstersInRange(float range = Monster.RadiusAwareness)
        {
            var distSq = range * range;

            var landblocks = CurrentLandblock.GetLandblocksInRange(Location, range);

            foreach (var landblock in landblocks)
            {
                var monsters = landblock.worldObjects.Values.OfType<Creature>().ToList();
                foreach (var monster in monsters)
                {
                    if (this == monster) continue;

                    if (Location.SquaredDistanceTo(monster.Location) < distSq)
                        AlertMonster(monster);
                }
            }
        }

        /// <summary>
        /// Sends alerts to monsters within PVS range for indoor areas
        /// </summary>
        private void GetMonstersInPVS(float range = Monster.RadiusAwareness)
        {
            var distSq = range * range;

            var visibleObjs = Physics.PhysicsObj.ObjMaint.VisibleObjectTable.Values;

            foreach (var obj in visibleObjs)
            {
                if (PhysicsObj == obj) continue;

                var monster = obj.WeenieObj.WorldObject as Creature;

                if (monster == null) continue;

                if (Location.SquaredDistanceTo(monster.Location) < distSq)
                    AlertMonster(monster);
            }
        }

        /// <summary>
        /// Wakes up a monster if it can be alerted
        /// </summary>
        private bool AlertMonster(Creature monster)
        {
            var attackable = monster.GetProperty(PropertyBool.Attackable) ?? false;
            var tolerance = (Tolerance)(monster.GetProperty(PropertyInt.Tolerance) ?? 0);

            if (attackable && monster.MonsterState == State.Idle/* && tolerance == Tolerance.None*/)
            {
                //Console.WriteLine("Waking up " + monster.Name);

                monster.AttackTarget = this;
                monster.WakeUp();
                return true;
            }
            return false;
        }
    }
}
