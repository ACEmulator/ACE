using System;
using System.Collections.Generic;
using System.Threading;

namespace ACE.Entity.Models
{
    public static class PropertiesPaletteExtensions
    {
        public static int GetCount(this IList<PropertiesPalette> value, Object rwLock)
        {
            if (value == null)
                return 0;

            lock (rwLock)
            {
                return value.Count;
            }
        }

        public static List<PropertiesPalette> Clone(this IList<PropertiesPalette> value, Object rwLock)
        {
            if (value == null)
                return null;

            lock (rwLock)
            {
                return new List<PropertiesPalette>(value);
            }
        }

        public static void CopyTo(this IList<PropertiesPalette> value, ICollection<PropertiesPalette> destination, Object rwLock)
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
