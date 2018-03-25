using System.Linq;
using System.Threading;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Database.Models.Shard
{
    public static class BiotaExtensions
    {
        //private static readonly ReaderWriterLockSlim BiotaPropertiesAnimPartLock = new ReaderWriterLockSlim();
        //private static readonly ReaderWriterLockSlim BiotaPropertiesAttributeLock = new ReaderWriterLockSlim();
        //private static readonly ReaderWriterLockSlim BiotaPropertiesAttribute2ndLock = new ReaderWriterLockSlim();
        //private static readonly ReaderWriterLockSlim BiotaPropertiesBodyPartLock = new ReaderWriterLockSlim();
        //private static readonly ReaderWriterLockSlim BiotaPropertiesBookPageDataLock = new ReaderWriterLockSlim();
        private static readonly ReaderWriterLockSlim BiotaPropertiesBoolLock = new ReaderWriterLockSlim();
        //private static readonly ReaderWriterLockSlim BiotaPropertiesContractLock = new ReaderWriterLockSlim();
        //private static readonly ReaderWriterLockSlim BiotaPropertiesCreateListLock = new ReaderWriterLockSlim();
        private static readonly ReaderWriterLockSlim BiotaPropertiesDIDLock = new ReaderWriterLockSlim();
        //private static readonly ReaderWriterLockSlim BiotaPropertiesEmoteLock = new ReaderWriterLockSlim();
        //private static readonly ReaderWriterLockSlim BiotaPropertiesEventFilterLock = new ReaderWriterLockSlim();
        private static readonly ReaderWriterLockSlim BiotaPropertiesFloatLock = new ReaderWriterLockSlim();
        private static readonly ReaderWriterLockSlim BiotaPropertiesIIDLock = new ReaderWriterLockSlim();
        private static readonly ReaderWriterLockSlim BiotaPropertiesIntLock = new ReaderWriterLockSlim();
        private static readonly ReaderWriterLockSlim BiotaPropertiesInt64Lock = new ReaderWriterLockSlim();
        //private static readonly ReaderWriterLockSlim BiotaPropertiesPaletteLock = new ReaderWriterLockSlim();
        private static readonly ReaderWriterLockSlim BiotaPropertiesPositionLock = new ReaderWriterLockSlim();
        //private static readonly ReaderWriterLockSlim BiotaPropertiesSkillLock = new ReaderWriterLockSlim();
        //private static readonly ReaderWriterLockSlim BiotaPropertiesSpellBookLock = new ReaderWriterLockSlim();
        private static readonly ReaderWriterLockSlim BiotaPropertiesStringLock = new ReaderWriterLockSlim();
        //private static readonly ReaderWriterLockSlim BiotaPropertiesTextureMapLock = new ReaderWriterLockSlim();

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

        public static bool? GetProperty(this Biota biota, PropertyBool property)
        {
            BiotaPropertiesBoolLock.EnterReadLock();
            try
            {
                return biota.BiotaPropertiesBool.FirstOrDefault(x => x.Type == (uint) property)?.Value;
            }
            finally
            {
                BiotaPropertiesBoolLock.ExitReadLock();
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

        public static uint? GetProperty(this Biota biota, PropertyDataId property)
        {
            BiotaPropertiesDIDLock.EnterReadLock();
            try
            {
                return biota.BiotaPropertiesDID.FirstOrDefault(x => x.Type == (uint)property)?.Value;
            }
            finally
            {
                BiotaPropertiesDIDLock.ExitReadLock();
            }
        }

        public static BiotaPropertiesEmote GetEmote(this Biota biota, uint category)
        {
            return biota.BiotaPropertiesEmote.FirstOrDefault(x => x.Category == category);
        }

        // BiotaPropertiesEmoteAction

        public static BiotaPropertiesEventFilter GetEventFilter(this Biota biota, int eventId)
        {
            return biota.BiotaPropertiesEventFilter.FirstOrDefault(x => x.Event == eventId);
        }

        public static double? GetProperty(this Biota biota, PropertyFloat property)
        {
            BiotaPropertiesFloatLock.EnterReadLock();
            try
            {
                return biota.BiotaPropertiesFloat.FirstOrDefault(x => x.Type == (ushort)property)?.Value;
            }
            finally
            {
                BiotaPropertiesFloatLock.ExitReadLock();
            }
        }

        // BiotaPropertiesFriendList

        // BiotaPropertiesGenerator

        public static uint? GetProperty(this Biota biota, PropertyInstanceId property)
        {
            BiotaPropertiesIIDLock.EnterReadLock();
            try
            {
                return biota.BiotaPropertiesIID.FirstOrDefault(x => x.Type == (uint)property)?.Value;
            }
            finally
            {
                BiotaPropertiesIIDLock.ExitReadLock();
            }
        }

        public static int? GetProperty(this Biota biota, PropertyInt property)
        {
            BiotaPropertiesIntLock.EnterReadLock();
            try
            {
                return biota.BiotaPropertiesInt.FirstOrDefault(x => x.Type == (uint)property)?.Value;
            }
            finally
            {
                BiotaPropertiesIntLock.ExitReadLock();
            }
        }

        public static long? GetProperty(this Biota biota, PropertyInt64 property)
        {
            BiotaPropertiesInt64Lock.EnterReadLock();
            try
            {
                return biota.BiotaPropertiesInt64.FirstOrDefault(x => x.Type == (uint)property)?.Value;
            }
            finally
            {
                BiotaPropertiesInt64Lock.ExitReadLock();
            }
        }

        public static BiotaPropertiesPalette GetPalette(this Biota biota, uint subPaletteId)
        {
            return biota.BiotaPropertiesPalette.FirstOrDefault(x => x.SubPaletteId == subPaletteId);
        }

        public static Position GetPosition(this Biota biota, PositionType positionType)
        {
            BiotaPropertiesPositionLock.EnterReadLock();
            try
            {
                var result = biota.BiotaPropertiesPosition.FirstOrDefault(x => x.PositionType == (uint)positionType);

                if (result == null)
                    return null;

                return new Position(result.ObjCellId, result.OriginX, result.OriginY, result.OriginZ, result.AnglesX, result.AnglesY, result.AnglesZ, result.AnglesW);
            }
            finally
            {
                BiotaPropertiesPositionLock.ExitReadLock();
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

        public static string GetProperty(this Biota biota, PropertyString property)
        {
            BiotaPropertiesStringLock.EnterReadLock();
            try
            {
                return biota.BiotaPropertiesString.FirstOrDefault(x => x.Type == (uint)property)?.Value;
            }
            finally
            {
                BiotaPropertiesStringLock.ExitReadLock();
            }
        }

        public static BiotaPropertiesTextureMap GetTextureMap(this Biota biota, byte index)
        {
            return biota.BiotaPropertiesTextureMap.FirstOrDefault(x => x.Index == index);
        }



        public static void SetProperty(this Biota biota, PropertyBool property, bool value)
        {
            BiotaPropertiesBoolLock.EnterUpgradeableReadLock();
            try
            {
                var result = biota.BiotaPropertiesBool.FirstOrDefault(x => x.Type == (uint) property);
                if (result != null)
                    result.Value = value;
                else
                {
                    BiotaPropertiesBoolLock.EnterWriteLock();
                    try
                    {
                        var entity = new BiotaPropertiesBool { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                        biota.BiotaPropertiesBool.Add(entity);
                    }
                    finally
                    {
                        BiotaPropertiesBoolLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                BiotaPropertiesBoolLock.ExitUpgradeableReadLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyDataId property, uint value)
        {
            BiotaPropertiesDIDLock.EnterUpgradeableReadLock();
            try
            {
                var result = biota.BiotaPropertiesDID.FirstOrDefault(x => x.Type == (uint)property);
                if (result != null)
                    result.Value = value;
                else
                {
                    BiotaPropertiesDIDLock.EnterWriteLock();
                    try
                    {
                        var entity = new BiotaPropertiesDID { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                        biota.BiotaPropertiesDID.Add(entity);
                    }
                    finally
                    {
                        BiotaPropertiesDIDLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                BiotaPropertiesDIDLock.ExitUpgradeableReadLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyFloat property, double value)
        {
            BiotaPropertiesFloatLock.EnterUpgradeableReadLock();
            try
            {
                var result = biota.BiotaPropertiesFloat.FirstOrDefault(x => x.Type == (ushort) property);
                if (result != null)
                    result.Value = value;
                else
                {
                    BiotaPropertiesFloatLock.EnterWriteLock();
                    try
                    {
                        var entity = new BiotaPropertiesFloat { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                        biota.BiotaPropertiesFloat.Add(entity);
                    }
                    finally
                    {
                        BiotaPropertiesFloatLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                BiotaPropertiesFloatLock.ExitUpgradeableReadLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyInstanceId property, uint value)
        {
            BiotaPropertiesIIDLock.EnterUpgradeableReadLock();
            try
            {
                var result = biota.BiotaPropertiesIID.FirstOrDefault(x => x.Type == (uint) property);
                if (result != null)
                    result.Value = value;
                else
                {
                    BiotaPropertiesIIDLock.EnterWriteLock();
                    try
                    {
                        var entity = new BiotaPropertiesIID { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                        biota.BiotaPropertiesIID.Add(entity);
                    }
                    finally
                    {
                        BiotaPropertiesIIDLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                BiotaPropertiesIIDLock.ExitUpgradeableReadLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyInt property, int value)
        {
            BiotaPropertiesIntLock.EnterUpgradeableReadLock();
            try
            {
                var result = biota.BiotaPropertiesInt.FirstOrDefault(x => x.Type == (uint)property);
                if (result != null)
                    result.Value = value;
                else
                {
                    BiotaPropertiesIntLock.EnterWriteLock();
                    try
                    {
                        var entity = new BiotaPropertiesInt { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                        biota.BiotaPropertiesInt.Add(entity);
                    }
                    finally
                    {
                        BiotaPropertiesIntLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                BiotaPropertiesIntLock.ExitUpgradeableReadLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyInt64 property, long value)
        {
            BiotaPropertiesInt64Lock.EnterUpgradeableReadLock();
            try
            {
                var result = biota.BiotaPropertiesInt64.FirstOrDefault(x => x.Type == (uint)property);
                if (result != null)
                    result.Value = value;
                else
                {
                    BiotaPropertiesInt64Lock.EnterWriteLock();
                    try
                    {
                        var entity = new BiotaPropertiesInt64 { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                        biota.BiotaPropertiesInt64.Add(entity);
                    }
                    finally
                    {
                        BiotaPropertiesInt64Lock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                BiotaPropertiesInt64Lock.ExitUpgradeableReadLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyString property, string value)
        {
            BiotaPropertiesStringLock.EnterUpgradeableReadLock();
            try
            {
                var result = biota.BiotaPropertiesString.FirstOrDefault(x => x.Type == (uint) property);
                if (result != null)
                    result.Value = value;
                else
                {
                    BiotaPropertiesStringLock.EnterWriteLock();
                    try
                    { 
                        var entity = new BiotaPropertiesString { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                        biota.BiotaPropertiesString.Add(entity);
                    }
                    finally
                    {
                        BiotaPropertiesStringLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                BiotaPropertiesStringLock.ExitUpgradeableReadLock();
            }
        }

        public static void SetPosition(this Biota biota, PositionType positionType, Position position)
        {
            BiotaPropertiesPositionLock.EnterUpgradeableReadLock();
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
                    BiotaPropertiesPositionLock.EnterWriteLock();
                    try
                    { 
                        var entity = new BiotaPropertiesPosition { ObjectId = biota.Id, PositionType = (ushort)positionType, ObjCellId = position.Cell, OriginX = position.PositionX, OriginY = position.PositionY, OriginZ = position.PositionZ, AnglesW = position.RotationW, AnglesX = position.RotationX, AnglesY = position.RotationY, AnglesZ = position.RotationZ, Object = biota };
                        biota.BiotaPropertiesPosition.Add(entity);
                    }
                    finally
                    {
                        BiotaPropertiesPositionLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                BiotaPropertiesPositionLock.ExitUpgradeableReadLock();
            }
        }



        public static bool TryRemoveProperty(this Biota biota, PropertyBool property, out BiotaPropertiesBool entity)
        {
            BiotaPropertiesBoolLock.EnterUpgradeableReadLock();
            try
            {
                entity = biota.BiotaPropertiesBool.FirstOrDefault(x => x.Type == (uint) property);
                if (entity != null)
                {
                    BiotaPropertiesBoolLock.EnterWriteLock();
                    try
                    {
                        biota.BiotaPropertiesBool.Remove(entity);
                        entity.Object = null;
                        return true;
                    }
                    finally
                    {
                        BiotaPropertiesBoolLock.ExitWriteLock();
                    }
                }
                return false;
            }
            finally
            {
                BiotaPropertiesBoolLock.ExitUpgradeableReadLock();
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyDataId property, out BiotaPropertiesDID entity)
        {
            BiotaPropertiesDIDLock.EnterUpgradeableReadLock();
            try
            {
                entity = biota.BiotaPropertiesDID.FirstOrDefault(x => x.Type == (uint)property);
                if (entity != null)
                {
                    BiotaPropertiesDIDLock.EnterWriteLock();
                    try
                    {
                        biota.BiotaPropertiesDID.Remove(entity);
                        entity.Object = null;
                        return true;
                    }
                    finally
                    {
                        BiotaPropertiesDIDLock.ExitWriteLock();
                    }
                }
                return false;
            }
            finally
            {
                BiotaPropertiesDIDLock.ExitUpgradeableReadLock();
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyFloat property, out BiotaPropertiesFloat entity)
        {
            BiotaPropertiesFloatLock.EnterUpgradeableReadLock();
            try
            {
                entity = biota.BiotaPropertiesFloat.FirstOrDefault(x => x.Type == (ushort)property);
                if (entity != null)
                {
                    BiotaPropertiesFloatLock.EnterWriteLock();
                    try
                    {
                        biota.BiotaPropertiesFloat.Remove(entity);
                        entity.Object = null;
                        return true;
                    }
                    finally
                    {
                        BiotaPropertiesFloatLock.ExitWriteLock();
                    }
                }
                return false;
            }
            finally
            {
                BiotaPropertiesFloatLock.ExitUpgradeableReadLock();
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyInstanceId property, out BiotaPropertiesIID entity)
        {
            BiotaPropertiesIIDLock.EnterUpgradeableReadLock();
            try
            {
                entity = biota.BiotaPropertiesIID.FirstOrDefault(x => x.Type == (uint) property);
                if (entity != null)
                {
                    BiotaPropertiesIIDLock.EnterWriteLock();
                    try
                    {
                        biota.BiotaPropertiesIID.Remove(entity);
                        entity.Object = null;
                        return true;
                    }
                    finally
                    {
                        BiotaPropertiesIIDLock.ExitWriteLock();
                    }
                }
                return false;
            }
            finally
            {
                BiotaPropertiesIIDLock.ExitUpgradeableReadLock();
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyInt property, out BiotaPropertiesInt entity)
        {
            BiotaPropertiesIntLock.EnterUpgradeableReadLock();
            try
            {
                entity = biota.BiotaPropertiesInt.FirstOrDefault(x => x.Type == (uint)property);
                if (entity != null)
                {
                    BiotaPropertiesIntLock.EnterWriteLock();
                    try
                    {
                        biota.BiotaPropertiesInt.Remove(entity);
                        entity.Object = null;
                        return true;
                    }
                    finally
                    {
                        BiotaPropertiesIntLock.ExitWriteLock();
                    }
                }
                return false;
            }
            finally
            {
                BiotaPropertiesIntLock.ExitUpgradeableReadLock();
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyInt64 property, out BiotaPropertiesInt64 entity)
        {
            BiotaPropertiesInt64Lock.EnterUpgradeableReadLock();
            try
            {
                entity = biota.BiotaPropertiesInt64.FirstOrDefault(x => x.Type == (uint)property);
                if (entity != null)
                {
                    BiotaPropertiesInt64Lock.EnterWriteLock();
                    try
                    {
                        biota.BiotaPropertiesInt64.Remove(entity);
                        entity.Object = null;
                        return true;
                    }
                    finally
                    {
                        BiotaPropertiesInt64Lock.ExitWriteLock();
                    }
                }
                return false;
            }
            finally
            {
                BiotaPropertiesInt64Lock.ExitUpgradeableReadLock();
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PositionType positionType, out BiotaPropertiesPosition entity)
        {
            BiotaPropertiesPositionLock.EnterUpgradeableReadLock();
            try
            {
                entity = biota.BiotaPropertiesPosition.FirstOrDefault(x => x.PositionType == (uint)positionType);
                if (entity != null)
                {
                    BiotaPropertiesPositionLock.EnterWriteLock();
                    try
                    {
                        biota.BiotaPropertiesPosition.Remove(entity);
                        entity.Object = null;
                        return true;
                    }
                    finally
                    {
                        BiotaPropertiesPositionLock.ExitWriteLock();
                    }
                }
                return false;
            }
            finally
            {
                BiotaPropertiesPositionLock.ExitUpgradeableReadLock();
            }
        }

        public static bool TryRemoveProperty(this Biota biota, PropertyString property, out BiotaPropertiesString entity)
        {
            BiotaPropertiesStringLock.EnterUpgradeableReadLock();
            try
            {
                entity = biota.BiotaPropertiesString.FirstOrDefault(x => x.Type == (uint)property);
                if (entity != null)
                {
                    BiotaPropertiesStringLock.EnterWriteLock();
                    try
                    {
                        biota.BiotaPropertiesString.Remove(entity);
                        entity.Object = null;
                        return true;
                    }
                    finally
                    {
                        BiotaPropertiesStringLock.ExitWriteLock();
                    }
                }
                return false;
            }
            finally
            {
                BiotaPropertiesStringLock.ExitUpgradeableReadLock();
            }
        }
    }
}
