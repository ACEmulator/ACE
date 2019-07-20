using System;
using System.Collections.Generic;
using System.Linq;

namespace ACE.Common.Extensions
{
    public static class EnumHelper
    {
        /// <summary>
        /// Returns a list of flags for enum
        /// </summary>
        public static List<Enum> GetFlags(this Enum e)
        {
            return Enum.GetValues(e.GetType()).Cast<Enum>().Where(e.HasFlag).ToList();
        }

        /// <summary>
        /// Returns the # of bits set in a Flags enum
        /// </summary>
        /// <param name="enumVal">The enum uint value</param>
        public static int NumFlags(uint enumVal)
        {
            var cnt = 0;
            while (enumVal != 0)
            {
                // remove the next set bit
                enumVal &= enumVal - 1;
                cnt++;
            }
            return cnt;
        }

        /// <summary>
        /// Returns TRUE if this flags enum has multiple flags set
        /// </summary>
        /// <param name="enumVal">The enum uint value</param>
        public static bool HasMultiple(uint enumVal)
        {
            return (enumVal & (enumVal - 1)) != 0;
        }
    }
}
