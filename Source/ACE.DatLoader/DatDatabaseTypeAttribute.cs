using System;

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
