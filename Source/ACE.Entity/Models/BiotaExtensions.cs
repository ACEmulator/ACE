using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Entity.Models
{
    public static class BiotaExtensions
    {
        // =====================================
        // Get
        // Bool, DID, Float, IID, Int, Int64, Position, String
        // =====================================

        public static int? GetProperty(this Biota biota, PropertyInt property, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                if (!biota.PropertiesInt.TryGetValue(property, out var value))
                    return null;

                return value;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }


        // =====================================
        // Set
        // Bool, DID, Float, IID, Int, Int64, Position, String
        // =====================================

        public static void SetProperty(this Biota biota, PropertyBool property, bool value, ReaderWriterLockSlim rwLock, out bool biotaChanged)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (!biota.PropertiesBool.TryGetValue(property, out var existingValue) || value != existingValue)
                {
                    biota.PropertiesBool[property] = value;
                    biotaChanged = true;
                }
                else
                {
                    biotaChanged = false;
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }


        // =====================================
        // BiotaPropertiesSpellBook
        // =====================================

        public static Dictionary<int, float> CloneSpells(this Biota biota, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                var results = new Dictionary<int, float>();

                foreach (var kvp in biota.PropertiesSpellBook)
                    results[kvp.Key] = kvp.Value;

                return results;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static bool HasKnownSpell(this Biota biota, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return biota.PropertiesSpellBook != null && biota.PropertiesSpellBook.Count > 0;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static List<int> GetKnownSpellsIds(this Biota biota, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                if (biota.PropertiesSpellBook == null)
                    return new List<int>();

                return new List<int>(biota.PropertiesSpellBook.Keys);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static List<int> GetKnownSpellsIdsWhere(this Biota biota, Func<int, bool> predicate, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                if (biota.PropertiesSpellBook == null)
                    return new List<int>();

                return biota.PropertiesSpellBook.Keys.Where(predicate).ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static List<float> GetKnownSpellsProbabilities(this Biota biota, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                if (biota.PropertiesSpellBook == null)
                    return new List<float>();

                return new List<float>(biota.PropertiesSpellBook.Values);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static bool SpellIsKnown(this Biota biota, int spell, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return biota.PropertiesSpellBook != null && biota.PropertiesSpellBook.ContainsKey(spell);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static float GetOrAddKnownSpell(this Biota biota, int spell, ReaderWriterLockSlim rwLock, out bool spellAdded, float probability = 2.0f)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (biota.PropertiesSpellBook != null && biota.PropertiesSpellBook.TryGetValue(spell, out var value))
                {
                    spellAdded = false;
                    return value;
                }

                if (biota.PropertiesSpellBook == null)
                    biota.PropertiesSpellBook = new Dictionary<int, float>();

                biota.PropertiesSpellBook[spell] = probability;
                spellAdded = true;

                return probability;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static Dictionary<int, float> GetMatchingSpells(this Biota biota, HashSet<int> match, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                var results = new Dictionary<int, float>();

                foreach (var value in biota.PropertiesSpellBook)
                {
                    if (match.Contains(value.Key))
                        results[value.Key] = value.Value;
                }

                return results;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static bool TryRemoveKnownSpell(this Biota biota, int spell, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (biota.PropertiesSpellBook == null || !biota.PropertiesSpellBook.ContainsKey(spell))
                    return false;

                return biota.PropertiesSpellBook.Remove(spell);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void ClearSpells(this Biota biota, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            try
            {
                biota.PropertiesSpellBook?.Clear();
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }


        // =====================================
        // BiotaPropertiesSkill
        // =====================================
        public static PropertiesSkill GetSkill(this Biota biota, Skill skill, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                if (biota.PropertiesSkill == null)
                    return null;

                biota.PropertiesSkill.TryGetValue(skill, out var value);
                return value;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static  PropertiesSkill GetOrAddSkill(this Biota biota, Skill skill, ReaderWriterLockSlim rwLock, out bool skillAdded)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (biota.PropertiesSkill != null && biota.PropertiesSkill.TryGetValue(skill, out var value))
                {
                    skillAdded = false;
                    return value;
                }

                if (biota.PropertiesSkill == null)
                    biota.PropertiesSkill = new Dictionary<Skill, PropertiesSkill>();

                var entity = new PropertiesSkill();
                biota.PropertiesSkill[skill] = entity;
                skillAdded = true;

                return entity;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }
    }
}
