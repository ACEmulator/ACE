using System;
using System.Collections.Generic;
using System.Linq;

namespace ACE.Common
{
    public class DbGetAggregateAttribute : Attribute
    {
        public string TableName { get; private set; }

        public int ConstructedStatementId { get; private set; }

        public string AggregatFunction { get; private set; }

        public List<string> ParameterFields { get; private set; }

        public DbGetAggregateAttribute(string tableName, int statementId, string aggregatfunction, params string[] fields)
        {
            this.TableName = tableName;
            this.ConstructedStatementId = statementId;
            this.AggregatFunction = aggregatfunction;
            this.ParameterFields = fields?.ToList() ?? new List<string>();
        }
    }
}
