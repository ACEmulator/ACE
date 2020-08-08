using ACE.Entity.Enum;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Handles pets waking up monsters
    /// </summary>
    partial class Creature
    {
        /// <summary>
        /// Wakes up any monsters within the applicable range
        /// </summary>
        public void PetCheckMonsters()
        {
            //if (!Attackable) return;

            var creatures = PhysicsObj.ObjMaint.GetVisibleTargetsValuesOfTypeCreature();

            foreach (var monster in creatures)
            {
                if (monster.IsDead) continue;

                //if (Location.SquaredDistanceTo(monster.Location) <= monster.VisualAwarenessRangeSq)
                if (PhysicsObj.get_distance_sq_to_object(monster.PhysicsObj, true) <= monster.VisualAwarenessRangeSq)
                    PetAlertMonster(monster);
            }
        }

        /// <summary>
        /// Wakes up a monster if it can be alerted
        /// </summary>
        private bool PetAlertMonster(Creature monster)
        {
            if (!monster.Attackable || monster.MonsterState != State.Idle || monster.Tolerance != Tolerance.None)
                return false;

            // if the combat pet's owner belongs to a faction,
            // and the monster also belongs to the same faction, don't attack the monster?
            if (Faction1Bits != null && monster.Faction1Bits != null && (Faction1Bits & monster.Faction1Bits) != 0)
                return false;

            monster.AttackTarget = this;
            monster.WakeUp();

            return true;
        }

        /// <summary>
        /// Called when a combat pet attacks a monster
        /// </summary>
        public void PetOnAttackMonster(Creature monster)
        {
            /*Console.WriteLine($"{Name}.PetOnAttackMonster({monster.Name})");
            Console.WriteLine($"Attackable: {monster.Attackable}");
            Console.WriteLine($"Tolerance: {monster.Tolerance}");*/

            if (monster.MonsterState == State.Idle && !monster.Tolerance.HasFlag(Tolerance.NoAttack))
            {
                monster.AttackTarget = this;
                monster.WakeUp();
            }
        }
    }
}
