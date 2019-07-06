using System;
using System.IO;

using Microsoft.EntityFrameworkCore;

using ACE.Adapter.Lifestoned;
using ACE.Database;
using ACE.Database.Models.World;
using ACE.Database.SQLFormatters.World;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network;

namespace ACE.Server.Command.Handlers.Processors
{
    public class DeveloperContentCommands
    {
        [CommandHandler("import-json", AccessLevel.Developer, CommandHandlerFlag.None, 1, "Imports a JSON weenie from the Content folder", "<wcid>")]
        public static void HandleImportJson(Session session, params string[] parameters)
        {
            DirectoryInfo di = VerifyContentFolder(session);
            if (!di.Exists) return;

            var sep = Path.DirectorySeparatorChar;

            var json_folder = $"{di.FullName}{sep}json{sep}weenies{sep}";
            var sql_folder = json_folder.Replace("json", "sql");

            var wcid = parameters[0];
            var prefix = wcid + " - ";

            di = new DirectoryInfo(json_folder);

            var files = di.Exists ? di.GetFiles($"{prefix}*.json") : null;

            if (files == null || files.Length == 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find {json_folder}{prefix}*.json");
                return;
            }

            var fi = files[0];

            // convert json -> sql
            var sqlFile = json2sql(session, json_folder, fi.Name);
            if (sqlFile == null) return;

            // import sql to db
            ImportSQL(sql_folder + sqlFile);
            CommandHandlerHelper.WriteOutputInfo(session, $"Imported {sqlFile}");

            // clear this weenie out of the cache
            if (uint.TryParse(wcid, out var weenieClassId))
                DatabaseManager.World.ClearCachedWeenie(weenieClassId);
        }

        [CommandHandler("import-sql", AccessLevel.Developer, CommandHandlerFlag.None, 1, "Imports SQL weenie from the Content folder", "<wcid>")]
        public static void HandleImportSQL(Session session, params string[] parameters)
        {
            if (!uint.TryParse(parameters[0], out var wcid))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Invalid wcid {parameters[0]}");
                return;
            }

            DirectoryInfo di = VerifyContentFolder(session);
            if (!di.Exists) return;

            var sep = Path.DirectorySeparatorChar;

            var sql_folder = $"{di.FullName}{sep}sql{sep}weenies{sep}";

            di = new DirectoryInfo(sql_folder);

            var files = di.Exists ? di.GetFiles($"{wcid} *.sql") : null;

            if (files == null || files.Length == 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find {sql_folder}{wcid} - *.sql");
                return;
            }

            var sqlFile = files[0].Name;

            // import sql to db
            ImportSQL(sql_folder + sqlFile);
            CommandHandlerHelper.WriteOutputInfo(session, $"Imported {sqlFile}");

            // clear this weenie out of the cache
            DatabaseManager.World.ClearCachedWeenie(wcid);

            // load weenie from database
            var weenie = DatabaseManager.World.GetCachedWeenie(wcid);

            if (weenie == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't load weenie {wcid} from db");
                return;
            }

            sql2json(session, weenie, sql_folder, sqlFile);
        }

        private static DirectoryInfo VerifyContentFolder(Session session)
        {
            var content_folder = PropertyManager.GetString("content_folder").Item;

            var sep = Path.DirectorySeparatorChar;

            // handle relative path
            if (content_folder.StartsWith("."))
            {
                var cwd = Directory.GetCurrentDirectory() + sep;
                content_folder = cwd + content_folder;
            }

            var di = new DirectoryInfo(content_folder);

            if (!di.Exists)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find content folder: {di.FullName}");
                CommandHandlerHelper.WriteOutputInfo(session, "To set your content folder, /modifystring content_folder <path>");
            }
            return di;
        }

        public static string json2sql(Session session, string folder, string json_filename)
        {
            var json_file = folder + json_filename;

            var success = LifestonedLoader.TryLoadWeenie(json_file, out var weenie);

            if (!success)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to load {json_file}");
                return null;
            }

            // output to sql
            success = LifestonedConverter.TryConvert(weenie, out var output);

            if (!success)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to convert {json_file}");
                return null;
            }

            var sqlFolder = folder.Replace("json", "sql");

            var di = new DirectoryInfo(sqlFolder);

            if (!di.Exists)
                di.Create();

            var sqlFilename = "";

            try
            {
                var converter = new WeenieSQLWriter();
                converter.WeenieNames = DatabaseManager.World.GetAllWeenieNames();
                converter.SpellNames = DatabaseManager.World.GetAllSpellNames();
                converter.TreasureDeath = DatabaseManager.World.GetAllTreasureDeath();
                converter.TreasureWielded = DatabaseManager.World.GetAllTreasureWielded();
                if (output.LastModified == DateTime.MinValue)
                    output.LastModified = DateTime.UtcNow;
                sqlFilename = converter.GetDefaultFileName(output);
                var sqlFile = new StreamWriter(sqlFolder + sqlFilename);
                
                converter.CreateSQLDELETEStatement(output, sqlFile);
                sqlFile.WriteLine();
                converter.CreateSQLINSERTStatement(output, sqlFile);
                sqlFile.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to convert {json_file}");
                return null;
            }

            CommandHandlerHelper.WriteOutputInfo(session, $"Converted {json_filename} to {sqlFilename}");

            return sqlFilename;
        }

        public static bool sql2json(Session session, Weenie weenie, string sql_folder, string sql_filename)
        {
            if (!LifestonedConverter.TryConvertACEWeenieToLSDJSON(weenie, out var json))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to convert {sql_filename} to json");
                return false;
            }

            var json_folder = sql_folder.Replace("sql", "json");
            var json_filename = sql_filename.Replace(".sql", ".json");

            File.WriteAllText(json_folder + json_filename, json);

            CommandHandlerHelper.WriteOutputInfo(session, $"Converted {sql_filename} to {json_filename}");

            return true;
        }

        public static void ImportSQL(string sqlFile)
        {
            var sqlCommands = File.ReadAllText(sqlFile);

            using (var ctx = new WorldDbContext())
                ctx.Database.ExecuteSqlCommand(sqlCommands);
        }
    }
}
