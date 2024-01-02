using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using log4net;

using ACE.Common;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;

using Biota = ACE.Entity.Models.Biota;
using ACE.Database.Models.Log;
using ACE.Server.Network.GameAction.Actions;
using ACE.Server.Entity.Chess;
using System.Runtime.CompilerServices;
using System.Net.NetworkInformation;
using ACE.Server.Entity.Actions;
using Microsoft.Extensions.Logging;
using ACE.Server.Entity;
using ACE.Server.Entity.WorldBoss;
using ACE.Server.Factories;
using ACE.Server.Network.Handlers;

namespace ACE.Server.Managers
{
    public static class WorldBossManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static DateTime LastTickDateTime = DateTime.MinValue;
        private static DateTime? nextBossSpawnTime = null;
        private static WorldBoss activeWorldBoss = null;

        public static void Initialize()
        {
            
        }

        
        public static void Tick()
        {
            if (DateTime.Now.AddSeconds(-300) < LastTickDateTime)
                return;

            LastTickDateTime = DateTime.Now;

            bool isWorldBossesDisabled = PropertyManager.GetBool("disable_world_bosses").Item;
            if (isWorldBossesDisabled)
            {
                return;
            }

            if(!nextBossSpawnTime.HasValue)
            {
                nextBossSpawnTime = RollNextSpawnTime();
            }

            //if there's no active boss, and the next spawn time is in the past, spawn a boss
            if(activeWorldBoss == null && DateTime.Now > nextBossSpawnTime)
            {
                SpawnNewWorldBoss();

                bool bossSpawnedAfterMidnight = false;
                if(nextBossSpawnTime.Value.Hour < 3)
                {
                    bossSpawnedAfterMidnight = true;
                }

                nextBossSpawnTime = RollNextSpawnTime();
                if(!bossSpawnedAfterMidnight)
                {
                    nextBossSpawnTime = nextBossSpawnTime.Value.AddDays(1);
                }
            }
        }

        private static DateTime RollNextSpawnTime()
        {            
            var hr = ThreadSafeRandom.Next(12, 25);
            var min = ThreadSafeRandom.Next(0, 59);
            return DateTime.Today.AddHours(hr).AddMinutes(min);
        }

        public static void SpawnNewWorldBoss()
        {
            //Get a random boss to spawn, and get a random spawn location
            var boss = WorldBosses.GetRandomWorldBoss();
            var spawnLoc = boss.RollRandomSpawnLocation();
            boss.Location = spawnLoc.Value;

            //Perma load the landblock for the spawn location
            var landblockID = new LandblockId(spawnLoc.Key << 16);
            var landblock = LandblockManager.GetLandblock(landblockID, false, true);

            //Spawn the boss
            var bossWeenie = DatabaseManager.World.GetCachedWeenie(boss.WeenieID);

            var bossWorldObj = WorldObjectFactory.CreateNewWorldObject(bossWeenie);
            bossWorldObj.Location = spawnLoc.Value;
            bossWorldObj.CurrentLandblock = landblock;
            bossWorldObj.EnterWorld();

            //Send global message
            PlayerManager.BroadcastToAll(new GameMessageSystemChat(boss.SpawnMsg, ChatMessageType.Broadcast));

            //Send global to webhook
            try
            {
                var webhookUrl = PropertyManager.GetString("world_boss_webhook").Item;
                if (!string.IsNullOrEmpty(webhookUrl))
                {
                    _ = TurbineChatHandler.SendWebhookedChat("World Boss", boss.SpawnMsg, webhookUrl, "Global");
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Failed sending World Boss global message to webhook. Ex:{0}", ex);
            }

            activeWorldBoss = boss;
        }

        public static WorldBoss GetActiveWorldBoss()
        {
            return activeWorldBoss;            
        }

        public static DateTime? GetNextSpawnTime()
        {
            return nextBossSpawnTime;
        }

        public static void HandleBossDeath()
        {
            activeWorldBoss = null;
        }
    }
}
