using System;
using System.Diagnostics;

using ACE.Database;
using ACE.Server.Managers;
using ACE.Server.Network.Managers;
using ACE.Server.WorldObjects;

using Prometheus;
using Prometheus.DotNetRuntime;

namespace ACE.Server
{
    partial class Program
    {
        private static MetricServer metricServer;

        private static IDisposable dotNetMetricsCollector;

        private static readonly Gauge ace_Process_TotalRunTime = Metrics.CreateGauge("ace_Process_TotalRunTime", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_Process_TotalProcessorTime = Metrics.CreateGauge("ace_Process_TotalProcessorTime", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_Process_Threads = Metrics.CreateGauge("ace_Process_Threads", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_Process_PrivateMemorySize64 = Metrics.CreateGauge("ace_Process_PrivateMemorySize64", null, new GaugeConfiguration { SuppressInitialValue = true });

        private static readonly Gauge ace_NetworkManager_SessionCount = Metrics.CreateGauge("ace_NetworkManager_SessionCount", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_NetworkManager_AuthenticatedSessionCount = Metrics.CreateGauge("ace_NetworkManager_AuthenticatedSessionCount", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_NetworkManager_UniqueSessionEndpointCount = Metrics.CreateGauge("ace_NetworkManager_UniqueSessionEndpointCount", null, new GaugeConfiguration { SuppressInitialValue = true });

        private static readonly Gauge ace_PlayerManager_OnlineCount = Metrics.CreateGauge("ace_PlayerManager_OnlineCount", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_PlayerManager_TotalCount = Metrics.CreateGauge("ace_PlayerManager_TotalCount", null, new GaugeConfiguration { SuppressInitialValue = true });

        private static readonly Gauge ace_DatabaseManager_AccountCount = Metrics.CreateGauge("ace_DatabaseManager_AccountCount", null, new GaugeConfiguration { SuppressInitialValue = true });

        private static readonly Gauge ace_LandblockManager_ActiveLandblocks = Metrics.CreateGauge("ace_LandblockManager_ActiveLandblocks", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_LandblockManager_DormantLandblocks = Metrics.CreateGauge("ace_LandblockManager_DormantLandblocks", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_LandblockManager_ActiveDungeons = Metrics.CreateGauge("ace_LandblockManager_ActiveDungeons", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_LandblockManager_DormantDungeons = Metrics.CreateGauge("ace_LandblockManager_DormantDungeons", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_LandblockManager_LandblockGroups = Metrics.CreateGauge("ace_LandblockManager_LandblockGroups", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_LandblockManager_Objects_Players = Metrics.CreateGauge("ace_LandblockManager_Objects_Players", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_LandblockManager_Objects_Creatures = Metrics.CreateGauge("ace_LandblockManager_Objects_Creatures", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_LandblockManager_Objects_Missiles = Metrics.CreateGauge("ace_LandblockManager_Objects_Missiles", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_LandblockManager_Objects_Other = Metrics.CreateGauge("ace_LandblockManager_Objects_Other", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_LandblockManager_Objects_Total = Metrics.CreateGauge("ace_LandblockManager_Objects_Total", null, new GaugeConfiguration { SuppressInitialValue = true });

        private static readonly Gauge ace_ServerPerformanceMonitor_5m_UpdateGameWorld_Entire = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_UpdateGameWorld_Entire", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_ServerPerformanceMonitor_1h_UpdateGameWorld_Entire = Metrics.CreateGauge("ace_ServerPerformanceMonitor_1h_UpdateGameWorld_Entire", null, new GaugeConfiguration { SuppressInitialValue = true });

        static void InitMetrics()
        {
            // https://github.com/prometheus-net/prometheus-net
            Metrics.DefaultRegistry.AddBeforeCollectCallback(MetricsAddBeforeCollectCallback);

            metricServer = new MetricServer(hostname: "localhost", port: 1234);
            metricServer.Start();

            // https://github.com/djluck/prometheus-net.DotNetRuntime
            dotNetMetricsCollector = DotNetRuntimeStatsBuilder.Default().StartCollecting();
        }

        static void MetricsAddBeforeCollectCallback()
        {
            var proc = Process.GetCurrentProcess();

            ace_Process_TotalRunTime.Set((DateTime.Now - proc.StartTime).TotalSeconds);
            ace_Process_TotalProcessorTime.Set(proc.TotalProcessorTime.TotalSeconds);
            ace_Process_Threads.Set(proc.Threads.Count);
            ace_Process_PrivateMemorySize64.Set(proc.PrivateMemorySize64);

            ace_NetworkManager_SessionCount.Set(NetworkManager.GetSessionCount());
            ace_NetworkManager_AuthenticatedSessionCount.Set(NetworkManager.GetAuthenticatedSessionCount());
            ace_NetworkManager_UniqueSessionEndpointCount.Set(NetworkManager.GetUniqueSessionEndpointCount());

            ace_PlayerManager_OnlineCount.Set(PlayerManager.GetOnlineCount());
            ace_PlayerManager_TotalCount.Set(PlayerManager.GetOfflineCount() + PlayerManager.GetOnlineCount());

            ace_DatabaseManager_AccountCount.Set(DatabaseManager.Authentication.GetAccountCount());

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
            ace_LandblockManager_ActiveLandblocks.Set(loadedLandblocks.Count - dormantLandblocks);
            ace_LandblockManager_DormantLandblocks.Set(activeDungeonLandblocks);
            ace_LandblockManager_ActiveDungeons.Set(dormantLandblocks);
            ace_LandblockManager_DormantDungeons.Set(dormantDungeonLandblocks);
            ace_LandblockManager_LandblockGroups.Set(LandblockManager.LandblockGroupsCount);
            ace_LandblockManager_Objects_Players.Set(players);
            ace_LandblockManager_Objects_Creatures.Set(creatures);
            ace_LandblockManager_Objects_Missiles.Set(missiles);
            ace_LandblockManager_Objects_Other.Set(other);
            ace_LandblockManager_Objects_Total.Set(total);

            if (ServerPerformanceMonitor.IsRunning)
            {
                ace_ServerPerformanceMonitor_5m_UpdateGameWorld_Entire.Set(ServerPerformanceMonitor.GetEventHistory5m(ServerPerformanceMonitor.MonitorType.UpdateGameWorld_Entire).AverageEventDuration);
                ace_ServerPerformanceMonitor_1h_UpdateGameWorld_Entire.Set(ServerPerformanceMonitor.GetEventHistory1h(ServerPerformanceMonitor.MonitorType.UpdateGameWorld_Entire).AverageEventDuration);
            }

            /*
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
            */
        }
    }
}
