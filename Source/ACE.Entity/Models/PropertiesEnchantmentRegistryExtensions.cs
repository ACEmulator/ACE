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
