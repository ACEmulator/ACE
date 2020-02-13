using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using ACE.Adapter.GDLE;
using ACE.Adapter.Lifestoned;
using ACE.Common.Extensions;
using ACE.Database;
using ACE.Database.Models.World;
using ACE.Database.SQLFormatters.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;

namespace ACE.Server.Command.Handlers.Processors
{
    public class DeveloperContentCommands
    {
        public enum FileType
        {
            Undefined,
            Encounter,
            LandblockInstance,
            Quest,
            Recipe,
            Spell,
            Weenie,
        }

        public static FileType GetContentType(string fileType)
        {
            fileType = fileType.ToLower();

            if (fileType.StartsWith("landblock"))
                return FileType.LandblockInstance;
            else if (fileType.StartsWith("quest"))
                return FileType.Quest;
            else if (fileType.StartsWith("recipe"))
                return FileType.Recipe;
            else if (fileType.StartsWith("weenie"))
                return FileType.Weenie;
            else
                return FileType.Undefined;
        }

        [CommandHandler("import-json", AccessLevel.Developer, CommandHandlerFlag.None, 1, "Imports json data from the Content folder", "<wcid>")]
        public static void HandleImportJson(Session session, params string[] parameters)
        {
            var contentType = FileType.Weenie;

            if (parameters.Length > 1)
            {
                contentType = GetContentType(parameters[1]);

                if (contentType == FileType.Undefined)
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"Unknown content type '{parameters[1]}'");
                    return;
                }
            }
            switch (contentType)
            {
                case FileType.LandblockInstance:
                    ImportJsonLandblock(session, parameters);
                    break;

                case FileType.Quest:
                    ImportJsonQuest(session, parameters);
                    break;

                case FileType.Recipe:
                    ImportJsonRecipe(session, parameters);
                    break;

                case FileType.Weenie:
                    ImportJsonWeenie(session, parameters);
                    break;
            }
        }

        public static void ImportJsonWeenie(Session session, params string[] parameters)
        {
            DirectoryInfo di = VerifyContentFolder(session);
            if (!di.Exists) return;

            var sep = Path.DirectorySeparatorChar;

            var json_folder = $"{di.FullName}{sep}json{sep}weenies{sep}";

            var wcid = parameters[0];
            var prefix = wcid + " - ";

            if (wcid.Equals("all", StringComparison.OrdinalIgnoreCase))
                prefix = "";

            di = new DirectoryInfo(json_folder);

            var files = di.Exists ? di.GetFiles($"{prefix}*.json") : null;

            if (files == null || files.Length == 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find {json_folder}{prefix}*.json");
                return;
            }

            foreach (var file in files)
                ImportJsonWeenie(session, json_folder, file.Name);
        }

        public static void ImportJsonRecipe(Session session, params string[] parameters)
        {
            DirectoryInfo di = VerifyContentFolder(session);
            if (!di.Exists) return;

            var sep = Path.DirectorySeparatorChar;

            var json_folder = $"{di.FullName}{sep}json{sep}recipes{sep}";

            var recipeId = parameters[0];
            var prefix = recipeId + " - ";

            if (recipeId.Equals("all", StringComparison.OrdinalIgnoreCase))
                prefix = "";

            di = new DirectoryInfo(json_folder);

            var files = di.Exists ? di.GetFiles($"{prefix}*.json") : null;

            if (files == null || files.Length == 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find {json_folder}{prefix}*.json");
                return;
            }

            foreach (var file in files)
                ImportJsonRecipe(session, json_folder, file.Name);
        }

        public static void ImportJsonLandblock(Session session, params string[] parameters)
        {
            DirectoryInfo di = VerifyContentFolder(session);
            if (!di.Exists) return;

            var sep = Path.DirectorySeparatorChar;

            var json_folder = $"{di.FullName}{sep}json{sep}landblocks{sep}";

            var landblockId = parameters[0];
            var prefix = landblockId;

            if (landblockId.Equals("all", StringComparison.OrdinalIgnoreCase))
                prefix = "";

            di = new DirectoryInfo(json_folder);

            var files = di.Exists ? di.GetFiles($"{prefix}*.json") : null;

            if (files == null || files.Length == 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find {json_folder}{prefix}*.json");
                return;
            }

            foreach (var file in files)
                ImportJsonLandblock(session, json_folder, file.Name);
        }

        public static void ImportJsonQuest(Session session, params string[] parameters)
        {
            DirectoryInfo di = VerifyContentFolder(session);
            if (!di.Exists) return;

            var sep = Path.DirectorySeparatorChar;

            var json_folder = $"{di.FullName}{sep}json{sep}quests{sep}";

            var questName = parameters[0];
            var prefix = questName;

            if (questName.Equals("all", StringComparison.OrdinalIgnoreCase))
                prefix = "";

            di = new DirectoryInfo(json_folder);

            var files = di.Exists ? di.GetFiles($"{prefix}*.json") : null;

            if (files == null || files.Length == 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find {json_folder}{prefix}*.json");
                return;
            }

            foreach (var file in files)
                ImportJsonQuest(session, json_folder, file.Name);
        }

        [CommandHandler("import-sql", AccessLevel.Developer, CommandHandlerFlag.None, 1, "Imports sql data from the Content folder", "<wcid>")]
        public static void HandleImportSQL(Session session, params string[] parameters)
        {
            var contentType = FileType.Weenie;

            if (parameters.Length > 1)
            {
                contentType = GetContentType(parameters[1]);

                if (contentType == FileType.Undefined)
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"Unknown content type '{parameters[1]}'");
                    return;
                }
            }
            switch (contentType)
            {
                case FileType.LandblockInstance:
                    ImportSQLLandblock(session, parameters);
                    break;

                case FileType.Quest:
                    ImportSQLQuest(session, parameters);
                    break;

                case FileType.Recipe:
                    ImportSQLRecipe(session, parameters);
                    break;

                case FileType.Weenie:
                    ImportSQLWeenie(session, parameters);
                    break;
            }
        }

        public static void ImportSQLWeenie(Session session, params string[] parameters)
        {
            DirectoryInfo di = VerifyContentFolder(session);
            if (!di.Exists) return;

            var sep = Path.DirectorySeparatorChar;

            var sql_folder = $"{di.FullName}{sep}sql{sep}weenies{sep}";

            var wcid = parameters[0];
            var prefix = wcid + " ";

            if (wcid.Equals("all", StringComparison.OrdinalIgnoreCase))
                prefix = "";

            di = new DirectoryInfo(sql_folder);

            var files = di.Exists ? di.GetFiles($"{prefix}*.sql") : null;

            if (files == null || files.Length == 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find {sql_folder}{prefix}*.sql");
                return;
            }

            foreach (var file in files)
                ImportSQLWeenie(session, sql_folder, file.Name);
        }

        public static void ImportSQLRecipe(Session session, params string[] parameters)
        {
            DirectoryInfo di = VerifyContentFolder(session);
            if (!di.Exists) return;

            var sep = Path.DirectorySeparatorChar;

            var sql_folder = $"{di.FullName}{sep}sql{sep}recipes{sep}";

            var recipeId = parameters[0];
            var prefix = recipeId + " ";

            if (recipeId.Equals("all", StringComparison.OrdinalIgnoreCase))
                prefix = "";

            di = new DirectoryInfo(sql_folder);

            var files = di.Exists ? di.GetFiles($"{prefix}*.sql") : null;

            if (files == null || files.Length == 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find {sql_folder}{prefix}*.sql");
                return;
            }

            foreach (var file in files)
                ImportSQLRecipe(session, sql_folder, file.Name);
        }

        public static void ImportSQLLandblock(Session session, params string[] parameters)
        {
            DirectoryInfo di = VerifyContentFolder(session);
            if (!di.Exists) return;

            var sep = Path.DirectorySeparatorChar;

            var sql_folder = $"{di.FullName}{sep}sql{sep}landblocks{sep}";

            var landblockId = parameters[0];
            var prefix = landblockId;

            if (landblockId.Equals("all", StringComparison.OrdinalIgnoreCase))
                prefix = "";

            di = new DirectoryInfo(sql_folder);

            var files = di.Exists ? di.GetFiles($"{prefix}*.sql") : null;

            if (files == null || files.Length == 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find {sql_folder}{prefix}*.sql");
                return;
            }

            foreach (var file in files)
                ImportSQLLandblock(session, sql_folder, file.Name);
        }

        public static void ImportSQLQuest(Session session, params string[] parameters)
        {
            DirectoryInfo di = VerifyContentFolder(session);
            if (!di.Exists) return;

            var sep = Path.DirectorySeparatorChar;

            var sql_folder = $"{di.FullName}{sep}sql{sep}quests{sep}";

            var questName = parameters[0];
            var prefix = questName;

            if (questName.Equals("all", StringComparison.OrdinalIgnoreCase))
                prefix = "";

            di = new DirectoryInfo(sql_folder);

            var files = di.Exists ? di.GetFiles($"{prefix}*.sql") : null;

            if (files == null || files.Length == 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find {sql_folder}{prefix}*.sql");
                return;
            }

            foreach (var file in files)
                ImportSQLQuest(session, sql_folder, file.Name);
        }

        /// <summary>
        /// Returns the absolute content folder path, and verifies it exists
        /// </summary>
        private static DirectoryInfo VerifyContentFolder(Session session, bool showError = true)
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

            if (!di.Exists && showError)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find content folder: {di.FullName}");
                CommandHandlerHelper.WriteOutputInfo(session, "To set your content folder, /modifystring content_folder <path>");
            }
            return di;
        }

        /// <summary>
        /// Converts JSON to SQL, imports to database, and clears the weenie cache
        /// </summary>
        private static void ImportJsonWeenie(Session session, string json_folder, string json_file)
        {
            if (!uint.TryParse(Regex.Match(json_file, @"\d+").Value, out var wcid))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find wcid from {json_file}");
                return;
            }

            // convert json -> sql
            var sqlFile = json2sql_weenie(session, json_folder, json_file);
            if (sqlFile == null) return;

            // import sql to db
            var sql_folder = json_folder.Replace("json", "sql");
            ImportSQL(sql_folder + sqlFile);
            CommandHandlerHelper.WriteOutputInfo(session, $"Imported {sqlFile}");

            // clear this weenie out of the cache
            DatabaseManager.World.ClearCachedWeenie(wcid);
        }

        private static void ImportJsonRecipe(Session session, string json_folder, string json_file)
        {
            if (!uint.TryParse(Regex.Match(json_file, @"\d+").Value, out var recipeId))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find recipe id from {json_file}");
                return;
            }

            // convert json -> sql
            var sqlFile = json2sql_recipe(session, json_folder, json_file);
            if (sqlFile == null) return;

            // import sql to db
            var sql_folder = json_folder.Replace("json", "sql");
            ImportSQL(sql_folder + sqlFile);
            CommandHandlerHelper.WriteOutputInfo(session, $"Imported {sqlFile}");

            // clear recipe cache
            DatabaseManager.World.ClearCookbookCache();
        }

        private static void ImportJsonLandblock(Session session, string json_folder, string json_file)
        {
            if (!ushort.TryParse(Regex.Match(json_file, @"[0-9A-F]+").Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var landblockId))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find landblock id from {json_file}");
                return;
            }

            // convert json -> sql
            var sqlFile = json2sql_landblock(session, json_folder, json_file);
            if (sqlFile == null) return;

            // import sql to db
            var sql_folder = json_folder.Replace("json", "sql");
            ImportSQL(sql_folder + sqlFile);
            CommandHandlerHelper.WriteOutputInfo(session, $"Imported {sqlFile}");

            // clear any cached instances for this landblock
            DatabaseManager.World.ClearCachedInstancesByLandblock(landblockId);
        }

        private static void ImportJsonQuest(Session session, string json_folder, string json_file)
        {
            var questName = json_file.TrimEnd(".json");

            // convert json -> sql
            var sqlFile = json2sql_quest(session, json_folder, json_file);
            if (sqlFile == null) return;

            // import sql to db
            var sql_folder = json_folder.Replace("json", "sql");
            ImportSQL(sql_folder + sqlFile);
            CommandHandlerHelper.WriteOutputInfo(session, $"Imported {sqlFile}");

            // clear cached quest
            DatabaseManager.World.ClearCachedQuest(questName);
        }

        public static WeenieSQLWriter WeenieSQLWriter;

        /// <summary>
        /// Converts a json file to sql file
        /// </summary>
        public static string json2sql_weenie(Session session, string folder, string json_filename)
        {
            var json_file = folder + json_filename;

            // read json into lsd weenie
            var success = LifestonedLoader.TryLoadWeenie(json_file, out var weenie);

            if (!success)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to load {json_file}");
                return null;
            }

            // convert to ace weenie
            success = LifestonedConverter.TryConvert(weenie, out var output);

            if (!success)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to convert {json_file}");
                return null;
            }

            // output to sql
            var sqlFolder = folder.Replace("json", "sql");

            var di = new DirectoryInfo(sqlFolder);

            if (!di.Exists)
                di.Create();

            var sqlFilename = "";

            try
            {
                if (WeenieSQLWriter == null)
                {
                    WeenieSQLWriter = new WeenieSQLWriter();
                    WeenieSQLWriter.WeenieNames = DatabaseManager.World.GetAllWeenieNames();
                    WeenieSQLWriter.SpellNames = DatabaseManager.World.GetAllSpellNames();
                    WeenieSQLWriter.TreasureDeath = DatabaseManager.World.GetAllTreasureDeath();
                    WeenieSQLWriter.TreasureWielded = DatabaseManager.World.GetAllTreasureWielded();
                }

                if (output.LastModified == DateTime.MinValue)
                    output.LastModified = DateTime.UtcNow;

                sqlFilename = WeenieSQLWriter.GetDefaultFileName(output);
                var sqlFile = new StreamWriter(sqlFolder + sqlFilename);

                WeenieSQLWriter.CreateSQLDELETEStatement(output, sqlFile);
                sqlFile.WriteLine();

                WeenieSQLWriter.CreateSQLINSERTStatement(output, sqlFile);

                var metadata = new Adapter.GDLE.Models.Metadata(weenie);
                if (metadata.HasInfo)
                {
                    var jsonEx = JsonConvert.SerializeObject(metadata, LifestonedConverter.SerializerSettings);
                    sqlFile.WriteLine($"\n/* Lifestoned Changelog:\n{jsonEx}\n*/");
                }

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

        public static CookBookSQLWriter CookBookSQLWriter;
        public static RecipeSQLWriter RecipeSQLWriter;

        public static string json2sql_recipe(Session session, string folder, string json_filename)
        {
            var json_file = folder + json_filename;

            // read json into lsd recipe
            var success = GDLELoader.TryLoadRecipeCombined(json_file, out var result);

            if (!success)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to load {json_file}");
                return null;
            }

            // convert to ace cookbooks + recipe
            success = GDLEConverter.TryConvert(result, out var cookbooks, out var recipe);

            if (!success)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to convert {json_file}");
                return null;
            }

            // output to sql
            var sqlFolder = folder.Replace("json", "sql");

            var di = new DirectoryInfo(sqlFolder);

            if (!di.Exists)
                di.Create();

            var sqlFilename = "";

            try
            {
                if (RecipeSQLWriter == null)
                {
                    RecipeSQLWriter = new RecipeSQLWriter();
                    RecipeSQLWriter.WeenieNames = DatabaseManager.World.GetAllWeenieNames();
                }

                if (CookBookSQLWriter == null)
                {
                    CookBookSQLWriter = new CookBookSQLWriter();
                    CookBookSQLWriter.WeenieNames = DatabaseManager.World.GetAllWeenieNames();
                }

                if (recipe.LastModified == DateTime.MinValue)
                    recipe.LastModified = DateTime.UtcNow;

                foreach (var cookbook in cookbooks)
                {
                    if (cookbook.LastModified == DateTime.MinValue)
                        cookbook.LastModified = DateTime.UtcNow;
                }

                sqlFilename = RecipeSQLWriter.GetDefaultFileName(recipe, cookbooks);
                var sqlFile = new StreamWriter(sqlFolder + sqlFilename);

                RecipeSQLWriter.CreateSQLDELETEStatement(recipe, sqlFile);
                sqlFile.WriteLine();

                RecipeSQLWriter.CreateSQLINSERTStatement(recipe, sqlFile);
                sqlFile.WriteLine();

                CookBookSQLWriter.CreateSQLDELETEStatement(cookbooks, sqlFile);
                sqlFile.WriteLine();

                CookBookSQLWriter.CreateSQLINSERTStatement(cookbooks, sqlFile);

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

        public static string json2sql_landblock(Session session, string folder, string json_filename)
        {
            var json_file = folder + json_filename;

            // read json into gdle spawnmap
            var success = GDLELoader.TryLoadLandblock(json_file, out var result);

            if (!success)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to load {json_file}");
                return null;
            }

            // convert to ace landblock_instances
            success = GDLEConverter.TryConvert(result, out var landblockInstances, out var landblockInstanceLinks);

            if (!success)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to convert {json_file}");
                return null;
            }

            // link up instances
            // TODO: move this to TryConvert
            foreach (var link in landblockInstanceLinks)
            {
                var parent = landblockInstances.FirstOrDefault(i => i.Guid == link.ParentGuid);
                if (parent == null)
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find parent guid for {link.ParentGuid:X8}");
                    continue;
                }
                parent.LandblockInstanceLink.Add(link);

                var child = landblockInstances.FirstOrDefault(i => i.Guid == link.ChildGuid);
                if (child == null)
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find child guid for {link.ChildGuid:X8}");
                    continue;
                }
                child.IsLinkChild = true;
            }

            // output to sql
            var sqlFolder = folder.Replace("spawnmaps", "landblock_instances").Replace("json", "sql");

            var di = new DirectoryInfo(sqlFolder);

            if (!di.Exists)
                di.Create();

            var sqlFilename = "";

            try
            {
                if (LandblockInstanceWriter == null)
                {
                    LandblockInstanceWriter = new LandblockInstanceWriter();
                    LandblockInstanceWriter.WeenieNames = DatabaseManager.World.GetAllWeenieNames();
                }

                foreach (var landblockInstance in landblockInstances)
                {
                    if (landblockInstance.LastModified == DateTime.MinValue)
                        landblockInstance.LastModified = DateTime.UtcNow;
                }

                foreach (var landblockInstanceLink in landblockInstanceLinks)
                {
                    if (landblockInstanceLink.LastModified == DateTime.MinValue)
                        landblockInstanceLink.LastModified = DateTime.UtcNow;
                }

                sqlFilename = LandblockInstanceWriter.GetDefaultFileName(landblockInstances[0]);
                var sqlFile = new StreamWriter(sqlFolder + sqlFilename);

                LandblockInstanceWriter.CreateSQLDELETEStatement(landblockInstances, sqlFile);
                sqlFile.WriteLine();

                LandblockInstanceWriter.CreateSQLINSERTStatement(landblockInstances, sqlFile);

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

        public static QuestSQLWriter QuestSQLWriter;

        public static string json2sql_quest(Session session, string folder, string json_filename)
        {
            var json_file = folder + json_filename;

            // read json quest
            var success = GDLELoader.TryLoadQuest(json_file, out var result);

            if (!success)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to load {json_file}");
                return null;
            }

            // convert to sql quest
            success = GDLEConverter.TryConvert(result, out var quest);

            if (!success)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to convert {json_file}");
                return null;
            }

            // output to sql
            var sqlFolder = folder.Replace("json", "sql");
            var sqlFilename = json_filename.Replace(".json", ".sql");

            var di = new DirectoryInfo(sqlFolder);

            if (!di.Exists)
                di.Create();

            try
            {
                if (QuestSQLWriter == null)
                    QuestSQLWriter = new QuestSQLWriter();

                if (quest.LastModified == DateTime.MinValue)
                    quest.LastModified = DateTime.UtcNow;

                var sqlFile = new StreamWriter(sqlFolder + sqlFilename);

                QuestSQLWriter.CreateSQLDELETEStatement(quest, sqlFile);
                sqlFile.WriteLine();

                QuestSQLWriter.CreateSQLINSERTStatement(quest, sqlFile);

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

        /// <summary>
        /// Converts SQL to JSON, imports to database, clears the weenie cache
        /// </summary>
        private static void ImportSQLWeenie(Session session, string sql_folder, string sql_file)
        {
            if (!uint.TryParse(Regex.Match(sql_file, @"\d+").Value, out var wcid))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find wcid from {sql_file}");
                return;
            }

            // import sql to db
            ImportSQL(sql_folder + sql_file);
            CommandHandlerHelper.WriteOutputInfo(session, $"Imported {sql_file}");

            // clear this weenie out of the cache
            DatabaseManager.World.ClearCachedWeenie(wcid);

            // load weenie from database
            var weenie = DatabaseManager.World.GetWeenie(wcid);

            if (weenie == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't load weenie {wcid} from db");
                return;
            }

            sql2json_weenie(session, weenie, sql_folder, sql_file);
        }

        private static void ImportSQLRecipe(Session session, string sql_folder, string sql_file)
        {
            if (!uint.TryParse(Regex.Match(sql_file, @"\d+").Value, out var recipeId))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find recipe id from {sql_file}");
                return;
            }

            // import sql to db
            ImportSQL(sql_folder + sql_file);
            CommandHandlerHelper.WriteOutputInfo(session, $"Imported {sql_file}");

            // clear this recipe out of the cache
            DatabaseManager.World.ClearCookbookCache();

            // load cookbooks + recipe from database
            var cookbooks = DatabaseManager.World.GetCachedCookbooks(recipeId);

            if (cookbooks == null || cookbooks.Count == 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't load recipe {recipeId} from db");
                return;
            }

            sql2json_recipe(session, cookbooks, sql_folder, sql_file);
        }

        private static void ImportSQLLandblock(Session session, string sql_folder, string sql_file)
        {
            if (!ushort.TryParse(Regex.Match(sql_file, @"[0-9A-F]+").Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var landblockId))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find landblock id from {sql_file}");
                return;
            }

            // import sql to db
            ImportSQL(sql_folder + sql_file);
            CommandHandlerHelper.WriteOutputInfo(session, $"Imported {sql_file}");

            // clear any cached instances for this landblock
            DatabaseManager.World.ClearCachedInstancesByLandblock(landblockId);

            // load landblock instances from database
            var instances = DatabaseManager.World.GetCachedInstancesByLandblock(landblockId);

            // convert to json file
            sql2json_landblock(session, instances, sql_folder, sql_file);
        }

        private static void ImportSQLQuest(Session session, string sql_folder, string sql_file)
        {
            // import sql to db
            ImportSQL(sql_folder + sql_file);
            CommandHandlerHelper.WriteOutputInfo(session, $"Imported {sql_file}");

            // clear cached quest
            var questName = sql_file.TrimEnd(".sql");
            DatabaseManager.World.ClearCachedQuest(questName);

            // load quest from db
            var quest = DatabaseManager.World.GetCachedQuest(questName);

            // convert to json file
            sql2json_quest(session, quest, sql_folder, sql_file);
        }

        /// <summary>
        /// Converts a sql file to json file
        /// </summary>
        public static bool sql2json_weenie(Session session, Weenie weenie, string sql_folder, string sql_filename)
        {
            if (!LifestonedConverter.TryConvertACEWeenieToLSDJSON(weenie, out var json, out var json_weenie))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to convert {sql_filename} to json");
                return false;
            }

            var json_folder = sql_folder.Replace("sql", "json");
            var json_filename = sql_filename.Replace(".sql", ".json");

            var match = Regex.Match(json_filename, @"^(\d+)");
            if (match.Success)
            {
                var wcid = match.Groups[1].Value;
                if (!json_filename.StartsWith(wcid + " -"))
                    json_filename = wcid + " -" + json_filename.Substring(wcid.Length);
            }

            var di = new DirectoryInfo(json_folder);

            if (!di.Exists)
                di.Create();

            if (File.Exists(json_folder + json_filename) && LifestonedLoader.AppendMetadata(json_folder + json_filename, json_weenie))
            {
                json = JsonConvert.SerializeObject(json_weenie, LifestonedConverter.SerializerSettings);
            }

            File.WriteAllText(json_folder + json_filename, json);

            CommandHandlerHelper.WriteOutputInfo(session, $"Converted {sql_filename} to {json_filename}");

            return true;
        }

        public static bool sql2json_recipe(Session session, List<CookBook> cookbooks, string sql_folder, string sql_filename)
        {
            if (!GDLEConverter.TryConvert(cookbooks, out var result))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to convert {sql_filename} to json");
                return false;
            }

            var json_folder = sql_folder.Replace("sql", "json");
            var json_filename = sql_filename.Replace(".sql", ".json");

            var match = Regex.Match(json_filename, @"^(\d+)");
            if (match.Success)
            {
                var wcid = match.Groups[1].Value;
                if (!json_filename.StartsWith(wcid + " -"))
                    json_filename = wcid + " -" + json_filename.Substring(wcid.Length);
            }

            var di = new DirectoryInfo(json_folder);

            if (!di.Exists)
                di.Create();

            var json = JsonConvert.SerializeObject(result, LifestonedConverter.SerializerSettings);

            File.WriteAllText(json_folder + json_filename, json);

            CommandHandlerHelper.WriteOutputInfo(session, $"Converted {sql_filename} to {json_filename}");

            return true;
        }

        public static bool sql2json_landblock(Session session, List<LandblockInstance> instances, string sql_folder, string sql_filename)
        {
            if (!GDLEConverter.TryConvert(instances, out var result))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to convert {sql_filename} to json");
                return false;
            }

            var json_folder = sql_folder.Replace("sql", "json");
            var json_filename = sql_filename.Replace(".sql", ".json");

            var di = new DirectoryInfo(json_folder);

            if (!di.Exists)
                di.Create();

            var json = JsonConvert.SerializeObject(result, LifestonedConverter.SerializerSettings);

            File.WriteAllText(json_folder + json_filename, json);

            CommandHandlerHelper.WriteOutputInfo(session, $"Converted {sql_filename} to {json_filename}");

            return true;
        }

        public static bool sql2json_quest(Session session, Quest quest, string sql_folder, string sql_filename)
        {
            if (!GDLEConverter.TryConvert(quest, out var result))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to convert {sql_filename} to json");
                return false;
            }

            var json_folder = sql_folder.Replace("sql", "json");
            var json_filename = sql_filename.Replace(".sql", ".json");

            var di = new DirectoryInfo(json_folder);

            if (!di.Exists)
                di.Create();

            var json = JsonConvert.SerializeObject(result, LifestonedConverter.SerializerSettings);

            File.WriteAllText(json_folder + json_filename, json);

            CommandHandlerHelper.WriteOutputInfo(session, $"Converted {sql_filename} to {json_filename}");

            return true;
        }

        /// <summary>
        /// Imports an SQL file into the database
        /// </summary>
        public static void ImportSQL(string sqlFile)
        {
            var sqlCommands = File.ReadAllText(sqlFile);

            // not sure why ExecuteSqlCommand doesn't parse this correctly..
            var idx = sqlCommands.IndexOf($"/* Lifestoned Changelog:");
            if (idx != -1)
                sqlCommands = sqlCommands.Substring(0, idx);

            using (var ctx = new WorldDbContext())
                ctx.Database.ExecuteSqlCommand(sqlCommands);
        }

        public static LandblockInstanceWriter LandblockInstanceWriter;

        [CommandHandler("createinst", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Spawns a new wcid or classname as a landblock instance", "<wcid or classname>")]
        public static void HandleCreateInst(Session session, params string[] parameters)
        {
            var loc = new Position(session.Player.Location);

            var param = parameters[0];

            Weenie weenie = null;

            uint? parentGuid = null;

            var landblock = session.Player.CurrentLandblock.Id.Landblock;

            var firstStaticGuid = 0x70000000 | (uint)landblock << 12;

            if (parameters.Length > 1)
            {
                var allParams = string.Join(" ", parameters);

                var match = Regex.Match(allParams, @"-p ([\S]+) -c ([\S]+)", RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    var parentGuidStr = match.Groups[1].Value;
                    param = match.Groups[2].Value;

                    if (parentGuidStr.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                        parentGuidStr = parentGuidStr.Substring(2);

                    if (!uint.TryParse(parentGuidStr, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var _parentGuid))
                    {
                        session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't parse parent guid {match.Groups[1].Value}", ChatMessageType.Broadcast));
                        return;
                    }

                    parentGuid = _parentGuid;

                    if (parentGuid <= 0xFFF)
                        parentGuid = firstStaticGuid | parentGuid;
                }

                else if (parameters[1].StartsWith("-c", StringComparison.OrdinalIgnoreCase))
                {
                    // get parent from last appraised object
                    var parent = CommandHandlerHelper.GetLastAppraisedObject(session);

                    if (parent == null)
                    {
                        session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find parent object", ChatMessageType.Broadcast));
                        return;
                    }

                    parentGuid = parent.Guid.Full;
                }
            }

            if (uint.TryParse(param, out var wcid))
                weenie = DatabaseManager.World.GetWeenie(wcid);   // wcid
            else
                weenie = DatabaseManager.World.GetWeenie(param);  // classname

            if (weenie == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find weenie {param}", ChatMessageType.Broadcast));
                return;
            }

            // clear any cached instances for this landblock
            DatabaseManager.World.ClearCachedInstancesByLandblock(landblock);

            var instances = DatabaseManager.World.GetCachedInstancesByLandblock(landblock);

            // for link mode, ensure parent guid instance exists
            WorldObject parentObj = null;
            LandblockInstance parentInstance = null;

            if (parentGuid != null)
            {
                parentInstance = instances.FirstOrDefault(i => i.Guid == parentGuid);

                if (parentInstance == null)
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find landblock instance for parent guid 0x{parentGuid:X8}", ChatMessageType.Broadcast));
                    return;
                }

                parentObj = session.Player.CurrentLandblock.GetObject(parentGuid.Value);

                if (parentObj == null)
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find parent object 0x{parentGuid:X8}", ChatMessageType.Broadcast));
                    return;
                }
            }

            var nextStaticGuid = GetNextStaticGuid(landblock, instances);

            var maxStaticGuid = firstStaticGuid | 0xFFF;

            // manually specify a start guid?
            if (parameters.Length == 2)
            {
                if (uint.TryParse(parameters[1].Replace("0x", ""), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var startGuid))
                {
                    if (startGuid <= 0xFFF)
                        startGuid = firstStaticGuid | startGuid;

                    if (startGuid < firstStaticGuid || startGuid > maxStaticGuid)
                    {
                        session.Network.EnqueueSend(new GameMessageSystemChat($"Landblock instance guid {startGuid:X8} must be between {firstStaticGuid:X8} and {maxStaticGuid:X8}", ChatMessageType.Broadcast));
                        return;
                    }

                    var existing = instances.FirstOrDefault(i => i.Guid == startGuid);

                    if (existing != null)
                    {
                        session.Network.EnqueueSend(new GameMessageSystemChat($"Landblock instance guid {startGuid:X8} already exists", ChatMessageType.Broadcast));
                        return;
                    }
                    nextStaticGuid = startGuid;
                }
            }


            if (nextStaticGuid >= maxStaticGuid)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Landblock {landblock:X4} has reached the maximum # of static guids", ChatMessageType.Broadcast));
                return;
            }

            // create and spawn object
            var entityWeenie = ACE.Database.Adapter.WeenieConverter.ConvertToEntityWeenie(weenie);

            var wo = WorldObjectFactory.CreateWorldObject(entityWeenie, new ObjectGuid(nextStaticGuid));

            if (wo == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Failed to create new object for {weenie.ClassId} - {weenie.ClassName}", ChatMessageType.Broadcast));
                return;
            }

            if (!wo.Stuck)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"{weenie.ClassId} - {weenie.ClassName} is missing PropertyBool.Stuck, cannot spawn as landblock instance", ChatMessageType.Broadcast));
                return;
            }

            // spawn as ethereal temporarily, to spawn directly on player position
            wo.Ethereal = true;
            wo.Location = new Position(loc);

            // even on flat ground, objects can sometimes fail to spawn at the player's current Z
            // Position.Z has some weird thresholds when moving around, but i guess the same logic doesn't apply when trying to spawn in...
            wo.Location.PositionZ += 0.05f;

            var isLinkChild = parentInstance != null;

            session.Network.EnqueueSend(new GameMessageSystemChat($"Creating new landblock instance {(isLinkChild ? "child object " : "")}@ {loc.ToLOCString()}\n{wo.WeenieClassId} - {wo.Name} ({nextStaticGuid:X8})", ChatMessageType.Broadcast));

            if (!wo.EnterWorld())
            {
                session.Network.EnqueueSend(new GameMessageSystemChat("Failed to spawn new object at this location", ChatMessageType.Broadcast));
                return;
            }

            // create new landblock instance
            var instance = CreateLandblockInstance(wo, isLinkChild);

            instances.Add(instance);

            if (isLinkChild)
            {
                var link = new LandblockInstanceLink();

                link.ParentGuid = parentGuid.Value;
                link.ChildGuid = wo.Guid.Full;
                link.LastModified = DateTime.Now;

                parentInstance.LandblockInstanceLink.Add(link);

                parentObj.LinkedInstances.Add(instance);

                // ActivateLinks?
                parentObj.SetLinkProperties(wo);
                parentObj.ChildLinks.Add(wo);
                wo.ParentLink = parentObj;
            }

            SyncInstances(session, landblock, instances);
        }

        /// <summary>
        /// Serializes landblock instances to XXYY.sql file,
        /// import into database, and clears the cached landblock instances
        /// </summary>
        public static void SyncInstances(Session session, ushort landblock, List<LandblockInstance> instances)
        {
            // serialize to .sql file
            var contentFolder = VerifyContentFolder(session, false);

            var sep = Path.DirectorySeparatorChar;
            var folder = new DirectoryInfo($"{contentFolder.FullName}{sep}sql{sep}encounters{sep}");

            if (!folder.Exists)
                folder.Create();

            var sqlFilename = $"{folder.FullName}{sep}{landblock:X4}.sql";

            var fileWriter = new StreamWriter(sqlFilename);

            if (LandblockInstanceWriter == null)
            {
                LandblockInstanceWriter = new LandblockInstanceWriter();
                LandblockInstanceWriter.WeenieNames = DatabaseManager.World.GetAllWeenieNames();
            }

            LandblockInstanceWriter.CreateSQLDELETEStatement(instances, fileWriter);

            fileWriter.WriteLine();

            LandblockInstanceWriter.CreateSQLINSERTStatement(instances, fileWriter);

            fileWriter.Close();

            // import into db
            ImportSQL(sqlFilename);

            // clear landblock instances for this landblock (again)
            DatabaseManager.World.ClearCachedInstancesByLandblock(landblock);
        }

        public static LandblockInstance CreateLandblockInstance(WorldObject wo, bool isLinkChild = false)
        {
            var instance = new LandblockInstance();

            instance.Guid = wo.Guid.Full;

            instance.Landblock = (int)wo.Location.Landblock;

            instance.WeenieClassId = wo.WeenieClassId;

            instance.ObjCellId = wo.Location.Cell;

            instance.OriginX = wo.Location.PositionX;
            instance.OriginY = wo.Location.PositionY;
            instance.OriginZ = wo.Location.PositionZ;

            instance.AnglesW = wo.Location.RotationW;
            instance.AnglesX = wo.Location.RotationX;
            instance.AnglesY = wo.Location.RotationY;
            instance.AnglesZ = wo.Location.RotationZ;

            instance.IsLinkChild = isLinkChild;

            instance.LastModified = DateTime.Now;

            return instance;
        }

        public static uint GetNextStaticGuid(ushort landblock, List<LandblockInstance> instances)
        {
            // TODO: eventually find gaps
            var highestLandblockInst = instances.Where(i => i.Landblock == landblock).OrderByDescending(i => i.Guid).FirstOrDefault();

            if (highestLandblockInst == null)
                return (uint)(0x70000000 | (landblock << 12));
            else
                return highestLandblockInst.Guid + 1;
        }

        [CommandHandler("removeinst", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Removes the last appraised object from the current landblock instances")]
        public static void HandleRemoveInst(Session session, params string[] parameters)
        {
            RemoveInstance(session);
        }

        public static void RemoveInstance(Session session, bool confirmed = false)
        {
            var wo = CommandHandlerHelper.GetLastAppraisedObject(session);
            if (wo == null) return;

            var landblock = (ushort)wo.Location.Landblock;

            var instances = DatabaseManager.World.GetCachedInstancesByLandblock(landblock);

            var instance = instances.FirstOrDefault(i => i.Guid == wo.Guid.Full);

            if (instance == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find landblock_instance for {wo.WeenieClassId} - {wo.Name} (0x{wo.Guid})", ChatMessageType.Broadcast));
                return;
            }

            var numChilds = instance.LandblockInstanceLink.Count;

            if (numChilds > 0 && !confirmed)
            {
                // get total numChilds iteratively
                numChilds = 0;
                foreach (var link in instance.LandblockInstanceLink)
                    numChilds += GetNumChilds(session, link, instances);

                // require confirmation for parent objects
                var msg = $"Are you sure you want to delete this parent object, and {numChilds} child object{(numChilds != 1 ? "s" : "")}?";
                session.Player.ConfirmationManager.EnqueueSend(new Confirmation_Custom(session.Player.Guid, () => RemoveInstance(session, true)), msg);
                return;
            }

            if (instance.IsLinkChild)
            {
                LandblockInstanceLink link = null;

                foreach (var parent in instances.Where(i => i.LandblockInstanceLink.Count > 0))
                {
                    link = parent.LandblockInstanceLink.FirstOrDefault(i => i.ChildGuid == instance.Guid);

                    if (link != null)
                    {
                        parent.LandblockInstanceLink.Remove(link);
                        break;
                    }
                }
                if (link == null)
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find parent link for child {wo.WeenieClassId} - {wo.Name} (0x{wo.Guid})", ChatMessageType.Broadcast));
                    return;
                }
            }

            wo.DeleteObject();

            foreach (var link in instance.LandblockInstanceLink)
                RemoveChild(session, link, instances);

            instances.Remove(instance);

            SyncInstances(session, landblock, instances);

            session.Network.EnqueueSend(new GameMessageSystemChat($"Removed {(instance.IsLinkChild ? "child " : "")}{wo.WeenieClassId} - {wo.Name} (0x{wo.Guid}) from landblock instances", ChatMessageType.Broadcast));
        }

        public static int GetNumChilds(Session session, LandblockInstanceLink link, List<LandblockInstance> instances)
        {
            var child = instances.FirstOrDefault(i => i.Guid == link.ChildGuid);

            if (child == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find child instance 0x{link.ChildGuid:X8}", ChatMessageType.Broadcast));
                return 0;
            }

            var numChilds = 1;

            foreach (var subLink in child.LandblockInstanceLink)
                numChilds += GetNumChilds(session, subLink, instances);

            return numChilds;
        }

        public static void RemoveChild(Session session, LandblockInstanceLink link, List<LandblockInstance> instances)
        {
            var child = instances.FirstOrDefault(i => i.Guid == link.ChildGuid);

            if (child == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find child instance 0x{link.ChildGuid:X8}", ChatMessageType.Broadcast));
                return;
            }

            instances.Remove(child);

            var wo = session.Player.CurrentLandblock.GetObject(child.Guid);

            if (wo != null)
            {
                wo.DeleteObject();

                session.Network.EnqueueSend(new GameMessageSystemChat($"Removed child {wo.WeenieClassId} - {wo.Name} (0x{wo.Guid}) from landblock instances", ChatMessageType.Broadcast));
            }
            else
                session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find child object for 0x{link.ChildGuid:X8}", ChatMessageType.Broadcast));

            foreach (var subLink in child.LandblockInstanceLink)
                RemoveChild(session, subLink, instances);
        }

        [CommandHandler("addenc", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Spawns a new wcid or classname in the current outdoor cell as an encounter", "<wcid or classname>")]
        public static void HandleAddEncounter(Session session, params string[] parameters)
        {
            var param = parameters[0];

            Weenie weenie = null;

            if (uint.TryParse(param, out var wcid))
                weenie = DatabaseManager.World.GetWeenie(wcid);   // wcid
            else
                weenie = DatabaseManager.World.GetWeenie(param);  // classname

            if (weenie == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find weenie {param}", ChatMessageType.Broadcast));
                return;
            }

            var pos = session.Player.Location;

            if ((pos.Cell & 0xFFFF) >= 0x100)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat("You must be outdoors to create an encounter!", ChatMessageType.Broadcast));
                return;
            }

            var cellX = (int)pos.PositionX / 24;
            var cellY = (int)pos.PositionY / 24;

            // spawn encounter
            var wo = SpawnEncounter(weenie, cellX, cellY, pos, session);

            if (wo == null) return;

            session.Network.EnqueueSend(new GameMessageSystemChat($"Creating new encounter @ landblock {pos.Landblock:X4}, cellX={cellX}, cellY={cellY}\n{wo.WeenieClassId} - {wo.Name}", ChatMessageType.Broadcast));

            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var sql = $"INSERT INTO encounter set landblock=0x{pos.Landblock:X4}, weenie_Class_Id={weenie.ClassId} /* {wo.Name} */, cell_X={cellX}, cell_Y={cellY}, last_Modified='{timestamp}';";

            Console.WriteLine(sql);

            // serialize to .sql file
            var contentFolder = VerifyContentFolder(session, false);

            var sep = Path.DirectorySeparatorChar;
            var folder = new DirectoryInfo($"{contentFolder.FullName}{sep}sql{sep}encounters{sep}");

            if (!folder.Exists)
                folder.Create();

            var sql_filename = $"{pos.Landblock:X4}.sql";

            using (var file = File.Open($"{folder.FullName}{sep}{sql_filename}", FileMode.OpenOrCreate))
            {
                file.Seek(0, SeekOrigin.End);

                using (var stream = new StreamWriter(file))
                    stream.WriteLine(sql);
            }
        }

        public static WorldObject SpawnEncounter(Weenie weenie, int cellX, int cellY, Position pos, Session session)
        {
            var wo = WorldObjectFactory.CreateNewWorldObject(weenie.ClassId);

            if (wo == null)
            {
                Console.WriteLine($"Failed to create encounter weenie");
                return null;
            }

            var xPos = Math.Clamp(cellX * 24.0f, 0.5f, 191.5f);
            var yPos = Math.Clamp(cellY * 24.0f, 0.5f, 191.5f);

            var newPos = new Physics.Common.Position();
            newPos.ObjCellID = pos.Cell;
            newPos.Frame = new Physics.Animation.AFrame(new Vector3(xPos, yPos, 0), Quaternion.Identity);
            newPos.adjust_to_outside();

            newPos.Frame.Origin.Z = session.Player.CurrentLandblock.PhysicsLandblock.GetZ(newPos.Frame.Origin);

            wo.Location = new Position(newPos.ObjCellID, newPos.Frame.Origin, newPos.Frame.Orientation);

            var sortCell = Physics.Common.LScape.get_landcell(newPos.ObjCellID) as Physics.Common.SortCell;
            if (sortCell != null && sortCell.has_building())
            {
                Console.WriteLine($"Failed to create encounter near building cell");
                return null;
            }

            if (PropertyManager.GetBool("override_encounter_spawn_rates").Item)
            {
                wo.RegenerationInterval = PropertyManager.GetDouble("encounter_regen_interval").Item;

                wo.ReinitializeHeartbeats();

                foreach (var profile in wo.Biota.BiotaPropertiesGenerator)
                    profile.Delay = (float)PropertyManager.GetDouble("encounter_delay").Item;
            }

            wo.EnterWorld();

            return wo;
        }

        [CommandHandler("export-json", AccessLevel.Developer, CommandHandlerFlag.None, 1, "Exports a weenie from database to JSON file", "<wcid>")]
        public static void HandleExportJson(Session session, params string[] parameters)
        {
            DirectoryInfo di = VerifyContentFolder(session, false);

            var sep = Path.DirectorySeparatorChar;

            if (!uint.TryParse(parameters[0], out var wcid))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"{parameters[0]} not a valid wcid");
                return;
            }

            var weenie = DatabaseManager.World.GetWeenie(wcid);
            if (weenie == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find weenie {wcid}");
                return;
            }

            if (!LifestonedConverter.TryConvertACEWeenieToLSDJSON(weenie, out var json, out var json_weenie))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to convert {weenie.ClassId} - {weenie.ClassName} to json");
                return;
            }

            var json_folder = $"{di.FullName}{sep}json{sep}weenies{sep}";

            di = new DirectoryInfo(json_folder);

            if (!di.Exists)
                di.Create();

            var json_filename = $"{weenie.ClassId} - {weenie.WeeniePropertiesString.FirstOrDefault(i => i.Type == (int)PropertyString.Name)?.Value}.json";

            if (File.Exists(json_folder + json_filename) && LifestonedLoader.AppendMetadata(json_folder + json_filename, json_weenie))
            {
                json = JsonConvert.SerializeObject(json_weenie, LifestonedConverter.SerializerSettings);
            }

            File.WriteAllText(json_folder + json_filename, json);

            CommandHandlerHelper.WriteOutputInfo(session, $"Exported {json_folder}{json_filename}");
        }

        [CommandHandler("export-sql", AccessLevel.Developer, CommandHandlerFlag.None, 1, "Exports a weenie from database to SQL file", "<wcid>")]
        public static void HandleExportSql(Session session, params string[] parameters)
        {
            DirectoryInfo di = VerifyContentFolder(session, false);

            var sep = Path.DirectorySeparatorChar;

            if (!uint.TryParse(parameters[0], out var wcid))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"{parameters[0]} not a valid wcid");
                return;
            }

            var weenie = DatabaseManager.World.GetWeenie(wcid);
            if (weenie == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find weenie {wcid}");
                return;
            }

            var sql_folder = $"{di.FullName}{sep}sql{sep}weenies{sep}";

            di = new DirectoryInfo(sql_folder);

            if (!di.Exists)
                di.Create();

            var converter = new WeenieSQLWriter();

            converter.WeenieNames = DatabaseManager.World.GetAllWeenieNames();
            converter.SpellNames = DatabaseManager.World.GetAllSpellNames();
            converter.TreasureDeath = DatabaseManager.World.GetAllTreasureDeath();
            converter.TreasureWielded = DatabaseManager.World.GetAllTreasureWielded();

            var sql_filename = converter.GetDefaultFileName(weenie);

            var writer = new StreamWriter(sql_folder + sql_filename);

            try
            {
                converter.CreateSQLDELETEStatement(weenie, writer);
                writer.WriteLine();
                converter.CreateSQLINSERTStatement(weenie, writer);
                writer.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to convert {weenie.ClassId} - {weenie.ClassName}");
                return;
            }

            CommandHandlerHelper.WriteOutputInfo(session, $"Exported {sql_folder}{sql_filename}");
        }

        [CommandHandler("clearcache", AccessLevel.Developer, CommandHandlerFlag.None, "Clears the various database caches. This enables live editing of the database information")]
        public static void HandleClearCache(Session session, params string[] parameters)
        {
            var mode = CacheType.All;
            if (parameters.Length > 0)
            {
                if (parameters[0].Contains("weenie", StringComparison.OrdinalIgnoreCase))
                    mode = CacheType.Weenie;
                if (parameters[0].Contains("spell", StringComparison.OrdinalIgnoreCase))
                    mode = CacheType.Spell;
            }

            if (mode.HasFlag(CacheType.Weenie))
            {
                CommandHandlerHelper.WriteOutputInfo(session, "Clearing weenie cache");
                DatabaseManager.World.ClearWeenieCache();
            }

            if (mode.HasFlag(CacheType.Spell))
            {
                CommandHandlerHelper.WriteOutputInfo(session, "Clearing spell cache");
                DatabaseManager.World.ClearSpellCache();
                WorldObject.ClearSpellCache();
            }
        }

        [Flags]
        public enum CacheType
        {
            None   = 0x0,
            Weenie = 0x1,
            Spell  = 0x2,
            All    = 0xFFFF
        };

        public static FileType GetFileType(string filename)
        {
            if (filename.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                return GetJsonFileType(filename);
            }
            else if (filename.EndsWith(".sql", StringComparison.OrdinalIgnoreCase))
            {
                return GetSQLFileType(filename);
            }
            return FileType.Undefined;
        }

        public static FileType GetJsonFileType(string filename)
        {
            if (!File.Exists(filename))
                return FileType.Undefined;

            // can possibly be indented format
            var json = File.ReadAllText(filename);

            if (json.Contains("\"wcid\":"))
                return FileType.Weenie;
            else if (json.Contains("\"recipe\":"))
                return FileType.Recipe;
            else
                return FileType.Undefined;
        }

        public static FileType GetSQLFileType(string filename)
        {
            if (!File.Exists(filename))
                return FileType.Undefined;

            using (var streamReader = new StreamReader(filename))
            {
                var line = streamReader.ReadLine();

                while (line != null)
                {
                    if (line.Trim().Length == 0)
                        continue;

                    if (line.Contains("`encounter`"))
                        return FileType.Encounter;
                    else if (line.Contains("`landblock_instance`"))
                        return FileType.LandblockInstance;
                    else if (line.Contains("`quest`"))
                        return FileType.Quest;
                    else if (line.Contains("`recipe`"))
                        return FileType.Recipe;
                    else if (line.Contains("`spell`"))
                        return FileType.Spell;
                    else if (line.Contains("`weenie`"))
                        return FileType.Weenie;
                    else
                        break;
                }
                return FileType.Undefined;
            }
        }
    }
}
