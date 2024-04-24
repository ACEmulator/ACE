using System;
using System.Collections.Generic;
using System.Threading;

namespace ACE.Entity.Models
{
    public static class PropertiesTextureMapExtensions
    {
        public static int GetCount(this IList<PropertiesTextureMap> value, Object rwLock)
        {
            if (value == null)
                return 0;

            lock (rwLock)
            {
                return value.Count;
            }
        }

        public static List<PropertiesTextureMap> Clone(this IList<PropertiesTextureMap> value, Object rwLock)
        {
            if (value == null)
                return null;

            lock (rwLock)
            {
                return new List<PropertiesTextureMap>(value);
            }
        }

        public static void CopyTo(this IList<PropertiesTextureMap> value, ICollection<PropertiesTextureMap> destination, Object rwLock)
        {
            if (value == null)
                return;

            lock (rwLock)
            {
                foreach (var entry in value)
                    destination.Add(entry);
            }
        }
    }
}
