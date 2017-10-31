using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using log4net;
using ACE.Common;
using ACE.Common.Extensions;
using System.Reflection;

namespace ACE.Database
{
    public class Database
    {
        // This is a debug channel for the general debugging of the database.
        private ILog log = LogManager.GetLogger("Database");

        private static readonly Dictionary<Type, List<Tuple<PropertyInfo, DbFieldAttribute>>> propertyCache = new Dictionary<Type, List<Tuple<PropertyInfo, DbFieldAttribute>>>();

        private static readonly Dictionary<Type, DbTableAttribute> dbTableCache = new Dictionary<Type, DbTableAttribute>();

        public class DatabaseTransaction
        {
            // This logging function will log specific db transactions - this class may be instantiated outside of the database namespace
            private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            private readonly Database database;
            private readonly List<Tuple<StoredPreparedStatement, object[]>> queries = new List<Tuple<StoredPreparedStatement, object[]>>();

            public DatabaseTransaction(Database database) { this.database = database; }

            public void AddPreparedStatement<T>(T id, params object[] parameters)
            {
                // Debug.Assert(typeof(T) == database.PreparedStatementType, "Invalid prepared statement type.");

                StoredPreparedStatement preparedStatement;
                if (!database.preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement))
                {
                    Debug.Assert(preparedStatement != null, "Invalid prepared statement id.");
                    return;
                }

                queries.Add(new Tuple<StoredPreparedStatement, object[]>(preparedStatement, parameters));
            }

            public void AddPreparedDeleteListStatement<T1, T2>(T1 id, Dictionary<string, object> criteria)
            {
                // Debug.Assert(typeof(T1) == database.PreparedStatementType, "Invalid prepared statement type.");
                
                var propertyInfo = GetPropertyCache(typeof(T2));

                StoredPreparedStatement preparedStatement;
                if (!database.preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement))
                {
                    Debug.Assert(preparedStatement != null, "Invalid prepared statement id.");
                    return;
                }

                List<object> objects = new List<object>();
                foreach (var p in propertyInfo)
                {
                    if (p.Item2.ListDelete)
                    {
                        object val;
                        bool success = criteria.TryGetValue(p.Item2.DbFieldName, out val);
                        Debug.Assert(success, "Criteria does not contain essential key");
                        objects.Add(val);
                    }
                }

                queries.Add(new Tuple<StoredPreparedStatement, object[]>(preparedStatement, objects.ToArray()));
            }

            public void AddPreparedInsertListStatement<T1, T2>(T1 id, List<T2> info)
            {
                // Debug.Assert(typeof(T1) == database.PreparedStatementType, "Invalid prepared statement type.");
                
                var propertyInfo = GetPropertyCache(typeof(T2));

                uint statementId = Convert.ToUInt32(id);

                StoredPreparedStatement preparedStatement;
                if (!database.preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement))
                {
                    Debug.Assert(preparedStatement != null, "Invalid prepared statement id.");
                    return;
                }

                foreach (var type in info)
                {
                    object[] parameters = new object[propertyInfo.Count];
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        // Reflection (woot woot)
                        parameters[i] = propertyInfo[i].Item1.GetValue(type);
                    }

                    queries.Add(new Tuple<StoredPreparedStatement, object[]>(preparedStatement, parameters));
                }
            }

            public void AddPreparedInsertStatement<T1, T2>(T1 id, T2 instance)
            {
                // Debug.Assert(typeof(T1) == preparedStatementType);

                StoredPreparedStatement preparedStatement;
                if (!database.preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement))
                {
                    Debug.Assert(preparedStatement != null, "Invalid prepared statement id.");
                    return;
                }

                var properties = GetPropertyCache(typeof(T2));

                object[] parameters = new object[properties.Count];
                for (int i = 0; i < parameters.Length; i++)
                {
                    // Reflection (woot woot)
                    parameters[i] = properties[i].Item1.GetValue(instance);
                }

                queries.Add(new Tuple<StoredPreparedStatement, object[]>(preparedStatement, parameters));

                return;
            }

            public void AddPreparedUpdateStatement<T1, T2>(T1 id, T2 instance)
            {
                StoredPreparedStatement preparedStatement;
                if (!database.preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement))
                {
                    Debug.Assert(preparedStatement != null, "Invalid prepared statement id.");
                    return;
                }

                var properties = GetPropertyCache(typeof(T2));

                List<object> p = new List<object>();
                properties.Where(x => x.Item2.Update).ToList().ForEach(x => p.Add(x.Item1.GetValue(instance)));
                properties.Where(x => x.Item2.IsCriteria).ToList().ForEach(x => p.Add(x.Item1.GetValue(instance)));
                object[] parameters = p.ToArray();

                queries.Add(new Tuple<StoredPreparedStatement, object[]>(preparedStatement, parameters));

                return;
            }

            public void AddPreparedDeleteStatement<T1, T2>(T1 id, object instance)
            {
                // Debug.Assert(typeof(T1) == database.PreparedStatementType, "Invalid prepared statement type.");

                var propertyInfo = GetPropertyCache(typeof(T2));

                uint statementId = Convert.ToUInt32(id);

                StoredPreparedStatement preparedStatement;
                if (!database.preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement))
                {
                    Debug.Assert(preparedStatement != null, "Invalid prepared statement id.");
                    return;
                }

                List<object> objects = new List<object>();
                foreach (var p in propertyInfo)
                {
                    if (p.Item2.IsCriteria)
                    {
                        object val = p.Item1.GetValue(instance);
                        objects.Add(val);
                    }
                }

                queries.Add(new Tuple<StoredPreparedStatement, object[]>(preparedStatement, objects.ToArray()));
            }

            public async Task<bool> Commit()
            {
                if (queries.Count == 0)
                    return false;

                MySqlTransaction transaction = null;
                MySqlConnection connection = new MySqlConnection(database.connectionString);
                try
                {
                    await connection.OpenAsync();
                    return await Task.Run(() =>
                    {
                        transaction = connection.BeginTransaction();
                        foreach (var query in queries)
                        {
                            using (var command = new MySqlCommand(query.Item1.Query, connection, transaction))
                            {
                                for (int i = 0; i < query.Item2.Length; i++)
                                {
                                    command.Parameters.Add("", query.Item1.Types[i]).Value = query.Item2[i];
                                }
#if DBDEBUG
                                string debugString = "QUERY LINE - " + command.CommandText + " - ";
                                foreach (MySqlParameter p in command.Parameters)
                                {
                                    if (p?.Value != null)
                                        debugString += p.Value + " ";
                                }
                                log.Debug(debugString);
#endif
                                command.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return true;
                    });
                }
                catch (MySqlException transactionException)
                {
                    log.Error($"An exception occured while commiting a transaction of {queries.Count} queries, a rollback will be performed!");
                    log.Error($"Exception: {transactionException.Message}");

                    try
                    {
                        // serious problem if rollback also fails
                        transaction?.Rollback();
                    }
                    catch (MySqlException rollbackException)
                    {
                        log.Error("An exception occured while rolling back transaction!");
                        log.Error($"Exception: {rollbackException.Message}");
                        Debug.Assert(false, "Transaction was rolled back.");
                    }

                    return false;
                }
                finally
                {
                    queries.Clear();

                    // rollback will fail if connection or transaction is disposed before this
                    connection.Dispose();
                    transaction?.Dispose();
                }
            }
        }

        protected string connectionString;
        private readonly Dictionary<uint, StoredPreparedStatement> preparedStatements = new Dictionary<uint, StoredPreparedStatement>();

        protected virtual Type PreparedStatementType { get; }

        public void Initialize(string host, uint port, string user, string password, string database, bool autoReconnect = true)
        {
            var connectionBuilder = new MySqlConnectionStringBuilder()
            {
                Server = host,
                Port = port,
                UserID = user,
                Password = password,
                Database = database,
                IgnorePrepare = false,
                Pooling = true
            };

            connectionString = connectionBuilder.ToString();

            for (;;)
            {
                try
                {
                    using (var connection = new MySqlConnection(connectionString))
                        connection.Open();

                    log.Debug($"Successfully connected to {database} database on {host}:{port}.");
                    break;
                }
                catch (Exception exception)
                {
                    log.Error($"Exception: {exception.Message}");
                    log.Error($"Attempting to reconnect to {database} database on {host}:{port} in 5 seconds...");

                    if (autoReconnect)
                        Thread.Sleep(5000);
                    else
                        throw;
                }
            }

            InitializePreparedStatements();
        }

        public DatabaseTransaction BeginTransaction() { return new DatabaseTransaction(this); }

        protected virtual void InitializePreparedStatements() { }

        protected void AddPreparedStatement<T>(T id, string query, params MySqlDbType[] types)
        {
            // Debug.Assert(typeof(T) == PreparedStatementType, "Invalid prepared statement type.");
            Debug.Assert(types.Length == query.Count(c => c == '?'), "Invalid prepared statement parameter length.");

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        for (uint i = 0; i < types.Length; i++)
                            command.Parameters.Add("", types[i]);

                        command.Prepare();

                        uint uintId = Convert.ToUInt32(id);
                        preparedStatements.Add(uintId, new StoredPreparedStatement(uintId, query, types));
                    }
                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                log.Error($"An exception occured while preparing statement {id}!");
                log.Error($"Exception: {exception.Message}");
                Debug.Assert(false, "Preparation of statement failed.");
            }
        }

        private void ConstructGetListStatement<T1>(T1 id, Type type)
        {
            var properties = GetPropertyCache(type);

            // ColumnNames
            var criteria = new HashSet<string>(properties.Where(x => x.Item2.ListGet).Select(x => x.Item2.DbFieldName));

            ConstructGetListStatement<T1>(id, type, criteria);
        }

        public void ConstructGetListStatement<T1>(T1 id, Type type, HashSet<string> columnNames)
        {
            uint statementId = Convert.ToUInt32(id);
            DbTableAttribute dbTable = GetDbTableAttribute(type);

            List<MySqlDbType> types = new List<MySqlDbType>();
            var properties = GetPropertyCache(type);
            string tableName = dbTable.DbTableName;
            string selectList = null;
            string whereList = null;

            foreach (var p in properties)
            {
                if (p.Item2.Get)
                {
                    if (selectList != null)
                        selectList += ", ";
                    selectList += "`" + p.Item2.DbFieldName + "`";
                }

                if (columnNames.Contains(p.Item2.DbFieldName))
                {
                    if (whereList != null)
                        whereList += " AND ";
                    whereList += $"`{p.Item2.DbFieldName}` = ?";
                    types.Add((MySqlDbType)p.Item2.DbFieldType);
                }
            }

            string query = $"SELECT {selectList} FROM `{tableName}`";

            if (whereList != null)
                query = query + $" WHERE {whereList}";

            PrepareStatement(statementId, query, types);
        }

        private void ConstructDeleteListStatement<T1>(T1 id, Type type)
        {
            uint statementId = Convert.ToUInt32(id);
            DbTableAttribute dbTable = GetDbTableAttribute(type);

            List<MySqlDbType> types = new List<MySqlDbType>();
            var properties = GetPropertyCache(type);
            string tableName = dbTable.DbTableName;
            string whereList = null;

            foreach (var p in properties)
            {
                if (p.Item2.ListDelete)
                {
                    if (whereList != null)
                        whereList += " AND ";
                    whereList += "`" + p.Item2.DbFieldName + "` = ?";
                    types.Add((MySqlDbType)p.Item2.DbFieldType);
                }
            }

            string query = $"DELETE FROM `{tableName}` WHERE {whereList}";

            PrepareStatement(statementId, query, types);
        }

        private void ConstructDeleteStatement<T1>(T1 id, Type type)
        {
            uint statementId = Convert.ToUInt32(id);
            DbTableAttribute dbTable = GetDbTableAttribute(type);

            List<MySqlDbType> types = new List<MySqlDbType>();
            var properties = GetPropertyCache(type);
            string whereList = null;

            foreach (var p in properties)
            {
                if (p.Item2.IsCriteria)
                {
                    if (whereList != null)
                        whereList += " AND ";
                    whereList += "`" + p.Item2.DbFieldName + "` = ?";
                    types.Add((MySqlDbType)p.Item2.DbFieldType);
                }
            }

            string query = $"DELETE FROM `{dbTable.DbTableName}` WHERE {whereList}";

            PrepareStatement(statementId, query, types);
        }

        private void ConstructInsertListStatement<T1>(T1 id, Type type)
        {
            uint statementId = Convert.ToUInt32(id);
            DbTableAttribute dbTable = GetDbTableAttribute(type);

            List<MySqlDbType> types = new List<MySqlDbType>();
            var properties = GetPropertyCache(type);
            string tableName = dbTable.DbTableName;
            string valueList = "";
            string fieldList = "";
            bool inserted = false;

            foreach (var p in properties)
            {
                if (inserted)
                {
                    valueList += ", ";
                    fieldList += ", ";
                }
                fieldList += "`" + p.Item2.DbFieldName + "`";

                valueList += "?";

                types.Add((MySqlDbType)p.Item2.DbFieldType);
                inserted = true;
            }

            string query = $"INSERT INTO `{tableName}` ( {fieldList} ) VALUES ( {valueList} )";

            PrepareStatement(statementId, query, types);
        }

        private void ConstructGetAggregateStatement<T1>(T1 id, Type type)
        {
            uint statementId = Convert.ToUInt32(id);
            DbTableAttribute dbTable = GetDbTableAttribute(type);
            DbGetAggregateAttribute getAggregate = type.GetCustomAttributes(false)?.OfType<DbGetAggregateAttribute>()?.FirstOrDefault(d => d.ConstructedStatementId == statementId);

            if (dbTable == null)
                Debug.Assert(false, $"Statement Construction failed for type {type}");

            if (getAggregate == null)
                Debug.Assert(false, $"Statement Construction failed for type {type}");

            List<MySqlDbType> types = new List<MySqlDbType>();
            var properties = GetPropertyCache(type);
            string tableName = getAggregate.TableName;
            string aggregatFunction = getAggregate.AggregatFunction;
            string aggregateList = null;

            foreach (var p in properties)
            {
                if (getAggregate.ParameterFields.Contains(p.Item2.DbFieldName))
                {
                    if (aggregateList != null)
                        aggregateList += ", ";
                    aggregateList += aggregatFunction + "(`" + p.Item2.DbFieldName + "`)";
                    types.Add((MySqlDbType)p.Item2.DbFieldType);
                }
            }

            string query = $"SELECT {aggregateList} FROM `{tableName}`";

            PrepareStatement(statementId, query, types);
        }

        public void ConstructStatement<T1>(T1 id, Type type, ConstructedStatementType statementType)
        {
            if (statementType == ConstructedStatementType.GetList)
            {
                ConstructGetListStatement(id, type);
                return;
            }
            if (statementType == ConstructedStatementType.GetAggregate)
            {
                ConstructGetAggregateStatement(id, type);
                return;
            }
            if (statementType == ConstructedStatementType.DeleteList)
            {
                ConstructDeleteListStatement(id, type);
                return;
            }
            if (statementType == ConstructedStatementType.InsertList)
            {
                ConstructInsertListStatement(id, type);
                return;
            }

            if (statementType == ConstructedStatementType.Delete)
            {
                ConstructDeleteStatement(id, type);
                return;
            }

            DbTableAttribute dbTable = GetDbTableAttribute(type);

            string query = "";
            string tableName = dbTable.DbTableName;
            List<MySqlDbType> types = new List<MySqlDbType>();
            List<MySqlDbType> criteriaTypes = new List<MySqlDbType>();

            string updateList = null;
            string insertList = null;
            string insertValues = null;
            string selectList = null;
            string whereList = null;

            var properties = GetPropertyCache(type);
            foreach (var p in properties)
            {
#if DBDEBUG
                log.Debug("P1: " + p.Item1 + " P2: " + p.Item2);
#endif
                if (p.Item2.Get)
                {
                    if (selectList != null)
                        selectList += ", ";
                    selectList += "`" + p.Item2.DbFieldName + "`";
                }

                if (p.Item2.Update && statementType == ConstructedStatementType.Update)
                {
                    if (updateList != null)
                        updateList += ", ";
                    updateList += "`" + p.Item2.DbFieldName + "` = ?";
                    types.Add((MySqlDbType)p.Item2.DbFieldType);
                }

                if (p.Item2.Insert && statementType == ConstructedStatementType.Insert)
                {
                    if (insertList != null)
                        insertList += ", ";
                    insertList += "`" + p.Item2.DbFieldName + "`";

                    if (insertValues != null)
                        insertValues += ", ";
                    insertValues += "?";

                    types.Add((MySqlDbType)p.Item2.DbFieldType);
                }

                if (p.Item2.IsCriteria)
                {
                    if (whereList != null)
                        whereList += " AND ";
                    whereList += "`" + p.Item2.DbFieldName + "` = ?";

                    if (statementType == ConstructedStatementType.Get || statementType == ConstructedStatementType.Update)
                        criteriaTypes.Add((MySqlDbType)p.Item2.DbFieldType);
                }
            }

            types.AddRange(criteriaTypes);

            switch (statementType)
            {
                case ConstructedStatementType.Get:
                    query = $"SELECT {selectList} FROM `{tableName}` WHERE {whereList}";
                    break;
                case ConstructedStatementType.Insert:
                    query = $"INSERT INTO `{tableName}` ({insertList}) VALUES ({insertValues})";
                    break;
                case ConstructedStatementType.Update:
                    query = $"UPDATE `{tableName}` SET {updateList} WHERE {whereList}";
                    break;
                case ConstructedStatementType.Delete:
                    query = $"DELETE `{tableName}` WHERE {whereList}";
                    break;
            }
#if DBDEBUG
            log.Debug("Id: " + Convert.ToUInt32(id) + "Query: " + query);
            foreach (var name in types)
            {
                log.Debug("Types: + " + name.ToString());
            }
#endif
            PrepareStatement(Convert.ToUInt32(id), query, types);
        }

        public bool ExecuteConstructedGetStatement<T1, T2>(T2 id, Dictionary<string, object> criteria, object instance) where T1 : class
        {
            // Debug.Assert(typeof(T1) == preparedStatementType);

            StoredPreparedStatement preparedStatement;
            if (!preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement))
            {
                Debug.Assert(preparedStatement != null, "Invalid prepared statement id.");
                return false;
            }

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(preparedStatement.Query, connection))
                    {
                        var properties = GetPropertyCache(typeof(T1));
                        foreach (var p in properties)
                        {
                            if (p.Item2.IsCriteria)
                            {
                                if (criteria.ContainsKey(p.Item2.DbFieldName))
                                    command.Parameters.Add("", (MySqlDbType)p.Item2.DbFieldType).Value = criteria[p.Item2.DbFieldName];
                                else
                                    command.Parameters.Add("", (MySqlDbType)p.Item2.DbFieldType).Value = DBNull.Value;
                            }
                        }

                        connection.Open();
                        using (var commandReader = command.ExecuteReader(CommandBehavior.Default))
                        {
                            if (commandReader.Read())
                            {
                                ReadObject<T1>(commandReader, instance as T1);

                                return true;
                            }
                        }
                    }
                }
            }
            catch (MySqlException exception)
            {
                log.Error($"An exception occured while executing prepared statement {id}!");
                log.Error($"Exception: {exception.Message}");
            }

            return false;
        }

        public List<T2> ExecuteConstructedGetListStatement<T1, T2>(T1 id, Dictionary<string, object> criteria) where T2 : class
        {
            var results = new List<T2>();

            StoredPreparedStatement preparedStatement;
            if (!preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement))
            {
                Debug.Assert(preparedStatement != null, "Invalid prepared statement id.");
                return null;
            }

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(preparedStatement.Query, connection))
                    {
                        var properties = GetPropertyCache(typeof(T2));
                        foreach (var p in properties)
                        {
                            // Add field name and value parameters
                            if (criteria.ContainsKey(p.Item2.DbFieldName))
                            {
                                command.Parameters.Add("", (MySqlDbType)p.Item2.DbFieldType).Value = criteria[p.Item2.DbFieldName];
                            }
                        }
                        connection.Open();
                        using (var commandReader = command.ExecuteReader(CommandBehavior.Default))
                        {
                            while (commandReader.Read())
                            {
                                results.Add(ReadObject<T2>(commandReader));
                            }
                        }
                    }
                }
            }
            catch (MySqlException exception)
            {
                log.Error($"An exception occured while executing prepared statement {id}!");
                log.Error($"Exception: {exception.Message}");
            }

            return results;
        }

        protected T ReadObject<T>(MySqlDataReader commandReader, T o = null) where T : class
        {
            var properties = GetPropertyCache(typeof(T));

            if (o == null)
                o = Activator.CreateInstance<T>();

            foreach (var p in properties)
            {
                var assignable = commandReader[p.Item2.DbFieldName];
                if (Convert.IsDBNull(assignable))
                {
                    p.Item1.SetValue(o, null);
                }
                else
                {
                    p.Item1.SetValue(o, assignable);
                }
            }
            return o;
        }

        public T3 ExecuteConstructedGetAggregateStatement<T1, T2, T3>(T1 id)
        {
            uint statementId = Convert.ToUInt32(id);

            StoredPreparedStatement preparedStatement;
            if (!preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement))
            {
                Debug.Assert(preparedStatement != null, "Invalid prepared statement id.");
                return default(T3);
            }

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(preparedStatement.Query, connection))
                    {
                        connection.Open();
                        using (var commandReader = command.ExecuteReader(CommandBehavior.Default))
                        {
                            if (commandReader.Read())
                            {
                                // TODO: Extend this to read multiple Aggregate functions if/when there is a use case for this
                                if (commandReader[0] == DBNull.Value)
                                    return default(T3);
                                else
                                    return (T3)commandReader[0];
                            }
                        }
                    }
                }
            }
            catch (MySqlException exception)
            {
                log.Error($"An exception occured while executing prepared statement {id}!");
                log.Error($"Exception: {exception.Message}");
            }

            return default(T3);
        }

        public bool ExecuteConstructedInsertStatement<T1>(T1 id, Type type, object instance)
        {
            // Debug.Assert(typeof(T1) == preparedStatementType);

            StoredPreparedStatement preparedStatement;

            if (!preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement))
            {
                Debug.Assert(preparedStatement != null, "Invalid prepared statement id.");
                return false;
            }

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(preparedStatement.Query, connection))
                    {
                        var properties = GetPropertyCache(type);
                        properties.Where(p => p.Item2.Insert).ToList().ForEach(p => command.Parameters.Add("", (MySqlDbType)p.Item2.DbFieldType).Value = p.Item1.GetValue(instance));

                        connection.Open();

                        DbTableAttribute dbTable = GetDbTableAttribute(type);
                        if (!dbTable.HasAutoGeneratedId)
                        {
                            return command.ExecuteNonQuery() > 0;
                        }
                        else
                        {
                            command.CommandText += "; SELECT LAST_INSERT_ID();";
                            // backfill the generated id
                            using (var commandReader = command.ExecuteReader(CommandBehavior.Default))
                            {
                                if (commandReader.Read())
                                {
                                    var generatedId = Convert.ToUInt32(commandReader[0].ToString());
                                    var prop = properties.FirstOrDefault(p => p.Item2.DbFieldName == dbTable.AutoGeneratedIdColumn);
                                    prop?.Item1.SetValue(instance, generatedId);
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            return true;
                        }
                    }
                }
            }
            catch (MySqlException exception)
            {
                log.Error($"An exception occured while executing prepared statement {id}!");
                log.Error($"Exception: {exception.Message}");
            }

            return false;
        }

        public bool ExecuteConstructedUpdateStatement<T1>(T1 id, Type type, object instance)
        {
            // Debug.Assert(typeof(T1) == preparedStatementType);

            StoredPreparedStatement preparedStatement;
            if (!preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement))
            {
                Debug.Assert(preparedStatement != null, "Invalid prepared statement id.");
                return false;
            }

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(preparedStatement.Query, connection))
                    {
                        var properties = GetPropertyCache(type);

                        // add all the parameters to be updated
                        properties.Where(p => p.Item2.Update).ToList().ForEach(p => command.Parameters.Add("", (MySqlDbType)p.Item2.DbFieldType).Value = p.Item1.GetValue(instance));

                        // criteria (where clause) is at the end
                        properties.Where(p => p.Item2.IsCriteria).ToList().ForEach(p => command.Parameters.Add("", (MySqlDbType)p.Item2.DbFieldType).Value = p.Item1.GetValue(instance));

                        connection.Open();
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (MySqlException exception)
            {
                log.Error($"An exception occured while executing prepared statement {id}!");
                log.Error($"Exception: {exception.Message}");
            }

            return false;
        }

        public bool ExecuteConstructedDeleteStatement<T1>(T1 id, Type type, Dictionary<string, object> criteria)
        {
            StoredPreparedStatement preparedStatement;
            if (!preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement))
            {
                Debug.Assert(preparedStatement != null, "Invalid prepared statement id.");
                return false;
            }

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(preparedStatement.Query, connection))
                    {
                        var properties = GetPropertyCache(type);
                        properties.Where(p => p.Item2.IsCriteria).ToList().ForEach(p => command.Parameters.Add("", (MySqlDbType)p.Item2.DbFieldType).Value = criteria[p.Item2.DbFieldName]);

                        connection.Open();
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (MySqlException exception)
            {
                log.Error($"An exception occured while executing prepared statement {id}!");
                log.Error($"Exception: {exception.Message}");
                throw;
            }
        }

        protected void ExecutePreparedStatement<T>(T id, params object[] parameters)
        {
            ExecutePreparedStatement(false, id, parameters);
        }

        protected async Task ExecutePreparedStatementAsync<T>(T id, params object[] parameters)
        {
            await Task.Run(() => ExecutePreparedStatement(true, id, parameters));
        }

        private async void ExecutePreparedStatement<T>(bool async, T id, params object[] parameters)
        {
            // Debug.Assert(typeof(T) == PreparedStatementType, "Invalid prepared statement type.");

            StoredPreparedStatement preparedStatement;
            if (!preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement))
            {
                Debug.Assert(preparedStatement != null, "Invalid prepared statement id.");
                return;
            }

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(preparedStatement.Query, connection))
                    {
#if DBDEBUG
                        log.Debug(preparedStatement.Query);
#endif
                        for (int i = 0; i < preparedStatement.Types.Count; i++)
                        {
#if DBDEBUG
                            log.Debug(preparedStatement.Types[i]);
                            foreach (MySqlParameter p in command.Parameters)
                            {
                                log.Debug(p.Value);
                            }
#endif
                            command.Parameters.Add("", preparedStatement.Types[i]).Value = parameters[i];
#if DBDEBUG
                            log.Debug(command.Parameters);
#endif
                        }
                        if (async)
                        {
                            await connection.OpenAsync();
                            await Task.Run(() => command.ExecuteNonQuery()); // by default ExecuteNonQueryAsync is blocking
                        }
                        else
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (MySqlException exception)
            {
                log.Error($"An exception occured while executing prepared statement {id}!");
                log.Error($"Exception: {exception.Message}");
            }
        }

        protected MySqlResult SelectPreparedStatement<T>(T id, params object[] parameters)
        {
            // Debug.Assert(typeof(T) == PreparedStatementType, "Invalid prepared statement type.");

            StoredPreparedStatement preparedStatement;
            if (!preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement))
            {
                Debug.Assert(preparedStatement != null, "Invalid prepared statement id.");
                return null;
            }

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(preparedStatement.Query, connection))
                    {
                        for (int i = 0; i < preparedStatement.Types.Count; i++)
                            command.Parameters.Add("", preparedStatement.Types[i]).Value = parameters[i];

                        using (var commandReader = command.ExecuteReader(CommandBehavior.Default))
                        {
                            using (var result = new MySqlResult())
                            {
                                result.Load(commandReader);
                                result.Count = (uint)result.Rows.Count;
                                return result;
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                log.Error($"An exception occured while selecting prepared statement {id}!");
                log.Error($"Exception: {exception.Message}");
            }

            return null;
        }

        protected async Task<MySqlResult> SelectPreparedStatementAsync<T>(T id, params object[] parameters)
        {
            // Debug.Assert(typeof(T) == PreparedStatementType, "Invalid prepared statement type.");

            StoredPreparedStatement preparedStatement;
            if (!preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement))
            {
                Debug.Assert(preparedStatement != null, "Invalid prepared statement id.");
                return null;
            }

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(preparedStatement.Query, connection))
                    {
                        for (int i = 0; i < preparedStatement.Types.Count; i++)
                            command.Parameters.Add("", preparedStatement.Types[i]).Value = parameters[i];

                        await connection.OpenAsync();
                        return await Task.Run(() =>
                        {
                            using (var commandReader = command.ExecuteReader(CommandBehavior.Default))
                            {
                                using (var result = new MySqlResult())
                                {
                                    result.Load(commandReader);
                                    result.Count = (uint)result.Rows.Count;
                                    return result;
                                }
                            }
                        });
                    }
                }
            }
            catch (Exception exception)
            {
                log.Error($"An exception occured while selecting prepared statement {id}!");
                log.Error($"Exception: {exception.Message}");
            }

            return null;
        }

        protected static List<Tuple<PropertyInfo, DbFieldAttribute>> GetPropertyCache(Type t)
        {
            if (propertyCache.ContainsKey(t))
                return propertyCache[t].ToList(); // always return a copy

            List<Tuple<PropertyInfo, DbFieldAttribute>> newValue = new List<Tuple<PropertyInfo, DbFieldAttribute>>();
            var properties = t.GetProperties().Where(prop => prop.IsDefined(typeof(DbFieldAttribute), false));
            foreach (var p in properties)
            {
                DbFieldAttribute f = p.GetAttributeOfType<DbFieldAttribute>();
                newValue.Add(new Tuple<PropertyInfo, DbFieldAttribute>(p, f));
            }

            propertyCache.Add(t, newValue);
            return newValue;
        }

        private void PrepareStatement(uint id, string query, List<MySqlDbType> types)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        types.ForEach(t => command.Parameters.Add("", t));
#if DBDEBUG
                        log.Debug(types);
                        log.Debug(query);
#endif
                        command.Prepare();

                        uint uintId = Convert.ToUInt32(id);
                        preparedStatements.Add(uintId, new StoredPreparedStatement(uintId, query, types.ToArray()));
                    }
                }
            }
            catch (Exception exception)
            {
                log.Error($"An exception occured while preparing statement {id}!");
                log.Error($"Exception: {exception.Message}");
                Debug.Assert(false, "Prepared Statement Exception: " + query);
            }
        }

        protected DbTableAttribute GetDbTableAttribute(Type type)
        {
            if (!dbTableCache.ContainsKey(type))
            {
                DbTableAttribute dbTable = type.GetCustomAttributes(false)?.OfType<DbTableAttribute>()?.FirstOrDefault();
                Debug.Assert(dbTable != null, $"Statement Construction failed for type {type}");
                dbTableCache.Add(type, dbTable);
            }

            return dbTableCache[type];
        }

        protected string EscapeStringLiteral(string input)
        {
            string result = input.Replace(@"\", @"\\");
            result = result.Replace("'", @"\'");
            result = result.Replace("\"", "\\\"");
            result = result.Replace("%", "\\%");
            result = result.Replace("_", "\\_");
            result = result.Replace("\n", "\\n");
            result = result.Replace("\r", "\\r");
            result = result.Replace("\0", ""); // just remove null characters
            return result;
        }

        protected T ExecuteDynamicGet<T>(Dictionary<string, MySqlParameter> criteria) where T : class
        {
            var properties = GetPropertyCache(typeof(T));
            var dbTable = GetDbTableAttribute(typeof(T));
            string sql = "SELECT " + string.Join(", ", properties.Select(p => "`v`." + p.Item2.DbFieldName)) + " FROM " + dbTable.DbTableName + " `v`";

            string where = null;

            foreach (var p in criteria)
            {
                where = where == null ? " WHERE " : where + " AND ";
                where += $"`{p.Key}`= ?";
            }

            sql += (where ?? "");

            using (var connection = new MySqlConnection(connectionString))
            {
                using (var command = new MySqlCommand(sql, connection))
                {
                    criteria.Values.ToList().ForEach(p => command.Parameters.Add(p));

                    connection.Open();
                    using (var commandReader = command.ExecuteReader(CommandBehavior.Default))
                    {
                        if (commandReader.Read())
                        {
                            return ReadObject<T>(commandReader);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
    }
}
