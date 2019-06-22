using System;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class DamageHistoryEntry
    {
        public ObjectGuid DamageSource;

        public DamageType DamageType;
        public int Amount;

        public uint CurrentHealth;
        public uint MaxHealth;

        public DateTime Time;

        /// <summary>
        /// Constructs a new entry for the DamageHistory
        /// </summary>
        /// <param name="damageSource">The attacker or source of the damage</param>
        /// <param name="amount">A negative amount for damage taken, positive for healing</param>
        public DamageHistoryEntry(Creature creature, ObjectGuid damageSource, DamageType damageType, int amount)
        {
            DamageSource = damageSource;

            DamageType = damageType;
            Amount = amount;

            CurrentHealth = creature.Health.Current;
            MaxHealth = creature.Health.MaxValue;

            Time = DateTime.UtcNow;
        }
    }
}
