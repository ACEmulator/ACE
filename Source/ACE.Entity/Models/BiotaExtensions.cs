using System;
using System.Collections.Generic;
using System.Threading;

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
                if (!biota.PropertiesInt.TryGetValue((ushort)property, out var value))
                    return null;

                return value;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }


        // =====================================
        // BiotaPropertiesSpellBook
        // =====================================

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
            rwLock.EnterUpgradeableReadLock();
            try
            {
                if (biota.PropertiesSpellBook != null && biota.PropertiesSpellBook.TryGetValue(spell, out var value))
                {
                    spellAdded = false;
                    return value;
                }

                rwLock.EnterWriteLock();
                try
                {
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
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }

        public static bool TryRemoveKnownSpell(this Biota biota, int spell, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                if (biota.PropertiesSpellBook == null || !biota.PropertiesSpellBook.ContainsKey(spell))
                    return false;

                rwLock.EnterWriteLock();
                try
                {
                    return biota.PropertiesSpellBook.Remove(spell);
                }
                finally
                {
                    rwLock.ExitWriteLock();
                }
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }


        // =====================================
        // PropertiesAnimPart
        // =====================================


        // =====================================
        // BiotaPropertiesSkill
        // =====================================
        public static PropertiesSkill GetSkill(this Biota biota, ushort type, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                if (biota.PropertiesSkill == null)
                    return null;

                biota.PropertiesSkill.TryGetValue(type, out var value);
                return value;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static  PropertiesSkill GetOrAddSkill(this Biota biota, ushort type, ReaderWriterLockSlim rwLock, out bool skillAdded)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                if (biota.PropertiesSkill != null && biota.PropertiesSkill.TryGetValue(type, out var value))
                {
                    skillAdded = false;
                    return value;
                }

                rwLock.EnterWriteLock();
                try
                {
                    if (biota.PropertiesSkill == null)
                        biota.PropertiesSkill = new Dictionary<ushort, PropertiesSkill>();

                    var entity = new PropertiesSkill();
                    biota.PropertiesSkill[type] = entity;
                    skillAdded = true;
                    return entity;
                }
                finally
                {
                    rwLock.ExitWriteLock();
                }
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }
    }
}
