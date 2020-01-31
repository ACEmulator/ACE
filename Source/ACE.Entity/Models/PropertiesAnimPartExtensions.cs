using System;
using System.Collections.Generic;
using System.Threading;

namespace ACE.Entity.Models
{
    public static class PropertiesAnimPartExtensions
    {
        public static int GetCount(this IList<PropertiesAnimPart> value, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                if (value == null)
                    return 0;

                return value.Count;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static List<PropertiesAnimPart> Clone(this IList<PropertiesAnimPart> value, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return new List<PropertiesAnimPart>(value);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }


        public static void Add(this IList<PropertiesAnimPart> value, IList<PropertiesAnimPart> entries, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            try
            {
                foreach (var entry in entries)
                    value.Add(entry);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }
    }
}
