using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Common
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DbTableAttribute : Attribute
    {
        public string DbTableName { get; private set; }

        public DbTableAttribute(string tableName)
        {
            DbTableName = tableName;
        }
    }
}
