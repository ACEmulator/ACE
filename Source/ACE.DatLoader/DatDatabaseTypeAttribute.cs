using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class DatDatabaseTypeAttribute : Attribute
    {
        public DatDatabaseType Type { get; set; }

        public DatDatabaseTypeAttribute(DatDatabaseType type)
        {
            Type = type;
        }
    }
}
