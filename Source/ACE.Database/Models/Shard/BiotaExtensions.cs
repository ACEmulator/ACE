using System.Collections.Generic;
using System.Linq;
using System.Threading;

using ACE.Entity;
using ACE.Entity.Enum.Properties;

namespace ACE.Database.Models.Shard
{
    public static class BiotaExtensions
    {
        // =====================================
        // Get
        // Bool, DID, Float, IID, Int, Int64, Position, String
        // =====================================

        public static bool? GetProperty(this Biota biota, PropertyBool property, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return biota.BiotaPropertiesBool.FirstOrDefault(x => x.Type == (uint)property)?.Value;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
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

        public static Dictionary<PositionType, Position> GetPositions(this Biota biota, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                var results = new Dictionary<PositionType, Position>();

                foreach (var value in biota.BiotaPropertiesPosition)
                {
                    var position = new Position(value.ObjCellId, value.OriginX, value.OriginY, value.OriginZ, value.AnglesX, value.AnglesY, value.AnglesZ, value.AnglesW);

                    results.Add((PositionType)value.PositionType, position);
                }

                return results;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
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


        // =====================================
        // Set
        // Bool, DID, Float, IID, Int, Int64, Position, String
        // =====================================

        public static void SetProperty(this Biota biota, PropertyBool property, bool value, ReaderWriterLockSlim rwLock, out bool biotaChanged)
        {
            rwLock.EnterWriteLock();
            try
            {
                var result = biota.BiotaPropertiesBool.FirstOrDefault(x => x.Type == (uint)property);
                if (result != null)
                {
                    biotaChanged = (result.Value != value);
                    result.Value = value;
                }
                else
                {
                    var entity = new BiotaPropertiesBool { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                    biota.BiotaPropertiesBool.Add(entity);
                    biotaChanged = true;
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyBool property, bool value, ReaderWriterLockSlim rwLock, IDictionary<PropertyBool, BiotaPropertiesBool> cache, out bool biotaChanged)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (cache.TryGetValue(property, out var record))
                {
                    biotaChanged = (record.Value != value);
                    record.Value = value;
                }
                else
                {
                    var entity = new BiotaPropertiesBool { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                    biota.BiotaPropertiesBool.Add(entity);

                    cache[property] = entity;

                    biotaChanged = true;
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyDataId property, uint value, ReaderWriterLockSlim rwLock, out bool biotaChanged)
        {
            rwLock.EnterWriteLock();
            try
            {
                var result = biota.BiotaPropertiesDID.FirstOrDefault(x => x.Type == (uint)property);
                if (result != null)
                {
                    biotaChanged = (result.Value != value);
                    result.Value = value;
                }
                else
                {
                    var entity = new BiotaPropertiesDID { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                    biota.BiotaPropertiesDID.Add(entity);
                    biotaChanged = true;
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyDataId property, uint value, ReaderWriterLockSlim rwLock, IDictionary<PropertyDataId, BiotaPropertiesDID> cache, out bool biotaChanged)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (cache.TryGetValue(property, out var record))
                {
                    biotaChanged = (record.Value != value);
                    record.Value = value;
                }
                else
                {
                    var entity = new BiotaPropertiesDID { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                    biota.BiotaPropertiesDID.Add(entity);

                    cache[property] = entity;

                    biotaChanged = true;
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyFloat property, double value, ReaderWriterLockSlim rwLock, out bool biotaChanged)
        {
            rwLock.EnterWriteLock();
            try
            {
                var result = biota.BiotaPropertiesFloat.FirstOrDefault(x => x.Type == (ushort)property);
                if (result != null)
                {
                    biotaChanged = (result.Value != value);
                    result.Value = value;
                }
                else
                {
                    var entity = new BiotaPropertiesFloat { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                    biota.BiotaPropertiesFloat.Add(entity);
                    biotaChanged = true;
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyFloat property, double value, ReaderWriterLockSlim rwLock, IDictionary<PropertyFloat, BiotaPropertiesFloat> cache, out bool biotaChanged)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (cache.TryGetValue(property, out var record))
                {
                    biotaChanged = (record.Value != value);
                    record.Value = value;
                }
                else
                {
                    var entity = new BiotaPropertiesFloat { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                    biota.BiotaPropertiesFloat.Add(entity);

                    cache[property] = entity;

                    biotaChanged = true;
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyInstanceId property, uint value, ReaderWriterLockSlim rwLock, out bool biotaChanged)
        {
            rwLock.EnterWriteLock();
            try
            {
                var result = biota.BiotaPropertiesIID.FirstOrDefault(x => x.Type == (uint)property);
                if (result != null)
                {
                    biotaChanged = (result.Value != value);
                    result.Value = value;
                }
                else
                {
                    var entity = new BiotaPropertiesIID { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                    biota.BiotaPropertiesIID.Add(entity);
                    biotaChanged = true;
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyInstanceId property, uint value, ReaderWriterLockSlim rwLock, IDictionary<PropertyInstanceId, BiotaPropertiesIID> cache, out bool biotaChanged)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (cache.TryGetValue(property, out var record))
                {
                    biotaChanged = (record.Value != value);
                    record.Value = value;
                }
                else
                {
                    var entity = new BiotaPropertiesIID { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                    biota.BiotaPropertiesIID.Add(entity);

                    cache[property] = entity;

                    biotaChanged = true;
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyInt property, int value, ReaderWriterLockSlim rwLock, out bool biotaChanged)
        {
            rwLock.EnterWriteLock();
            try
            {
                var result = biota.BiotaPropertiesInt.FirstOrDefault(x => x.Type == (uint)property);
                if (result != null)
                {
                    biotaChanged = (result.Value != value);
                    result.Value = value;
                }
                else
                {
                    var entity = new BiotaPropertiesInt { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                    biota.BiotaPropertiesInt.Add(entity);
                    biotaChanged = true;
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyInt property, int value, ReaderWriterLockSlim rwLock, IDictionary<PropertyInt, BiotaPropertiesInt> cache, out bool biotaChanged)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (cache.TryGetValue(property, out var record))
                {
                    biotaChanged = (record.Value != value);
                    record.Value = value;
                }
                else
                {
                    var entity = new BiotaPropertiesInt { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                    biota.BiotaPropertiesInt.Add(entity);

                    cache[property] = entity;

                    biotaChanged = true;
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyInt64 property, long value, ReaderWriterLockSlim rwLock, out bool biotaChanged)
        {
            rwLock.EnterWriteLock();
            try
            {
                var result = biota.BiotaPropertiesInt64.FirstOrDefault(x => x.Type == (uint)property);
                if (result != null)
                {
                    biotaChanged = (result.Value != value);
                    result.Value = value;
                }
                else
                {
                    var entity = new BiotaPropertiesInt64 { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                    biota.BiotaPropertiesInt64.Add(entity);
                    biotaChanged = true;
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyInt64 property, long value, ReaderWriterLockSlim rwLock, IDictionary<PropertyInt64, BiotaPropertiesInt64> cache, out bool biotaChanged)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (cache.TryGetValue(property, out var record))
                {
                    biotaChanged = (record.Value != value);
                    record.Value = value;
                }
                else
                {
                    var entity = new BiotaPropertiesInt64 { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                    biota.BiotaPropertiesInt64.Add(entity);

                    cache[property] = entity;

                    biotaChanged = true;
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void SetPosition(this Biota biota, PositionType positionType, Position position, ReaderWriterLockSlim rwLock, out bool biotaChanged)
        {
            rwLock.EnterWriteLock();
            try
            {
                var result = biota.BiotaPropertiesPosition.FirstOrDefault(x => x.PositionType == (uint)positionType);
                if (result != null)
                {
                    biotaChanged = true; // we just assume at least one of the values changed...
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
                    var entity = new BiotaPropertiesPosition { ObjectId = biota.Id, PositionType = (ushort)positionType, ObjCellId = position.Cell, OriginX = position.PositionX, OriginY = position.PositionY, OriginZ = position.PositionZ, AnglesW = position.RotationW, AnglesX = position.RotationX, AnglesY = position.RotationY, AnglesZ = position.RotationZ, Object = biota };
                    biota.BiotaPropertiesPosition.Add(entity);
                    biotaChanged = true;
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyString property, string value, ReaderWriterLockSlim rwLock, out bool biotaChanged)
        {
            rwLock.EnterWriteLock();
            try
            {
                var result = biota.BiotaPropertiesString.FirstOrDefault(x => x.Type == (uint)property);
                if (result != null)
                {
                    biotaChanged = (result.Value != value);
                    result.Value = value;
                }
                else
                {
                    var entity = new BiotaPropertiesString { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                    biota.BiotaPropertiesString.Add(entity);
                    biotaChanged = true;
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void SetProperty(this Biota biota, PropertyString property, string value, ReaderWriterLockSlim rwLock, IDictionary<PropertyString, BiotaPropertiesString> cache, out bool biotaChanged)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (cache.TryGetValue(property, out var record))
                {
                    biotaChanged = (record.Value != value);
                    record.Value = value;
                }
                else
                {
                    var entity = new BiotaPropertiesString { ObjectId = biota.Id, Type = (ushort)property, Value = value, Object = biota };
                    biota.BiotaPropertiesString.Add(entity);

                    cache[property] = entity;

                    biotaChanged = true;
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }


        // =====================================
        // Remove
        // Bool, DID, Float, IID, Int, Int64, Position, String
        // =====================================

        public static bool TryRemoveProperty(this Biota biota, PropertyBool property, out BiotaPropertiesBool entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                entity = biota.BiotaPropertiesBool.FirstOrDefault(x => x.Type == (uint)property);
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

        public static bool TryRemoveProperty(this Biota biota, PropertyBool property, ReaderWriterLockSlim rwLock, IDictionary<PropertyBool, BiotaPropertiesBool> cache)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                if (cache.ContainsKey(property))
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        var entity = biota.BiotaPropertiesBool.FirstOrDefault(x => x.Type == (uint)property);

                        biota.BiotaPropertiesBool.Remove(entity);
                        entity.Object = null;

                        cache.Remove(property);

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

        public static bool TryRemoveProperty(this Biota biota, PropertyDataId property, ReaderWriterLockSlim rwLock, IDictionary<PropertyDataId, BiotaPropertiesDID> cache)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                if (cache.ContainsKey(property))
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        var entity = biota.BiotaPropertiesDID.FirstOrDefault(x => x.Type == (uint)property);

                        biota.BiotaPropertiesDID.Remove(entity);
                        entity.Object = null;

                        cache.Remove(property);

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

        public static bool TryRemoveProperty(this Biota biota, PropertyFloat property, ReaderWriterLockSlim rwLock, IDictionary<PropertyFloat, BiotaPropertiesFloat> cache)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                if (cache.ContainsKey(property))
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        var entity = biota.BiotaPropertiesFloat.FirstOrDefault(x => x.Type == (uint)property);

                        biota.BiotaPropertiesFloat.Remove(entity);
                        entity.Object = null;

                        cache.Remove(property);

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
                entity = biota.BiotaPropertiesIID.FirstOrDefault(x => x.Type == (uint)property);
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

        public static bool TryRemoveProperty(this Biota biota, PropertyInstanceId property, ReaderWriterLockSlim rwLock, IDictionary<PropertyInstanceId, BiotaPropertiesIID> cache)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                if (cache.ContainsKey(property))
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        var entity = biota.BiotaPropertiesIID.FirstOrDefault(x => x.Type == (uint)property);

                        biota.BiotaPropertiesIID.Remove(entity);
                        entity.Object = null;

                        cache.Remove(property);

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

        public static bool TryRemoveProperty(this Biota biota, PropertyInt property, ReaderWriterLockSlim rwLock, IDictionary<PropertyInt, BiotaPropertiesInt> cache)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                if (cache.ContainsKey(property))
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        var entity = biota.BiotaPropertiesInt.FirstOrDefault(x => x.Type == (uint)property);

                        biota.BiotaPropertiesInt.Remove(entity);
                        entity.Object = null;

                        cache.Remove(property);

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

        public static bool TryRemoveProperty(this Biota biota, PropertyInt64 property, ReaderWriterLockSlim rwLock, IDictionary<PropertyInt64, BiotaPropertiesInt64> cache)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                if (cache.ContainsKey(property))
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        var entity = biota.BiotaPropertiesInt64.FirstOrDefault(x => x.Type == (uint)property);

                        biota.BiotaPropertiesInt64.Remove(entity);
                        entity.Object = null;

                        cache.Remove(property);

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

        public static bool TryRemovePosition(this Biota biota, PositionType positionType, out BiotaPropertiesPosition entity, ReaderWriterLockSlim rwLock)
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

        public static bool TryRemoveProperty(this Biota biota, PropertyString property, ReaderWriterLockSlim rwLock, IDictionary<PropertyString, BiotaPropertiesString> cache)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                if (cache.ContainsKey(property))
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        var entity = biota.BiotaPropertiesString.FirstOrDefault(x => x.Type == (uint)property);

                        biota.BiotaPropertiesString.Remove(entity);
                        entity.Object = null;

                        cache.Remove(property);

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


        // =====================================
        // BiotaPropertiesEnchantmentRegistry
        // =====================================

        public static bool HasEnchantments(this Biota biota, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return biota.BiotaPropertiesEnchantmentRegistry.Any();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static bool HasEnchantment(this Biota biota, uint spellId, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return biota.BiotaPropertiesEnchantmentRegistry.Any(e => e.SpellId == spellId);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static BiotaPropertiesEnchantmentRegistry GetEnchantmentBySpell(this Biota biota, int spellId, uint? casterGuid, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                var results = biota.BiotaPropertiesEnchantmentRegistry.Where(e => e.SpellId == spellId);

                if (casterGuid != null)
                    results = results.Where(e => e.CasterObjectId == casterGuid);

                return results.FirstOrDefault();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static List<BiotaPropertiesEnchantmentRegistry> GetEnchantments(this Biota biota, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return biota.BiotaPropertiesEnchantmentRegistry.ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static List<BiotaPropertiesEnchantmentRegistry> GetEnchantmentsByCategory(this Biota biota, ushort spellCategory, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return biota.BiotaPropertiesEnchantmentRegistry.Where(e => e.SpellCategory == spellCategory).ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static List<BiotaPropertiesEnchantmentRegistry> GetEnchantmentsByStatModType(this Biota biota, uint statModType, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return biota.BiotaPropertiesEnchantmentRegistry.Where(e => (e.StatModType & statModType) == statModType).ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static void AddEnchantment(this Biota biota, BiotaPropertiesEnchantmentRegistry entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            try
            {
                biota.BiotaPropertiesEnchantmentRegistry.Add(entity);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static bool TryRemoveEnchantment(this Biota biota, BiotaPropertiesEnchantmentRegistry entry, out BiotaPropertiesEnchantmentRegistry entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                entity = biota.BiotaPropertiesEnchantmentRegistry.FirstOrDefault(x => x.SpellId == entry.SpellId && x.CasterObjectId == entry.CasterObjectId);
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

        public static void RemoveAllEnchantments(this Biota biota, ICollection<int> spellsToExclude, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            try
            {
               var enchantments = biota.BiotaPropertiesEnchantmentRegistry.Where(e => !spellsToExclude.Contains(e.SpellId)).ToList();

                foreach (var enchantment in enchantments)
                    biota.BiotaPropertiesEnchantmentRegistry.Remove(enchantment);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }


        // =====================================
        // BiotaPropertiesSkill
        // =====================================

        public static BiotaPropertiesSkill GetOrAddSkill(this Biota biota, ushort type, ReaderWriterLockSlim rwLock, out bool skillAdded)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                var entity = biota.BiotaPropertiesSkill.FirstOrDefault(x => x.Type == type);
                if (entity != null)
                {
                    skillAdded = false;
                    return entity;
                }

                rwLock.EnterWriteLock();
                try
                {
                    entity = new BiotaPropertiesSkill { ObjectId = biota.Id, Type = type, Object = biota };
                    biota.BiotaPropertiesSkill.Add(entity);
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

        // =====================================
        // BiotaPropertiesBook
        // =====================================

        public static BiotaPropertiesBookPageData GetBookPageData(this Biota biota, uint bookGuid, uint pageId, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return biota.BiotaPropertiesBookPageData.FirstOrDefault(i => i.ObjectId == bookGuid && i.PageId == pageId);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static List<BiotaPropertiesBookPageData> GetBookAllPages(this Biota biota, uint bookGuid, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return biota.BiotaPropertiesBookPageData.Where(i => i.ObjectId == bookGuid).ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static BiotaPropertiesBookPageData AddBookPage(this Biota biota, BiotaPropertiesBookPageData page, ReaderWriterLockSlim rwLock, out bool alreadyExists)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                var entity = biota.BiotaPropertiesBookPageData.FirstOrDefault(i => i.PageId == page.PageId);
                if (entity != null)
                {
                    alreadyExists = true;
                    return entity;
                }

                rwLock.EnterWriteLock();
                try
                {
                    biota.BiotaPropertiesBookPageData.Add(page);
                    alreadyExists = false;
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

        public static bool DeleteBookPage(this Biota biota, uint pageId, out BiotaPropertiesBookPageData entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                entity = biota.BiotaPropertiesBookPageData.FirstOrDefault(i => i.PageId == pageId);
                if (entity != null)
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        biota.BiotaPropertiesBookPageData.Remove(entity);
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


        // =====================================
        // BiotaPropertiesSpellBook
        // =====================================

        public static bool SpellIsKnown(this Biota biota, int spell, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return biota.BiotaPropertiesSpellBook.FirstOrDefault(x => x.Spell == spell) != null;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static BiotaPropertiesSpellBook GetOrAddKnownSpell(this Biota biota, int spell, ReaderWriterLockSlim rwLock, out bool spellAdded)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                var entity = biota.BiotaPropertiesSpellBook.FirstOrDefault(x => x.Spell == spell);
                if (entity != null)
                {
                    spellAdded = false;
                    return entity;
                }

                rwLock.EnterWriteLock();
                try
                {
                    entity = new BiotaPropertiesSpellBook { ObjectId = biota.Id, Spell = spell, Object = biota };
                    biota.BiotaPropertiesSpellBook.Add(entity);
                    spellAdded = true;
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

        public static bool TryRemoveKnownSpell(this Biota biota, int spell, out BiotaPropertiesSpellBook entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                entity = biota.BiotaPropertiesSpellBook.FirstOrDefault(x => x.Spell == spell);
                if (entity != null)
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        biota.BiotaPropertiesSpellBook.Remove(entity);
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

        public static List<HousePermission> GetHousePermission(this Biota biota, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return biota.HousePermission.ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static void AddHousePermission(this Biota biota, HousePermission entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            try
            {
                biota.HousePermission.Add(entity);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static bool TryRemoveHousePermission(this Biota biota, uint playerGuid, out HousePermission entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                entity = biota.HousePermission.FirstOrDefault(x => x.PlayerGuid == playerGuid);
                if (entity != null)
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        biota.HousePermission.Remove(entity);
                        entity.House = null;
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


        // =====================================
        // Clone
        // =====================================

        public static Biota Clone(this Biota biota)
        {
            var result = new Biota();

            result.Id = biota.Id;
            result.WeenieClassId = biota.WeenieClassId;
            result.WeenieType = biota.WeenieType;
            result.PopulatedCollectionFlags = biota.PopulatedCollectionFlags;

            if (biota.BiotaPropertiesBook != null)
            {
                result.BiotaPropertiesBook = new BiotaPropertiesBook();
                result.BiotaPropertiesBook.Id = biota.BiotaPropertiesBook.Id;
                result.BiotaPropertiesBook.ObjectId = biota.BiotaPropertiesBook.ObjectId;
                result.BiotaPropertiesBook.MaxNumPages = biota.BiotaPropertiesBook.MaxNumPages;
                result.BiotaPropertiesBook.MaxNumCharsPerPage = biota.BiotaPropertiesBook.MaxNumCharsPerPage;
            }

            foreach (var value in biota.BiotaPropertiesAnimPart)
            {
                result.BiotaPropertiesAnimPart.Add(new BiotaPropertiesAnimPart
                {
                    Id = value.Id,
                    ObjectId = value.Id,
                    Index = value.Index,
                    AnimationId = value.AnimationId,
                    Order = value.Order,
                });
            }

            foreach (var value in biota.BiotaPropertiesAttribute)
            {
                result.BiotaPropertiesAttribute.Add(new BiotaPropertiesAttribute
                {
                    Id = value.Id,
                    ObjectId = value.ObjectId,
                    Type = value.Type,
                    InitLevel = value.InitLevel,
                    LevelFromCP = value.LevelFromCP,
                    CPSpent = value.CPSpent,
                });
            }

            foreach (var value in biota.BiotaPropertiesAttribute2nd)
            {
                result.BiotaPropertiesAttribute2nd.Add(new BiotaPropertiesAttribute2nd
                {
                    Id = value.Id,
                    ObjectId = value.ObjectId,
                    Type = value.Type,
                    InitLevel = value.InitLevel,
                    LevelFromCP = value.LevelFromCP,
                    CPSpent = value.CPSpent,
                    CurrentLevel = value.CurrentLevel,
                });
            }

            foreach (var value in biota.BiotaPropertiesBodyPart)
            {
                result.BiotaPropertiesBodyPart.Add(new BiotaPropertiesBodyPart
                {
                    Id = value.Id,
                    ObjectId = value.ObjectId,
                    Key = value.Key,
                    DType = value.DType,
                    DVal = value.DVal,
                    DVar = value.DVar,
                    BaseArmor = value.BaseArmor,
                    ArmorVsSlash = value.ArmorVsSlash,
                    ArmorVsPierce = value.ArmorVsPierce,
                    ArmorVsBludgeon = value.ArmorVsBludgeon,
                    ArmorVsCold = value.ArmorVsCold,
                    ArmorVsFire = value.ArmorVsFire,
                    ArmorVsAcid = value.ArmorVsAcid,
                    ArmorVsElectric = value.ArmorVsElectric,
                    ArmorVsNether = value.ArmorVsNether,
                    BH = value.BH,
                    HLF = value.HLF,
                    MLF = value.MLF,
                    LLF = value.LLF,
                    HRF = value.HRF,
                    MRF = value.MRF,
                    LRF = value.LRF,
                    HLB = value.HLB,
                    MLB = value.MLB,
                    LLB = value.LLB,
                    HRB = value.HRB,
                    MRB = value.MRB,
                    LRB = value.LRB,
                });
            }

            foreach (var value in biota.BiotaPropertiesBookPageData)
            {
                result.BiotaPropertiesBookPageData.Add(new BiotaPropertiesBookPageData
                {
                    Id = value.Id,
                    ObjectId = value.ObjectId,
                    PageId = value.PageId,
                    AuthorId = value.AuthorId,
                    AuthorName = value.AuthorName,
                    AuthorAccount = value.AuthorAccount,
                    IgnoreAuthor = value.IgnoreAuthor,
                    PageText = value.PageText,
                });
            }

            foreach (var value in biota.BiotaPropertiesBool)
            {
                result.BiotaPropertiesBool.Add(new BiotaPropertiesBool
                {
                    Id = value.Id,
                    ObjectId = value.ObjectId,
                    Type = value.Type,
                    Value = value.Value,
                });
            }

            foreach (var value in biota.BiotaPropertiesCreateList)
            {
                result.BiotaPropertiesCreateList.Add(new BiotaPropertiesCreateList
                {
                    Id = value.Id,
                    ObjectId = value.ObjectId,
                    DestinationType = value.DestinationType,
                    WeenieClassId = value.WeenieClassId,
                    StackSize = value.StackSize,
                    Palette = value.Palette,
                    Shade = value.Shade,
                    TryToBond = value.TryToBond,
                });
            }

            foreach (var value in biota.BiotaPropertiesDID)
            {
                result.BiotaPropertiesDID.Add(new BiotaPropertiesDID
                {
                    Id = value.Id,
                    ObjectId = value.ObjectId,
                    Type = value.Type,
                    Value = value.Value,
                });
            }


            foreach (var value in biota.BiotaPropertiesEmote)
            {
                var emote = new BiotaPropertiesEmote
                {
                    Id = value.Id,
                    ObjectId = value.ObjectId,
                    Category = value.Category,
                    Probability = value.Probability,
                    WeenieClassId = value.WeenieClassId,
                    Style = value.Style,
                    Substyle = value.Substyle,
                    Quest = value.Quest,
                    VendorType = value.VendorType,
                    MinHealth = value.MinHealth,
                    MaxHealth = value.MaxHealth,
                };

                foreach (var value2 in value.BiotaPropertiesEmoteAction)
                {
                    var action = new BiotaPropertiesEmoteAction
                    {
                        Id = value2.Id,
                        EmoteId = value2.EmoteId,
                        Order = value2.Order,
                        Type = value2.Type,
                        Delay = value2.Delay,
                        Extent = value2.Extent,
                        Motion = value2.Motion,
                        Message = value2.Message,
                        TestString = value2.TestString,
                        Min = value2.Min,
                        Max = value2.Max,
                        Min64 = value2.Min64,
                        Max64 = value2.Max64,
                        MinDbl = value2.MinDbl,
                        MaxDbl = value2.MaxDbl,
                        Stat = value2.Stat,
                        Display = value2.Display,
                        Amount = value2.Amount,
                        Amount64 = value2.Amount64,
                        HeroXP64 = value2.HeroXP64,
                        Percent = value2.Percent,
                        SpellId = value2.SpellId,
                        WealthRating = value2.WealthRating,
                        TreasureClass = value2.TreasureClass,
                        TreasureType = value2.TreasureType,
                        PScript = value2.PScript,
                        Sound = value2.Sound,
                        DestinationType = value2.DestinationType,
                        WeenieClassId = value2.WeenieClassId,
                        StackSize = value2.StackSize,
                        Palette = value2.Palette,
                        Shade = value2.Shade,
                        TryToBond = value2.TryToBond,
                        ObjCellId = value2.ObjCellId,
                        OriginX = value2.OriginX,
                        OriginY = value2.OriginY,
                        OriginZ = value2.OriginZ,
                        AnglesW = value2.AnglesW,
                        AnglesX = value2.AnglesX,
                        AnglesY = value2.AnglesY,
                        AnglesZ = value2.AnglesZ,
                    };

                    emote.BiotaPropertiesEmoteAction.Add(action);
                }

                result.BiotaPropertiesEmote.Add(emote);
            }


            foreach (var value in biota.BiotaPropertiesEnchantmentRegistry)
            {
                result.BiotaPropertiesEnchantmentRegistry.Add(new BiotaPropertiesEnchantmentRegistry
                {
                    //Id = value.Id,
                    ObjectId = value.ObjectId,
                    EnchantmentCategory = value.EnchantmentCategory,
                    SpellId = value.SpellId,
                    LayerId = value.LayerId,
                    HasSpellSetId = value.HasSpellSetId,
                    SpellCategory = value.SpellCategory,
                    PowerLevel = value.PowerLevel,
                    StartTime = value.StartTime,
                    Duration = value.Duration,
                    CasterObjectId = value.CasterObjectId,
                    DegradeModifier = value.DegradeModifier,
                    DegradeLimit = value.DegradeLimit,
                    LastTimeDegraded = value.LastTimeDegraded,
                    StatModType = value.StatModType,
                    StatModKey = value.StatModKey,
                    StatModValue = value.StatModValue,
                    SpellSetId = value.SpellSetId,
                });
            }


            foreach (var value in biota.BiotaPropertiesEventFilter)
            {
                result.BiotaPropertiesEventFilter.Add(new BiotaPropertiesEventFilter
                {
                    Id = value.Id,
                    ObjectId = value.ObjectId,
                    Event = value.Event,
                });
            }

            foreach (var value in biota.BiotaPropertiesFloat)
            {
                result.BiotaPropertiesFloat.Add(new BiotaPropertiesFloat
                {
                    Id = value.Id,
                    ObjectId = value.ObjectId,
                    Type = value.Type,
                    Value = value.Value,
                });
            }

            foreach (var value in biota.BiotaPropertiesGenerator)
            {
                result.BiotaPropertiesGenerator.Add(new BiotaPropertiesGenerator
                {
                    Id = value.Id,
                    ObjectId = value.ObjectId,
                    Probability = value.Probability,
                    WeenieClassId = value.WeenieClassId,
                    Delay = value.Delay,
                    InitCreate = value.InitCreate,
                    MaxCreate = value.MaxCreate,
                    WhenCreate = value.WhenCreate,
                    WhereCreate = value.WhereCreate,
                    StackSize = value.StackSize,
                    PaletteId = value.PaletteId,
                    Shade = value.Shade,
                    ObjCellId = value.ObjCellId,
                    OriginX = value.OriginX,
                    OriginY = value.OriginY,
                    OriginZ = value.OriginZ,
                    AnglesW = value.AnglesW,
                    AnglesX = value.AnglesX,
                    AnglesY = value.AnglesY,
                    AnglesZ = value.AnglesZ,
                });
            }

            foreach (var value in biota.BiotaPropertiesIID)
            {
                result.BiotaPropertiesIID.Add(new BiotaPropertiesIID
                {
                    Id = value.Id,
                    ObjectId = value.ObjectId,
                    Type = value.Type,
                    Value = value.Value,
                });
            }

            foreach (var value in biota.BiotaPropertiesInt)
            {
                result.BiotaPropertiesInt.Add(new BiotaPropertiesInt
                {
                    Id = value.Id,
                    ObjectId = value.ObjectId,
                    Type = value.Type,
                    Value = value.Value,
                });
            }

            foreach (var value in biota.BiotaPropertiesInt64)
            {
                result.BiotaPropertiesInt64.Add(new BiotaPropertiesInt64
                {
                    Id = value.Id,
                    ObjectId = value.ObjectId,
                    Type = value.Type,
                    Value = value.Value,
                });
            }

            foreach (var value in biota.BiotaPropertiesPalette)
            {
                result.BiotaPropertiesPalette.Add(new BiotaPropertiesPalette
                {
                    Id = value.Id,
                    ObjectId = value.ObjectId,
                    SubPaletteId = value.SubPaletteId,
                    Offset = value.Offset,
                    Length = value.Length,
                });
            }

            foreach (var value in biota.BiotaPropertiesPosition)
            {
                result.BiotaPropertiesPosition.Add(new BiotaPropertiesPosition
                {
                    Id = value.Id,
                    ObjectId = value.ObjectId,
                    PositionType = value.PositionType,
                    ObjCellId = value.ObjCellId,
                    OriginX = value.OriginX,
                    OriginY = value.OriginY,
                    OriginZ = value.OriginZ,
                    AnglesW = value.AnglesW,
                    AnglesX = value.AnglesX,
                    AnglesY = value.AnglesY,
                    AnglesZ = value.AnglesZ,
                });
            }

            foreach (var value in biota.BiotaPropertiesSkill)
            {
                result.BiotaPropertiesSkill.Add(new BiotaPropertiesSkill
                {
                    Id = value.Id,
                    ObjectId = value.ObjectId,
                    Type = value.Type,
                    LevelFromPP = value.LevelFromPP,
                    SAC = value.SAC,
                    PP = value.PP,
                    InitLevel = value.InitLevel,
                    ResistanceAtLastCheck = value.ResistanceAtLastCheck,
                    LastUsedTime = value.LastUsedTime,
                });
            }

            foreach (var value in biota.BiotaPropertiesSpellBook)
            {
                result.BiotaPropertiesSpellBook.Add(new BiotaPropertiesSpellBook
                {
                    Id = value.Id,
                    ObjectId = value.ObjectId,
                    Spell = value.Spell,
                    Probability = value.Probability,
                });
            }

            foreach (var value in biota.BiotaPropertiesString)
            {
                result.BiotaPropertiesString.Add(new BiotaPropertiesString
                {
                    Id = value.Id,
                    ObjectId = value.ObjectId,
                    Type = value.Type,
                    Value = value.Value,
                });
            }

            foreach (var value in biota.BiotaPropertiesTextureMap)
            {
                result.BiotaPropertiesTextureMap.Add(new BiotaPropertiesTextureMap
                {
                    Id = value.Id,
                    ObjectId = value.ObjectId,
                    Index = value.Index,
                    OldId = value.OldId,
                    NewId = value.NewId,
                    Order = value.Order,
                });
            }

            return result;
        }
    }
}
