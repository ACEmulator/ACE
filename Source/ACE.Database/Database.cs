using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            Id    = id;
            Query = query;
            Types.AddRange(types);
        }
    }

    public abstract class Database
    {
        public class DatabaseTransaction
        {
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
                                    command.Parameters.Add("", query.Item1.Types[i]).Value = query.Item2[i];

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

        protected abstract Type preparedStatementType { get; }

        public void Initialise(string host, uint port, string user, string password, string database)
        {
            var connectionBuilder = new MySqlConnectionStringBuilder()
            {
                Server        = host,
                Port          = port,
                UserID        = user,
                Password      = password,
                Database      = database,
                IgnorePrepare = false,
                Pooling       = true
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
    }
}
