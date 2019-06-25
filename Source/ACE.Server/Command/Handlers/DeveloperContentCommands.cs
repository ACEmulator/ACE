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
        [CommandHandler("import", AccessLevel.Developer, CommandHandlerFlag.None, 1, "Imports a weenie from the Content folder", "<wcid>")]
        public static void HandleImport(Session session, params string[] parameters)
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
                return;
            }

            var json_folder = $"{di.FullName}{sep}json{sep}weenies{sep}";

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
            if (sqlFile == null)
                return;

            // import sql to db
            ImportSQL(sqlFile);
            CommandHandlerHelper.WriteOutputInfo(session, $"Imported {sqlFile}");

            // clear this weenie out of the cache
            if (uint.TryParse(wcid, out var weenieClassId))
            {
                DatabaseManager.World.ClearCachedWeenie(weenieClassId);
                //var wo = WorldObjectFactory.CreateNewWorldObject(weenieClassId);
            }
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

            var sqlFilename = json_filename.Replace(".json", ".sql");

            var sqlFile = new StreamWriter(sqlFilename);
            var converter = new WeenieSQLWriter();
            converter.CreateSQLDELETEStatement(output, sqlFile);
            sqlFile.WriteLine();
            converter.CreateSQLINSERTStatement(output, sqlFile);
            sqlFile.Close();

            CommandHandlerHelper.WriteOutputInfo(session, $"Converted {json_filename} to {sqlFilename}");

            return sqlFilename;
        }

        public static void ImportSQL(string sqlFile)
        {
            var sqlCommands = File.ReadAllText(sqlFile);

            using (var ctx = new WorldDbContext())
                ctx.Database.ExecuteSqlCommand(sqlCommands);
        }
    }
}
