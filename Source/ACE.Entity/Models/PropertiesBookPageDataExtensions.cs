using System;
using System.Collections.Generic;
using System.Threading;

namespace ACE.Entity.Models
{
    public static class PropertiesBookPageDataExtensions
    {
        public static int GetPageCount(this IList<PropertiesBookPageData> value, ReaderWriterLockSlim rwLock)
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

        public static List<PropertiesBookPageData> Clone(this IList<PropertiesBookPageData> value, ReaderWriterLockSlim rwLock)
        {
            if (value == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                return new List<PropertiesBookPageData>(value);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }


        public static PropertiesBookPageData GetPage(this IList<PropertiesBookPageData> value, int index, ReaderWriterLockSlim rwLock)
        {
            if (value == null)
                return null;

            rwLock.EnterReadLock();
            try
            {
                if (value.Count <= index)
                    return null;

                return value[index];
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static void AddPage(this IList<PropertiesBookPageData> value, PropertiesBookPageData page, out int index, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            try
            {
                value.Add(page);

                index = value.Count;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static bool RemovePage(this IList<PropertiesBookPageData> value, int index, ReaderWriterLockSlim rwLock)
        {
            if (value == null)
                return false;

            rwLock.EnterWriteLock();
            try
            {
                if (value.Count <= index)
                    return false;

                value.RemoveAt(index);

                return true;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }
    }
}
