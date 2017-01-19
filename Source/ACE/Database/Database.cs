using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ACE.Database
{
    public enum DatabaseType
    {
        Authentication,
        Character,
        World
    }

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
        private string connectionString;
        private Dictionary<uint, StoredPreparedStatement> preparedStatements = new Dictionary<uint, StoredPreparedStatement>();

        public void Initialise(DatabaseType type, string host, uint port, string user, string password, string database)
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

                    Console.WriteLine($"Successfully connected to {type.ToString()} database.");
                    break;
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Exception: {exception.Message}");
                    Console.WriteLine($"Attempting to reconnect to {type.ToString()} database in 5 seconds...");

                    Thread.Sleep(5000);
                }
            }

            InitialisePreparedStatements();
        }

        protected abstract Type GetPreparedStatementType();

        protected virtual void InitialisePreparedStatements() { }

        protected void AddPreparedStatement<T>(T id, string query, params MySqlDbType[] types)
        {
            Debug.Assert(typeof(T) == GetPreparedStatementType());
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
                Console.WriteLine($"An exception occured while preparing statement {id.ToString()}!");
                Console.WriteLine($"Exception: {exception.Message}");
            }
        }

        public void ExecutePreparedStatement<T>(T id, params object[] parameters)
        {
            ExecutePreparedStatement(false, id, parameters);
        }

        public void ExecutePreparedStatementAsync<T>(T id, params object[] parameters)
        {
            ExecutePreparedStatement(true, id, parameters);
        }

        private async void ExecutePreparedStatement<T>(bool async, T id, params object[] parameters)
        {
            Debug.Assert(typeof(T) == GetPreparedStatementType());

            StoredPreparedStatement preparedStatement;
            Debug.Assert(preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement));

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
                Console.WriteLine($"An exception occured while executing prepared statement {id.ToString()}!");
                Console.WriteLine($"Exception: {exception.Message}");
            }
        }

        public MySqlResult SelectPreparedStatement<T>(T id, params object[] parameters)
        {
            Debug.Assert(typeof(T) == GetPreparedStatementType());

            StoredPreparedStatement preparedStatement;
            Debug.Assert(preparedStatements.TryGetValue(Convert.ToUInt32(id), out preparedStatement));

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
                Console.WriteLine($"An exception occured while selecting prepared statement {id.ToString()}!");
                Console.WriteLine($"Exception: {exception.Message}");
            }

            return null;
        }

        /*public void SelectPreparedStatementAsync<T>(T id, params object[] parameters)
        {
        }*/
    }
}
