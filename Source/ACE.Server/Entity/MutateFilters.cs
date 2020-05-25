using System;
using System.Collections.Generic;

using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public static class MutateFilters
    {
        // mutate filters are the 0x0E records
        // only 6 mutate filter ids total in 16py?

        private readonly static Dictionary<uint, MutateFilter> Filters = new Dictionary<uint, MutateFilter>();

        static MutateFilters()
        {
            // armor
            Filters.Add(0x0E000012, MutateFilter.Base | MutateFilter.ArmorModVsType | MutateFilter.EncumbranceVal);

            // shields
            Filters.Add(0x0E000013, MutateFilter.Base | MutateFilter.ArmorModVsType | MutateFilter.ShieldValue);

            // weapons - exactly the same?
            Filters.Add(0x0E000014, MutateFilter.Base | MutateFilter.WeaponTime);
            Filters.Add(0x0E000015, MutateFilter.Base | MutateFilter.WeaponTime);

            Filters.Add(0x0E000016, MutateFilter.Base);

            Filters.Add(0x0E00001D, MutateFilter.Base | MutateFilter.WeaponTime);
        }

        public static MutateFilter GetMutateFilters(this WorldObject wo)
        {
            if (wo.MutateFilter == null)
                return MutateFilter.Undef;

            if (!Filters.TryGetValue(wo.MutateFilter.Value, out var filters))
            {
                //Console.WriteLine($"Unknown MutateFilter {wo.MutateFilter:X8} on {wo.Name} ({wo.WeenieClassId})");
                return MutateFilter.Undef;
            }
            return filters;
        }

        public static bool HasMutateFilter(this WorldObject wo, MutateFilter filter)
        {
            if (wo.MutateFilter == null)
                return false;

            if (Filters.TryGetValue(wo.MutateFilter.Value, out var filters))
                return filters.HasFlag(filter);
            else
                return false;
        }
    }
}
