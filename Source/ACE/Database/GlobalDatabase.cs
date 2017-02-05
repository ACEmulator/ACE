using ACE.Managers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ACE.Database
{

    public class GlobalDatabase : Database, IGlobalDatabase
    {
        private enum GlobalPreparedStatement
        {
            DBSchemaSelect,
            DBSchemasSelect,
            DBSchemaInsert,
            DBSchemaTableExistenceCheck,
            DBSchemaUpdate
        }

        protected override Type preparedStatementType => typeof(GlobalPreparedStatement);
        protected override string nodeName { get { return "Global";  } }

        protected override void InitialisePreparedStatements()
        {
            AddPreparedStatement(GlobalPreparedStatement.DBSchemaUpdate, "UPDATE `db_schemas` SET `schema_revision` = ? WHERE `node_name` = ? LIMIT 1;", MySqlDbType.UInt32, MySqlDbType.VarChar);
            AddPreparedStatement(GlobalPreparedStatement.DBSchemasSelect, "SELECT `node_name`, `schema_name`, `schema_revision`, `date_updated` FROM `db_schemas`;");
        }

        protected override void InitialiseTables()
        {
            base.InitialiseTables();

            AddPreparedStatement(GlobalPreparedStatement.DBSchemaSelect, "SELECT `node_name`, `schema_name`, `schema_revision`, `date_updated` FROM `db_schemas` WHERE `node_name` = ?;", MySqlDbType.VarChar);
        }

        protected override void RunBaseSql()
        {
            base.RunBaseSql();

            AddPreparedStatement(GlobalPreparedStatement.DBSchemaInsert, "INSERT INTO `db_schemas` (`node_name`, `schema_name`, `schema_revision`) VALUES (?, ?, ?);", MySqlDbType.VarChar, MySqlDbType.VarChar, MySqlDbType.UInt32);
        }

        protected override bool BaseSqlExecuted()
        {
            AddPreparedStatement(GlobalPreparedStatement.DBSchemaTableExistenceCheck, "SELECT table_name FROM information_schema.tables WHERE table_schema = ? AND table_name = 'db_schemas';", MySqlDbType.VarChar);
            var config = ConfigManager.Config.MySql;
            var result = SelectPreparedStatement(GlobalPreparedStatement.DBSchemaTableExistenceCheck, config.Global.Database); 

            return (result.Count > 0);
        }

        public void UpdateSchemaRevision(string nodeName, uint schemaRevision)
        {
            ExecutePreparedStatement(GlobalPreparedStatement.DBSchemaUpdate, schemaRevision, nodeName);
        }

        public bool InsertDBSchema(string nodeName, string schemaName)
        {
            ExecutePreparedStatement(GlobalPreparedStatement.DBSchemaInsert, nodeName, schemaName, 0);

            return true;
        }

        public DBSchema GetDBSchema(string nodeName)
        {
            var result = SelectPreparedStatement(GlobalPreparedStatement.DBSchemaSelect, nodeName);

            if (result.Count == 0)
            {
                return null;
            }

            string schemaName = result.Read<string>(0, "schema_name");
            uint schemaRevision = result.Read<uint>(0, "schema_revision");
            DateTime dateUpdated = DateTime.Parse(result.Read<string>(0, "date_updated"));

            DBSchema schema = new DBSchema()
            {
                NodeName = nodeName,
                SchemaName = schemaName,
                SchemaRevision = schemaRevision,
                DateUpdated = dateUpdated
            };

            return schema;
        }

        public List<DBSchema> GetDBSchemas()
        {
            var result = SelectPreparedStatement(GlobalPreparedStatement.DBSchemasSelect);
            List<DBSchema> schemas = new List<DBSchema>();

            for (uint i = 0u; i < result.Count; i++)
            {
                schemas.Add(new DBSchema()
                {
                    NodeName = result.Read<string>(i, "node_name"),
                    SchemaName = result.Read<string>(i, "schema_name"),
                    SchemaRevision = result.Read<uint>(i, "schema_revision"),
                });
            }

            return schemas;
        }
    }
}
