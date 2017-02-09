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
            TeleportLocationSelect,
            PropertiesBoolSelect,
            PropertiesIntSelect,
            PropertiesBigIntSelect,
            PropertiesDoubleSelect,
            PropertiesStringSelect,

            PropertiesBoolSave,
            PropertiesIntSave,
            PropertiesBigIntSave,
            PropertiesDoubleSave,
            PropertiesStringSave
        }

        protected override Type preparedStatementType { get { return typeof(WorldPreparedStatement); } }

        protected override void InitialisePreparedStatements()
        {
            AddPreparedStatement(WorldPreparedStatement.TeleportLocationSelect, "SELECT `location`, `cell`, `x`, `y`, `z`, `qx`, `qy`, `qz`, `qw` FROM `teleport_location`;");

            AddPreparedStatement(WorldPreparedStatement.PropertiesBoolSelect, "SELECT `property_id`, `property_value` FROM `object_properties_bool` WHERE `object_guid` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(WorldPreparedStatement.PropertiesIntSelect, "SELECT `property_id`, `property_value` FROM `object_properties_int` WHERE `object_guid` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(WorldPreparedStatement.PropertiesBigIntSelect, "SELECT `property_id`, `property_value` FROM `object_properties_bigint` WHERE `object_guid` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(WorldPreparedStatement.PropertiesDoubleSelect, "SELECT `property_id`, `property_value` FROM `object_properties_double` WHERE `object_guid` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(WorldPreparedStatement.PropertiesStringSelect, "SELECT `property_id`, `property_value` FROM `object_properties_string` WHERE `object_guid` = ?;", MySqlDbType.UInt32);

            AddPreparedStatement(WorldPreparedStatement.PropertiesBoolSave, "INSERT INTO `object_properties_bool` (`object_guid`, `property_id`, `property_value`) VALUES (?, ?, ?) ON duplicate key update property_value = VALUES(property_value);", MySqlDbType.UInt32, MySqlDbType.UInt16, MySqlDbType.Bit);
            AddPreparedStatement(WorldPreparedStatement.PropertiesIntSave, "INSERT INTO `object_properties_int` (`object_guid`, `property_id`, `property_value`) VALUES (?, ?, ?) ON duplicate key update property_value = VALUES(property_value);", MySqlDbType.UInt32, MySqlDbType.UInt16, MySqlDbType.UInt32);
            AddPreparedStatement(WorldPreparedStatement.PropertiesBigIntSave, "INSERT INTO `object_properties_bigint` (`object_guid`, `property_id`, `property_value`) VALUES (?, ?, ?) ON duplicate key update property_value = VALUES(property_value);", MySqlDbType.UInt32, MySqlDbType.UInt16, MySqlDbType.UInt64);
            AddPreparedStatement(WorldPreparedStatement.PropertiesDoubleSave, "INSERT INTO `object_properties_double` (`object_guid`, `property_id`, `property_value`) VALUES (?, ?, ?) ON duplicate key update property_value = VALUES(property_value);", MySqlDbType.UInt32, MySqlDbType.UInt16, MySqlDbType.Double);
            AddPreparedStatement(WorldPreparedStatement.PropertiesStringSave, "INSERT INTO `object_properties_string` (`object_guid`, `property_id`, `property_value`) VALUES (?, ?, ?) ON duplicate key update property_value = VALUES(property_value);", MySqlDbType.UInt32, MySqlDbType.UInt16, MySqlDbType.VarChar);
        }


        public List<TeleportLocation> GetLocations()
        {
            var result = SelectPreparedStatement(WorldPreparedStatement.TeleportLocationSelect);
            List<TeleportLocation> locations = new List<TeleportLocation>();

            for (uint i = 0u; i < result.Count; i++)
            {
                locations.Add(new TeleportLocation()
                {
                    Location = result.Read<string>(i, "location"),
                    Position = new Position(result.Read<uint>(i, "cell"), result.Read<float>(i, "x"), result.Read<float>(i, "y"),
                    result.Read<float>(i, "z"), result.Read<float>(i, "qx"), result.Read<float>(i, "qy"), result.Read<float>(i, "qz"), result.Read<float>(i, "qw"))
                });
            }

            return locations;
        }

        public void LoadProperties(DbObject dbObject)
        {
            var results = SelectPreparedStatement(WorldPreparedStatement.PropertiesBoolSelect, dbObject.Id);

            for (uint i = 0; i < results.Count; i++)
            {
                dbObject.SetPropertyBool(results.Read<PropertyBool>(i, "property_id"), results.Read<bool>(i, "property_value"));
            }

            results = SelectPreparedStatement(WorldPreparedStatement.PropertiesIntSelect, dbObject.Id);

            for (uint i = 0; i < results.Count; i++)
            {
                dbObject.SetPropertyInt(results.Read<PropertyInt>(i, "property_id"), results.Read<uint>(i, "property_value"));
            }

            results = SelectPreparedStatement(WorldPreparedStatement.PropertiesBigIntSelect, dbObject.Id);

            for (uint i = 0; i < results.Count; i++)
            {
                dbObject.SetPropertyInt64(results.Read<PropertyInt64>(i, "property_id"), results.Read<ulong>(i, "property_value"));
            }

            results = SelectPreparedStatement(WorldPreparedStatement.PropertiesDoubleSelect, dbObject.Id);

            for (uint i = 0; i < results.Count; i++)
            {
                dbObject.SetPropertyDouble(results.Read<PropertyDouble>(i, "property_id"), results.Read<double>(i, "property_value"));
            }

            results = SelectPreparedStatement(WorldPreparedStatement.PropertiesStringSelect, dbObject.Id);

            for (uint i = 0; i < results.Count; i++)
            {
                dbObject.SetPropertyString(results.Read<PropertyString>(i, "property_id"), results.Read<string>(i, "property_value"));
            }
        }

        public void SaveProperties(DbObject dbObject)
        {
            // known issue: properties that were removed from the bucket will not updated.  this is a problem if we
            // ever need to straight up "delete" a property.

            foreach (var prop in dbObject.PropertiesBool)
            {
                ExecutePreparedStatement(WorldPreparedStatement.PropertiesBoolSave, dbObject.Id, (ushort)prop.Key, prop.Value);
            }

            foreach (var prop in dbObject.PropertiesInt)
            {
                ExecutePreparedStatement(WorldPreparedStatement.PropertiesIntSave, dbObject.Id, (ushort)prop.Key, prop.Value);
            }

            foreach (var prop in dbObject.PropertiesInt64)
            {
                ExecutePreparedStatement(WorldPreparedStatement.PropertiesBigIntSave, dbObject.Id, (ushort)prop.Key, prop.Value);
            }

            foreach (var prop in dbObject.PropertiesDouble)
            {
                ExecutePreparedStatement(WorldPreparedStatement.PropertiesDoubleSave, dbObject.Id, (ushort)prop.Key, prop.Value);
            }

            foreach (var prop in dbObject.PropertiesString)
            {
                ExecutePreparedStatement(WorldPreparedStatement.PropertiesStringSave, dbObject.Id, (ushort)prop.Key, prop.Value);
            }
        }
    }
}
