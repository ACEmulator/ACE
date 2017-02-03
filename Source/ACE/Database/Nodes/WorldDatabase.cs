using ACE.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ACE.Database
{

    public class WorldDatabase : Database, IWorldDatabase
    {
        private enum WorldPreparedStatement
        {
            TeleportLocationSelect
        }

        protected override Type preparedStatementType { get { return typeof(WorldPreparedStatement); } }

        protected override void InitialisePreparedStatements()
        {
            AddPreparedStatement(WorldPreparedStatement.TeleportLocationSelect, "SELECT `location`, `cell`, `x`, `y`, `z`, `qx`, `qy`, `qz`, `qw` FROM `teleport_location`;");
        }

        public List<TeleportLocation> GetLocations()
        {
            var result = SelectPreparedStatement(WorldPreparedStatement.TeleportLocationSelect);
            List<TeleportLocation> locations = new List<TeleportLocation>();

            for (uint i = 0u; i < result.Count; i++)
            {
                locations.Add(new TeleportLocation()
                {
                    Location = result.Read<string>(0, "location"),
                    Position = new Position(result.Read<uint>(0, "cell"), result.Read<float>(0, "x"), result.Read<float>(0, "y"),
                    result.Read<float>(0, "z"), result.Read<float>(0, "qx"), result.Read<float>(0, "qy"), result.Read<float>(0, "qz"), result.Read<float>(0, "qw"))
                });
            }

            return locations;
        }
    }
}
