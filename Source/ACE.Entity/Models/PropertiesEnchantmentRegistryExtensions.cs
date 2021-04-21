using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using ACE.Entity.Enum;

namespace ACE.Entity.Models
{
    public static class PropertiesEnchantmentRegistryExtensions
    {
        public static List<PropertiesEnchantmentRegistry> Clone(this ICollection<PropertiesEnchantmentRegistry> value, ReaderWriterLockSlim rwLock)
        {
            if (value == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                return value.ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }


        public static bool HasEnchantments(this ICollection<PropertiesEnchantmentRegistry> value, ReaderWriterLockSlim rwLock)
        {
            if (value == null)
                return false;

            rwLock.EnterReadLock();
            try
            {
                return value.Any();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static bool HasEnchantment(this ICollection<PropertiesEnchantmentRegistry> value, uint spellId, ReaderWriterLockSlim rwLock)
        {
            if (value == null)
                return false;

            rwLock.EnterReadLock();
            try
            {
                return value.Any(e => e.SpellId == spellId);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static PropertiesEnchantmentRegistry GetEnchantmentBySpell(this ICollection<PropertiesEnchantmentRegistry> value, int spellId, uint? casterGuid, ReaderWriterLockSlim rwLock)
        {
            if (value == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                var results = value.Where(e => e.SpellId == spellId);

                if (casterGuid != null)
                    results = results.Where(e => e.CasterObjectId == casterGuid);

                return results.FirstOrDefault();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static PropertiesEnchantmentRegistry GetEnchantmentBySpellSet(this ICollection<PropertiesEnchantmentRegistry> value, int spellId, EquipmentSet spellSetId, ReaderWriterLockSlim rwLock)
        {
            if (value == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                return value.FirstOrDefault(e => e.SpellId == spellId && e.SpellSetId == spellSetId);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static List<PropertiesEnchantmentRegistry> GetEnchantmentsByCategory(this ICollection<PropertiesEnchantmentRegistry> value, SpellCategory spellCategory, ReaderWriterLockSlim rwLock)
        {
            if (value == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                return value.Where(e => e.SpellCategory == spellCategory).ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static List<PropertiesEnchantmentRegistry> GetEnchantmentsByStatModType(this ICollection<PropertiesEnchantmentRegistry> value, EnchantmentTypeFlags statModType, ReaderWriterLockSlim rwLock)
        {
            if (value == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                return value.Where(e => (e.StatModType & statModType) == statModType).ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        // this ensures level 8 item self spells always take precedence over level 8 item other spells
        private static HashSet<int> Level8AuraSelfSpells = new HashSet<int>
        {
            (int)SpellId.BloodDrinkerSelf8,
            (int)SpellId.DefenderSelf8,
            (int)SpellId.HeartSeekerSelf8,
            (int)SpellId.SpiritDrinkerSelf8,
            (int)SpellId.SwiftKillerSelf8,
            (int)SpellId.HermeticLinkSelf8,
        };

        public static List<PropertiesEnchantmentRegistry> GetEnchantmentsTopLayer(this ICollection<PropertiesEnchantmentRegistry> value, ReaderWriterLockSlim rwLock, HashSet<int> setSpells)
        {
            if (value == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                var results = from e in value
                    group e by e.SpellCategory
                    into categories
                    //select categories.OrderByDescending(c => c.LayerId).First();
                    select categories.OrderByDescending(c => c.PowerLevel)
                        .ThenByDescending(c => Level8AuraSelfSpells.Contains(c.SpellId))
                        .ThenByDescending(c => setSpells.Contains(c.SpellId) ? c.SpellId : c.StartTime).First();

                return results.ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Returns the top layers in each spell category for a StatMod type
        /// </summary>
        public static List<PropertiesEnchantmentRegistry> GetEnchantmentsTopLayerByStatModType(this ICollection<PropertiesEnchantmentRegistry> value, EnchantmentTypeFlags statModType, ReaderWriterLockSlim rwLock, HashSet<int> setSpells)
        {
            if (value == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                var valuesByStatModType = value.Where(e => (e.StatModType & statModType) == statModType);

                var results = from e in valuesByStatModType
                    group e by e.SpellCategory
                    into categories
                    //select categories.OrderByDescending(c => c.LayerId).First();
                    select categories.OrderByDescending(c => c.PowerLevel)
                        .ThenByDescending(c => Level8AuraSelfSpells.Contains(c.SpellId))
                        .ThenByDescending(c => setSpells.Contains(c.SpellId) ? c.SpellId : c.StartTime).First();

                return results.ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        // todo: this is starting to get a bit messy here, EnchantmentTypeFlags handling should be more adaptable
        // perhaps the enchantment registry in acclient should be investigated for reference logic
        private static readonly EnchantmentTypeFlags MultiAttribute = EnchantmentTypeFlags.MultipleStat | EnchantmentTypeFlags.Attribute;

        private static readonly EnchantmentTypeFlags MultiSkill = EnchantmentTypeFlags.MultipleStat | EnchantmentTypeFlags.Skill;

        /// <summary>
        /// Returns the top layers in each spell category for a StatMod type + key
        /// </summary>
        public static List<PropertiesEnchantmentRegistry> GetEnchantmentsTopLayerByStatModType(this ICollection<PropertiesEnchantmentRegistry> value, EnchantmentTypeFlags statModType, uint statModKey, ReaderWriterLockSlim rwLock, HashSet<int> setSpells, bool handleMultiple = false)
        {
            if (value == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                var multipleStat = EnchantmentTypeFlags.Undef;

                if (handleMultiple)
                {
                    if (statModType == EnchantmentTypeFlags.Attribute)
                        multipleStat = MultiAttribute;
                    else if (statModType == EnchantmentTypeFlags.Skill)
                        multipleStat = MultiSkill;
                }

                var valuesByStatModTypeAndKey = value.Where(e => (e.StatModType & statModType) == statModType && e.StatModKey == statModKey || (handleMultiple && (e.StatModType & multipleStat) == multipleStat && e.StatModKey == 0));

                // 3rd spell id sort added for Gauntlet Damage Boost I / Gauntlet Damage Boost II, which is contained in multiple sets, and can overlap
                // without this sorting criteria, it's already matched up to the client, but produces logically incorrect results for server spell stacking
                // confirmed this bug still exists in acclient Enchantment.Duel(), unknown if it existed in retail server

                var results = from e in valuesByStatModTypeAndKey
                    group e by e.SpellCategory
                    into categories
                    //select categories.OrderByDescending(c => c.LayerId).First();
                    select categories.OrderByDescending(c => c.PowerLevel)
                        .ThenByDescending(c => Level8AuraSelfSpells.Contains(c.SpellId))
                        .ThenByDescending(c => setSpells.Contains(c.SpellId) ? c.SpellId : c.StartTime).First();

                return results.ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static List<PropertiesEnchantmentRegistry> HeartBeatEnchantmentsAndReturnExpired(this ICollection<PropertiesEnchantmentRegistry> value, double heartbeatInterval, ReaderWriterLockSlim rwLock)
        {
            if (value == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                var expired = new List<PropertiesEnchantmentRegistry>();

                foreach (var enchantment in value)
                {
                    enchantment.StartTime -= heartbeatInterval;

                    // StartTime ticks backwards to -Duration
                    if (enchantment.Duration >= 0 && enchantment.StartTime <= -enchantment.Duration)
                        expired.Add(enchantment);
                }

                return expired;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static void AddEnchantment(this ICollection<PropertiesEnchantmentRegistry> value, PropertiesEnchantmentRegistry entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            try
            {
                value.Add(entity);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static bool TryRemoveEnchantment(this ICollection<PropertiesEnchantmentRegistry> value, int spellId, uint casterObjectId, ReaderWriterLockSlim rwLock)
        {
            if (value == null)
                return false;

            rwLock.EnterWriteLock();
            try
            {
                var entity = value.FirstOrDefault(x => x.SpellId == spellId && x.CasterObjectId == casterObjectId);

                if (entity != null)
                {
                    value.Remove(entity);

                    return true;
                }

                return false;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void RemoveAllEnchantments(this ICollection<PropertiesEnchantmentRegistry> value, IEnumerable<int> spellsToExclude, ReaderWriterLockSlim rwLock)
        {
            if (value == null)
                return;

            rwLock.EnterWriteLock();
            try
            {
                var enchantments = value.Where(e => !spellsToExclude.Contains(e.SpellId)).ToList();

                foreach (var enchantment in enchantments)
                    value.Remove(enchantment);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }
    }
}
