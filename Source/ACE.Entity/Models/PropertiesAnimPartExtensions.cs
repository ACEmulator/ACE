using System;
using System.Collections.Generic;
using System.Threading;

namespace ACE.Entity.Models
{
    public static class PropertiesAnimPartExtensions
    {
        public static int GetCount(this IList<PropertiesAnimPart> value, Object rwLock)
        {
            if (value == null)
                return 0;

            lock (rwLock)
            {
                return value.Count;
            }
        }

        public static List<PropertiesAnimPart> Clone(this IList<PropertiesAnimPart> value, Object rwLock)
        {
            if (value == null)
                return null;

            lock (rwLock)
            {
                return new List<PropertiesAnimPart>(value);
            }
        }

        public static void CopyTo(this IList<PropertiesAnimPart> value, ICollection<PropertiesAnimPart> destination, Object rwLock)
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
