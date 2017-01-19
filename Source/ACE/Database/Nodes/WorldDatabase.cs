using MySql.Data.MySqlClient;
using System;

namespace ACE.Database
{
    public enum WorldPreparedStatement
    {
    }

    public class WorldDatabase : Database
    {
        protected override Type GetPreparedStatementType() { return typeof(WorldPreparedStatement); }

        protected override void InitialisePreparedStatements()
        {
        }
    }
}
