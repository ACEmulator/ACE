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
        // Bool, DID, Float, IID, Int, Int64, String, Position
        // =====================================

        public static bool? GetProperty(this Biota biota, PropertyBool property, ReaderWriterLockSlim rwLock)
        {
            if (biota.PropertiesBool == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                if (biota.PropertiesBool.TryGetValue(property, out var value))
                    return value;

                return null;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static uint? GetProperty(this Biota biota, PropertyDataId property, ReaderWriterLockSlim rwLock)
        {
            if (biota.PropertiesDID == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                if (biota.PropertiesDID.TryGetValue(property, out var value))
                    return value;

                return null;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static double? GetProperty(this Biota biota, PropertyFloat property, ReaderWriterLockSlim rwLock)
        {
            if (biota.PropertiesFloat == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                if (biota.PropertiesFloat.TryGetValue(property, out var value))
                    return value;

                return null;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static uint? GetProperty(this Biota biota, PropertyInstanceId property, ReaderWriterLockSlim rwLock)
        {
            if (biota.PropertiesIID == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                if (biota.PropertiesIID.TryGetValue(property, out var value))
                    return value;

                return null;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static int? GetProperty(this Biota biota, PropertyInt property, ReaderWriterLockSlim rwLock)
        {
            if (biota.PropertiesInt == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                if (biota.PropertiesInt.TryGetValue(property, out var value))
                    return value;

                return null;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static long? GetProperty(this Biota biota, PropertyInt64 property, ReaderWriterLockSlim rwLock)
        {
            if (biota.PropertiesInt64 == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                if (biota.PropertiesInt64.TryGetValue(property, out var value))
                    return value;

                return null;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static string GetProperty(this Biota biota, PropertyString property, ReaderWriterLockSlim rwLock)
        {
            if (biota.PropertiesString == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                if (biota.PropertiesString.TryGetValue(property, out var value))
                    return value;

                return null;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static PropertiesPosition GetProperty(this Biota biota, PositionType property, ReaderWriterLockSlim rwLock)
        {
            if (biota.PropertiesPosition == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                if (biota.PropertiesPosition.TryGetValue(property, out var value))
                    return value;

                return null;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static Position GetPosition(this Biota biota, PositionType property, ReaderWriterLockSlim rwLock)
        {
            if (biota.PropertiesPosition == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                if (biota.PropertiesPosition.TryGetValue(property, out var value))
                    return new Position(value.ObjCellId, value.PositionX, value.PositionY, value.PositionZ, value.RotationX, value.RotationY, value.RotationZ, value.RotationW, property == PositionType.RelativeDestination);

                return null;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }


        // =====================================
        // Set
        // Bool, DID, Float, IID, Int, Int64, String, Position
        // =====================================

        public static void SetProperty(this Biota biota, PropertyBool property, bool value, ReaderWriterLockSlim rwLock, out bool changed)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (biota.PropertiesBool == null)
                    biota.PropertiesBool = new Dictionary<PropertyBool, bool>();

                changed = (!biota.PropertiesBool.TryGetValue(property, out var existing) || value != existing);

                if (changed)
                    biota.PropertiesBool[property] = value;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyDataId property, uint value, ReaderWriterLockSlim rwLock, out bool changed)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (biota.PropertiesDID == null)
                    biota.PropertiesDID = new Dictionary<PropertyDataId, uint>();

                changed = (!biota.PropertiesDID.TryGetValue(property, out var existing) || value != existing);

                if (changed)
                    biota.PropertiesDID[property] = value;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyFloat property, double value, ReaderWriterLockSlim rwLock, out bool changed)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (biota.PropertiesFloat == null)
                    biota.PropertiesFloat = new Dictionary<PropertyFloat, double>();

                changed = (!biota.PropertiesFloat.TryGetValue(property, out var existing) || value != existing);

                if (changed)
                    biota.PropertiesFloat[property] = value;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyInstanceId property, uint value, ReaderWriterLockSlim rwLock, out bool changed)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (biota.PropertiesIID == null)
                    biota.PropertiesIID = new Dictionary<PropertyInstanceId, uint>();

                changed = (!biota.PropertiesIID.TryGetValue(property, out var existing) || value != existing);

                if (changed)
                    biota.PropertiesIID[property] = value;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyInt property, int value, ReaderWriterLockSlim rwLock, out bool changed)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (biota.PropertiesInt == null)
                    biota.PropertiesInt = new Dictionary<PropertyInt, int>();

                changed = (!biota.PropertiesInt.TryGetValue(property, out var existing) || value != existing);

                if (changed)
                    biota.PropertiesInt[property] = value;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyInt64 property, long value, ReaderWriterLockSlim rwLock, out bool changed)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (biota.PropertiesInt64 == null)
                    biota.PropertiesInt64 = new Dictionary<PropertyInt64, long>();

                changed = (!biota.PropertiesInt64.TryGetValue(property, out var existing) || value != existing);

                if (changed)
                    biota.PropertiesInt64[property] = value;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyString property, string value, ReaderWriterLockSlim rwLock, out bool changed)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (biota.PropertiesString == null)
                    biota.PropertiesString = new Dictionary<PropertyString, string>();

                changed = (!biota.PropertiesString.TryGetValue(property, out var existing) || value != existing);

                if (changed)
                    biota.PropertiesString[property] = value;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void SetProperty(this Biota biota, PositionType property, PropertiesPosition value, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (biota.PropertiesPosition == null)
                    biota.PropertiesPosition = new Dictionary<PositionType, PropertiesPosition>();

                biota.PropertiesPosition[property] = value;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void SetPosition(this Biota biota, PositionType property, Position value, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (biota.PropertiesPosition == null)
                    biota.PropertiesPosition = new Dictionary<PositionType, PropertiesPosition>();

                var entity = new PropertiesPosition { ObjCellId = value.Cell, PositionX = value.PositionX, PositionY = value.PositionY, PositionZ = value.PositionZ, RotationW = value.RotationW, RotationX = value.RotationX, RotationY = value.RotationY, RotationZ = value.RotationZ };

                biota.PropertiesPosition[property] = entity;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }


        // =====================================
        // Remove
        // Bool, DID, Float, IID, Int, Int64, String, Position
        // =====================================

        public static bool TryRemoveProperty(this Biota biota, PropertyBool property, ReaderWriterLockSlim rwLock)
        {
            if (biota.PropertiesBool == null)
                return false;

            rwLock.EnterWriteLock();
            try
            {
                return biota.PropertiesBool.Remove(property);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyDataId property, ReaderWriterLockSlim rwLock)
        {
            if (biota.PropertiesDID == null)
                return false;

            rwLock.EnterWriteLock();
            try
            {
                return biota.PropertiesDID.Remove(property);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyFloat property, ReaderWriterLockSlim rwLock)
        {
            if (biota.PropertiesFloat == null)
                return false;

            rwLock.EnterWriteLock();
            try
            {
                return biota.PropertiesFloat.Remove(property);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyInstanceId property, ReaderWriterLockSlim rwLock)
        {
            if (biota.PropertiesIID == null)
                return false;

            rwLock.EnterWriteLock();
            try
            {
                return biota.PropertiesIID.Remove(property);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyInt property, ReaderWriterLockSlim rwLock)
        {
            if (biota.PropertiesInt == null)
                return false;

            rwLock.EnterWriteLock();
            try
            {
                return biota.PropertiesInt.Remove(property);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyInt64 property, ReaderWriterLockSlim rwLock)
        {
            if (biota.PropertiesInt64 == null)
                return false;

            rwLock.EnterWriteLock();
            try
            {
                return biota.PropertiesInt64.Remove(property);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyString property, ReaderWriterLockSlim rwLock)
        {
            if (biota.PropertiesString == null)
                return false;

            rwLock.EnterWriteLock();
            try
            {
                return biota.PropertiesString.Remove(property);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PositionType property, ReaderWriterLockSlim rwLock)
        {
            if (biota.PropertiesPosition == null)
                return false;

            rwLock.EnterWriteLock();
            try
            {
                return biota.PropertiesPosition.Remove(property);
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
            if (biota.PropertiesSpellBook == null)
                return new Dictionary<int, float>();

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
            if (biota.PropertiesSpellBook == null)
                return false;

            rwLock.EnterReadLock();
            try
            {
                return biota.PropertiesSpellBook.Count > 0;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static List<int> GetKnownSpellsIds(this Biota biota, ReaderWriterLockSlim rwLock)
        {
            if (biota.PropertiesSpellBook == null)
                return new List<int>();

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
            if (biota.PropertiesSpellBook == null)
                return new List<int>();

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
            if (biota.PropertiesSpellBook == null)
                return new List<float>();

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
            if (biota.PropertiesSpellBook == null)
                return false;

            rwLock.EnterReadLock();
            try
            {
                return biota.PropertiesSpellBook.ContainsKey(spell);
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
            if (biota.PropertiesSpellBook == null)
                return new Dictionary<int, float>();

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
            if (biota.PropertiesSpellBook == null)
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

        public static void ClearSpells(this Biota biota, ReaderWriterLockSlim rwLock)
        {
            if (biota.PropertiesSpellBook == null)
                return;

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
            if (biota.PropertiesSkill == null)
                return null;

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

        public static PropertiesSkill GetOrAddSkill(this Biota biota, Skill skill, ReaderWriterLockSlim rwLock, out bool skillAdded)
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


        // =====================================
        // HousePermissions
        // =====================================

        public static Dictionary<uint, bool> CloneHousePermissions(this Biota biota, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                if (biota.HousePermissions == null)
                    return new Dictionary<uint, bool>();

                return new Dictionary<uint, bool>(biota.HousePermissions);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static bool HasHouseGuest(this Biota biota, uint guestGuid, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                if (biota.HousePermissions == null)
                    return false;

                return biota.HousePermissions.ContainsKey(guestGuid);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static bool? GetHouseGuestStoragePermission(this Biota biota, uint guestGuid, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                if (biota.HousePermissions == null)
                    return null;

                if (!biota.HousePermissions.TryGetValue(guestGuid, out var value))
                    return null;

                return value;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static void AddOrUpdateHouseGuest(this Biota biota, uint guestGuid, bool storage, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (biota.HousePermissions == null)
                    biota.HousePermissions = new Dictionary<uint, bool>();

                biota.HousePermissions[guestGuid] = storage;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static bool RemoveHouseGuest(this Biota biota, uint guestGuid, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (biota.HousePermissions == null)
                    return false;

                return biota.HousePermissions.Remove(guestGuid);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }


        // =====================================
        // Utility
        // =====================================

        public static string GetName(this Biota biota)
        {
            if (biota.PropertiesString.TryGetValue(PropertyString.Name, out var value))
                return value;

            return null;
        }
    }
}
