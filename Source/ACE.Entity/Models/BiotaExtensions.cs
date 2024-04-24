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

        public static bool? GetProperty(this Biota biota, PropertyBool property, Object rwLock)
        {
            if (biota.PropertiesBool == null)
                return null;

            lock (rwLock)
            {
                if (biota.PropertiesBool.TryGetValue(property, out var value))
                    return value;

                return null;
            }
        }

        public static uint? GetProperty(this Biota biota, PropertyDataId property, Object rwLock)
        {
            if (biota.PropertiesDID == null)
                return null;

            lock (rwLock)
            {
                if (biota.PropertiesDID.TryGetValue(property, out var value))
                    return value;

                return null;
            }
        }

        public static double? GetProperty(this Biota biota, PropertyFloat property, Object rwLock)
        {
            if (biota.PropertiesFloat == null)
                return null;

            lock (rwLock)
            {
                if (biota.PropertiesFloat.TryGetValue(property, out var value))
                    return value;

                return null;
            }
        }

        public static uint? GetProperty(this Biota biota, PropertyInstanceId property, Object rwLock)
        {
            if (biota.PropertiesIID == null)
                return null;

            lock (rwLock)
            {
                if (biota.PropertiesIID.TryGetValue(property, out var value))
                    return value;

                return null;
            }
        }

        public static int? GetProperty(this Biota biota, PropertyInt property, Object rwLock)
        {
            if (biota.PropertiesInt == null)
                return null;

            lock (rwLock)
            {
                if (biota.PropertiesInt.TryGetValue(property, out var value))
                    return value;

                return null;
            }
        }

        public static long? GetProperty(this Biota biota, PropertyInt64 property, Object rwLock)
        {
            if (biota.PropertiesInt64 == null)
                return null;

            lock (rwLock)
            {
                if (biota.PropertiesInt64.TryGetValue(property, out var value))
                    return value;

                return null;
            }
        }

        public static string GetProperty(this Biota biota, PropertyString property, Object rwLock)
        {
            if (biota.PropertiesString == null)
                return null;

            lock (rwLock)
            {
                if (biota.PropertiesString.TryGetValue(property, out var value))
                    return value;

                return null;
            }
        }

        public static PropertiesPosition GetProperty(this Biota biota, PositionType property, Object rwLock)
        {
            if (biota.PropertiesPosition == null)
                return null;

            lock (rwLock)
            {
                if (biota.PropertiesPosition.TryGetValue(property, out var value))
                    return value;

                return null;
            }
        }

        public static Position GetPosition(this Biota biota, PositionType property, Object rwLock)
        {
            if (biota.PropertiesPosition == null)
                return null;

            lock (rwLock)
            {
                if (biota.PropertiesPosition.TryGetValue(property, out var value))
                    return new Position(value.ObjCellId, value.PositionX, value.PositionY, value.PositionZ, value.RotationX, value.RotationY, value.RotationZ, value.RotationW, property == PositionType.RelativeDestination);

                return null;
            }
        }


        // =====================================
        // Set
        // Bool, DID, Float, IID, Int, Int64, String, Position
        // =====================================

        public static void SetProperty(this Biota biota, PropertyBool property, bool value, Object rwLock, out bool changed)
        {
            lock (rwLock)
            {
                if (biota.PropertiesBool == null)
                    biota.PropertiesBool = new Dictionary<PropertyBool, bool>();

                changed = (!biota.PropertiesBool.TryGetValue(property, out var existing) || value != existing);

                if (changed)
                    biota.PropertiesBool[property] = value;
            }
        }

        public static void SetProperty(this Biota biota, PropertyDataId property, uint value, Object rwLock, out bool changed)
        {
            lock (rwLock)
            {
                if (biota.PropertiesDID == null)
                    biota.PropertiesDID = new Dictionary<PropertyDataId, uint>();

                changed = (!biota.PropertiesDID.TryGetValue(property, out var existing) || value != existing);

                if (changed)
                    biota.PropertiesDID[property] = value;
            }
        }

        public static void SetProperty(this Biota biota, PropertyFloat property, double value, Object rwLock, out bool changed)
        {
            lock (rwLock)
            {
                if (biota.PropertiesFloat == null)
                    biota.PropertiesFloat = new Dictionary<PropertyFloat, double>();

                changed = (!biota.PropertiesFloat.TryGetValue(property, out var existing) || value != existing);

                if (changed)
                    biota.PropertiesFloat[property] = value;
            }
        }

        public static void SetProperty(this Biota biota, PropertyInstanceId property, uint value, Object rwLock, out bool changed)
        {
            lock (rwLock)
            {
                if (biota.PropertiesIID == null)
                    biota.PropertiesIID = new Dictionary<PropertyInstanceId, uint>();

                changed = (!biota.PropertiesIID.TryGetValue(property, out var existing) || value != existing);

                if (changed)
                    biota.PropertiesIID[property] = value;
            }
        }

        public static void SetProperty(this Biota biota, PropertyInt property, int value, Object rwLock, out bool changed)
        {
            lock (rwLock)
            {
                if (biota.PropertiesInt == null)
                    biota.PropertiesInt = new Dictionary<PropertyInt, int>();

                changed = (!biota.PropertiesInt.TryGetValue(property, out var existing) || value != existing);

                if (changed)
                    biota.PropertiesInt[property] = value;
            }
        }

        public static void SetProperty(this Biota biota, PropertyInt64 property, long value, Object rwLock, out bool changed)
        {
            lock (rwLock)
            {
                if (biota.PropertiesInt64 == null)
                    biota.PropertiesInt64 = new Dictionary<PropertyInt64, long>();

                changed = (!biota.PropertiesInt64.TryGetValue(property, out var existing) || value != existing);

                if (changed)
                    biota.PropertiesInt64[property] = value;
            }
        }

        public static void SetProperty(this Biota biota, PropertyString property, string value, Object rwLock, out bool changed)
        {
            lock (rwLock)
            {
                if (biota.PropertiesString == null)
                    biota.PropertiesString = new Dictionary<PropertyString, string>();

                changed = (!biota.PropertiesString.TryGetValue(property, out var existing) || value != existing);

                if (changed)
                    biota.PropertiesString[property] = value;
            }
        }

        public static void SetProperty(this Biota biota, PositionType property, PropertiesPosition value, Object rwLock)
        {
            lock (rwLock)
            {
                if (biota.PropertiesPosition == null)
                    biota.PropertiesPosition = new Dictionary<PositionType, PropertiesPosition>();

                biota.PropertiesPosition[property] = value;
            }
        }

        public static void SetPosition(this Biota biota, PositionType property, Position value, Object rwLock)
        {
            lock (rwLock)
            {
                if (biota.PropertiesPosition == null)
                    biota.PropertiesPosition = new Dictionary<PositionType, PropertiesPosition>();

                var entity = new PropertiesPosition { ObjCellId = value.Cell, PositionX = value.PositionX, PositionY = value.PositionY, PositionZ = value.PositionZ, RotationW = value.RotationW, RotationX = value.RotationX, RotationY = value.RotationY, RotationZ = value.RotationZ };

                biota.PropertiesPosition[property] = entity;
            }
        }


        // =====================================
        // Remove
        // Bool, DID, Float, IID, Int, Int64, String, Position
        // =====================================

        public static bool TryRemoveProperty(this Biota biota, PropertyBool property, Object rwLock)
        {
            if (biota.PropertiesBool == null)
                return false;

            lock (rwLock)
            {
                return biota.PropertiesBool.Remove(property);
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyDataId property, Object rwLock)
        {
            if (biota.PropertiesDID == null)
                return false;

            lock (rwLock)
            {
                return biota.PropertiesDID.Remove(property);
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyFloat property, Object rwLock)
        {
            if (biota.PropertiesFloat == null)
                return false;

            lock (rwLock)
            {
                return biota.PropertiesFloat.Remove(property);
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyInstanceId property, Object rwLock)
        {
            if (biota.PropertiesIID == null)
                return false;

            lock (rwLock)
            {
                return biota.PropertiesIID.Remove(property);
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyInt property, Object rwLock)
        {
            if (biota.PropertiesInt == null)
                return false;

            lock (rwLock)
            {
                return biota.PropertiesInt.Remove(property);
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyInt64 property, Object rwLock)
        {
            if (biota.PropertiesInt64 == null)
                return false;

            lock (rwLock)
            {
                return biota.PropertiesInt64.Remove(property);
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyString property, Object rwLock)
        {
            if (biota.PropertiesString == null)
                return false;

            lock (rwLock)
            {
                return biota.PropertiesString.Remove(property);
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PositionType property, Object rwLock)
        {
            if (biota.PropertiesPosition == null)
                return false;

            lock (rwLock)
            {
                return biota.PropertiesPosition.Remove(property);
            }
        }


        // =====================================
        // BiotaPropertiesSpellBook
        // =====================================

        public static Dictionary<int, float> CloneSpells(this Biota biota, Object rwLock)
        {
            if (biota.PropertiesSpellBook == null)
                return new Dictionary<int, float>();

            lock (rwLock)
            {
                var results = new Dictionary<int, float>();

                foreach (var kvp in biota.PropertiesSpellBook)
                    results[kvp.Key] = kvp.Value;

                return results;
            }
        }

        public static bool HasKnownSpell(this Biota biota, Object rwLock)
        {
            if (biota.PropertiesSpellBook == null)
                return false;

            lock (rwLock)
            {
                return biota.PropertiesSpellBook.Count > 0;
            }
        }

        public static List<int> GetKnownSpellsIds(this Biota biota, Object rwLock)
        {
            if (biota.PropertiesSpellBook == null)
                return new List<int>();

            lock (rwLock)
            {
                if (biota.PropertiesSpellBook == null)
                    return new List<int>();

                return new List<int>(biota.PropertiesSpellBook.Keys);
            }
        }

        public static List<int> GetKnownSpellsIdsWhere(this Biota biota, Func<int, bool> predicate, Object rwLock)
        {
            if (biota.PropertiesSpellBook == null)
                return new List<int>();

            lock (rwLock)
            {
                if (biota.PropertiesSpellBook == null)
                    return new List<int>();

                return biota.PropertiesSpellBook.Keys.Where(predicate).ToList();
            }

        }

        public static List<float> GetKnownSpellsProbabilities(this Biota biota, Object rwLock)
        {
            if (biota.PropertiesSpellBook == null)
                return new List<float>();

            lock (rwLock)
            {
                if (biota.PropertiesSpellBook == null)
                    return new List<float>();

                return new List<float>(biota.PropertiesSpellBook.Values);
            }
 
        }

        public static bool SpellIsKnown(this Biota biota, int spell, Object rwLock)
        {
            if (biota.PropertiesSpellBook == null)
                return false;

            lock (rwLock)
            {
                return biota.PropertiesSpellBook.ContainsKey(spell);
            }
        }

        public static float GetOrAddKnownSpell(this Biota biota, int spell, Object rwLock, out bool spellAdded, float probability = 2.0f)
        {
            lock (rwLock)
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

        }

        public static Dictionary<int, float> GetMatchingSpells(this Biota biota, HashSet<int> match, Object rwLock)
        {
            if (biota.PropertiesSpellBook == null)
                return new Dictionary<int, float>();

            lock (rwLock)
            {
                var results = new Dictionary<int, float>();

                foreach (var value in biota.PropertiesSpellBook)
                {
                    if (match.Contains(value.Key))
                        results[value.Key] = value.Value;
                }

                return results;
            }
        }

        public static bool TryRemoveKnownSpell(this Biota biota, int spell, Object rwLock)
        {
            if (biota.PropertiesSpellBook == null)
                return false;

            lock (rwLock)
            {
                return biota.PropertiesSpellBook.Remove(spell);
            }
        }

        public static void ClearSpells(this Biota biota, Object rwLock)
        {
            if (biota.PropertiesSpellBook == null)
                return;

            lock (rwLock)
            {
                biota.PropertiesSpellBook?.Clear();
            }
        }


        // =====================================
        // BiotaPropertiesSkill
        // =====================================

        public static PropertiesSkill GetSkill(this Biota biota, Skill skill, Object rwLock)
        {
            if (biota.PropertiesSkill == null)
                return null;

            lock (rwLock)
            {
                if (biota.PropertiesSkill == null)
                    return null;

                biota.PropertiesSkill.TryGetValue(skill, out var value);
                return value;
            }
        }

        public static PropertiesSkill GetOrAddSkill(this Biota biota, Skill skill, Object rwLock, out bool skillAdded)
        {
            lock (rwLock)
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
        }


        // =====================================
        // HousePermissions
        // =====================================

        public static Dictionary<uint, bool> CloneHousePermissions(this Biota biota, Object rwLock)
        {
            lock (rwLock)
            {
                if (biota.HousePermissions == null)
                    return new Dictionary<uint, bool>();

                return new Dictionary<uint, bool>(biota.HousePermissions);
            }
        }

        public static bool HasHouseGuest(this Biota biota, uint guestGuid, Object rwLock)
        {
            lock (rwLock)
            {
                if (biota.HousePermissions == null)
                    return false;

                return biota.HousePermissions.ContainsKey(guestGuid);
            }
        }

        public static bool? GetHouseGuestStoragePermission(this Biota biota, uint guestGuid, Object rwLock)
        {
            lock (rwLock)
            {
                if (biota.HousePermissions == null)
                    return null;

                if (!biota.HousePermissions.TryGetValue(guestGuid, out var value))
                    return null;

                return value;
            }
        }

        public static void AddOrUpdateHouseGuest(this Biota biota, uint guestGuid, bool storage, Object rwLock)
        {
            lock (rwLock)
            {
                if (biota.HousePermissions == null)
                    biota.HousePermissions = new Dictionary<uint, bool>();

                biota.HousePermissions[guestGuid] = storage;
            }
        }

        public static bool RemoveHouseGuest(this Biota biota, uint guestGuid, Object rwLock)
        {
            lock (rwLock)
            {
                if (biota.HousePermissions == null)
                    return false;

                return biota.HousePermissions.Remove(guestGuid);
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
