using System;
using System.Collections.Generic;
using System.Linq;

namespace ACE.Common
{
    /// <summary>
    /// Attribute specifying the keys to identify all elements of a list within a database table
    ///   That is --
    ///     SELECT /Fields/ FROM {TableName} WHERE {ParameterFields} = /ObjectParameterFields/;
    ///   returns an object list
    /// </summary>
    public class DbListAttribute : Attribute
    {
        public string TableName { get; private set; }

        public int ConstructedStatementId { get; private set; }

        public List<string> ParameterFields { get; private set; }

        public DbListAttribute(string tableName, params string[] fields)
        {
            this.TableName = tableName;
            this.ParameterFields = fields?.ToList() ?? new List<string>();
        }
    }
}
