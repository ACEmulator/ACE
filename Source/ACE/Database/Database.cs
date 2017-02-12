using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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
        private string connectionString;
        private Dictionary<uint, StoredPreparedStatement> preparedStatements = new Dictionary<uint, StoredPreparedStatement>();

        protected string schemaName { get; set; }

        protected abstract Type preparedStatementType { get; }
        protected abstract string nodeName { get; }

        public void Initialise(string host, uint port, string user, string password, string database)
        {
            this.schemaName = database;

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

                    Console.WriteLine($"Successfully connected to {database} database.");
                    break;
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Exception: {exception.Message}");
                    Console.WriteLine($"Attempting to reconnect to {database} database in 5 seconds...");

                    Thread.Sleep(5000);
                }
            }

            InitialiseTables();

            InsertSchemaRow();

            var dbSchema = DatabaseManager.Global.GetDBSchema(this.nodeName);
            RunMigrations(dbSchema.SchemaRevision);

            InitialisePreparedStatements();
        }

        protected void InsertSchemaRow()
        {
            var dbSchema = DatabaseManager.Global.GetDBSchema(this.nodeName);

            if (dbSchema == null)
                DatabaseManager.Global.InsertDBSchema(this.nodeName, this.schemaName);
        }

        protected virtual void InitialiseTables()
        {
            if (!BaseSqlExecuted())
            {
                RunBaseSql();
            }
        }

        protected virtual void RunBaseSql()
        {
            string path = @".\Database\Base\" + this.nodeName + @"Base.sql";
            Console.WriteLine("Running base SQL file: " + path);
            RunSqlFile(path);
        }

        protected virtual bool BaseSqlExecuted()
        {
            return false;
        }

        protected List<DBMigration> GetAvailableMigrations()
        {
            var availableMigrations = new List<DBMigration>();
            // 1_2017-02-03-character_stats.sql
            var migrationFileNameRegex = @"^\d+_\d{4}-\d{2}-\d{2}-.+$";

            try
            {
                Console.WriteLine(@"Database\Updates\" + this.nodeName, "*.sql");
                var files = Directory.EnumerateFiles(@"Database\Updates\" + this.nodeName, "*.sql");

                foreach (var f in files)
                {
                    var fileName = Path.GetFileNameWithoutExtension(f);

                    var match = Regex.Match(fileName, migrationFileNameRegex);

                    if (!match.Success)
                    {
                        Console.WriteLine("Found migration SQL that didn't match naming convention, skipping: " + fileName);
                        continue;
                    }

                    char[] delimiter = { '_' };
                    var fileNameParts = fileName.Split(delimiter, 2);

                    var migration = new DBMigration();
                    migration.MigrationPath = f;
                    migration.MigrationNumber = Convert.ToUInt32(fileNameParts[0], 10);
                    migration.MigrationName = fileNameParts[1];

                    availableMigrations.Add(migration);
                }

                Console.WriteLine("{0} files found.", files.Count().ToString());
            }
            catch (UnauthorizedAccessException UAEx)
            {
                Console.WriteLine(UAEx.Message);
            }
            catch (PathTooLongException PathEx)
            {
                Console.WriteLine(PathEx.Message);
            }
            catch (DirectoryNotFoundException)
            {
                // no migrations folder exists for DB on disk
            }

            return availableMigrations;
        }

        protected void RunMigrations(uint currentRevision)
        {
            Console.WriteLine("");
            Console.WriteLine("Running database schema migrations...");

            // Run any migrations for the database
            var availableMigrations = GetAvailableMigrations();

            // now filter them by ones that are newer than current version
            var migrationsToRun = availableMigrations.
                Where(x => x.MigrationNumber > currentRevision).
                OrderBy(x => x.MigrationNumber).
                ToList();

            Console.WriteLine("Running " + migrationsToRun.Count + " migrations for " + this.nodeName);

            foreach (var migration in migrationsToRun)
            {
                Console.WriteLine("");
                Console.WriteLine("Running migration: " + migration.MigrationName);
                RunSqlFile(migration.MigrationPath);
                DatabaseManager.Global.UpdateSchemaRevision(this.nodeName, migration.MigrationNumber);
            }
        }

        protected virtual void InitialisePreparedStatements() { }

        protected List<string> SplitSqlCommands(string allSql)
        {
            List<string> commands = new List<String>();

            MySqlCommandSplitter splitter = new MySqlCommandSplitter(allSql);
            splitter.ReadAllCommands(c => commands.Add(c));

            return commands;
        }

        protected void RunSqlFile(string fileName)
        {
            var allSql = File.ReadAllText(fileName);

            var commands = SplitSqlCommands(allSql);

            // Todo: transaction with rollback?
            foreach (var query in commands)
            {
                #region DEBUG
                Console.WriteLine(query);
                #endregion

                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

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
                Console.WriteLine($"An exception occured while executing prepared statement {id}!");
                Console.WriteLine($"Exception: {exception.Message}");
            }
        }

        protected MySqlResult SelectPreparedStatement<T>(T id, params object[] parameters)
        {
            Debug.Assert(typeof(T) == preparedStatementType);

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
                Console.WriteLine($"An exception occured while selecting prepared statement {id}!");
                Console.WriteLine($"Exception: {exception.Message}");
            }

            return null;
        }

        protected async Task<MySqlResult> SelectPreparedStatementAsync<T>(T id, params object[] parameters)
        {
            Debug.Assert(typeof(T) == preparedStatementType);

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

            return new MySqlResult();
        }
    }

    public class DBSchema
    {
        public string SchemaName { get; set; }
        public uint SchemaRevision { get; set; }
        public string NodeName { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
