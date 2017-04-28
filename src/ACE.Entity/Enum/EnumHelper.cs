using System.Reflection;
using System.Linq;

namespace ACE.Entity.Enum
{
    public static class EnumHelper
    {
        public static T GetAttributeOfType<T>(this System.Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);

            return (attributes.Count() > 0)
                ? (T)attributes.ElementAt(0)
                : null;
        }
    }
}