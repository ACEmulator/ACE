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

        public static string GetProperty(this Biota biota, PropertyString property)
        {
            return biota.BiotaPropertiesString.FirstOrDefault(x => x.Type == (uint)property)?.Value;
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
        // BiotaPropertiesSkill
        // =====================================
        public static BiotaPropertiesSkill GetSkill(this Biota biota, ushort type, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                return biota.BiotaPropertiesSkill.FirstOrDefault(x => x.Type == type);
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }


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
    }
}
