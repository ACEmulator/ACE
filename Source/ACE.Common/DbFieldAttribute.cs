using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class DbFieldAttribute : Attribute
    {
        public string DbFieldName { get; private set; }

        public int DbFieldType { get; private set; }

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
