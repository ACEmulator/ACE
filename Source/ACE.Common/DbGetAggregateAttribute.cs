using System;
using System.Collections.Generic;
using System.Linq;

namespace ACE.Common
{
    public class DbGetAggregateAttribute : Attribute
    {
        public string TableName { get; }

        public int ConstructedStatementId { get; }

        public string AggregatFunction { get; }

        public List<string> ParameterFields { get; }

        public DbGetAggregateAttribute(string tableName, int statementId, string aggregatfunction, params string[] fields)
        {
            this.TableName = tableName;
            this.ConstructedStatementId = statementId;
            this.AggregatFunction = aggregatfunction;
            this.ParameterFields = fields?.ToList() ?? new List<string>();
        }
    }
}
