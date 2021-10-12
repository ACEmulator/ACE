using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

using ACE.Common;
using ACE.Database;
using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.Managers;
using ACE.Server.Physics.Entity;
using ACE.Server.Physics.Managers;
using ACE.Server.WorldObjects;

using log4net;

namespace ACE.Server.Command.Handlers
{
    public static class AdminStatCommands
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // allstats
        [CommandHandler("allstats", AccessLevel.Advocate, CommandHandlerFlag.None, 0, "Displays a summary of all server statistics and usage")]
        public static void HandleAllStats(Session session, params string[] parameters)
        {
            HandleServerStatus(session, parameters);

            HandleServerPerformance(session, parameters);

            HandleLandblockPerformance(session, parameters);

            HandleGCStatus(session, parameters);

            DeveloperDatabaseCommands.HandleDatabaseQueueInfo(session, parameters);
        }

        // serverstatus
        [CommandHandler("serverstatus", AccessLevel.Advocate, CommandHandlerFlag.None, 0, "Displays a summary of server statistics and usage")]
        public static void HandleServerStatus(Session session, params string[] parameters)
        {
            // This is formatted very similarly to GDL.

            var sb = new StringBuilder();

            var proc = Process.GetCurrentProcess();

            sb.Append($"Server Status:{'\n'}");

            sb.Append($"Host Info: {Environment.OSVersion}, vCPU: {Environment.ProcessorCount}{'\n'}");

            var runTime = DateTime.Now - proc.StartTime;
            sb.Append($"Server Runtime: {(int)runTime.TotalHours}h {runTime.Minutes}m {runTime.Seconds}s{'\n'}");

            sb.Append($"Total CPU Time: {(int)proc.TotalProcessorTime.TotalHours}h {proc.TotalProcessorTime.Minutes}m {proc.TotalProcessorTime.Seconds}s, Threads: {proc.Threads.Count}{'\n'}");

            // todo, add actual system memory used/avail
            sb.Append($"{(proc.PrivateMemorySize64 >> 20):N0} MB used{'\n'}");  // sb.Append($"{(proc.PrivateMemorySize64 >> 20)} MB used, xxxx / yyyy MB physical mem free.{'\n'}");

            sb.Append($"{NetworkManager.GetSessionCount():N0} connections, {NetworkManager.GetAuthenticatedSessionCount():N0} authenticated connections, {NetworkManager.GetUniqueSessionEndpointCount():N0} unique connections, {PlayerManager.GetOnlineCount():N0} players online{'\n'}");
            sb.Append($"Total Accounts Created: {DatabaseManager.Authentication.GetAccountCount():N0}, Total Characters Created: {(PlayerManager.GetOfflineCount() + PlayerManager.GetOnlineCount()):N0}{'\n'}");

            // 330 active objects, 1931 total objects(16777216 buckets.)

            // todo, expand this
            var loadedLandblocks = LandblockManager.GetLoadedLandblocks();
            int dormantLandblocks = 0, activeDungeonLandblocks = 0, dormantDungeonLandblocks = 0;
            int players = 0, creatures = 0, missiles = 0, other = 0, total = 0;
            foreach (var landblock in loadedLandblocks)
            {
                if (landblock.IsDormant)
                    dormantLandblocks++;

                if (landblock.IsDungeon)
                {
                    if (landblock.IsDormant)
                        dormantDungeonLandblocks++;
                    else
                        activeDungeonLandblocks++;
                }

                foreach (var worldObject in landblock.GetAllWorldObjectsForDiagnostics())
                {
                    if (worldObject is Player)
                        players++;
                    else if (worldObject is Creature)
                        creatures++;
                    else if (worldObject.Missile ?? false)
                        missiles++;
                    else
                        other++;

                    total++;
                }
            }
            sb.Append($"Landblocks: {(loadedLandblocks.Count - dormantLandblocks):N0} active ({activeDungeonLandblocks:N0} dungeons), {dormantLandblocks:N0} dormant ({dormantDungeonLandblocks:N0} dungeons), Landblock Groups: {LandblockManager.LandblockGroupsCount:N0} - Players: {players:N0}, Creatures: {creatures:N0}, Missiles: {missiles:N0}, Other: {other:N0}, Total: {total:N0}.{'\n'}"); // 11 total blocks loaded. 11 active. 0 pending dormancy. 0 dormant. 314 unloaded.
            // 11 total blocks loaded. 11 active. 0 pending dormancy. 0 dormant. 314 unloaded.

            if (ServerPerformanceMonitor.IsRunning)
                sb.Append($"Server Performance Monitor - UpdateGameWorld ~5m {ServerPerformanceMonitor.GetEventHistory5m(ServerPerformanceMonitor.MonitorType.UpdateGameWorld_Entire).AverageEventDuration:N3}, ~1h {ServerPerformanceMonitor.GetEventHistory1h(ServerPerformanceMonitor.MonitorType.UpdateGameWorld_Entire).AverageEventDuration:N3} s{'\n'}");
            else
                sb.Append($"Server Performance Monitor - Not running. To start use /serverperformance start{'\n'}");

            sb.Append($"Threading - WorldThreadCount: {ConfigManager.Config.Server.Threading.LandblockManagerParallelOptions.MaxDegreeOfParallelism}, Multithread Physics: {ConfigManager.Config.Server.Threading.MultiThreadedLandblockGroupPhysicsTicking}, Multithread Non-Physics: {ConfigManager.Config.Server.Threading.MultiThreadedLandblockGroupTicking}, DatabaseThreadCount: {ConfigManager.Config.Server.Threading.DatabaseParallelOptions.MaxDegreeOfParallelism}{'\n'}");

            sb.Append($"Physics Cache Counts - BSPCache: {BSPCache.Count:N0}, GfxObjCache: {GfxObjCache.Count:N0}, PolygonCache: {PolygonCache.Count:N0}, VertexCache: {VertexCache.Count:N0}{'\n'}");

            sb.Append($"Total Server Objects: {ServerObjectManager.ServerObjects.Count:N0}{'\n'}");

            sb.Append($"World DB Cache Counts - Weenies: {DatabaseManager.World.GetWeenieCacheCount():N0}, LandblockInstances: {DatabaseManager.World.GetLandblockInstancesCacheCount():N0}, PointsOfInterest: {DatabaseManager.World.GetPointsOfInterestCacheCount():N0}, Cookbooks: {DatabaseManager.World.GetCookbookCacheCount():N0}, Spells: {DatabaseManager.World.GetSpellCacheCount():N0}, Encounters: {DatabaseManager.World.GetEncounterCacheCount():N0}, Events: {DatabaseManager.World.GetEventsCacheCount():N0}{'\n'}");
            sb.Append($"Shard DB Counts - Biotas: {DatabaseManager.Shard.BaseDatabase.GetBiotaCount():N0}{'\n'}");
            if (DatabaseManager.Shard.BaseDatabase is ShardDatabaseWithCaching shardDatabaseWithCaching)
            {
                var biotaIds = shardDatabaseWithCaching.GetBiotaCacheKeys();
                var playerBiotaIds = biotaIds.Count(id => ObjectGuid.IsPlayer(id));
                var nonPlayerBiotaIds = biotaIds.Count - playerBiotaIds;
                sb.Append($"Shard DB Cache Counts - Player Biotas: {playerBiotaIds} ~ {shardDatabaseWithCaching.PlayerBiotaRetentionTime.TotalMinutes:N0} m, Non Players {nonPlayerBiotaIds} ~ {shardDatabaseWithCaching.NonPlayerBiotaRetentionTime.TotalMinutes:N0} m{'\n'}");
            }

            sb.Append(GuidManager.GetDynamicGuidDebugInfo() + '\n');

            sb.Append($"Portal.dat has {DatManager.PortalDat.FileCache.Count:N0} files cached of {DatManager.PortalDat.AllFiles.Count:N0} total{'\n'}");
            sb.Append($"Cell.dat has {DatManager.CellDat.FileCache.Count:N0} files cached of {DatManager.CellDat.AllFiles.Count:N0} total{'\n'}");

            CommandHandlerHelper.WriteOutputInfo(session, $"{sb}");
        }

        // serverstatus
        [CommandHandler("serverperformance", AccessLevel.Advocate, CommandHandlerFlag.None, 0, "Displays a summary of server performance statistics")]
        public static void HandleServerPerformance(Session session, params string[] parameters)
        {
            if (parameters != null && parameters.Length == 1)
            {
                if (parameters[0].ToLower() == "start")
                {
                    ServerPerformanceMonitor.Start();
                    CommandHandlerHelper.WriteOutputInfo(session, "Server Performance Monitor started");
                    return;
                }

                if (parameters[0].ToLower() == "stop")
                {
                    ServerPerformanceMonitor.Stop();
                    CommandHandlerHelper.WriteOutputInfo(session, "Server Performance Monitor stopped");
                    return;
                }

                if (parameters[0].ToLower() == "reset")
                {
                    ServerPerformanceMonitor.Reset();
                    CommandHandlerHelper.WriteOutputInfo(session, "Server Performance Monitor reset");
                    return;
                }
            }

            if (!ServerPerformanceMonitor.IsRunning)
            {
                CommandHandlerHelper.WriteOutputInfo(session, "Server Performance Monitor not running. To start use /serverperformance start");
                return;
            }

            CommandHandlerHelper.WriteOutputInfo(session, ServerPerformanceMonitor.ToString());
        }

        [CommandHandler("landblockperformance", AccessLevel.Advocate, CommandHandlerFlag.None, 0, "Displays a summary of landblock performance statistics")]
        public static void HandleLandblockPerformance(Session session, params string[] parameters)
        {
            var sb = new StringBuilder();

            var loadedLandblocks = LandblockManager.GetLoadedLandblocks();

            // Filter out landblocks that haven't recorded a certain amount of events
            var sortedBy5mAverage = loadedLandblocks.Where(r => r.Monitor5m.EventHistory.TotalEvents >= 10).OrderByDescending(r => r.Monitor5m.EventHistory.AverageEventDuration).Take(10).ToList();
            var sortedBy1hrAverage = loadedLandblocks.Where(r => r.Monitor1h.EventHistory.TotalEvents >= 1000).OrderByDescending(r => r.Monitor1h.EventHistory.AverageEventDuration).Take(10).ToList();

            var combinedByAverage = sortedBy5mAverage.Concat(sortedBy1hrAverage).Distinct().OrderByDescending(r => Math.Max(r.Monitor5m.EventHistory.AverageEventDuration, r.Monitor1h.EventHistory.AverageEventDuration)).Take(10);

            sb.Append($"Most Busy Landblock - By Average{'\n'}");
            sb.Append($"~5m Hits   Avg  Long  Last - ~1h Hits   Avg  Long  Last - Location   Players  Creatures{'\n'}");

            foreach (var entry in combinedByAverage)
            {
                int players = 0, creatures = 0;
                foreach (var worldObject in entry.GetAllWorldObjectsForDiagnostics())
                {
                    if (worldObject is Player)
                        players++;
                    else if (worldObject is Creature)
                        creatures++;
                }

                sb.Append($"{entry.Monitor5m.EventHistory.TotalEvents.ToString().PadLeft(7)} {entry.Monitor5m.EventHistory.AverageEventDuration:N4} {entry.Monitor5m.EventHistory.LongestEvent:N3} {entry.Monitor5m.EventHistory.LastEvent:N3} - " +
                          $"{entry.Monitor1h.EventHistory.TotalEvents.ToString().PadLeft(7)} {entry.Monitor1h.EventHistory.AverageEventDuration:N4} {entry.Monitor1h.EventHistory.LongestEvent:N3} {entry.Monitor1h.EventHistory.LastEvent:N3} - " +
                          $"0x{entry.Id.Raw:X8} {players.ToString().PadLeft(7)}  {creatures.ToString().PadLeft(9)}{'\n'}");
            }

            var sortedBy5mLong = loadedLandblocks.OrderByDescending(r => r.Monitor5m.EventHistory.LongestEvent).Take(10);
            var sortedBy1hrLong = loadedLandblocks.OrderByDescending(r => r.Monitor1h.EventHistory.LongestEvent).Take(10);

            var combinedByLong = sortedBy5mLong.Concat(sortedBy1hrLong).Distinct().OrderByDescending(r => Math.Max(r.Monitor5m.EventHistory.LongestEvent, r.Monitor1h.EventHistory.LongestEvent)).Take(10);

            sb.Append($"Most Busy Landblock - By Longest{'\n'}");
            sb.Append($"~5m Hits   Avg  Long  Last - ~1h Hits   Avg  Long  Last - Location   Players  Creatures{'\n'}");

            foreach (var entry in combinedByLong)
            {
                int players = 0, creatures = 0;
                foreach (var worldObject in entry.GetAllWorldObjectsForDiagnostics())
                {
                    if (worldObject is Player)
                        players++;
                    else if (worldObject is Creature)
                        creatures++;
                }

                sb.Append($"{entry.Monitor5m.EventHistory.TotalEvents.ToString().PadLeft(7)} {entry.Monitor5m.EventHistory.AverageEventDuration:N4} {entry.Monitor5m.EventHistory.LongestEvent:N3} {entry.Monitor5m.EventHistory.LastEvent:N3} - " +
                          $"{entry.Monitor1h.EventHistory.TotalEvents.ToString().PadLeft(7)} {entry.Monitor1h.EventHistory.AverageEventDuration:N4} {entry.Monitor1h.EventHistory.LongestEvent:N3} {entry.Monitor1h.EventHistory.LastEvent:N3} - " +
                          $"0x{entry.Id.Raw:X8} {players.ToString().PadLeft(7)}  {creatures.ToString().PadLeft(9)}{'\n'}");
            }

            CommandHandlerHelper.WriteOutputInfo(session, sb.ToString());
        }

        // gcstatus
        [CommandHandler("gcstatus", AccessLevel.Advocate, CommandHandlerFlag.None, 0, "Displays a summary of server GC Information")]
        public static void HandleGCStatus(Session session, params string[] parameters)
        {
            var sb = new StringBuilder();

            sb.Append($"GC.GetTotalMemory: {(GC.GetTotalMemory(false) >> 20):N0} MB, GC.GetTotalAllocatedBytes: {(GC.GetTotalAllocatedBytes() >> 20):N0} MB{'\n'}");

            // https://docs.microsoft.com/en-us/dotnet/api/system.gcmemoryinfo?view=net-5.0
            var gcmi = GC.GetGCMemoryInfo();

            sb.Append($"GCMI Index: {gcmi.Index:N0}, Generation: {gcmi.Generation}, Compacted: {gcmi.Compacted}, Concurrent: {gcmi.Concurrent}, PauseTimePercentage: {gcmi.PauseTimePercentage}{'\n'}");
            for (int i = 0 ; i < gcmi.GenerationInfo.Length ; i++)
                sb.Append($"GCMI.GenerationInfo[{i}] FragmentationBeforeBytes: {(gcmi.GenerationInfo[i].FragmentationBeforeBytes >> 20):N0} MB, FragmentationAfterBytes: {(gcmi.GenerationInfo[i].FragmentationAfterBytes >> 20):N0} MB, SizeBeforeBytes: {(gcmi.GenerationInfo[i].SizeBeforeBytes >> 20):N0} MB, SizeAfterBytes: {(gcmi.GenerationInfo[i].SizeAfterBytes >> 20):N0} MB{'\n'}");
            for (int i = 0; i < gcmi.PauseDurations.Length; i++)
                sb.Append($"GCMI.PauseDurations[{i}]: {gcmi.PauseDurations[i].TotalMilliseconds:N0} ms{'\n'}");
            sb.Append($"GCMI PinnedObjectsCount: {gcmi.PinnedObjectsCount}, FinalizationPendingCount: {gcmi.FinalizationPendingCount:N0}{'\n'}");

            sb.Append($"GCMI FragmentedBytes: {(gcmi.FragmentedBytes >> 20):N0} MB, PromotedBytes: {(gcmi.PromotedBytes >> 20):N0} MB, HeapSizeBytes: {(gcmi.HeapSizeBytes >> 20):N0} MB, TotalCommittedBytes: {(gcmi.TotalCommittedBytes >> 20):N0} MB{'\n'}");
            sb.Append($"GCMI MemoryLoadBytes: {(gcmi.MemoryLoadBytes >> 20):N0} MB, HighMemoryLoadThresholdBytes: {(gcmi.HighMemoryLoadThresholdBytes >> 20):N0} MB, TotalAvailableMemoryBytes: {(gcmi.TotalAvailableMemoryBytes >> 20):N0} MB{'\n'}");

            CommandHandlerHelper.WriteOutputInfo(session, sb.ToString());
        }
    }
}
