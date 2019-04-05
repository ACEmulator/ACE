using System;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Monster AI functions
    /// </summary>
    partial class Creature
    {
        public bool IsMonster;

        /// <summary>
        /// The exclusive state of the monster
        /// </summary>
        public State MonsterState = State.Idle;

        /// <summary>
        /// The exclusive states the monster can be in
        /// </summary>
        public enum State
        {
            Idle,
            Awake,
            Return
        };

        /// <summary>
        /// Returns TRUE if this is an attackable monster
        /// </summary>
        public bool IsAttackable()
        {
            var attackable = GetProperty(PropertyBool.Attackable) ?? false;
            var tolerance = (Tolerance)(GetProperty(PropertyInt.Tolerance) ?? 0);

            return attackable && !tolerance.HasFlag(Tolerance.NoAttack);
        }
    }
}
