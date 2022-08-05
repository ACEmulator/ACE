using System;
using System.Collections.Generic;
using System.Threading;

namespace ACE.Entity.Models
{
    public static class PropertiesTextureMapExtensions
    {
        public static int GetCount(this IList<PropertiesTextureMap> value, ReaderWriterLockSlim rwLock)
        {
            if (value == null)
                return 0;

            rwLock.EnterReadLock();
            try
            {
                return value.Count;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static List<PropertiesTextureMap> Clone(this IList<PropertiesTextureMap> value, ReaderWriterLockSlim rwLock)
        {
            if (value == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                return new List<PropertiesTextureMap>(value);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static void CopyTo(this IList<PropertiesTextureMap> value, ICollection<PropertiesTextureMap> destination, ReaderWriterLockSlim rwLock)
        {
            if (value == null)
                return;

            rwLock.EnterReadLock();
            try
            {
                foreach (var entry in value)
                    destination.Add(entry);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }
    }
}
