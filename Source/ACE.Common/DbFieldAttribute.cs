using System;

namespace ACE.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class DbFieldAttribute : Attribute
    {
        public string DbFieldName { get; }

        public int DbFieldType { get; }

        public bool Insert { get; set; } = true;

        public bool Update { get; set; } = true;

        public bool Get { get; set; } = true;

        public bool ListGet { get; set; } = false;

        public bool ListDelete { get; set; } = false;

        public bool IsCriteria { get; set; } = false;

        public DbFieldAttribute(string fieldName, int fieldType)
        {
            DbFieldName = fieldName;
            DbFieldType = fieldType;
        }
    }
}
