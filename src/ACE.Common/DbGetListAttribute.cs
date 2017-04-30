using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Common
{
    public class DbGetListAttribute : Attribute
    {
        public string TableName { get; private set; }

        public int ConstructedStatementId { get; private set; }

        public List<string> ParameterFields { get; private set; }

        public DbGetListAttribute(string tableName, int statementId, params string[] fields)
        {
            this.TableName = tableName;
            this.ConstructedStatementId = statementId;
            this.ParameterFields = fields?.ToList() ?? new List<string>();
        }
    }
}
