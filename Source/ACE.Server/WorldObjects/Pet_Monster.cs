using System;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

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
        public void PetCheckMonsters(float rangeSquared = RadiusAwarenessSquared)
        {
            //if (GetProperty(PropertyBool.Attackable) ?? true == false) return;

            var visibleObjs = PhysicsObj.ObjMaint.VisibleObjectTable.Values;

            foreach (var obj in visibleObjs)
            {
                if (PhysicsObj == obj) continue;

                var monster = obj.WeenieObj.WorldObject as Creature;

                if (monster == null || monster is Player) continue;

                if (Location.SquaredDistanceTo(monster.Location) < rangeSquared)
                    PetAlertMonster(monster);
            }
        }

        /// <summary>
        /// Wakes up a monster if it can be alerted
        /// </summary>
        private bool PetAlertMonster(Creature monster)
        {
            var attackable = monster.GetProperty(PropertyBool.Attackable) ?? false;
            var tolerance = (Tolerance)(monster.GetProperty(PropertyInt.Tolerance) ?? 0);

            if (attackable && monster.MonsterState == State.Idle && tolerance == Tolerance.None)
            {
                monster.AttackTarget = this;
                monster.WakeUp();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Called when a combat pet attacks a monster
        /// </summary>
        public void PetOnAttackMonster(Creature monster)
        {
            var attackable = monster.GetProperty(PropertyBool.Attackable) ?? false;
            var tolerance = (Tolerance)(monster.GetProperty(PropertyInt.Tolerance) ?? 0);
            var hasTolerance = monster.GetProperty(PropertyInt.Tolerance).HasValue;

            /*Console.WriteLine("OnAttackMonster(" + monster.Name + ")");
            Console.WriteLine("Attackable: " + attackable);
            Console.WriteLine("Tolerance: " + tolerance);
            Console.WriteLine("HasTolerance: " + hasTolerance);*/

            if (monster.MonsterState == State.Idle && !tolerance.HasFlag(Tolerance.NoAttack))
            {
                monster.AttackTarget = this;
                monster.WakeUp();
            }
        }
    }
}
