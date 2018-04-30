using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ACE.Server.Entity;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Handles player->monster visibility checks
    /// </summary>
    partial class Player
    {
        public void CheckMonsters()
        {
            // get all monsters within range
            /*var nearbyMonsters = */GetMonstersInRange();
        }

        /// <summary>
        /// Returns all monsters within 2D distance of player
        /// </summary>
        public void GetMonstersInRange(float range = Monster.RadiusAwareness)
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
                    {
                        //Console.WriteLine("Found monster in range! " + monster.Name);

                        var attackable = monster.GetProperty(PropertyBool.Attackable) ?? false;
                        var tolerance = (Tolerance)(monster.GetProperty(PropertyInt.Tolerance) ?? 0);

                        if (attackable && monster.MonsterState == State.Idle/* && tolerance == Tolerance.None*/)
                        {
                            monster.AttackTarget = this;
                            monster.WakeUp();
                        }
                    }
                }
            }
        }
    }
}
