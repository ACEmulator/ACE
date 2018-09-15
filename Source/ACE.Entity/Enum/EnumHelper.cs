using System.Collections;

namespace ACE.Entity.Enum
{
    public static class EnumHelper
    {
        public static T GetAttributeOfType<T>(this System.Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }


        /// <summary>
        /// Returns the # of bits set in a Flags enum
        /// </summary>
        /// <param name="enumVal">The enum int value</param>
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
    }
}
