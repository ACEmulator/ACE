using System.Reflection;

namespace ACE.Common.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static T GetAttributeOfType<T>(this PropertyInfo prop) where T : System.Attribute
        {
            var attributes = prop.GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
    }
}
