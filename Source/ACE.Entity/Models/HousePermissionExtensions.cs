using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ACE.Entity.Models
{
    public static class HousePermissionExtensions
    {
        public static List<HousePermission> Clone(this ICollection<HousePermission> value, ReaderWriterLockSlim rwLock)
        {
            if (value == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                return new List<HousePermission>(value);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }


        public static HousePermission FindByPlayerGuid(this ICollection<HousePermission> value, uint playerGuid, ReaderWriterLockSlim rwLock)
        {
            if (value == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                return value.FirstOrDefault(r => r.PlayerGuid == playerGuid);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static void Add(this ICollection<HousePermission> value, HousePermission housePermission, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            try
            {
                value.Add(housePermission);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static bool TryRemove(this ICollection<HousePermission> value, uint playerGuid, ReaderWriterLockSlim rwLock)
        {
            if (value == null)
                return false;

            rwLock.EnterWriteLock();
            try
            {
                var entity = value.FirstOrDefault(x => x.PlayerGuid == playerGuid);

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
