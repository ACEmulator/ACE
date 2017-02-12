using ACE.Entity;
using ACE.Managers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ACE.Database
{

    public class WorldDatabase : Database, IWorldDatabase
    {
        private enum WorldPreparedStatement
        {
            TeleportLocationSelect,
            TeleportLocationTableExistenceCheck
        }

        protected override Type preparedStatementType => typeof(WorldPreparedStatement);
        protected override string nodeName { get { return "World";  } }

        protected override void InitialisePreparedStatements()
        {
            AddPreparedStatement(WorldPreparedStatement.TeleportLocationSelect, "SELECT `location`, `cell`, `x`, `y`, `z`, `qx`, `qy`, `qz`, `qw` FROM `teleport_location`;");
        }

        protected override bool BaseSqlExecuted()
        {
            AddPreparedStatement(WorldPreparedStatement.TeleportLocationTableExistenceCheck, "SELECT table_name FROM information_schema.tables WHERE table_schema = ? AND table_name = 'teleport_location';", MySqlDbType.VarChar);

            var config = ConfigManager.Config.MySql;
            var result = SelectPreparedStatement(WorldPreparedStatement.TeleportLocationTableExistenceCheck, config.World.Database);

            return (result.Count > 0);
        }

        public List<TeleportLocation> GetLocations()
        {
            var result = SelectPreparedStatement(WorldPreparedStatement.TeleportLocationSelect);
            List<TeleportLocation> locations = new List<TeleportLocation>();

            for (uint i = 0u; i < result.Count; i++)
            {
                locations.Add(new TeleportLocation
                {
                    Location = result.Read<string>(i, "location"),
                    Position = new Position(result.Read<uint>(i, "cell"), result.Read<float>(i, "x"), result.Read<float>(i, "y"),
                        result.Read<float>(i, "z"), result.Read<float>(i, "qx"), result.Read<float>(i, "qy"), result.Read<float>(i, "qz"), result.Read<float>(i, "qw"))
                });
            }

            return locations;
        }
    }
}
