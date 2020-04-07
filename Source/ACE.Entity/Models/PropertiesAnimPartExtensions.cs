using System;
using System.Collections.Generic;
using System.Threading;

namespace ACE.Entity.Models
{
    public static class PropertiesAnimPartExtensions
    {
        public static int GetCount(this IList<PropertiesAnimPart> value, ReaderWriterLockSlim rwLock)
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

        public static List<PropertiesAnimPart> Clone(this IList<PropertiesAnimPart> value, ReaderWriterLockSlim rwLock)
        {
            if (value == null)
                return null;

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

        public static void CopyTo(this IList<PropertiesAnimPart> value, ICollection<PropertiesAnimPart> destination, ReaderWriterLockSlim rwLock)
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
