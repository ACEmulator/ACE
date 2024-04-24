using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ACE.Entity.Models
{
    public static class PropertiesAllegianceExtensions
    {
        public static Dictionary<uint, PropertiesAllegiance> GetApprovedVassals(this IDictionary<uint, PropertiesAllegiance> value, Object rwLock)
        {
            lock (rwLock)
            {
                if (value == null)
                    return new Dictionary<uint, PropertiesAllegiance>();

                return value.Where(i => i.Value.ApprovedVassal).ToDictionary(i => i.Key, i => i.Value);
            }
        }

        public static Dictionary<uint, PropertiesAllegiance> GetBanList(this IDictionary<uint, PropertiesAllegiance> value, Object rwLock)
        {
            if (value == null)
                return new Dictionary<uint, PropertiesAllegiance>();

            lock (rwLock)
            {
                return value.Where(i => i.Value.Banned).ToDictionary(i => i.Key, i => i.Value);
            }
        }

        public static PropertiesAllegiance GetFirstOrDefaultByCharacterId(this IDictionary<uint, PropertiesAllegiance> value, uint characterId, Object rwLock)
        {
            if (value == null)
                return null;

            lock (rwLock)
            {
                value.TryGetValue(characterId, out var entity);

                return entity;
            }
        }

        public static void AddOrUpdateAllegiance(this IDictionary<uint, PropertiesAllegiance> value, uint characterId, bool isBanned, bool approvedVassal, Object rwLock)
        {
            lock (rwLock)
            {
                if (!value.TryGetValue(characterId, out var entity))
                {
                    entity = new PropertiesAllegiance { Banned = isBanned, ApprovedVassal = approvedVassal };

                    value.Add(characterId, entity);
                }

                entity.Banned = isBanned;
                entity.ApprovedVassal = approvedVassal;
            }
        }

        public static bool TryRemoveAllegiance(this IDictionary<uint, PropertiesAllegiance> value, uint characterId, Object rwLock)
        {
            if (value == null)
                return false;

            lock (rwLock)
            {
                return value.Remove(characterId);
            }
        }
    }
}
