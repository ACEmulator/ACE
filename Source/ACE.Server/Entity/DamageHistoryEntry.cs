using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class DamageHistoryEntry
    {
        public Creature Creature;
        public WorldObject DamageSource;
        public int Amount;
        public uint CurrentHealth;
        public uint MaxHealth;
        public double Time;

        /// <summary>
        /// Constructs a new entry for the DamageHistory
        /// </summary>
        /// <param name="creature">The player or creature taking damage</param>
        /// <param name="damageSource">The attacker or source of the damage</param>
        /// <param name="amount">A negative amount for damage taken, positive for healing</param>
        public DamageHistoryEntry(Creature creature, WorldObject damageSource, int amount)
        {
            Creature = creature;
            DamageSource = damageSource;
            Amount = amount;
            CurrentHealth = creature.Health.Current;
            MaxHealth = creature.Health.MaxValue;
            Time = Timer.CurrentTime;
        }
    }
}
