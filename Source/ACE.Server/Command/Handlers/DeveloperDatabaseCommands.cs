using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Server.Command.Handlers.Processors;
using ACE.Server.Managers;
using ACE.Server.Network;

using log4net;

namespace ACE.Server.Command.Handlers
{
    public static class DeveloperDatabaseCommands
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                    .FromSqlRaw("SELECT * FROM character_properties_shortcut_bar ORDER BY character_Id, shortcut_Bar_Index, id")
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
                        ctx.Database.ExecuteSqlRaw(cmd);
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

        [CommandHandler("database-shard-cache-pbrt", AccessLevel.Developer, CommandHandlerFlag.None, 0, "Shard Database, Player Biota Cache - Retention Time (in minutes)")]
        public static void HandleDatabaseShardCachePBRT(Session session, params string[] parameters)
        {
            if (!(DatabaseManager.Shard.BaseDatabase is ShardDatabaseWithCaching shardDatabaseWithCaching))
            {
                CommandHandlerHelper.WriteOutputInfo(session, "DatabaseManager is not using ShardDatabaseWithCaching");

                return;
            }

            if (parameters == null || parameters.Length == 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Shard Database, Player Biota Cache - Retention Time {shardDatabaseWithCaching.PlayerBiotaRetentionTime.TotalMinutes:N0} m");

                return;
            }

            if (!int.TryParse(parameters[0], out var value) || value < 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, "Unable to parse argument. Specify retention time in integer minutes.");

                return;
            }

            shardDatabaseWithCaching.PlayerBiotaRetentionTime = TimeSpan.FromMinutes(value);

            CommandHandlerHelper.WriteOutputInfo(session, $"Shard Database, Player Biota Cache - Retention Time {shardDatabaseWithCaching.PlayerBiotaRetentionTime.TotalMinutes:N0} m");
        }

        [CommandHandler("database-shard-cache-npbrt", AccessLevel.Developer, CommandHandlerFlag.None, 0, "Shard Database, Non-Player Biota Cache - Retention Time (in minutes)")]
        public static void HandleDatabaseShardCacheNPBRT(Session session, params string[] parameters)
        {
            if (!(DatabaseManager.Shard.BaseDatabase is ShardDatabaseWithCaching shardDatabaseWithCaching))
            {
                CommandHandlerHelper.WriteOutputInfo(session, "DatabaseManager is not using ShardDatabaseWithCaching");

                return;
            }

            if (parameters == null || parameters.Length == 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Shard Database, Non-Player Biota Cache - Retention Time {shardDatabaseWithCaching.NonPlayerBiotaRetentionTime.TotalMinutes:N0} m");

                return;
            }

            if (!int.TryParse(parameters[0], out var value) || value < 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, "Unable to parse argument. Specify retention time in integer minutes.");

                return;
            }

            shardDatabaseWithCaching.NonPlayerBiotaRetentionTime = TimeSpan.FromMinutes(value);

            CommandHandlerHelper.WriteOutputInfo(session, $"Shard Database, Non-Player Biota Cache - Retention Time {shardDatabaseWithCaching.NonPlayerBiotaRetentionTime.TotalMinutes:N0} m");
        }

        [CommandHandler("fix-spell-bars", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, "Fixes the players spell bars.", "<execute>")]
        public static void HandleFixSpellBars(Session session, params string[] parameters)
        {
            Console.WriteLine();

            Console.WriteLine("This command will attempt to fix player spell bars. Unless explictly indicated, command will dry run only");
            Console.WriteLine("You must have executed 2020-04-11-00-Update-Character-SpellBars.sql script first before running this command");

            Console.WriteLine();

            var execute = false;

            if (parameters.Length < 1)
                Console.WriteLine("This will be a dry run and show which characters that would be affected. To perform fix, please use command: fix-spell-bars execute");
            else if (parameters[0].ToLower() == "execute")
                execute = true;
            else
                Console.WriteLine("Please use command fix-spell-bars execute");


            if (!execute)
            {
                Console.WriteLine();
                Console.WriteLine("Press enter to start.");
                Console.ReadLine();
            }

            var numberOfRecordsFixed = 0;

            log.Info($"Starting FixSpellBarsPR2918 process. This could take a while...");

            using (var context = new ShardDbContext())
            {
                var characterSpellBarsNotFixed = context.CharacterPropertiesSpellBar.Where(c => c.SpellBarNumber == 0).ToList();

                if (characterSpellBarsNotFixed.Count > 0)
                {
                    log.Warn("2020-04-11-00-Update-Character-SpellBars.sql patch not yet applied. Please apply this patch ASAP! Skipping FixSpellBarsPR2918 for now...");
                    log.Fatal("2020-04-11-00-Update-Character-SpellBars.sql patch not yet applied. You must apply this patch before proceeding further...");
                    return;
                }

                var characterSpellBars = context.CharacterPropertiesSpellBar.OrderBy(c => c.CharacterId).ThenBy(c => c.SpellBarNumber).ThenBy(c => c.SpellBarIndex).ToList();

                uint characterId = 0;
                uint spellBarNumber = 0;
                uint spellBarIndex = 0;

                foreach (var entry in characterSpellBars)
                {
                    if (entry.CharacterId != characterId)
                    {
                        characterId = entry.CharacterId;
                        spellBarIndex = 0;
                    }

                    if (entry.SpellBarNumber != spellBarNumber)
                    {
                        spellBarNumber = entry.SpellBarNumber;
                        spellBarIndex = 0;
                    }

                    spellBarIndex++;

                    if (entry.SpellBarIndex != spellBarIndex)
                    {
                        Console.WriteLine($"FixSpellBarsPR2918: Character 0x{entry.CharacterId:X8}, SpellBarNumber = {entry.SpellBarNumber} | SpellBarIndex = {entry.SpellBarIndex:000}; Fixed - {spellBarIndex:000}");
                        entry.SpellBarIndex = spellBarIndex;
                        numberOfRecordsFixed++;
                    }
                    else
                    {
                        Console.WriteLine($"FixSpellBarsPR2918: Character 0x{entry.CharacterId:X8}, SpellBarNumber = {entry.SpellBarNumber} | SpellBarIndex = {entry.SpellBarIndex:000}; OK");
                    }
                }

                // Save
                if (execute)
                {
                    Console.WriteLine("Saving changes...");
                    context.SaveChanges();
                    log.Info($"Fixed {numberOfRecordsFixed:N0} CharacterPropertiesSpellBar records.");
                }
                else
                {
                    Console.WriteLine($"{numberOfRecordsFixed:N0} CharacterPropertiesSpellBar records need to be fixed!");
                    Console.WriteLine("dry run completed. Use fix-spell-bars execute to actually run command");
                }
            }
        }
    }
}
