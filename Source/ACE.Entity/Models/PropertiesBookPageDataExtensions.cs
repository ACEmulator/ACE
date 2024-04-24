using System;
using System.Collections.Generic;
using System.Threading;

namespace ACE.Entity.Models
{
    public static class PropertiesBookPageDataExtensions
    {
        public static int GetPageCount(this IList<PropertiesBookPageData> value, Object rwLock)
        {
            if (value == null)
                return 0;

            lock (rwLock)
            {
                return value.Count;
            }
        }

        public static List<PropertiesBookPageData> Clone(this IList<PropertiesBookPageData> value, Object rwLock)
        {
            if (value == null)
                return null;

            lock (rwLock)
            {
                return new List<PropertiesBookPageData>(value);
            }
        }


        public static PropertiesBookPageData GetPage(this IList<PropertiesBookPageData> value, int index, Object rwLock)
        {
            if (value == null)
                return null;

            lock (rwLock)
            {
                if (value.Count <= index)
                    return null;

                return value[index];
            }
        }

        public static void AddPage(this IList<PropertiesBookPageData> value, PropertiesBookPageData page, out int index, Object rwLock)
        {
            lock (rwLock)
            {
                value.Add(page);

                index = value.Count;
            }
        }

        public static bool RemovePage(this IList<PropertiesBookPageData> value, int index, Object rwLock)
        {
            if (value == null)
                return false;

            lock (rwLock)
            {
                if (value.Count <= index)
                    return false;

                value.RemoveAt(index);

                return true;
            }
        }
    }
}
