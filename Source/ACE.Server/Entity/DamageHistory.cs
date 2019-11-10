using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Entity;
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
        public readonly Creature Creature;

        /// <summary>
        /// A list of damage sources, amounts, and timestamps
        /// </summary>
        public readonly List<DamageHistoryEntry> Log = new List<DamageHistoryEntry>();

        /// <summary>
        /// A lookup table of WorldObjects that have damaged this WorldObject,
        /// and the total amount of damage they have inflicted
        /// </summary>
        public readonly Dictionary<ObjectGuid, WorldObjectInfo<float>> TotalDamage = new Dictionary<ObjectGuid, WorldObjectInfo<float>>();

        /// <summary>
        /// Returns the list of players or creatures who inflicted damage
        /// </summary>
        public List<WorldObjectInfo<float>> Damagers => TotalDamage.Values.ToList();

        /// <summary>
        /// Returns the WorldObject that last damaged this WorldObject
        /// </summary>
        public WorldObject LastDamager
        {
            get
            {
                var lastDamager = Log.LastOrDefault(l => l.Amount < 0);
                WorldObject lastDamagerObj = null;
                if (lastDamager != null)
                {
                    if (TotalDamage.TryGetValue(lastDamager.DamageSource, out var value))
                        lastDamagerObj = value.TryGetWorldObject();
                }
                //var lastDamagerName = lastDamagerObj != null ? lastDamagerObj.Name : null;
                //Console.WriteLine($"DamageHistory.LastDamager: {lastDamagerName}");
                return lastDamagerObj;
            }
        }

        /// <summary>
        /// Returns the WorldObject that did the most damage to this WorldObject
        /// Used to determine corpse looting rights
        /// </summary>
        public WorldObject TopDamager => GetTopDamager();

        public WorldObject GetTopDamager(bool includeSelf = true)
        {
            var sorted = TotalDamage.Values.Where(wo => includeSelf || wo.Guid != Creature.Guid).OrderByDescending(wo => wo.Value);

            var topDamager = sorted.FirstOrDefault();

            return topDamager?.TryGetWorldObject();
        }

        /// <summary>
        /// Constructs a new DamageHistory for a Player / Creature
        /// </summary>
        public DamageHistory(Creature creature)
        {
            Creature = creature;
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

            var entry = new DamageHistoryEntry(Creature, damager.Guid, damageType, -(int)amount);
            Log.Add(entry);

            AddInternal(damager, amount);
        }

        /// <summary>
        /// Internally increments the total damage table
        /// </summary>
        private void AddInternal(WorldObject damager, uint amount)
        {
            if (TotalDamage.TryGetValue(damager.Guid, out var value))
                value.Value += amount;
            else
            {
                var woi = new WorldObjectInfo<float>(damager, amount);

                TotalDamage.Add(damager.Guid, woi);
            }
        }

        /// <summary>
        /// Internally increments the total damage table
        /// </summary>
        private void AddInternal(ObjectGuid damager, uint amount)
        {
            TotalDamage[damager].Value += amount;
        }

        /// <summary>
        /// Called when this player or creature regains some health
        /// </summary>
        /// <param name="healAmount">The amount of health restored</param>
        public void OnHeal(uint healAmount)
        {
            //Console.WriteLine($"DamageHistory.OnHeal({Creature.Name}, {healAmount})");

            Log.Add(new DamageHistoryEntry(Creature, ObjectGuid.Invalid, DamageType.Undef, (int)healAmount));

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
                TotalDamage[damager].Value *= scalar;
        }

        /// <summary>
        /// Resets the damage log (eg. on player death)
        /// </summary>
        public void Reset()
        {
            Log.Clear();
            TotalDamage.Clear();
        }


        /// <summary>
        /// The last time the log was pruned
        /// </summary>
        public static DateTime LastPruneTime = DateTime.UtcNow;

        private static readonly TimeSpan minimumPruneInverval = TimeSpan.FromSeconds(30);

        private static readonly TimeSpan maximumTimeToRetain = TimeSpan.FromMinutes(3);

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
            // This is a little bit hacky.
            // We don't want to clear our TotalDamage entries because we might lose references to WorldObjects
            // Instead, we remove entries that are no longer needed, and set all the values to 0.

            var guids = new HashSet<ObjectGuid>();
            foreach (var entry in Log)
                guids.Add(entry.DamageSource);

            var keys = TotalDamage.Keys.ToList();
            foreach (var key in keys)
            {
                if (guids.Contains(key))
                    TotalDamage[key].Value = 0;
                else
                    TotalDamage.Remove(key);
            }

            // TotalDamage is now reset

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
