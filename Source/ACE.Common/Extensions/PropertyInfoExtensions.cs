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
            var attributes = prop.GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
    }
}
