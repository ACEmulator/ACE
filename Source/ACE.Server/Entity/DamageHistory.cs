using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    /// <summary>
    /// Tracks the recent damage sources for Players / Creatures
    /// </summary>
    public class DamageHistory
    {
        /// <summary>
        /// The player or creature this Damage History is tracking
        /// </summary>
        public Creature Creature;

        /// <summary>
        /// A list of damage sources, amounts, and timestamps
        /// </summary>
        public List<DamageHistoryEntry> Log;

        /// <summary>
        /// A lookup table of WorldObjects that have damaged this WorldObject,
        /// and the total amount of damage they have inflicted
        /// </summary>
        public Dictionary<WorldObject, float> TotalDamage;

        /// <summary>
        /// Constructs a new DamageHistory for a Player / Creature
        /// </summary>
        public DamageHistory(Creature creature)
        {
            Creature = creature;
            Init();
        }

        /// <summary>
        /// Clears the state of the Log and TotalDamage lists
        /// </summary>
        public void Init()
        {
            Log = new List<DamageHistoryEntry>();
            TotalDamage = new Dictionary<WorldObject, float>();
        }

        /// <summary>
        /// Logs a damaging event for this player or creature
        /// </summary>
        /// <param name="source">The attacker or source of damage</param>
        /// <param name="amount">The amount of damage hit for</param>
        public void Add(WorldObject damager, DamageType damageType, uint amount)
        {
            //Console.WriteLine($"DamageHistory.Add({Creature.Name}, {amount})");

            if (amount == 0) return;

            var entry = new DamageHistoryEntry(Creature, damager, damageType, -(int)amount);
            Log.Add(entry);

            AddInternal(damager, amount);

            TryPrune();
        }

        /// <summary>
        /// Internally increments the total damage table
        /// </summary>
        private void AddInternal(WorldObject damager, uint amount)
        {
            if (TotalDamage.ContainsKey(damager))
                TotalDamage[damager] += amount;
            else
                TotalDamage.Add(damager, amount);
        }

        /// <summary>
        /// Called when this player or creature regains some health
        /// </summary>
        /// <param name="healAmount">The amount of health restored</param>
        public void OnHeal(uint healAmount)
        {
            //Console.WriteLine($"DamageHistory.OnHeal({Creature.Name}, {healAmount})");

            Log.Add(new DamageHistoryEntry(Creature, null, DamageType.Undef, (int)healAmount));

            // calculate previous missingHealth
            OnHealInternal(healAmount, Creature.Health.Current, Creature.Health.MaxValue);
        }

        /// <summary>
        /// Internally scales TotalDamage entries by a healing amount
        /// </summary>
        /// <param name="healAmount">The amount of health restored</param>
        /// <param name="currentHealth">The current health after healing</param>
        /// <param name="maxHealth">The maximum health at the time of healing</param>
        private void OnHealInternal(uint healAmount, uint currentHealth, uint maxHealth)
        {
            // on heal, scale the damage from each source by 1 - healAmount / previous missingHealth
            var missingHealth = maxHealth - (currentHealth - healAmount);
            if (healAmount == 0 || missingHealth == 0) return;
            var scalar = 1.0f - (float)healAmount / missingHealth;

            var damagers = TotalDamage.Keys.ToList();

            foreach (var damager in damagers)
                TotalDamage[damager] *= scalar;
        }

        /// <summary>
        /// Returns the list of players or creatures who inflicted damage
        /// </summary>
        public List<WorldObject> Damagers { get => Log.Select(l => l.DamageSource).Distinct().ToList(); }

        /// <summary>
        /// Returns the WorldObject that last damaged this WorldObject
        /// </summary>
        public WorldObject LastDamager
        {
            get
            {
                var lastDamager = Log.LastOrDefault(l => l.Amount < 0);
                var lastDamagerObj = lastDamager != null ? lastDamager.DamageSource : null;
                //var lastDamagerName = lastDamagerObj != null ? lastDamagerObj.Name : null;
                //Console.WriteLine($"DamageHistory.LastDamager: {lastDamagerName}");
                return lastDamagerObj;
            }
        }

        /// <summary>
        /// Returns the WorldObject that did the most damage to this WorldObject
        /// Used to determine corpse looting rights
        /// </summary>
        public WorldObject TopDamager
        {
            get
            {
                var sorted = TotalDamage.OrderByDescending(wo => wo.Value);
                var topDamager = sorted.FirstOrDefault().Key;
                //var topDamagerName = topDamager != null ? topDamager.Name : null;
                //Console.WriteLine($"DamageHistory.TopDamager: {topDamagerName}");
                return topDamager;
            }
        }

        /// <summary>
        /// Resets the damage log (eg. on player death)
        /// </summary>
        public void Reset()
        {
            Init();
        }

        /// <summary>
        /// The last time the log was pruned
        /// </summary>
        public static DateTime LastPruneTime = DateTime.UtcNow;

        private static readonly TimeSpan minimumPruneInverval = TimeSpan.FromSeconds(30);

        private static readonly TimeSpan maximumTimeToRetain = TimeSpan.FromMinutes(3);

        /// <summary>
        /// The number of minutes to keep a history for
        /// </summary>
        public static int HistoryMinutes = 3;

        /// <summary>
        /// Tries pruning the log according to the minimum pruning time
        /// </summary>
        public void TryPrune()
        {
            if (LastPruneTime + minimumPruneInverval < DateTime.UtcNow)
                Prune();
        }

        /// <summary>
        /// Removes log entries older than HistoryMinutes
        /// </summary>
        public void Prune()
        {
            var entriesToRemove = 0;

            foreach (var entry in Log)
            {
                if (entry.Time + maximumTimeToRetain < DateTime.UtcNow)
                    entriesToRemove++;
                else
                    break;
            }

            if (entriesToRemove > 0)
            {
                Log.RemoveRange(0, entriesToRemove);
                BuildTotalDamage();
                //Console.WriteLine($"DamageHistory.Prune() - {entriesToRemove} entries removed");
            }

            LastPruneTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Rebuilds TotalDamage from the current state of the history log
        /// </summary>
        public void BuildTotalDamage()
        {
            TotalDamage = new Dictionary<WorldObject, float>();

            foreach (var entry in Log)
            {
                if (entry.Amount < 0)
                    AddInternal(entry.DamageSource, (uint)-entry.Amount);
                else
                    OnHealInternal((uint)entry.Amount, entry.CurrentHealth, entry.MaxHealth);
            }
        }
    }
}
