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
using ACE.Server.Physics.Extensions;
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

        public static FileType GetContentType(string[] parameters, ref string param)
        {
            for (var i = 0; i < parameters.Length; i++)
            {
                var otherIdx = i == 0 ? 1 : 0;

                param = parameters[otherIdx];

                var fileType = parameters[i].ToLower();

                if (fileType.StartsWith("landblock"))
                    return FileType.LandblockInstance;
                else if (fileType.StartsWith("quest"))
                    return FileType.Quest;
                else if (fileType.StartsWith("recipe"))
                    return FileType.Recipe;
                else if (fileType.StartsWith("weenie"))
                    return FileType.Weenie;
            }
            return FileType.Undefined;
        }

        [CommandHandler("import-json", AccessLevel.Developer, CommandHandlerFlag.None, 1, "Imports json data from the Content folder", "<wcid>")]
        public static void HandleImportJson(Session session, params string[] parameters)
        {
            var param = parameters[0];
            var contentType = FileType.Weenie;

            if (parameters.Length > 1)
            {
                contentType = GetContentType(parameters, ref param);

                if (contentType == FileType.Undefined)
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"Unknown content type '{parameters[1]}'");
                    return;
                }
            }
            switch (contentType)
            {
                case FileType.LandblockInstance:
                    ImportJsonLandblock(session, param);
                    break;

                case FileType.Quest:
                    ImportJsonQuest(session, param);
                    break;

                case FileType.Recipe:
                    ImportJsonRecipe(session, param);
                    break;

                case FileType.Weenie:
                    ImportJsonWeenie(session, param);
                    break;
            }
        }

        public static void ImportJsonWeenie(Session session, string wcid)
        {
            DirectoryInfo di = VerifyContentFolder(session);
            if (!di.Exists) return;

            var sep = Path.DirectorySeparatorChar;

            var json_folder = $"{di.FullName}{sep}json{sep}weenies{sep}";

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

        public static void ImportJsonRecipe(Session session, string recipeId)
        {
            DirectoryInfo di = VerifyContentFolder(session);
            if (!di.Exists) return;

            var sep = Path.DirectorySeparatorChar;

            var json_folder = $"{di.FullName}{sep}json{sep}recipes{sep}";

            var prefix = recipeId.PadLeft(5, '0') + " - ";

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

        public static void ImportJsonLandblock(Session session, string landblockId)
        {
            DirectoryInfo di = VerifyContentFolder(session);
            if (!di.Exists) return;

            var sep = Path.DirectorySeparatorChar;

            var json_folder = $"{di.FullName}{sep}json{sep}landblocks{sep}";

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

        public static void ImportJsonQuest(Session session, string questName)
        {
            DirectoryInfo di = VerifyContentFolder(session);
            if (!di.Exists) return;

            var sep = Path.DirectorySeparatorChar;

            var json_folder = $"{di.FullName}{sep}json{sep}quests{sep}";

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
            var param = parameters[0];
            var contentType = FileType.Weenie;

            if (parameters.Length > 1)
            {
                contentType = GetContentType(parameters, ref param);

                if (contentType == FileType.Undefined)
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"Unknown content type '{parameters[1]}'");
                    return;
                }
            }
            try
            {
                switch (contentType)
                {
                    case FileType.LandblockInstance:
                        ImportSQLLandblock(session, param);
                        break;

                    case FileType.Quest:
                        ImportSQLQuest(session, param);
                        break;

                    case FileType.Recipe:
                        ImportSQLRecipe(session, param);
                        break;

                    case FileType.Weenie:
                        ImportSQLWeenie(session, param);
                        break;
                }
            }
            catch(Exception e)
            {
                CommandHandlerHelper.WriteOutputError(session, $"There was an error importing the SQL:\n\n{e.Message}");
            }
        }

        public static void ImportSQLWeenie(Session session, string wcid)
        {
            DirectoryInfo di = VerifyContentFolder(session);
            if (!di.Exists) return;

            var sep = Path.DirectorySeparatorChar;

            var sql_folder = $"{di.FullName}{sep}sql{sep}weenies{sep}";

            var prefix = wcid.PadLeft(5, '0') + " ";

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

        public static void ImportSQLRecipe(Session session, string recipeId)
        {
            DirectoryInfo di = VerifyContentFolder(session);
            if (!di.Exists) return;

            var sep = Path.DirectorySeparatorChar;

            var sql_folder = $"{di.FullName}{sep}sql{sep}recipes{sep}";

            var prefix = recipeId.PadLeft(5, '0') + " ";

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

        public static void ImportSQLLandblock(Session session, string landblockId)
        {
            DirectoryInfo di = VerifyContentFolder(session);
            if (!di.Exists) return;

            var sep = Path.DirectorySeparatorChar;

            var sql_folder = $"{di.FullName}{sep}sql{sep}landblocks{sep}";

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

        public static void ImportSQLQuest(Session session, string questName)
        {
            DirectoryInfo di = VerifyContentFolder(session);
            if (!di.Exists) return;

            var sep = Path.DirectorySeparatorChar;

            var sql_folder = $"{di.FullName}{sep}sql{sep}quests{sep}";

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
            if (!ushort.TryParse(Regex.Match(json_file, @"[0-9A-F]{4}", RegexOptions.IgnoreCase).Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var landblockId))
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
            var cookbooks = DatabaseManager.World.GetCookbooksByRecipeId(recipeId);

            if (cookbooks == null || cookbooks.Count == 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't load recipe {recipeId} from db");
                return;
            }

            sql2json_recipe(session, cookbooks, sql_folder, sql_file);
        }

        private static void ImportSQLLandblock(Session session, string sql_folder, string sql_file)
        {
            if (!ushort.TryParse(Regex.Match(sql_file, @"[0-9A-F]{4}", RegexOptions.IgnoreCase).Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var landblockId))
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
            if (GDLEConverter.WeenieNames == null)
                GDLEConverter.WeenieNames = DatabaseManager.World.GetAllWeenieNames();

            if (GDLEConverter.WeenieClassNames == null)
                GDLEConverter.WeenieClassNames = DatabaseManager.World.GetAllWeenieClassNames();

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

            sqlCommands = sqlCommands.Replace("\r\n", "\n");

            // not sure why ExecuteSqlCommand doesn't parse this correctly..
            var idx = sqlCommands.IndexOf($"/* Lifestoned Changelog:");
            if (idx != -1)
                sqlCommands = sqlCommands.Substring(0, idx);

            using (var ctx = new WorldDbContext())
                ctx.Database.ExecuteSqlRaw(sqlCommands);
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

            if (nextStaticGuid > maxStaticGuid)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Landblock {landblock:X4} has reached the maximum # of static guids", ChatMessageType.Broadcast));
                return;
            }

            // create and spawn object
            var entityWeenie = Database.Adapter.WeenieConverter.ConvertToEntityWeenie(weenie);

            var wo = WorldObjectFactory.CreateWorldObject(entityWeenie, new ObjectGuid(nextStaticGuid));

            if (wo == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Failed to create new object for {weenie.ClassId} - {weenie.ClassName}", ChatMessageType.Broadcast));
                return;
            }

            var isLinkChild = parentInstance != null;

            if (!wo.Stuck && !isLinkChild)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"{weenie.ClassId} - {weenie.ClassName} is missing PropertyBool.Stuck, cannot spawn as landblock instance unless it is a child object", ChatMessageType.Broadcast));
                return;
            }

            // spawn as ethereal temporarily, to spawn directly on player position
            wo.Ethereal = true;
            wo.Location = new Position(loc);

            // even on flat ground, objects can sometimes fail to spawn at the player's current Z
            // Position.Z has some weird thresholds when moving around, but i guess the same logic doesn't apply when trying to spawn in...
            wo.Location.PositionZ += 0.05f;

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
            var folder = new DirectoryInfo($"{contentFolder.FullName}{sep}sql{sep}landblocks{sep}");

            if (!folder.Exists)
                folder.Create();

            var sqlFilename = $"{folder.FullName}{sep}{landblock:X4}.sql";

            if (instances.Count > 0)
            {
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
            }
            else
            {
                // handle special case: deleting the last instance from landblock
                File.Delete(sqlFilename);

                using (var ctx = new WorldDbContext())
                    ctx.Database.ExecuteSqlRaw($"DELETE FROM landblock_instance WHERE landblock={landblock};");
            }

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
            var firstGuid = 0x70000000 | ((uint)landblock << 12);
            var lastGuid = firstGuid | 0xFFF;

            var highestLandblockInst = instances.Where(i => i.Landblock == landblock).OrderByDescending(i => i.Guid).FirstOrDefault();

            if (highestLandblockInst == null)
                return firstGuid;

            var nextGuid = highestLandblockInst.Guid + 1;

            if (nextGuid <= lastGuid)
                return nextGuid;

            // try more exhaustive search
            return GetNextStaticGuid_GapFinder(landblock, instances) ?? nextGuid;
        }

        public static uint? GetNextStaticGuid_GapFinder(ushort landblock, List<LandblockInstance> instances)
        {
            var landblockGuids = instances.Where(i => i.Landblock == landblock).Select(i => i.Guid).ToHashSet();

            var firstGuid = 0x70000000 | ((uint)landblock << 12);
            var lastGuid = firstGuid | 0xFFF;

            for (var guid = firstGuid; guid <= lastGuid; guid++)
            {
                if (!landblockGuids.Contains(guid))
                    return guid;
            }
            return null;
        }

        [CommandHandler("removeinst", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Removes the last appraised object from the current landblock instances")]
        public static void HandleRemoveInst(Session session, params string[] parameters)
        {
            RemoveInstance(session);
        }

        public static void RemoveInstance(Session session, bool confirmed = false)
        {
            var wo = CommandHandlerHelper.GetLastAppraisedObject(session);

            if (wo?.Location == null) return;

            var landblock = (ushort)wo.Location.Landblock;

            // if generator child, try getting the "real" guid
            var guid = wo.Guid.Full;
            if (wo.Generator != null)
            {
                var staticGuid = wo.Generator.GetStaticGuid(guid);
                if (staticGuid != null)
                    guid = staticGuid.Value;
            }

            var instances = DatabaseManager.World.GetCachedInstancesByLandblock(landblock);

            var instance = instances.FirstOrDefault(i => i.Guid == guid);

            if (instance == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find landblock_instance for {wo.WeenieClassId} - {wo.Name} (0x{guid:X8})", ChatMessageType.Broadcast));
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
                if (!session.Player.ConfirmationManager.EnqueueSend(new Confirmation_Custom(session.Player.Guid, () => RemoveInstance(session, true)), msg))
                    session.Player.SendWeenieError(WeenieError.ConfirmationInProgress);
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
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find parent link for child {wo.WeenieClassId} - {wo.Name} (0x{guid:X8})", ChatMessageType.Broadcast));
                    return;
                }
            }

            wo.DeleteObject();

            foreach (var link in instance.LandblockInstanceLink)
                RemoveChild(session, link, instances);

            instances.Remove(instance);

            SyncInstances(session, landblock, instances);

            session.Network.EnqueueSend(new GameMessageSystemChat($"Removed {(instance.IsLinkChild ? "child " : "")}{wo.WeenieClassId} - {wo.Name} (0x{guid:X8}) from landblock instances", ChatMessageType.Broadcast));
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

        public static EncounterSQLWriter LandblockEncounterWriter;

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

            var landblock = (ushort)pos.Landblock;

            // clear any cached encounters for this landblock
            DatabaseManager.World.ClearCachedEncountersByLandblock(landblock);

            // get existing encounters for this landblock
            var encounters = DatabaseManager.World.GetCachedEncountersByLandblock(landblock);

            // check for existing encounter
            if (encounters.Any(i => i.CellX == cellX && i.CellY == cellY))
            {
                session.Network.EnqueueSend(new GameMessageSystemChat("This cell already contains an encounter!", ChatMessageType.Broadcast));
                return;
            }

            // spawn encounter
            var wo = SpawnEncounter(weenie, cellX, cellY, pos, session);

            if (wo == null) return;

            session.Network.EnqueueSend(new GameMessageSystemChat($"Creating new encounter @ landblock {pos.Landblock:X4}, cellX={cellX}, cellY={cellY}\n{wo.WeenieClassId} - {wo.Name}", ChatMessageType.Broadcast));

            // add a new encounter (verifications?)
            var encounter = new Encounter();
            encounter.Landblock = (int)pos.Landblock;
            encounter.CellX = cellX;
            encounter.CellY = cellY;
            encounter.WeenieClassId = weenie.ClassId;
            encounter.LastModified = DateTime.Now;

            encounters.Add(encounter);

            // write encounters to sql file / load into db
            SyncEncounters(session, landblock, encounters);
        }

        public static WorldObject SpawnEncounter(Weenie weenie, int cellX, int cellY, Position pos, Session session)
        {
            var wo = WorldObjectFactory.CreateNewWorldObject(weenie.ClassId);

            if (wo == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Failed to create encounter weenie", ChatMessageType.Broadcast));
                return null;
            }

            if (!wo.IsGenerator)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Encounter must be a Generator", ChatMessageType.Broadcast));
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
                session.Network.EnqueueSend(new GameMessageSystemChat($"Failed to create encounter near building cell", ChatMessageType.Broadcast));
                return null;
            }

            if (PropertyManager.GetBool("override_encounter_spawn_rates").Item)
            {
                wo.RegenerationInterval = PropertyManager.GetDouble("encounter_regen_interval").Item;

                wo.ReinitializeHeartbeats();

                if (wo.Biota.PropertiesGenerator != null)
                {
                    // While this may be ugly, it's done for performance reasons.
                    // Common weenie properties are not cloned into the bota on creation. Instead, the biota references simply point to the weenie collections.
                    // The problem here is that we want to update one of those common collection properties. If the biota is referencing the weenie collection,
                    // then we'll end up updating the global weenie (from the cache), instead of just this specific biota.
                    if (wo.Biota.PropertiesGenerator == wo.Weenie.PropertiesGenerator)
                    {
                        wo.Biota.PropertiesGenerator = new List<ACE.Entity.Models.PropertiesGenerator>(wo.Weenie.PropertiesGenerator.Count);

                        foreach (var record in wo.Weenie.PropertiesGenerator)
                            wo.Biota.PropertiesGenerator.Add(record.Clone());
                    }
                }
            }

            var success = wo.EnterWorld();

            if (!success)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Failed to spawn encounter", ChatMessageType.Broadcast));
                return null;
            }
            return wo;
        }

        /// <summary>
        /// Serializes encounters to XXYY.sql file,
        /// import into database, and clears the cached encounters
        /// </summary>
        public static void SyncEncounters(Session session, ushort landblock, List<Encounter> encounters)
        {
            // serialize to .sql file
            var contentFolder = VerifyContentFolder(session, false);

            var sep = Path.DirectorySeparatorChar;
            var folder = new DirectoryInfo($"{contentFolder.FullName}{sep}sql{sep}encounters{sep}");

            if (!folder.Exists)
                folder.Create();

            var sqlFilename = $"{folder.FullName}{sep}{landblock:X4}.sql";

            if (encounters.Count > 0)
            {
                var fileWriter = new StreamWriter(sqlFilename);

                if (LandblockEncounterWriter == null)
                {
                    LandblockEncounterWriter = new EncounterSQLWriter();
                    LandblockEncounterWriter.WeenieNames = DatabaseManager.World.GetAllWeenieNames();
                }

                LandblockEncounterWriter.CreateSQLDELETEStatement(encounters, fileWriter);

                fileWriter.WriteLine();

                LandblockEncounterWriter.CreateSQLINSERTStatement(encounters, fileWriter);

                fileWriter.Close();

                // import into db
                ImportSQL(sqlFilename);
            }
            else
            {
                // handle special case: deleting the last encounter from landblock
                File.Delete(sqlFilename);

                using (var ctx = new WorldDbContext())
                    ctx.Database.ExecuteSqlRaw($"DELETE FROM encounter WHERE landblock={landblock};");
            }

            // clear the encounters for this landblock (again)
            DatabaseManager.World.ClearCachedEncountersByLandblock(landblock);
        }

        [CommandHandler("removeenc", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Removes the last appraised object from the encounters table")]
        public static void HandleRemoveEnc(Session session, params string[] parameters)
        {
            var obj = CommandHandlerHelper.GetLastAppraisedObject(session);

            if (obj == null)
                return;

            // find root generator
            while (obj.Generator != null)
                obj = obj.Generator;

            var cellX = (int)obj.Location.PositionX / 24;
            var cellY = (int)obj.Location.PositionY / 24;

            var landblock = (ushort)obj.Location.Landblock;

            // clear any cached encounters for this landblock
            DatabaseManager.World.ClearCachedEncountersByLandblock(landblock);

            // get existing encounters for this landblock
            var encounters = DatabaseManager.World.GetCachedEncountersByLandblock(landblock);

            // check for existing encounter
            var encounter = encounters.FirstOrDefault(i => i.CellX == cellX && i.CellY == cellY && i.WeenieClassId == obj.WeenieClassId);

            if (encounter == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find encounter for {obj.WeenieClassId} - {obj.Name}", ChatMessageType.Broadcast));
                return;
            }

            session.Network.EnqueueSend(new GameMessageSystemChat($"Removing encounter @ landblock {obj.Location.Landblock:X4}, cellX={cellX}, cellY={cellY}\n{obj.WeenieClassId} - {obj.Name}", ChatMessageType.Broadcast));

            encounters.Remove(encounter);

            SyncEncounters(session, landblock, encounters);

            // this is needed for any generators that don't have GeneratorDestructionType
            DestroyAll(obj);
        }

        /// <summary>
        /// Destroys a parent generator, and all of its child objects
        /// </summary>
        private static void DestroyAll(WorldObject wo)
        {
            wo.Destroy();

            if (wo.GeneratorProfiles == null)
                return;

            foreach (var profile in wo.GeneratorProfiles)
            {
                foreach (var kvp in profile.Spawned)
                {
                    var child = kvp.Value.TryGetWorldObject();

                    if (child != null)
                        DestroyAll(child);
                }
            }
        }

        [CommandHandler("export-json-folders", AccessLevel.Developer, CommandHandlerFlag.None, 1, "Exports content from database to JSON file in a WeenieType/ItemType folder structure", "<wcid>")]
        public static void HandleExportJsonFolder(Session session, params string[] parameters)
        {
            var param = parameters[0];
            ExportJsonWeenie(session, param, true);
        }

        [CommandHandler("export-json", AccessLevel.Developer, CommandHandlerFlag.None, 1, "Exports content from database to JSON file", "<wcid>")]
        public static void HandleExportJson(Session session, params string[] parameters)
        {
            var param = parameters[0];
            var contentType = FileType.Weenie;

            if (parameters.Length > 1)
            {
                contentType = GetContentType(parameters, ref param);

                if (contentType == FileType.Undefined)
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"Unknown content type '{parameters[1]}'");
                    return;
                }
            }
            switch (contentType)
            {
                case FileType.LandblockInstance:
                    ExportJsonLandblock(session, param);
                    break;

                case FileType.Quest:
                    ExportJsonQuest(session, param);
                    break;

                case FileType.Recipe:
                    ExportJsonRecipe(session, param);
                    break;

                case FileType.Weenie:
                    ExportJsonWeenie(session, param);
                    break;
            }
        }

        public static void ExportJsonWeenie(Session session, string param, bool withFolders = false)
        {
            DirectoryInfo di = VerifyContentFolder(session, false);

            var sep = Path.DirectorySeparatorChar;

            Weenie weenie = null;

            if (uint.TryParse(param, out var wcid))
                weenie = DatabaseManager.World.GetWeenie(wcid);
            else
                weenie = DatabaseManager.World.GetWeenie(param);

            if (weenie == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find weenie {param}");
                return;
            }

            if (!LifestonedConverter.TryConvertACEWeenieToLSDJSON(weenie, out var json, out var json_weenie))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to convert {weenie.ClassId} - {weenie.ClassName} to json");
                return;
            }

            string json_folder = null;
            if (withFolders)
            {
                var weenieType = (WeenieType)weenie.Type;
                switch (weenieType)
                {
                    case WeenieType.Creature: // Export to the "CreatureType" folder
                        WeeniePropertiesInt cType = (from x in weenie.WeeniePropertiesInt where x.Type == 2 select x).FirstOrDefault();
                        if (cType == null)
                            json_folder = $"{di.FullName}{sep}json{sep}weenies{sep}{weenieType}{sep}";
                        else
                        {
                            CreatureType creatureType = (CreatureType)cType.Value;
                            json_folder = $"{di.FullName}{sep}json{sep}weenies{sep}{weenieType}{sep}{creatureType}{sep}";
                        }
                        break;
                    default: // Otherwise goes to "ItemType" folder
                        WeeniePropertiesInt iType = (from x in weenie.WeeniePropertiesInt where x.Type == 1 select x).FirstOrDefault();
                        if (iType == null)
                            json_folder = $"{di.FullName}{sep}json{sep}weenies{sep}{weenieType}{sep}";
                        else
                        {
                            ItemType itemType = (ItemType)iType.Value;
                            json_folder = $"{di.FullName}{sep}json{sep}weenies{sep}{weenieType}{sep}{itemType}{sep}";
                        }
                        break;
                }
            }
            else
            {
                json_folder = $"{di.FullName}{sep}json{sep}weenies{sep}";
            }
            
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

        public static void ExportJsonRecipe(Session session, string param)
        {
            DirectoryInfo di = VerifyContentFolder(session, false);

            var sep = Path.DirectorySeparatorChar;

            if (!uint.TryParse(param, out var recipeId))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"{param} not a valid recipe id");
                return;
            }

            var cookbooks = DatabaseManager.World.GetCookbooksByRecipeId(recipeId);
            if (cookbooks == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find recipe id {recipeId}");
                return;
            }

            if (!GDLEConverter.TryConvert(cookbooks, out var result))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to convert recipe id {recipeId} to json");
                return;
            }

            var json_folder = $"{di.FullName}{sep}json{sep}recipes{sep}";

            di = new DirectoryInfo(json_folder);

            if (!di.Exists)
                di.Create();

            if (RecipeSQLWriter == null)
            {
                RecipeSQLWriter = new RecipeSQLWriter();
                RecipeSQLWriter.WeenieNames = DatabaseManager.World.GetAllWeenieNames();
            }

            var desc = RecipeSQLWriter.GetDefaultFileName(cookbooks[0].Recipe, cookbooks, true);

            var json_filename = $"{recipeId.ToString("00000")} - {desc}.json";

            var json = JsonConvert.SerializeObject(result, LifestonedConverter.SerializerSettings);

            File.WriteAllText(json_folder + json_filename, json);

            CommandHandlerHelper.WriteOutputInfo(session, $"Exported {json_folder}{json_filename}");
        }

        public static void ExportJsonLandblock(Session session, string param)
        {
            DirectoryInfo di = VerifyContentFolder(session, false);

            var sep = Path.DirectorySeparatorChar;

            if (!ushort.TryParse(Regex.Match(param, @"[0-9A-F]{4}", RegexOptions.IgnoreCase).Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var landblockId))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"{param} not a valid landblock");
                return;
            }

            var instances = DatabaseManager.World.GetCachedInstancesByLandblock(landblockId);
            if (instances == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find landblock {landblockId:X4}");
                return;
            }

            if (GDLEConverter.WeenieNames == null)
                GDLEConverter.WeenieNames = DatabaseManager.World.GetAllWeenieNames();

            if (GDLEConverter.WeenieClassNames == null)
                GDLEConverter.WeenieClassNames = DatabaseManager.World.GetAllWeenieClassNames();

            if (!GDLEConverter.TryConvert(instances, out var result))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to convert landblock {landblockId:X4} to json");
                return;
            }

            var json_folder = $"{di.FullName}{sep}json{sep}landblocks{sep}";

            di = new DirectoryInfo(json_folder);

            if (!di.Exists)
                di.Create();

            var json_filename = $"{landblockId:X4}.json";

            var json = JsonConvert.SerializeObject(result, LifestonedConverter.SerializerSettings);

            File.WriteAllText(json_folder + json_filename, json);

            CommandHandlerHelper.WriteOutputInfo(session, $"Exported {json_folder}{json_filename}");
        }

        public static void ExportJsonQuest(Session session, string questName)
        {
            DirectoryInfo di = VerifyContentFolder(session, false);

            var sep = Path.DirectorySeparatorChar;

            var quest = DatabaseManager.World.GetCachedQuest(questName);

            if (quest == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find quest {questName}");
                return;
            }

            if (!GDLEConverter.TryConvert(quest, out var result))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to convert quest {questName} to json");
                return;
            }

            var json_folder = $"{di.FullName}{sep}json{sep}quests{sep}";

            di = new DirectoryInfo(json_folder);

            if (!di.Exists)
                di.Create();

            var json_filename = $"{questName}.json";

            var json = JsonConvert.SerializeObject(result, LifestonedConverter.SerializerSettings);

            File.WriteAllText(json_folder + json_filename, json);

            CommandHandlerHelper.WriteOutputInfo(session, $"Exported {json_folder}{json_filename}");
        }

        [CommandHandler("export-sql-folders", AccessLevel.Developer, CommandHandlerFlag.None, 1, "Exports weenie content from database to an SQL file in a WeenieType/ItemType folder structure", "<wcid>")]
        public static void HandleExportSqlFolder(Session session, params string[] parameters)
        {
            var param = parameters[0];
            ExportSQLWeenie(session, param, true);
        }

        [CommandHandler("export-sql", AccessLevel.Developer, CommandHandlerFlag.None, 1, "Exports content from database to SQL file", "<wcid> [content-type]")]
        public static void HandleExportSql(Session session, params string[] parameters)
        {
            var param = parameters[0];
            var contentType = FileType.Weenie;

            if (parameters.Length > 1)
            {
                contentType = GetContentType(parameters, ref param);

                if (contentType == FileType.Undefined)
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"Unknown content type '{parameters[1]}'");
                    return;
                }
            }
            switch (contentType)
            {
                case FileType.LandblockInstance:
                    ExportSQLLandblock(session, param);
                    break;

                case FileType.Quest:
                    ExportSQLQuest(session, param);
                    break;

                case FileType.Recipe:
                    ExportSQLRecipe(session, param);
                    break;

                case FileType.Weenie:
                    ExportSQLWeenie(session, param);
                    break;
            }
        }

        public static void ExportSQLWeenie(Session session, string param, bool withFolders = false)
        {
            DirectoryInfo di = VerifyContentFolder(session, false);

            var sep = Path.DirectorySeparatorChar;

            Weenie weenie = null;

            if (uint.TryParse(param, out var wcid))
                weenie = DatabaseManager.World.GetWeenie(wcid);
            else
                weenie = DatabaseManager.World.GetWeenie(param);

            if (weenie == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find weenie {param}");
                return;
            }

            string sql_folder = null;
            if (withFolders)
            {
                var weenieType = (WeenieType)weenie.Type;
                switch (weenieType)
                {
                    case WeenieType.Creature: // Export to the "CreatureType" folder
                        WeeniePropertiesInt cType = (from x in weenie.WeeniePropertiesInt where x.Type == 2 select x).FirstOrDefault();
                        if (cType == null)
                            sql_folder = $"{di.FullName}{sep}sql{sep}weenies{sep}{weenieType}{sep}";
                        else
                        {
                            CreatureType creatureType = (CreatureType)cType.Value;
                            sql_folder = $"{di.FullName}{sep}sql{sep}weenies{sep}{weenieType}{sep}{creatureType}{sep}";
                        }
                        break;
                    default: // Otherwise goes to "ItemType" folder
                        WeeniePropertiesInt iType = (from x in weenie.WeeniePropertiesInt where x.Type == 1 select x).FirstOrDefault();
                        if (iType == null)
                            sql_folder = $"{di.FullName}{sep}sql{sep}weenies{sep}{weenieType}{sep}";
                        else
                        {
                            ItemType itemType = (ItemType)iType.Value;
                            sql_folder = $"{di.FullName}{sep}sql{sep}weenies{sep}{weenieType}{sep}{itemType}{sep}";
                        }
                        break;
                }
            }
            else
            {
                sql_folder = $"{di.FullName}{sep}sql{sep}weenies{sep}";
            }

            di = new DirectoryInfo(sql_folder);

            if (!di.Exists)
                di.Create();

            if (WeenieSQLWriter == null)
            {
                WeenieSQLWriter = new WeenieSQLWriter();
                WeenieSQLWriter.WeenieNames = DatabaseManager.World.GetAllWeenieNames();
                WeenieSQLWriter.SpellNames = DatabaseManager.World.GetAllSpellNames();
                WeenieSQLWriter.TreasureDeath = DatabaseManager.World.GetAllTreasureDeath();
                WeenieSQLWriter.TreasureWielded = DatabaseManager.World.GetAllTreasureWielded();
                WeenieSQLWriter.PacketOpCodes = PacketOpCodeNames.Values;
            }

            var sql_filename = WeenieSQLWriter.GetDefaultFileName(weenie);

            var writer = new StreamWriter(sql_folder + sql_filename);

            try
            {
                WeenieSQLWriter.CreateSQLDELETEStatement(weenie, writer);
                writer.WriteLine();
                WeenieSQLWriter.CreateSQLINSERTStatement(weenie, writer);
                writer.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to export {sql_folder}{sql_filename}");
                return;
            }

            CommandHandlerHelper.WriteOutputInfo(session, $"Exported {sql_folder}{sql_filename}");
        }

        public static void ExportSQLRecipe(Session session, string param)
        {
            DirectoryInfo di = VerifyContentFolder(session, false);

            var sep = Path.DirectorySeparatorChar;

            if (!uint.TryParse(param, out var recipeId))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"{param} not a valid recipe id");
                return;
            }

            var cookbooks = DatabaseManager.World.GetCookbooksByRecipeId(recipeId);
            if (cookbooks == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find recipe id {recipeId}");
                return;
            }

            var sql_folder = $"{di.FullName}{sep}sql{sep}recipes{sep}";

            di = new DirectoryInfo(sql_folder);

            if (!di.Exists)
                di.Create();

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

            // same recipe for all cookbooks
            var recipe = cookbooks[0].Recipe;

            var sql_filename = RecipeSQLWriter.GetDefaultFileName(recipe, cookbooks);

            try
            {
                var sqlFile = new StreamWriter(sql_folder + sql_filename);

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
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to export {sql_folder}{sql_filename}");
                return;
            }

            CommandHandlerHelper.WriteOutputInfo(session, $"Exported {sql_folder}{sql_filename}");
        }

        public static void ExportSQLLandblock(Session session, string param)
        {
            DirectoryInfo di = VerifyContentFolder(session, false);

            var sep = Path.DirectorySeparatorChar;

            if (!ushort.TryParse(Regex.Match(param, @"[0-9A-F]{4}", RegexOptions.IgnoreCase).Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var landblockId))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"{param} not a valid landblock");
                return;
            }

            var instances = DatabaseManager.World.GetCachedInstancesByLandblock(landblockId);
            if (instances == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find landblock {landblockId:X4}");
                return;
            }

            var sql_folder = $"{di.FullName}{sep}sql{sep}landblocks{sep}";

            di = new DirectoryInfo(sql_folder);

            if (!di.Exists)
                di.Create();

            var sql_filename = $"{landblockId:X4}.sql";

            try
            {
                if (LandblockInstanceWriter == null)
                {
                    LandblockInstanceWriter = new LandblockInstanceWriter();
                    LandblockInstanceWriter.WeenieNames = DatabaseManager.World.GetAllWeenieNames();
                }

                var sqlFile = new StreamWriter(sql_folder + sql_filename);

                LandblockInstanceWriter.CreateSQLDELETEStatement(instances, sqlFile);
                sqlFile.WriteLine();

                LandblockInstanceWriter.CreateSQLINSERTStatement(instances, sqlFile);

                sqlFile.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to export {sql_folder}{sql_filename}");
                return;
            }

            CommandHandlerHelper.WriteOutputInfo(session, $"Exported {sql_folder}{sql_filename}");
        }

        public static void ExportSQLQuest(Session session, string questName)
        {
            DirectoryInfo di = VerifyContentFolder(session, false);

            var sep = Path.DirectorySeparatorChar;

            var quest = DatabaseManager.World.GetCachedQuest(questName);

            if (quest == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find quest {questName}");
                return;
            }

            var sql_folder = $"{di.FullName}{sep}sql{sep}quests{sep}";

            di = new DirectoryInfo(sql_folder);

            if (!di.Exists)
                di.Create();

            if (QuestSQLWriter == null)
                QuestSQLWriter = new QuestSQLWriter();

            var sql_filename = questName + ".sql";

            try
            {
                var sqlFile = new StreamWriter(sql_folder + sql_filename);

                QuestSQLWriter.CreateSQLDELETEStatement(quest, sqlFile);
                sqlFile.WriteLine();

                QuestSQLWriter.CreateSQLINSERTStatement(quest, sqlFile);

                sqlFile.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to export {sql_folder}{sql_filename}");
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
                if (parameters[0].Contains("landblock", StringComparison.OrdinalIgnoreCase))
                    mode = CacheType.Landblock;
                if (parameters[0].Contains("recipe", StringComparison.OrdinalIgnoreCase))
                    mode = CacheType.Recipe;
                if (parameters[0].Contains("spell", StringComparison.OrdinalIgnoreCase))
                    mode = CacheType.Spell;
                if (parameters[0].Contains("weenie", StringComparison.OrdinalIgnoreCase))
                    mode = CacheType.Weenie;
                if (parameters[0].Contains("wield", StringComparison.OrdinalIgnoreCase))
                    mode = CacheType.WieldedTreasure;
            }

            if (mode.HasFlag(CacheType.Landblock))
            {
                CommandHandlerHelper.WriteOutputInfo(session, "Clearing landblock instance cache");
                DatabaseManager.World.ClearCachedLandblockInstances();
            }

            if (mode.HasFlag(CacheType.Recipe))
            {
                CommandHandlerHelper.WriteOutputInfo(session, "Clearing recipe cache");
                DatabaseManager.World.ClearCookbookCache();
            }

            if (mode.HasFlag(CacheType.Spell))
            {
                CommandHandlerHelper.WriteOutputInfo(session, "Clearing spell cache");
                DatabaseManager.World.ClearSpellCache();
                WorldObject.ClearSpellCache();
            }

            if (mode.HasFlag(CacheType.Weenie))
            {
                CommandHandlerHelper.WriteOutputInfo(session, "Clearing weenie cache");
                DatabaseManager.World.ClearWeenieCache();
            }

            if (mode.HasFlag(CacheType.WieldedTreasure))
            {
                CommandHandlerHelper.WriteOutputInfo(session, "Clearing wielded treasure cache");
                DatabaseManager.World.ClearWieldedTreasureCache();
            }
        }

        [Flags]
        public enum CacheType
        {
            None            = 0x0,
            Landblock       = 0x1,
            Recipe          = 0x2,
            Spell           = 0x4,
            Weenie          = 0x8,
            WieldedTreasure = 0x10,
            All             = 0xFFFF
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

        [CommandHandler("nudge", AccessLevel.Developer, CommandHandlerFlag.None, 1, "Adjusts the spawn position of a landblock instance", "<dir> <amount>\nDirections: x, y, z, north, south, west, east, northwest, northeast, southwest, southeast, n, s, w, e, nw, ne, sw, se, up, down, here")]
        public static void HandleNudge(Session session, params string[] parameters)
        {
            WorldObject obj = null;

            var curParam = 0;

            if (parameters.Length == 3)
            {
                if (!uint.TryParse(parameters[curParam++].TrimStart("0x"), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var guid))
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Invalid guid: {parameters[0]}", ChatMessageType.Broadcast));
                    return;
                }

                obj = session.Player.FindObject(guid, Player.SearchLocations.Landblock);

                if (obj == null)
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find {parameters[0]}", ChatMessageType.Broadcast));
                    return;
                }
            }
            else
                obj = CommandHandlerHelper.GetLastAppraisedObject(session);

            if (obj == null) return;

            // ensure landblock instance
            if (!obj.Guid.IsStatic())
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"{obj.Name} ({obj.Guid}) is not landblock instance", ChatMessageType.Broadcast));
                return;
            }

            if (obj.PhysicsObj == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"{obj.Name} ({obj.Guid}) is not a physics object", ChatMessageType.Broadcast));
                return;
            }

            // get direction
            var dirname = parameters[curParam++].ToLower();
            var dir = GetNudgeDir(dirname);

            bool curPos = false;

            if (dir == null)
            {
                if (dirname.Equals("here") || dirname.Equals("to me"))
                {
                    dir = Vector3.Zero;
                    curPos = true;
                }
                else
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Invalid direction: {dirname}", ChatMessageType.Broadcast));
                    return;
                }
            }

            // get distance / amount
            var amount = 1.0f;
            if (curParam < parameters.Length)
            {
                if (!float.TryParse(parameters[curParam++], out amount))
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Invalid amount: {amount}", ChatMessageType.Broadcast));
                    return;
                }
            }

            var nudge = dir * amount;

            // get landblock for static guid
            var landblock_id = (ushort)(obj.Guid.Full >> 12);

            // get instances for landblock
            var instances = DatabaseManager.World.GetCachedInstancesByLandblock(landblock_id);

            // find instance
            var instance = instances.FirstOrDefault(i => i.Guid == obj.Guid.Full);

            if (instance == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find instance for {obj.Name} ({obj.Guid})", ChatMessageType.Broadcast));
                return;
            }

            if (curPos)
            {
                // ensure same landblock
                if ((instance.ObjCellId >> 16) != (session.Player.Location.Cell >> 16))
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Failed to move {obj.Name} ({obj.Guid}) to current location -- different landblock", ChatMessageType.Broadcast));
                    return;
                }

                obj.Ethereal = true;
                obj.EnqueueBroadcastPhysicsState();

                var newLoc = new Position(session.Player.Location);

                // slide?
                var setPos = new Physics.Common.SetPosition(newLoc.PhysPosition(), Physics.Common.SetPositionFlags.Teleport /* | Physics.Common.SetPositionFlags.Slide */);
                var result = obj.PhysicsObj.SetPosition(setPos);

                if (result != Physics.Common.SetPositionError.OK)
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Failed to move {obj.Name} ({obj.Guid}) to current location: {result}", ChatMessageType.Broadcast));
                    return;
                }

                instance.AnglesX = obj.Location.RotationX;
                instance.AnglesY = obj.Location.RotationY;
                instance.AnglesZ = obj.Location.RotationZ;
                instance.AnglesW = obj.Location.RotationW;
            }
            else
            {
                // compare current position with home position
                // the nudge should be performed as an offset from home position
                if (instance.OriginX != obj.Location.PositionX || instance.OriginY != obj.Location.PositionY || instance.OriginZ != obj.Location.PositionZ)
                {
                    //session.Network.EnqueueSend(new GameMessageSystemChat($"Moving {obj.Name} ({obj.Guid}) to home position: {obj.Location} to {instance.ObjCellId:X8} [{instance.OriginX} {instance.OriginY} {instance.OriginZ}]", ChatMessageType.Broadcast));

                    var homePos = new Position(instance.ObjCellId, instance.OriginX, instance.OriginY, instance.OriginZ, instance.AnglesX, instance.AnglesY, instance.AnglesZ, instance.AnglesW);

                    // slide?
                    var setPos = new Physics.Common.SetPosition(homePos.PhysPosition(), Physics.Common.SetPositionFlags.Teleport /* | Physics.Common.SetPositionFlags.Slide*/);
                    var result = obj.PhysicsObj.SetPosition(setPos);

                    if (result != Physics.Common.SetPositionError.OK)
                    {
                        session.Network.EnqueueSend(new GameMessageSystemChat($"Failed to move {obj.Name} ({obj.Guid}) to home position {homePos.ToLOCString()}", ChatMessageType.Broadcast));
                        return;
                    }
                }

                // perform physics transition
                var newPos = new Physics.Common.Position(obj.PhysicsObj.Position);
                newPos.add_offset(nudge.Value);

                var transit = obj.PhysicsObj.transition(obj.PhysicsObj.Position, newPos, true);

                var errorMsg = $"{obj.Name} ({obj.Guid}) failed to move from {obj.PhysicsObj.Position.ACEPosition()} to {newPos.ACEPosition()}";

                if (transit == null)
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat(errorMsg, ChatMessageType.Broadcast));
                    return;
                }

                // ensure same landblock
                if ((transit.SpherePath.CurPos.ObjCellID >> 16) != (obj.PhysicsObj.Position.ObjCellID >> 16))
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"{errorMsg} - cannot change landblock", ChatMessageType.Broadcast));
                    return;
                }

                obj.PhysicsObj.SetPositionInternal(transit);
            }

            // update ace location
            var prevLoc = new Position(obj.Location);
            obj.Location = obj.PhysicsObj.Position.ACEPosition();

            if (prevLoc.Landblock != obj.Location.Landblock)
                LandblockManager.RelocateObjectForPhysics(obj, true);

            // broadcast new position
            obj.SendUpdatePosition(true);

            session.Network.EnqueueSend(new GameMessageSystemChat($"{obj.Name} ({obj.Guid}) - moved from {prevLoc} to {obj.Location}", ChatMessageType.Broadcast));

            // update sql
            instance.ObjCellId = obj.Location.Cell;
            instance.OriginX = obj.Location.PositionX;
            instance.OriginY = obj.Location.PositionY;
            instance.OriginZ = obj.Location.PositionZ;

            SyncInstances(session, landblock_id, instances);
        }

        public static Vector3? GetNudgeDir(string dir)
        {
            if (dir.Equals("north") || dir.Equals("n") || dir.Equals("y"))
                return Vector3.UnitY;
            else if (dir.Equals("south") || dir.Equals("s"))
                return -Vector3.UnitY;
            else if (dir.Equals("west") || dir.Equals("w"))
                return -Vector3.UnitX;
            else if (dir.Equals("east") || dir.Equals("e") || dir.Equals("x"))
                return Vector3.UnitX;
            else if (dir.Equals("northwest") || dir.Equals("nw"))
                return Vector3.Normalize(new Vector3(-1, 1, 0));
            else if (dir.Equals("northeast") || dir.Equals("ne"))
                return Vector3.Normalize(new Vector3(1, 1, 0));
            else if (dir.Equals("southwest") || dir.Equals("sw"))
                return Vector3.Normalize(new Vector3(-1, -1, 0));
            else if (dir.Equals("southeast") || dir.Equals("se"))
                return Vector3.Normalize(new Vector3(1, -1, 0));
            else if (dir.Equals("up") || dir.Equals("z"))
                return Vector3.UnitZ;
            else if (dir.Equals("down"))
                return -Vector3.UnitZ;
            else
                return null;
        }

        [CommandHandler("rotate", AccessLevel.Developer, CommandHandlerFlag.None, 1, "Adjusts the rotation of a landblock instance", "<dir>\nDirections: north, south, west, east, northwest, northeast, southwest, southeast, n, s, w, e, nw, ne, sw, se, -or-\n0-360, with 0 being north, and 90 being west")]
        public static void HandleRotate(Session session, params string[] parameters)
        {
            WorldObject obj = null;

            var curParam = 0;

            if (parameters.Length == 2)
            {
                if (!uint.TryParse(parameters[curParam++].TrimStart("0x"), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var guid))
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Invalid guid: {parameters[0]}", ChatMessageType.Broadcast));
                    return;
                }

                obj = session.Player.FindObject(guid, Player.SearchLocations.Landblock);

                if (obj == null)
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find {parameters[0]}", ChatMessageType.Broadcast));
                    return;
                }
            }
            else
                obj = CommandHandlerHelper.GetLastAppraisedObject(session);

            if (obj == null) return;

            // ensure landblock instance
            if (!obj.Guid.IsStatic())
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"{obj.Name} ({obj.Guid}) is not landblock instance", ChatMessageType.Broadcast));
                return;
            }

            if (obj.PhysicsObj == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"{obj.Name} ({obj.Guid}) is not a physics object", ChatMessageType.Broadcast));
                return;
            }

            // get direction
            var dirname = parameters[curParam++].ToLower();
            var dir = GetNudgeDir(dirname);

            bool curRotate = false;

            if (dir == null)
            {
                if (float.TryParse(dirname, out var degrees))
                {
                    var rads = degrees.ToRadians();
                    var q = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, rads);
                    dir = Vector3.Transform(Vector3.UnitY, q);
                }
                else if (dirname.Equals("here") || dirname.Equals("me"))
                {
                    dir = Vector3.Zero;
                    curRotate = true;
                }
                else
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Invalid direction: {dirname}", ChatMessageType.Broadcast));
                    return;
                }
            }

            // get quaternion
            var newRotation = Quaternion.Identity;

            if (curRotate)
            {
                newRotation = session.Player.Location.Rotation;
            }
            else
            {
                var angle = Math.Atan2(-dir.Value.X, dir.Value.Y);
                newRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)angle);
            }

            // get landblock for static guid
            var landblock_id = (ushort)(obj.Guid.Full >> 12);

            // get instances for landblock
            var instances = DatabaseManager.World.GetCachedInstancesByLandblock(landblock_id);

            // find instance
            var instance = instances.FirstOrDefault(i => i.Guid == obj.Guid.Full);

            if (instance == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find instance for {obj.Name} ({obj.Guid})", ChatMessageType.Broadcast));
                return;
            }

            session.Network.EnqueueSend(new GameMessageSystemChat($"{obj.Name} ({obj.Guid}) new rotation: {newRotation}", ChatMessageType.Broadcast));

            // update physics / ace rotation
            obj.PhysicsObj.Position.Frame.Orientation = newRotation;
            obj.Location.Rotation = newRotation;

            // update instance
            instance.AnglesW = newRotation.W;
            instance.AnglesX = newRotation.X;
            instance.AnglesY = newRotation.Y;
            instance.AnglesZ = newRotation.Z;

            SyncInstances(session, landblock_id, instances);

            // broadcast new rotation
            obj.SendUpdatePosition(true);
        }

        [CommandHandler("generate-classnames", AccessLevel.Developer, CommandHandlerFlag.None, "Generates WeenieClassName.cs from current world database")]
        public static void HandleGenerateClassNames(Session session, params string[] parameters)
        {
            var lines = new List<string>();

            var replaceChars = new Dictionary<string, string>()
            {
                { " ", "_" },
                { "-", "_" },
                { "!", "" },
                { "#", "" },
                { "?", "" },
            };

            using (var ctx = new WorldDbContext())
            {
                var weenies = ctx.Weenie.OrderBy(i => i.ClassId);

                lines.Add("namespace ACE.Server.Factories.Enum");
                lines.Add("{");
                lines.Add("    public enum WeenieClassName");
                lines.Add("    {");
                lines.Add("        undef = 0,");

                foreach (var weenie in weenies)
                {
                    var className = weenie.ClassName;

                    foreach (var kvp in replaceChars)
                        className = className.Replace(kvp.Key, kvp.Value);

                    if (className[0] >= '0' && className[0] <= '9')
                        className = $"_{className}";

                    lines.Add($"        {className} = {weenie.ClassId},");
                }

                lines.Add("    }");
                lines.Add("}");
            }

            var filename = "WeenieClassName.cs";
            var sep = Path.DirectorySeparatorChar;
            var path = $"..{sep}..{sep}..{sep}..{sep}Factories{sep}Enum{sep}{filename}";
            if (!File.Exists(path))
                path = filename;
            File.WriteAllLines(path, lines);

            CommandHandlerHelper.WriteOutputInfo(session, $"Wrote {path}");
        }

        [CommandHandler("vloc2loc", AccessLevel.Developer, CommandHandlerFlag.None, 1, "Output a set of LOCs for a given landblock found in the VLOCS dataset", "<LandblockID>\nExample: @vloc2loc 0x0007\n         @vloc2loc 0xCE95")]
        public static void HandleVLOCtoLOC(Session session, params string[] parameters)
        {
            var hex = parameters[0];

            if (hex.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase)
             || hex.StartsWith("&H", StringComparison.CurrentCultureIgnoreCase))
            {
                hex = hex[2..];
            }

            if (uint.TryParse(hex, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out var lbid))
            {
                DirectoryInfo di = VerifyContentFolder(session);
                if (!di.Exists) return;

                var sep = Path.DirectorySeparatorChar;

                var vloc_folder = $"{di.FullName}{sep}vlocs{sep}";

                di = new DirectoryInfo(vloc_folder);

                var vlocDB = vloc_folder + "vlocDB.txt";

                var vlocs = di.Exists ? new FileInfo(vlocDB).Exists ? File.ReadLines(vlocDB).ToArray() : null : null;

                if (vlocs == null)
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"Unable to read VLOC database file located here: {vlocDB}");
                    return;
                }

                // Name,ObjectClass,LandCell,X,Y
                // Master MacTavish,37,-114359889,97.14075000286103,-63.93749958674113

                if (vlocs.Length == 0 || !vlocs[0].Equals("Name,ObjectClass,LandCell,X,Y"))
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"{vlocDB} does not appear to be a valid VLOC database file.");
                    return;
                }

                var vlocFile = vloc_folder + $"{lbid:X4}.txt";

                var vi = new FileInfo(vlocFile);
                if (vi.Exists)
                    vi.Delete();

                for (var i = 1; i < vlocs.Length; i++)
                {
                    var split = vlocs[i].Split(",");

                    var name = split[0].Trim();
                    var objectClass = split[1].Trim();
                    var strLandCell = split[2].Trim();
                    var strX = split[3].Trim();
                    var strY = split[4].Trim();

                    if (!int.TryParse(strLandCell, out var landCell))
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"Unable to parse LandCell ({strLandCell}) value from line {i} in vlocDB: {vlocs[i]}");
                        continue;
                    }
                    var objCellId = (uint)landCell;
                    if (!float.TryParse(strX, out var x))
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"Unable to parse X ({strX}) value from line {i} in vlocDB: {vlocs[i]}");
                        continue;
                    }    
                    if (!float.TryParse(strY, out var y))
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"Unable to parse Y ({strY}) value from line {i} in vlocDB: {vlocs[i]}");
                        continue;
                    }    

                    if ((objCellId >> 16) != lbid) continue;

                    try
                    {
                        var pos = new Position(new Vector2(x, y));
                        pos.AdjustMapCoords();
                        pos.Translate(objCellId);
                        pos.FindZ();

                        using (StreamWriter sw = File.AppendText(vlocFile))
                        {
                            sw.WriteLine($"{name} - @teleloc {pos.ToLOCString()}");
                        }
                    }
                    catch (Exception)
                    {
                        using (StreamWriter sw = File.AppendText(vlocFile))
                        {
                            sw.WriteLine($"Unable to parse {name} - 0x{objCellId:X8} {strX}, {strY}");
                        }
                    }
                }

                vi = new FileInfo(vlocFile);
                if (vi.Exists)
                    CommandHandlerHelper.WriteOutputInfo(session, $"Successfully wrote VLOCs for 0x{lbid:X4} to {vlocFile}");
                else
                    CommandHandlerHelper.WriteOutputInfo(session, $"No VLOCs able to be written for 0x{lbid:X4}");
            }
            else
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Invalid Landblock ID: {parameters[0]}\nLandblock ID should be in the hex format such as this: @vloc2loc 0xAB94");
            }
        }
    }
}
