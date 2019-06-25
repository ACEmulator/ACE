using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using ACE.Adapter.Lifestoned;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Database.SQLFormatters.World;
using ACE.Entity.Enum;
using ACE.Server.Command.Handlers.Processors;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.Command.Handlers
{
    public static class DeveloperDatabaseCommands
    {
        [CommandHandler("databasequeueinfo", AccessLevel.Developer, CommandHandlerFlag.None, 0, "Show database queue information.")]
        public static void HandleDatabaseQueueInfo(Session session, params string[] parameters)
        {
            CommandHandlerHelper.WriteOutputInfo(session, $"Current database queue count: {DatabaseManager.Shard.QueueCount}");

            DatabaseManager.Shard.GetCurrentQueueWaitTime(result =>
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Current database queue wait time: {result.TotalMilliseconds:N0} ms");
            });
        }

        [CommandHandler("databaseperftest", AccessLevel.Developer, CommandHandlerFlag.None, 0, "Test server/database performance.", "biotasPerTest\n" + "optional parameter biotasPerTest if omitted 1000")]
        public static void HandleDatabasePerfTest(Session session, params string[] parameters)
        {
            int biotasPerTest = DatabasePerfTest.DefaultBiotasTestCount;

            if (parameters?.Length > 0)
                int.TryParse(parameters[0], out biotasPerTest);

            var processor = new DatabasePerfTest();
            processor.RunAsync(session, biotasPerTest);
        }

        [CommandHandler("fix-shortcut-bars", AccessLevel.Admin, CommandHandlerFlag.None, 0, "Outputs to console SQL scripting to fix the players with duplicate items on their shortcut bars.", "")]
        public static void HandleFixShortcutBars(Session session, params string[] parameters)
        {
            Console.WriteLine();

            Console.WriteLine("This command will attempt to fix duplicate shortcuts found in player shortcut bars. Unless explictly indicated, command will dry run only");
            Console.WriteLine("If the command outputs nothing or errors, you are ready to proceed with updating your shard db with 2019-04-17-00-Character_Shortcut_Changes.sql script");

            Console.WriteLine();

            var execute = false;

            if (parameters.Length < 1)
                Console.WriteLine("This will be a dry run and show which characters that would be affected. To perform fix, please use command: fix-shortcut-bars execute");
            else if (parameters[0].ToLower() == "execute")
                execute = true;
            else
                Console.WriteLine("Please use command fix-shortcut-bars execute");

            using (var ctx = new ShardDbContext())
            {
                var results = ctx.CharacterPropertiesShortcutBar
                    .FromSql("SELECT * FROM character_properties_shortcut_bar ORDER BY character_Id, shortcut_Bar_Index, id")
                    .ToList();

                var sqlCommands = new List<string>();

                uint characterId = 0;
                string playerName = null;
                var idxToObj = new Dictionary<uint, uint>();
                var objToIdx = new Dictionary<uint, uint>();
                var buggedChar = false;
                var buggedPlayerCount = 0;

                foreach (var result in results)
                {
                    if (characterId != result.CharacterId)
                    {
                        if (buggedChar)
                        {
                            buggedPlayerCount++;
                            Console.WriteLine($"Player {playerName} ({characterId}) was found to have errors in their shortcuts.");
                            sqlCommands.AddRange(OutputShortcutSQLCommand(playerName, characterId, idxToObj));
                            buggedChar = false;
                        }

                        // begin parsing new character
                        characterId = result.CharacterId;
                        var player = PlayerManager.FindByGuid(characterId);
                        playerName = player != null ? player.Name : $"{characterId:X8}";
                        idxToObj = new Dictionary<uint, uint>();
                        objToIdx = new Dictionary<uint, uint>();
                    }

                    var dupeIdx = idxToObj.ContainsKey(result.ShortcutBarIndex);
                    var dupeObj = objToIdx.ContainsKey(result.ShortcutObjectId);

                    if (dupeIdx || dupeObj)
                    {
                        //Console.WriteLine($"Player: {playerName}, Idx: {result.ShortcutBarIndex}, Obj: {result.ShortcutObjectId:X8} ({result.Id})");
                        buggedChar = true;
                    }

                    objToIdx[result.ShortcutObjectId] = result.ShortcutBarIndex;

                    if (!dupeObj)
                        idxToObj[result.ShortcutBarIndex] = result.ShortcutObjectId;
                }

                if (buggedChar)
                {
                    Console.WriteLine($"Player {playerName} ({characterId}) was found to have errors in their shortcuts.");
                    buggedPlayerCount++;
                    sqlCommands.AddRange(OutputShortcutSQLCommand(playerName, characterId, idxToObj));
                }

                Console.WriteLine($"Total players found with bugged shortcuts: {buggedPlayerCount}");

                if (execute)
                {
                    Console.WriteLine("Executing changes...");

                    foreach (var cmd in sqlCommands)
                        ctx.Database.ExecuteSqlCommand(cmd);
                }
                else
                    Console.WriteLine("dry run completed. Use fix-shortcut-bars execute to actually run command");
            }
        }

        public static List<string> OutputShortcutSQLCommand(string playerName, uint characterID, Dictionary<uint, uint> idxToObj)
        {
            var strings = new List<string>();

            strings.Add($"DELETE FROM `character_properties_shortcut_bar` WHERE `character_Id`={characterID};");

            foreach (var shortcut in idxToObj)
                strings.Add($"INSERT INTO `character_properties_shortcut_bar` SET `character_Id`={characterID}, `shortcut_Bar_Index`={shortcut.Key}, `shortcut_Object_Id`={shortcut.Value};");

            return strings;
        }

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
