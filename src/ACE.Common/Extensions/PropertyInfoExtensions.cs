using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Common.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static T GetAttributeOfType<T>(this PropertyInfo prop) where T : System.Attribute
        {
            var attributes = prop.GetType().GetTypeInfo().GetCustomAttributes(typeof(T), false);

            return (attributes.Count() > 0)
                ? (T)attributes.ElementAt(0)
                : null;
        }
    }
}
