
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Server.Command.Handlers.Processors;
using ACE.Server.Managers;
using ACE.Server.Network;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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
            using (var ctx = new ShardDbContext())
            {
                //var query = from shortcuts in ctx.CharacterPropertiesShortcutBar
                //            orderby shortcuts.CharacterId, shortcuts.ShortcutBarIndex, shortcuts.Id
                //            select shortcuts;

                var results = ctx.CharacterPropertiesShortcutBar
                    .FromSql("SELECT * FROM character_properties_shortcut_bar ORDER BY character_Id, shortcut_Bar_Index, id")
                    .ToList();

                //var results = query.ToList();

                uint characterId = 0;
                string playerName = null;
                Dictionary<uint, uint> IdxToObj = new Dictionary<uint, uint>();
                Dictionary<uint, uint> ObjToIdx = new Dictionary<uint, uint>();
                var buggedChar = false;

                foreach (var result in results)
                {
                    if (characterId != result.CharacterId)
                    {
                        if (buggedChar)
                        {
                            OutputShortcutSQL(playerName, characterId, IdxToObj);
                            buggedChar = false;
                        }

                        // begin parsing new character
                        characterId = result.CharacterId;
                        var player = PlayerManager.FindByGuid(characterId);
                        playerName = player != null ? player.Name : $"{characterId:X8}";
                        IdxToObj = new Dictionary<uint, uint>();
                        ObjToIdx = new Dictionary<uint, uint>();
                    }

                    if (IdxToObj.TryGetValue(result.ShortcutBarIndex, out var obj))
                    {
                        //Console.WriteLine($"Player: {playerName}, Idx: {result.ShortcutBarIndex}, Obj: {result.ShortcutObjectId:X8} ({result.Id}) - idx");
                        buggedChar = true;
                    }

                    if (ObjToIdx.TryGetValue(result.ShortcutObjectId, out var idx))
                    {
                        //Console.WriteLine($"Player: {playerName}, Idx: {result.ShortcutBarIndex}, Obj: {result.ShortcutObjectId:X8} ({result.Id}) - obj");
                        buggedChar = true;
                    }

                    IdxToObj[result.ShortcutBarIndex] = result.ShortcutObjectId;
                    ObjToIdx[result.ShortcutObjectId] = result.ShortcutBarIndex;
                }

                if (buggedChar)
                    OutputShortcutSQL(playerName, characterId, IdxToObj);
            }
        }

        public static void OutputShortcutSQL(string playerName, uint characterID, Dictionary<uint, uint> idxToObj)
        {
            Console.WriteLine($"/* {playerName} */");

            Console.WriteLine($"DELETE FROM `character_properties_shortcut_bar` WHERE `character_Id`={characterID};");

            foreach (var shortcut in idxToObj)
                Console.WriteLine($"INSERT INTO `character_properties_shortcut_bar` SET `character_Id`={characterID}, `shortcut_Bar_Index`={shortcut.Key}, `shortcut_Object_Id`={shortcut.Value};");

            Console.WriteLine();
        }
    }
}
