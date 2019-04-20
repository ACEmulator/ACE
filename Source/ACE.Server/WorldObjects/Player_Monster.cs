using System;
using System.Linq;
using ACE.Server.Entity;
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
        /// Flag indicates if player is attackable
        /// </summary>
        public new bool IsAttackable { get => GetProperty(PropertyBool.Attackable) ?? false == true; }

        /// <summary>
        /// Wakes up any monsters within the applicable range
        /// </summary>
        public void CheckMonsters(float rangeSquared = RadiusAwarenessSquared)
        {
            if (!IsAttackable) return;

            var visibleObjs = PhysicsObj.ObjMaint.VisibleObjectTable.Values;

            foreach (var obj in visibleObjs)
            {
                if (PhysicsObj == obj) continue;

                var monster = obj.WeenieObj.WorldObject as Creature;

                if (monster == null || monster is Player) continue;

                if (Location.SquaredDistanceTo(monster.Location) < rangeSquared)
                    AlertMonster(monster);
            }
        }

        /// <summary>
        /// Wakes up a monster if it can be alerted
        /// </summary>
        public bool AlertMonster(Creature monster)
        {
            var attackable = monster.GetProperty(PropertyBool.Attackable) ?? false;
            var tolerance = (Tolerance)(monster.GetProperty(PropertyInt.Tolerance) ?? 0);
            var targetingTactic = monster.GetProperty(PropertyInt.TargetingTactic) ?? 0;

            if ((attackable || targetingTactic != 0) && monster.MonsterState == State.Idle && tolerance == Tolerance.None)
            {
                //Console.WriteLine($"[{Timers.RunningTime}] - {monster.Name} ({monster.Guid}) - waking up");
                monster.AttackTarget = this;
                monster.WakeUp();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Called when this player attacks a monster
        /// </summary>
        public void OnAttackMonster(Creature monster)
        {
            var attackable = monster.GetProperty(PropertyBool.Attackable) ?? false;
            var tolerance = (Tolerance)(monster.GetProperty(PropertyInt.Tolerance) ?? 0);
            var hasTolerance = monster.GetProperty(PropertyInt.Tolerance).HasValue;

            /*Console.WriteLine("OnAttackMonster(" + monster.Name + ")");
            Console.WriteLine("Attackable: " + attackable);
            Console.WriteLine("Tolerance: " + tolerance);
            Console.WriteLine("HasTolerance: " + hasTolerance);*/

            if (monster.MonsterState != State.Awake && !tolerance.HasFlag(Tolerance.NoAttack))
            {
                monster.AttackTarget = this;
                monster.WakeUp();
            }
        }
    }
}
