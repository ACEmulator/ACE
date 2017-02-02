using MySql.Data.MySqlClient;
using System;

namespace ACE.Database
{
    public enum WorldPreparedStatement
    {
        TeleportLocationSelect
    }

    public class WorldDatabase : Database
    {
        protected override Type preparedStatementType { get { return typeof(WorldPreparedStatement); } }

        protected override void InitialisePreparedStatements()
        {
            AddPreparedStatement(WorldPreparedStatement.TeleportLocationSelect, "SELECT `location`, `cell`, `x`, `y`, `z`, `qx`, `qy`, `qz`, `qw` FROM `teleport_location`;");
        }
    }
}
