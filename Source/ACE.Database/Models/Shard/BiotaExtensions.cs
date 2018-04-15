using System.Collections.Generic;
using System.Linq;
using System.Threading;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Database.Models.Shard
{
    public static class BiotaExtensions
    {
        public static uint? GetAnimationId(this Biota biota, byte index)
        {
            return biota.BiotaPropertiesAnimPart.FirstOrDefault(x => x.Index == index)?.AnimationId;
        }

        public static BiotaPropertiesAttribute GetAttribute(this Biota biota, PropertyAttribute attribute)
        {
            return biota.BiotaPropertiesAttribute.FirstOrDefault(x => x.Type == (uint)attribute);
        }

        public static BiotaPropertiesAttribute2nd GetAttribute2nd(this Biota biota, PropertyAttribute2nd attribute)
        {
            return biota.BiotaPropertiesAttribute2nd.FirstOrDefault(x => x.Type == (uint)attribute);
        }

        public static BiotaPropertiesBodyPart GetBodyPart(this Biota biota, ushort key)
        {
            return biota.BiotaPropertiesBodyPart.FirstOrDefault(x => x.Key == key);
        }

        public static BiotaPropertiesBookPageData GetBookPageData(this Biota biota, uint pageId)
        {
            return biota.BiotaPropertiesBookPageData.FirstOrDefault(x => x.PageId == pageId);
        }

        public static bool? GetProperty(this Biota biota, PropertyBool property, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return biota.BiotaPropertiesBool.FirstOrDefault(x => x.Type == (uint) property)?.Value;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static BiotaPropertiesContract GetContract(this Biota biota, uint contractId)
        {
            return biota.BiotaPropertiesContract.FirstOrDefault(x => x.ContractId == contractId);
        }

        public static BiotaPropertiesCreateList GetCreateList(this Biota biota, sbyte destinationType)
        {
            return biota.BiotaPropertiesCreateList.FirstOrDefault(x => x.DestinationType == destinationType);
        }

        public static uint? GetProperty(this Biota biota, PropertyDataId property, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return biota.BiotaPropertiesDID.FirstOrDefault(x => x.Type == (uint)property)?.Value;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static BiotaPropertiesEmote GetEmote(this Biota biota, uint category)
        {
            return biota.BiotaPropertiesEmote.FirstOrDefault(x => x.Category == category);
        }

        public static IEnumerable<BiotaPropertiesEmote> GetEmotes(this Biota biota, uint category)
        {
            return biota.BiotaPropertiesEmote.Where(x => x.Category == category);
        }

        // BiotaPropertiesEmoteAction

        public static BiotaPropertiesEventFilter GetEventFilter(this Biota biota, int eventId)
        {
            return biota.BiotaPropertiesEventFilter.FirstOrDefault(x => x.Event == eventId);
        }

        public static double? GetProperty(this Biota biota, PropertyFloat property, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return biota.BiotaPropertiesFloat.FirstOrDefault(x => x.Type == (ushort)property)?.Value;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        // BiotaPropertiesFriendList

        // BiotaPropertiesGenerator

        public static uint? GetProperty(this Biota biota, PropertyInstanceId property, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return biota.BiotaPropertiesIID.FirstOrDefault(x => x.Type == (uint)property)?.Value;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static int? GetProperty(this Biota biota, PropertyInt property, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return biota.BiotaPropertiesInt.FirstOrDefault(x => x.Type == (uint)property)?.Value;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static long? GetProperty(this Biota biota, PropertyInt64 property, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return biota.BiotaPropertiesInt64.FirstOrDefault(x => x.Type == (uint)property)?.Value;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static BiotaPropertiesPalette GetPalette(this Biota biota, uint subPaletteId)
        {
            return biota.BiotaPropertiesPalette.FirstOrDefault(x => x.SubPaletteId == subPaletteId);
        }

        public static Position GetPosition(this Biota biota, PositionType positionType, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                var result = biota.BiotaPropertiesPosition.FirstOrDefault(x => x.PositionType == (uint)positionType);

                if (result == null)
                    return null;

                return new Position(result.ObjCellId, result.OriginX, result.OriginY, result.OriginZ, result.AnglesX, result.AnglesY, result.AnglesZ, result.AnglesW);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static BiotaPropertiesSkill GetProperty(this Biota biota, Skill skill)
        {
            return biota.BiotaPropertiesSkill.FirstOrDefault(x => x.Type == (uint)skill);
        }

        public static BiotaPropertiesSpellBook GetSpell(this Biota biota, int spell)
        {
            return biota.BiotaPropertiesSpellBook.FirstOrDefault(x => x.Spell == spell);
        }

        public static string GetProperty(this Biota biota, PropertyString property, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return biota.BiotaPropertiesString.FirstOrDefault(x => x.Type == (uint)property)?.Value;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static BiotaPropertiesTextureMap GetTextureMap(this Biota biota, byte index)
        {
            return biota.BiotaPropertiesTextureMap.FirstOrDefault(x => x.Index == index);
        }



        public static void SetProperty(this Biota biota, PropertyBool property, bool value, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                var result = biota.BiotaPropertiesBool.FirstOrDefault(x => x.Type == (uint) property);
                if (result != null)
                    result.Value = value;
                else
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        var entity = new BiotaPropertiesBool { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                        biota.BiotaPropertiesBool.Add(entity);
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyDataId property, uint value, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                var result = biota.BiotaPropertiesDID.FirstOrDefault(x => x.Type == (uint)property);
                if (result != null)
                    result.Value = value;
                else
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        var entity = new BiotaPropertiesDID { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                        biota.BiotaPropertiesDID.Add(entity);
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyFloat property, double value, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                var result = biota.BiotaPropertiesFloat.FirstOrDefault(x => x.Type == (ushort) property);
                if (result != null)
                    result.Value = value;
                else
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        var entity = new BiotaPropertiesFloat { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                        biota.BiotaPropertiesFloat.Add(entity);
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyInstanceId property, uint value, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                var result = biota.BiotaPropertiesIID.FirstOrDefault(x => x.Type == (uint) property);
                if (result != null)
                    result.Value = value;
                else
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        var entity = new BiotaPropertiesIID { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                        biota.BiotaPropertiesIID.Add(entity);
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyInt property, int value, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                var result = biota.BiotaPropertiesInt.FirstOrDefault(x => x.Type == (uint)property);
                if (result != null)
                    result.Value = value;
                else
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        var entity = new BiotaPropertiesInt { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                        biota.BiotaPropertiesInt.Add(entity);
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyInt64 property, long value, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                var result = biota.BiotaPropertiesInt64.FirstOrDefault(x => x.Type == (uint)property);
                if (result != null)
                    result.Value = value;
                else
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        var entity = new BiotaPropertiesInt64 { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                        biota.BiotaPropertiesInt64.Add(entity);
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyString property, string value, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                var result = biota.BiotaPropertiesString.FirstOrDefault(x => x.Type == (uint) property);
                if (result != null)
                    result.Value = value;
                else
                {
                    rwLock.EnterWriteLock();
                    try
                    { 
                        var entity = new BiotaPropertiesString { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                        biota.BiotaPropertiesString.Add(entity);
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }

        public static void SetPosition(this Biota biota, PositionType positionType, Position position, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            { 
                var result = biota.BiotaPropertiesPosition.FirstOrDefault(x => x.PositionType == (uint)positionType);
                if (result != null)
                {
                    result.ObjCellId = position.Cell;
                    result.OriginX = position.PositionX;
                    result.OriginY = position.PositionY;
                    result.OriginZ = position.PositionZ;
                    result.AnglesW = position.RotationW;
                    result.AnglesX = position.RotationX;
                    result.AnglesY = position.RotationY;
                    result.AnglesZ = position.RotationZ;
                }
                else
                {
                    rwLock.EnterWriteLock();
                    try
                    { 
                        var entity = new BiotaPropertiesPosition { ObjectId = biota.Id, PositionType = (ushort)positionType, ObjCellId = position.Cell, OriginX = position.PositionX, OriginY = position.PositionY, OriginZ = position.PositionZ, AnglesW = position.RotationW, AnglesX = position.RotationX, AnglesY = position.RotationY, AnglesZ = position.RotationZ, Object = biota };
                        biota.BiotaPropertiesPosition.Add(entity);
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }



        public static bool TryRemoveProperty(this Biota biota, PropertyBool property, out BiotaPropertiesBool entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                entity = biota.BiotaPropertiesBool.FirstOrDefault(x => x.Type == (uint) property);
                if (entity != null)
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        biota.BiotaPropertiesBool.Remove(entity);
                        entity.Object = null;
                        return true;
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
                return false;
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyDataId property, out BiotaPropertiesDID entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                entity = biota.BiotaPropertiesDID.FirstOrDefault(x => x.Type == (uint)property);
                if (entity != null)
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        biota.BiotaPropertiesDID.Remove(entity);
                        entity.Object = null;
                        return true;
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
                return false;
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyFloat property, out BiotaPropertiesFloat entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                entity = biota.BiotaPropertiesFloat.FirstOrDefault(x => x.Type == (ushort)property);
                if (entity != null)
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        biota.BiotaPropertiesFloat.Remove(entity);
                        entity.Object = null;
                        return true;
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
                return false;
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyInstanceId property, out BiotaPropertiesIID entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                entity = biota.BiotaPropertiesIID.FirstOrDefault(x => x.Type == (uint) property);
                if (entity != null)
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        biota.BiotaPropertiesIID.Remove(entity);
                        entity.Object = null;
                        return true;
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
                return false;
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyInt property, out BiotaPropertiesInt entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                entity = biota.BiotaPropertiesInt.FirstOrDefault(x => x.Type == (uint)property);
                if (entity != null)
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        biota.BiotaPropertiesInt.Remove(entity);
                        entity.Object = null;
                        return true;
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
                return false;
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyInt64 property, out BiotaPropertiesInt64 entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                entity = biota.BiotaPropertiesInt64.FirstOrDefault(x => x.Type == (uint)property);
                if (entity != null)
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        biota.BiotaPropertiesInt64.Remove(entity);
                        entity.Object = null;
                        return true;
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
                return false;
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PositionType positionType, out BiotaPropertiesPosition entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                entity = biota.BiotaPropertiesPosition.FirstOrDefault(x => x.PositionType == (uint)positionType);
                if (entity != null)
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        biota.BiotaPropertiesPosition.Remove(entity);
                        entity.Object = null;
                        return true;
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
                return false;
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyString property, out BiotaPropertiesString entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                entity = biota.BiotaPropertiesString.FirstOrDefault(x => x.Type == (uint)property);
                if (entity != null)
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        biota.BiotaPropertiesString.Remove(entity);
                        entity.Object = null;
                        return true;
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
                return false;
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }

        public static bool TryRemoveEnchantment(this Biota biota, int spellId, out BiotaPropertiesEnchantmentRegistry entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                entity = biota.BiotaPropertiesEnchantmentRegistry.FirstOrDefault(x => x.SpellId == spellId);
                if (entity != null)
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        biota.BiotaPropertiesEnchantmentRegistry.Remove(entity);
                        entity.Object = null;
                        return true;
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
                return false;
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }
    }
}
