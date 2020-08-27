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
        public readonly Dictionary<ObjectGuid, DamageHistoryInfo> TotalDamage = new Dictionary<ObjectGuid, DamageHistoryInfo>();

        /// <summary>
        /// Returns the list of players or creatures who inflicted damage
        /// </summary>
        public List<DamageHistoryInfo> Damagers => TotalDamage.Values.ToList();

        /// <summary>
        /// Returns the DamageHistoryInfo for the last damager
        /// </summary>
        public DamageHistoryInfo LastDamager
        {
            get
            {
                var lastDamager = Log.LastOrDefault(l => l.Amount < 0);
                if (lastDamager == null)
                    return null;

                TotalDamage.TryGetValue(lastDamager.Attacker, out var info);

                return info;
            }
        }

        public float TotalHealth => TotalDamage.Values.Sum(i => i.TotalDamage);

        /// <summary>
        /// Returns the DamageHistoryInfo for the top damager
        /// for determining 'Killed by' corpse looting rights
        /// </summary>
        public DamageHistoryInfo TopDamager => GetTopDamager();

        public DamageHistoryInfo GetTopDamager(bool includeSelf = true)
        {
            var sorted = TotalDamage.Values.Where(wo => includeSelf || wo.Guid != Creature.Guid).OrderByDescending(wo => wo.TotalDamage);

            return sorted.FirstOrDefault();
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
        public void Add(WorldObject attacker, DamageType damageType, uint amount)
        {
            //Console.WriteLine($"{Creature.Name}.DamageHistory.Add({attacker.Name}, {damageType}, {amount})");

            if (amount == 0) return;

            var entry = new DamageHistoryEntry(Creature, attacker.Guid, damageType, -(int)amount);
            Log.Add(entry);

            AddInternal(attacker, amount);

            Creature.OnHealthUpdate();
        }

        /// <summary>
        /// Internally increments the total damage table
        /// </summary>
        private void AddInternal(WorldObject attacker, uint amount)
        {
            if (TotalDamage.TryGetValue(attacker.Guid, out var value))
                value.TotalDamage += amount;
            else
                TotalDamage.Add(attacker.Guid, new DamageHistoryInfo(attacker, amount));
        }

        /// <summary>
        /// Internally increments the total damage table
        /// </summary>
        private void AddInternal(ObjectGuid attacker, uint amount)
        {
            // todo: investigate, this shouldn't happen?
            // key 0 from BuildTotalDamage()
            if (TotalDamage.ContainsKey(attacker))      
                TotalDamage[attacker].TotalDamage += amount;
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

            Creature.OnHealthUpdate();
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

            var attackers = TotalDamage.Keys.ToList();

            foreach (var attacker in attackers)
                TotalDamage[attacker].TotalDamage *= scalar;
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
        public DateTime LastPruneTime = DateTime.UtcNow;

        private static readonly TimeSpan minimumPruneInterval = TimeSpan.FromSeconds(30);

        private static readonly TimeSpan maximumTimeToRetain = TimeSpan.FromMinutes(3);

        /// <summary>
        /// Tries pruning the log according to the minimum pruning time
        /// </summary>
        public void TryPrune()
        {
            if (LastPruneTime + minimumPruneInterval < DateTime.UtcNow)
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
                guids.Add(entry.Attacker);

            var keys = TotalDamage.Keys.ToList();
            foreach (var key in keys)
            {
                if (guids.Contains(key))
                    TotalDamage[key].TotalDamage = 0;
                else
                    TotalDamage.Remove(key);
            }

            // TotalDamage is now reset

            foreach (var entry in Log)
            {
                if (entry.Amount < 0)
                    AddInternal(entry.Attacker, (uint)-entry.Amount);
                else
                    OnHealInternal((uint)entry.Amount, entry.CurrentHealth, entry.MaxHealth);
            }
        }

        /// <summary>
        /// Returns TRUE if damage history contains wo as recent attacker
        /// </summary>
        /// <param name="nonZero">If TRUE, attacker must have TotalDamage > 0</param>
        public bool HasDamager(WorldObject wo, bool nonZero = false)
        {
            if (!TotalDamage.TryGetValue(wo.Guid, out var totalDamage))
                return false;

            if (nonZero)
                return totalDamage.TotalDamage > 0;
            else
                return true;
        }

        public override string ToString()
        {
            var table = "";

            foreach (var attacker in TotalDamage.Values)
                table += $"{attacker.Name} ({attacker.Guid}) - {attacker.TotalDamage}\n";

            return table;
        }
    }
}
