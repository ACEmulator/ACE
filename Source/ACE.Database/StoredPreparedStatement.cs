using System.Collections.Generic;

using MySql.Data.MySqlClient;

namespace ACE.Database
{
    public class StoredPreparedStatement
    {
        public uint Id { get; }

        public string Query { get; }

        public List<MySqlDbType> Types { get; } = new List<MySqlDbType>();

        public StoredPreparedStatement(uint id, string query, params MySqlDbType[] types)
        {
            Id = id;
            Query = query;
            Types.AddRange(types);
        }
    }
}
