using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Server.Command.Handlers.Processors;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.WorldObjects;

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

        [CommandHandler("fix-shortcut-bars", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, "Fixes the players with duplicate items on their shortcut bars.", "<execute>")]
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





        // Old system

        // 9232 prismatic arrow     ~10.8 KB
        // 8563 mob archer          ~51.80 KB   reduces to 1.10 KB
        // 8563 mob archer          ~52.31 KB

        // World DB Caching         ~5.1 GB -> ~1.4 GB
        //                          ~4.8 GB -> ~1.4 GB

        // Large player db load     ~11.8 GB -> ~7.3 GB
        //                          ~11.0 GB -> ~7.2 GB

        // New system (Non Hybrid)
        // 8563 mob archer          ~41.45 KB

        // World DB Caching         ~2.4 GB -> ~1.3 GB
        //                          ~2.4 GB -> ~1.3 GB


        // New system (Non Hybrid, !instantiateEmptyCollections, Lazy Load property collections)
        // 8563 mob archer          ~26.03 KB

        // World DB Caching         ~2.1 GB -> ~1.3 GB
        //                          ~2.1 GB -> ~1.3 GB

        // Large player db load     ~10.3 GB -> ~1.9 GB


        private static readonly Collection<WorldObject> WorldObjects = new Collection<WorldObject>();

        [CommandHandler("test1", AccessLevel.Developer, CommandHandlerFlag.None)]
        public static void test1(Session session, params string[] parameters)
        {
            uint wcid = 8563;
            int count = 100000;

            var temp = WorldObjectFactory.CreateNewWorldObject(wcid);

            for (int i = 0; i < 20; i++)
                GC.Collect();

            var proc = Process.GetCurrentProcess();
            var origMemory = proc.PrivateMemorySize64;

            for (int i = 0; i < count; i++)
            {
                var wo = WorldObjectFactory.CreateNewWorldObject(wcid);

                WorldObjects.Add(wo);
            }

            for (int i = 0; i < 20; i++)
                GC.Collect();

            CommandHandlerHelper.WriteOutputInfo(session, "test1 completed");

            proc = Process.GetCurrentProcess();
            var finalMemory = proc.PrivateMemorySize64;

            CommandHandlerHelper.WriteOutputInfo(session, $"each wcid {wcid} consumes {(finalMemory - origMemory) / (double)count / 1000:N2} KB");
        }

        [CommandHandler("cleartest1", AccessLevel.Developer, CommandHandlerFlag.None)]
        public static void cleartest1(Session session, params string[] parameters)
        {
            WorldObjects.Clear();

            for (int i = 0; i < 20; i++)
                GC.Collect();

            CommandHandlerHelper.WriteOutputInfo(session, "cleartest1 completed");
        }

        [CommandHandler("test2", AccessLevel.Developer, CommandHandlerFlag.None)]
        public static void test2(Session session, params string[] parameters)
        {
            var weenieSQLWriter = new ACE.Database.SQLFormatters.World.WeenieSQLWriter();
            var biotaSQLWriter = new ACE.Database.SQLFormatters.Shard.BiotaSQLWriter();
            biotaSQLWriter.WeenieNames = new Dictionary<uint, string>();

            for (uint i = 0; i < 10000; i++)
            {
                var wcid = i;

                var dbWeenie = DatabaseManager.World.GetWeenie(wcid);

                if (dbWeenie == null)
                    continue;

                var dbWeenieToEntityWeenie = ACE.Database.Adapter.WeenieConverter.ConvertToEntityWeenie(dbWeenie);
                var dbWeenieToDbBiota = ACE.Database.Adapter.WeenieConverter.ConvertToDatabaseBiota(dbWeenie, 1);

                var dbWeenieToEntityWeenieToEntityBiota = ACE.Entity.Adapter.WeenieConverter.ConvertToBiota(dbWeenieToEntityWeenie, 1);
                var dbWeenieToEntityWeenieToEntityBiotaToDbBiota = ACE.Database.Adapter.BiotaConverter.ConvertFromEntityBiota(dbWeenieToEntityWeenieToEntityBiota);


                var dbWeenieToDbBiotaMS = new System.IO.MemoryStream();
                var dbWeenieToDbBiotaTW = new System.IO.StreamWriter(dbWeenieToDbBiotaMS);
                biotaSQLWriter.CreateSQLINSERTStatement(dbWeenieToDbBiota, dbWeenieToDbBiotaTW);
                var dbWeenieToDbBiotaSQL = System.Text.Encoding.ASCII.GetString(dbWeenieToDbBiotaMS.ToArray());


                var dbWeenieToDdbWeenieToEntityWeenieToEntityBiotaToDbBiotaMSbBiotaMS = new System.IO.MemoryStream();
                var dbWeenieToEntityWeenieToEntityBiotaToDbBiotaTW = new System.IO.StreamWriter(dbWeenieToDdbWeenieToEntityWeenieToEntityBiotaToDbBiotaMSbBiotaMS);
                biotaSQLWriter.CreateSQLINSERTStatement(dbWeenieToEntityWeenieToEntityBiotaToDbBiota, dbWeenieToEntityWeenieToEntityBiotaToDbBiotaTW);
                var dbWeenieToEntityWeenieToEntityBiotaToDbBiotaSQL = System.Text.Encoding.ASCII.GetString(dbWeenieToDdbWeenieToEntityWeenieToEntityBiotaToDbBiotaMSbBiotaMS.ToArray());

                if (dbWeenieToDbBiotaSQL != dbWeenieToEntityWeenieToEntityBiotaToDbBiotaSQL)
                {
                    // Probably a biota_properties_emote_action order issue with the original db weenie, ie, non sequential orders, 0, 1, 4.. (missing 2, 3)
                    CommandHandlerHelper.WriteOutputInfo(session, $"wcid {i} has an error in the translation path somewhere");
                }
            }

            CommandHandlerHelper.WriteOutputInfo(session, "test2 completed");
        }
    }
}
