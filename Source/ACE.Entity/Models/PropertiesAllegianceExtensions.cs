using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ACE.Entity.Models
{
    public static class PropertiesAllegianceExtensions
    {
        public static List<PropertiesAllegiance> GetApprovedVassals(this ICollection<PropertiesAllegiance> value, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                if (value == null)
                    return new List<PropertiesAllegiance>();

                return value.Where(i => i.ApprovedVassal).ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static List<PropertiesAllegiance> GetBanList(this ICollection<PropertiesAllegiance> value, ReaderWriterLockSlim rwLock)
        {
            if (value == null)
                return new List<PropertiesAllegiance>();

            rwLock.EnterReadLock();
            try
            {
                return value.Where(i => i.Banned).ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static PropertiesAllegiance GetFirstOrDefaultByCharacterId(this ICollection<PropertiesAllegiance> value, uint characterId, ReaderWriterLockSlim rwLock)
        {
            if (value == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                return value.FirstOrDefault(i => i.CharacterId == characterId);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static void AddOrUpdateAllegiance(this ICollection<PropertiesAllegiance> value, uint characterId, bool isBanned, bool approvedVassal, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            try
            {
                var entity = value.FirstOrDefault(x => x.CharacterId == characterId);

                if (entity == null)
                {
                    entity = new PropertiesAllegiance { CharacterId = characterId, Banned = isBanned, ApprovedVassal = approvedVassal };

                    value.Add(entity);
                }
                else
                {
                    entity.Banned = isBanned;
                    entity.ApprovedVassal = approvedVassal;
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static bool TryRemoveAllegiance(this ICollection<PropertiesAllegiance> value, uint characterId, ReaderWriterLockSlim rwLock)
        {
            if (value == null)
                return false;

            rwLock.EnterWriteLock();
            try
            {
                var entity = value.FirstOrDefault(x => x.CharacterId == characterId);

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
    }
}
