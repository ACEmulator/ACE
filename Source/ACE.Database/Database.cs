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
    public class StoredPreparedStatement
    {
        public uint Id { get; }
        public string Query { get; }
        public List<MySqlDbType> Types { get; } = new List<MySqlDbType>();

        public StoredPreparedStatement(uint id, string query, params MySqlDbType[] types)
        {
            Id    = id;
            Query = query;
            Types.AddRange(types);
        }
    }

    public class Database
    {
        private static readonly Dictionary<Type, List<Tuple<PropertyInfo, DbFieldAttribute>>> propertyCache = new Dictionary<Type, List<Tuple<PropertyInfo, DbFieldAttribute>>>();

        public class DatabaseTransaction
        {
            private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            private readonly Database database;
            private readonly List<Tuple<StoredPreparedStatement, object[]>> queries = new List<Tuple<StoredPreparedStatement, object[]>>();

            public DatabaseTransaction(Database database) { this.database = database; }

            public void AddPreparedStatement<T>(T id, params object[] parameters)
            {
                Debug.Assert(typeof(T) == database.preparedStatementType);

                StoredPreparedStatement preparedStatement;
                if (!database.preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement))
                {
                    Debug.Assert(preparedStatement != null);
                    return;
                }

                queries.Add(new Tuple<StoredPreparedStatement, object[]>(preparedStatement, parameters));
            }

            public async Task<bool> Commit()
            {
                if (queries.Count == 0)
                    return false;

                MySqlConnection connection = new MySqlConnection(database.connectionString);
                MySqlTransaction transaction = null;

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
#if NETWORKDEBUG
                                    foreach (MySqlParameter p in command.Parameters)
                                    {
                                        log.Info(p.Value);
                                    }
#endif
                                }
                                command.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return true;
                    });
                }
                catch (MySqlException transactionException)
                {
                    Console.WriteLine($"An exception occured while commiting a transaction of {queries.Count} queries, a rollback will be performed!");
                    Console.WriteLine($"Exception: {transactionException.Message}");

                    try
                    {
                        // serious problem if rollback also fails
                        transaction?.Rollback();
                    }
                    catch (MySqlException rollbackException)
                    {
                        Console.WriteLine("An exception occured while rolling back transaction!");
                        Console.WriteLine($"Exception: {rollbackException.Message}");
                        Debug.Assert(false);
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

        private string connectionString;
        private readonly Dictionary<uint, StoredPreparedStatement> preparedStatements = new Dictionary<uint, StoredPreparedStatement>();

        protected virtual Type preparedStatementType { get; }

        public void Initialise(string host, uint port, string user, string password, string database)
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

                    Console.WriteLine($"Successfully connected to {database} database on {host}:{port}.");
                    break;
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Exception: {exception.Message}");
                    Console.WriteLine($"Attempting to reconnect to {database} database on {host}:{port} in 5 seconds...");

                    Thread.Sleep(5000);
                }
            }

            InitialisePreparedStatements();
        }

        public DatabaseTransaction BeginTransaction() { return new DatabaseTransaction(this); }

        protected virtual void InitialisePreparedStatements() { }

        protected void AddPreparedStatement<T>(T id, string query, params MySqlDbType[] types)
        {
            Debug.Assert(typeof(T) == preparedStatementType);
            Debug.Assert(types.Length == query.Count(c => c == '?'));

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
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An exception occured while preparing statement {id}!");
                Console.WriteLine($"Exception: {exception.Message}");
                Debug.Assert(false);
            }
        }

        private void ConstructGetListStatement<T1>(T1 id, Type type)
        {
            uint statementId = Convert.ToUInt32(id);
            DbTableAttribute dbTable = type.GetCustomAttributes(false)?.OfType<DbTableAttribute>()?.FirstOrDefault();
            DbGetListAttribute getList = type.GetCustomAttributes(false)?.OfType<DbGetListAttribute>()?.FirstOrDefault(d => d.ConstructedStatementId == statementId);
            
            if (dbTable == null)
                Debug.Assert(false, $"Statement Construction failed for type {type}");

            if (getList == null)
                Debug.Assert(false, $"Statement Construction failed for type {type}");

            List<MySqlDbType> types = new List<MySqlDbType>();
            var properties = GetPropertyCache(type);
            string tableName = getList.TableName;
            string selectList = null;
            string whereList = null;

            foreach (var p in properties)
            {
                if (p.Item2.Get)
                {
                    if (selectList != null)
                        selectList += ", ";
                    selectList += p.Item2.DbFieldName;
                }

                if (getList.ParameterFields.Contains(p.Item2.DbFieldName))
                {
                    if (whereList != null)
                        whereList += " AND ";
                    whereList += "`" + p.Item2.DbFieldName + "` = ?";
                    types.Add((MySqlDbType)p.Item2.DbFieldType);
                }
            }
            
            string query = $"SELECT {selectList} FROM `{tableName}` WHERE {whereList}";

            PrepareStatement(statementId, query, types);
        }

        public void ConstructStatement<T1>(T1 id, Type type, ConstructedStatementType statementType)
        {
            if (statementType == ConstructedStatementType.GetList)
            {
                ConstructGetListStatement(id, type);
                return;
            }

            DbTableAttribute dbTable = type.GetCustomAttributes(false)?.OfType<DbTableAttribute>()?.FirstOrDefault();
            Debug.Assert(dbTable != null, $"Statement Construction failed for type {type}");

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
                if (p.Item2.Get)
                {
                    if (selectList != null)
                        selectList += ", ";
                    selectList += p.Item2.DbFieldName;
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
                    insertList += p.Item2.DbFieldName;

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
                    query = $"INSERT INTO `{tableName}` ({selectList}) VALUES ({insertValues})";
                    break;
                case ConstructedStatementType.Update:
                    query = $"UPDATE `{tableName}` SET {updateList} WHERE {whereList}";
                    break;
            }

            PrepareStatement(Convert.ToUInt32(id), query, types);
        }

        public bool ExecuteConstructedGetStatement<T1>(T1 id, Type type, Dictionary<string, object> criteria, object instance)
        {
            // Debug.Assert(typeof(T1) == preparedStatementType);

            StoredPreparedStatement preparedStatement;
            if (!preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement))
            {
                Debug.Assert(preparedStatement != null);
                return false;
            }

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(preparedStatement.Query, connection))
                    {
                        var properties = GetPropertyCache(type);
                        foreach (var p in properties)
                        {
                            if (p.Item2.IsCriteria)
                                command.Parameters.Add("", (MySqlDbType)p.Item2.DbFieldType).Value = criteria[p.Item2.DbFieldName];
                        }

                        connection.Open();
                        using (var commandReader = command.ExecuteReader(CommandBehavior.Default))
                        {
                            if (commandReader.Read())
                            {
                                foreach (var p in properties)
                                {
                                    p.Item1.SetValue(instance, commandReader[p.Item2.DbFieldName]);
                                }

                                return true;
                            }
                        }
                    }
                }
            }
            catch (MySqlException exception)
            {
                Console.WriteLine($"An exception occured while executing prepared statement {id}!");
                Console.WriteLine($"Exception: {exception.Message}");
            }

            return false;
        }

        public List<T2> ExecuteConstructedGetListStatement<T1, T2>(T1 id, Dictionary<string, object> criteria)
        {
            List<T2> results = new List<T2>();
            uint statementId = Convert.ToUInt32(id);
            DbGetListAttribute getList = typeof(T2).GetCustomAttributes(false)?.OfType<DbGetListAttribute>()?.FirstOrDefault(d => d.ConstructedStatementId == statementId);

            StoredPreparedStatement preparedStatement;
            if (!preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement))
            {
                Debug.Assert(preparedStatement != null);
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
                            if (getList.ParameterFields.Contains(p.Item2.DbFieldName))
                                command.Parameters.Add("", (MySqlDbType)p.Item2.DbFieldType).Value = criteria[p.Item2.DbFieldName];
                        }

                        connection.Open();
                        using (var commandReader = command.ExecuteReader(CommandBehavior.Default))
                        {
                            while (commandReader.Read())
                            {
                                T2 o = Activator.CreateInstance<T2>();
                                foreach (var p in properties)
                                {
                                    p.Item1.SetValue(o, commandReader[p.Item2.DbFieldName]);
                                }

                                results.Add(o);
                            }
                        }
                    }
                }
            }
            catch (MySqlException exception)
            {
                Console.WriteLine($"An exception occured while executing prepared statement {id}!");
                Console.WriteLine($"Exception: {exception.Message}");
            }

            return results;
        }

        public bool ExecuteConstructedInsertStatement<T1>(T1 id, Type type, object instance)
        {
            // Debug.Assert(typeof(T1) == preparedStatementType);

            StoredPreparedStatement preparedStatement;
            if (!preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement))
            {
                Debug.Assert(preparedStatement != null);
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
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (MySqlException exception)
            {
                Console.WriteLine($"An exception occured while executing prepared statement {id}!");
                Console.WriteLine($"Exception: {exception.Message}");
            }

            return false;
        }

        public bool ExecuteConstructedUpdateStatement<T1>(T1 id, Type type, object instance)
        {
            // Debug.Assert(typeof(T1) == preparedStatementType);

            StoredPreparedStatement preparedStatement;
            if (!preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement))
            {
                Debug.Assert(preparedStatement != null);
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
                Console.WriteLine($"An exception occured while executing prepared statement {id}!");
                Console.WriteLine($"Exception: {exception.Message}");
            }

            return false;
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
            Debug.Assert(typeof(T) == preparedStatementType);

            StoredPreparedStatement preparedStatement;
            if (!preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement))
            {
                Debug.Assert(preparedStatement != null);
                return;
            }

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(preparedStatement.Query, connection))
                    {
                        for (int i = 0; i < preparedStatement.Types.Count; i++)
                            command.Parameters.Add("", preparedStatement.Types[i]).Value = parameters[i];

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
                Console.WriteLine($"An exception occured while executing prepared statement {id}!");
                Console.WriteLine($"Exception: {exception.Message}");
            }
        }

        protected MySqlResult SelectPreparedStatement<T>(T id, params object[] parameters)
        {
            Debug.Assert(typeof(T) == preparedStatementType);

            StoredPreparedStatement preparedStatement;
            if (!preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement))
            {
                Debug.Assert(preparedStatement != null);
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
                Console.WriteLine($"An exception occured while selecting prepared statement {id}!");
                Console.WriteLine($"Exception: {exception.Message}");
            }

            return null;
        }

        protected async Task<MySqlResult> SelectPreparedStatementAsync<T>(T id, params object[] parameters)
        {
            Debug.Assert(typeof(T) == preparedStatementType);

            StoredPreparedStatement preparedStatement;
            if (!preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement))
            {
                Debug.Assert(preparedStatement != null);
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
                Console.WriteLine($"An exception occured while selecting prepared statement {id}!");
                Console.WriteLine($"Exception: {exception.Message}");
            }

            return null;
        }

        private static List<Tuple<PropertyInfo, DbFieldAttribute>> GetPropertyCache(Type t)
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

                        command.Prepare();

                        uint uintId = Convert.ToUInt32(id);
                        preparedStatements.Add(uintId, new StoredPreparedStatement(uintId, query, types.ToArray()));
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An exception occured while preparing statement {id}!");
                Console.WriteLine($"Exception: {exception.Message}");
                Debug.Assert(false);
            }
        }
    }
}
