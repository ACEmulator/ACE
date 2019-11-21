using System;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class DamageHistoryEntry
    {
        public ObjectGuid Attacker;

        public DamageType DamageType;
        public int Amount;

        public uint CurrentHealth;
        public uint MaxHealth;

        public DateTime Time;

        /// <summary>
        /// Constructs a new entry for the DamageHistory
        /// </summary>
        /// <param name="attacker">The creature source of the damage. In the case of a projectile, this would be the Creature that launched the projectile.</param>
        /// <param name="amount">A negative amount for damage taken, positive for healing</param>
        public DamageHistoryEntry(Creature creature, ObjectGuid attacker, DamageType damageType, int amount)
        {
            Attacker = attacker;

            DamageType = damageType;
            Amount = amount;

            CurrentHealth = creature.Health.Current;
            MaxHealth = creature.Health.MaxValue;

            Time = DateTime.UtcNow;
        }
    }
}
