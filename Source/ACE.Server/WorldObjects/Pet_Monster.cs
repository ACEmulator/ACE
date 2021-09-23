using ACE.Entity.Enum;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Handles pets waking up monsters
    /// </summary>
    partial class CombatPet
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
            if (!monster.Attackable || monster.MonsterState != State.Idle || (monster.Tolerance & PlayerCombatPet_MoveExclude) != 0)
                return false;

            // if the combat pet's owner belongs to a faction,
            // and the monster also belongs to the same faction, don't aggro the monster?
            if (SameFaction(monster))
            {
                // unless the pet owner or the pet is being retaliated against?
                if (!monster.HasRetaliateTarget(P_PetOwner) && !monster.HasRetaliateTarget(this))
                    return false;

                monster.AddRetaliateTarget(this);
            }

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

            // faction mobs will retaliate against combat pets belonging to the same faction
            if (monster.SameFaction(this))
                monster.AddRetaliateTarget(this);

            if (monster.MonsterState == State.Idle && (monster.Tolerance & PlayerCombatPet_RetaliateExclude) == 0)
            {
                monster.AttackTarget = this;
                monster.WakeUp();
            }
        }
    }
}
