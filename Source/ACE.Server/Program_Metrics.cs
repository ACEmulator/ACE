using System;
using System.Diagnostics;
using System.Linq;
using ACE.Common;
using ACE.Database;
using ACE.Entity;
using ACE.Server.Managers;
using ACE.Server.Network.Managers;
using ACE.Server.Physics.Managers;
using ACE.Server.WorldObjects;

using Prometheus;
using Prometheus.DotNetRuntime;

namespace ACE.Server
{
    partial class Program
    {
        private static MetricServer metricServer;

        private static IDisposable dotNetMetricsCollector;

        private static readonly Gauge ace_Info = Metrics.CreateGauge("ace_Info", null, new GaugeConfiguration { SuppressInitialValue = true, LabelNames = new[] { "server_version", "database_version_base", "database_version_patch"} });

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
        private static readonly Gauge ace_DatabaseManager_Shard_CachedPlayerBiotas = Metrics.CreateGauge("ace_DatabaseManager_Shard_CachedPlayerBiotas", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_DatabaseManager_Shard_CachedNonPlayerBiotas = Metrics.CreateGauge("ace_DatabaseManager_Shard_CachedNonPlayerBiotas", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_DatabaseManager_Shard_QueueCount = Metrics.CreateGauge("ace_DatabaseManager_Shard_QueueCount", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_DatabaseManager_Shard_QueueWaitTime = Metrics.CreateGauge("ace_DatabaseManager_Shard_QueueWaitTime", null, new GaugeConfiguration { SuppressInitialValue = true });

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

        // Calls from WorldManager.UpdateWorld()
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_PlayerManager_Tick_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_PlayerManager_Tick_Average", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_NetworkManager_InboundClientMessageQueueRun_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_NetworkManager_InboundClientMessageQueueRun_Average", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_actionQueue_RunActions_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_actionQueue_RunActions_Average", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_DelayManager_RunActions_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_DelayManager_RunActions_Average", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_UpdateGameWorld_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_UpdateGameWorld_Average", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_NetworkManager_DoSessionWork_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_NetworkManager_DoSessionWork_Average", null, new GaugeConfiguration { SuppressInitialValue = true });

        // WorldManager.UpdateGameWorld() time not including throttled returns
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_UpdateGameWorld_Entire_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_UpdateGameWorld_Entire_Average", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_UpdateGameWorld_Entire_Longest = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_UpdateGameWorld_Entire_Longest", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_ServerPerformanceMonitor_1h_UpdateGameWorld_Entire_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_1h_UpdateGameWorld_Entire_Average", null, new GaugeConfiguration { SuppressInitialValue = true });

        // Calls from WorldManager.UpdateGameWorld()
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_LandblockManager_TickPhysics_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_LandblockManager_TickPhysics_Average", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_LandblockManager_TickMultiThreadedWork_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_LandblockManager_TickMultiThreadedWork_Average", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_LandblockManager_TickSingleThreadedWork_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_LandblockManager_TickSingleThreadedWork_Average", null, new GaugeConfiguration { SuppressInitialValue = true });
        /*
        // Calls from Landblock.TickPhysics() - Cumulative over a single UpdateGameWorld Tick
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_Player_Tick_UpdateObjectPhysics_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_Player_Tick_UpdateObjectPhysics_Average", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_WorldObject_Tick_UpdateObjectPhysics_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_WorldObject_Tick_UpdateObjectPhysics_Average", null, new GaugeConfiguration { SuppressInitialValue = true });

        // Calls from Landblock.TickLandblockGroupThreadSafeWork() - Cumulative over a single UpdateGameWorld Tick
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_Landblock_Tick_RunActions_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_Landblock_Tick_RunActions_Average", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_Landblock_Tick_Monster_Tick_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_Landblock_Tick_Monster_Tick_Average", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_Landblock_Tick_GeneratorUpdate_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_Landblock_Tick_GeneratorUpdate_Average", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_Landblock_Tick_GeneratorRegeneration_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_Landblock_Tick_GeneratorRegeneration_Average", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_Landblock_Tick_Heartbeat_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_Landblock_Tick_Heartbeat_Average", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_Landblock_Tick_Database_Save_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_Landblock_Tick_Database_Save_Average", null, new GaugeConfiguration { SuppressInitialValue = true });

        // Calls from Landblock.TickLandblockGroupThreadSafeWork() - Misc - Cumulative over a single UpdateGameWorld Tick
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_Monster_Awareness_FindNextTarget_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_Monster_Awareness_FindNextTarget_Average", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_Monster_Navigation_UpdatePosition_PUO_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_Monster_Navigation_UpdatePosition_PUO_Average", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_LootGenerationFactory_CreateRandomLootObjects_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_LootGenerationFactory_CreateRandomLootObjects_Average", null, new GaugeConfiguration { SuppressInitialValue = true });

        // Calls from Landblock.TickSingleThreadedWork() - Cumulative over a single UpdateGameWorld Tick
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_Landblock_Tick_Player_Tick_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_Landblock_Tick_Player_Tick_Average", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_Landblock_Tick_WorldObject_Heartbeat_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_Landblock_Tick_WorldObject_Heartbeat_Average", null, new GaugeConfiguration { SuppressInitialValue = true });
        */
        // Calls from NetworkManager.DoSessionWork()
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_DoSessionWork_TickOutbound_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_DoSessionWork_TickOutbound_Average", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_ServerPerformanceMonitor_5m_DoSessionWork_RemoveSessions_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_DoSessionWork_RemoveSessions_Average", null, new GaugeConfiguration { SuppressInitialValue = true });

        // Calls from NetworkManager.ProcessPacket()
        //private static readonly Gauge ace_ServerPerformanceMonitor_5m_ProcessPacket_0_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_ProcessPacket_0_Average", null, new GaugeConfiguration { SuppressInitialValue = true });
        //private static readonly Gauge ace_ServerPerformanceMonitor_5m_ProcessPacket_1_Average = Metrics.CreateGauge("ace_ServerPerformanceMonitor_5m_ProcessPacket_1_Average", null, new GaugeConfiguration { SuppressInitialValue = true });

        private static readonly Gauge ace_ServerObjectManager_ServerObjects = Metrics.CreateGauge("ace_ServerObjectManager_ServerObjects", null, new GaugeConfiguration { SuppressInitialValue = true });

        private static readonly Gauge ace_GuidManager_PlayerGuidAllocator_Min = Metrics.CreateGauge("ace_GuidManager_PlayerGuidAllocator_Min", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_GuidManager_PlayerGuidAllocator_Current = Metrics.CreateGauge("ace_GuidManager_PlayerGuidAllocator_Current", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_GuidManager_PlayerGuidAllocator_Max = Metrics.CreateGauge("ace_GuidManager_PlayerGuidAllocator_Max", null, new GaugeConfiguration { SuppressInitialValue = true });

        private static readonly Gauge ace_GuidManager_DynamicGuidAllocator_Min = Metrics.CreateGauge("ace_GuidManager_DynamicGuidAllocator_Min", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_GuidManager_DynamicGuidAllocator_Current = Metrics.CreateGauge("ace_GuidManager_DynamicGuidAllocator_Current", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_GuidManager_DynamicGuidAllocator_Max = Metrics.CreateGauge("ace_GuidManager_DynamicGuidAllocator_Max", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_GuidManager_DynamicGuidAllocator_SequenceGapPairsTotal = Metrics.CreateGauge("ace_GuidManager_DynamicGuidAllocator_SequenceGapPairsTotal", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_GuidManager_DynamicGuidAllocator_RecycledGuidsTotal = Metrics.CreateGauge("ace_GuidManager_DynamicGuidAllocator_RecycledGuidsTotal", null, new GaugeConfiguration { SuppressInitialValue = true });

        private static readonly Gauge ace_AllegianceManager_AllegiancesLoadedInMemory = Metrics.CreateGauge("ace_AllegianceManager_AllegiancesLoadedInMemory", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_AllegianceManager_PlayersMappedInMemory = Metrics.CreateGauge("ace_AllegianceManager_PlayersMappedInMemory", null, new GaugeConfiguration { SuppressInitialValue = true });

        private static readonly Gauge ace_HouseManager_TotalOwnedHousing = Metrics.CreateGauge("ace_HouseManager_TotalOwnedHousing", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_HouseManager_TotalOwnedApartments = Metrics.CreateGauge("ace_HouseManager_TotalOwnedApartments", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_HouseManager_TotalOwnedCottages = Metrics.CreateGauge("ace_HouseManager_TotalOwnedCottages", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_HouseManager_TotalOwnedVillas = Metrics.CreateGauge("ace_HouseManager_TotalOwnedVillas", null, new GaugeConfiguration { SuppressInitialValue = true });
        private static readonly Gauge ace_HouseManager_TotalOwnedMansions = Metrics.CreateGauge("ace_HouseManager_TotalOwnedMansions", null, new GaugeConfiguration { SuppressInitialValue = true });

        private static string database_Base_Version = null;
        private static string database_Patch_Version = null;

        static void InitMetrics()
        {
            // https://github.com/prometheus-net/prometheus-net
            Metrics.DefaultRegistry.AddBeforeCollectCallback(MetricsAddBeforeCollectCallback);

            metricServer = new MetricServer(hostname: ConfigManager.Config.Metrics.Host,
                                            port: ConfigManager.Config.Metrics.Port,
                                            url: ConfigManager.Config.Metrics.Url,
                                            useHttps: ConfigManager.Config.Metrics.UseHTTPs);
            metricServer.Start();

            // https://github.com/djluck/prometheus-net.DotNetRuntime

            dotNetMetricsCollector = DotNetRuntimeStatsBuilder
                .Customize()
                .WithGcStats(CaptureLevel.Informational)
                .WithSocketStats()
                .StartCollecting();

            //dotNetMetricsCollector = DotNetRuntimeStatsBuilder
            //    .Customize()
            //    .WithGcStats(CaptureLevel.Verbose)
            //    .WithJitStats(CaptureLevel.Verbose)
            //    .WithContentionStats(CaptureLevel.Informational)
            //    .WithThreadPoolStats(CaptureLevel.Informational)
            //    .WithExceptionStats(CaptureLevel.Errors)
            //    .WithSocketStats()
            //    .StartCollecting();
        }

        static void ShutdownMetrics()
        {
            // todo this throws exception
            //dotNetMetricsCollector.Dispose();

            metricServer.Stop();
        }

        static void MetricsAddBeforeCollectCallback()
        {
            var proc = Process.GetCurrentProcess();

            if (database_Base_Version is null && DatabaseManager.World is not null)
            {
                var dbVersion = DatabaseManager.World.GetVersion();
                database_Base_Version = dbVersion.BaseVersion;
                database_Patch_Version = dbVersion.PatchVersion;
            }

            ace_Info.WithLabels(ServerBuildInfo.FullVersion, database_Base_Version ?? "", database_Patch_Version ?? "").Set(1);

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
            if (DatabaseManager.Shard?.BaseDatabase is ShardDatabaseWithCaching shardDatabaseWithCaching)
            {
                var biotaIds = shardDatabaseWithCaching.GetBiotaCacheKeys();
                var playerBiotaIds = biotaIds.Count(id => ObjectGuid.IsPlayer(id));
                var nonPlayerBiotaIds = biotaIds.Count - playerBiotaIds;

                ace_DatabaseManager_Shard_CachedPlayerBiotas.Set(playerBiotaIds);
                ace_DatabaseManager_Shard_CachedNonPlayerBiotas.Set(nonPlayerBiotaIds);
            }
            ace_DatabaseManager_Shard_QueueCount.Set(DatabaseManager.Shard?.QueueCount ?? 0);
            DatabaseManager.Shard?.GetCurrentQueueWaitTime(result => ace_DatabaseManager_Shard_QueueWaitTime.Set(result.TotalMilliseconds));

            var loadedLandblocks = LandblockManager.GetLoadedLandblocks();
            int dormantLandblocks = 0, activeDungeonLandblocks = 0, dormantDungeonLandblocks = 0;
            int players = 0, creatures = 0, missiles = 0, other = 0, total = 0;
            try
            {
                foreach (var landblock in loadedLandblocks)
                {
                    try
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
                            try
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
                            catch (Exception)
                            {

                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            catch (Exception)
            {

            }
            ace_LandblockManager_ActiveLandblocks.Set(loadedLandblocks.Count - dormantLandblocks);
            ace_LandblockManager_DormantLandblocks.Set(dormantLandblocks);
            ace_LandblockManager_ActiveDungeons.Set(activeDungeonLandblocks);
            ace_LandblockManager_DormantDungeons.Set(dormantDungeonLandblocks);
            ace_LandblockManager_LandblockGroups.Set(LandblockManager.LandblockGroupsCount);
            ace_LandblockManager_Objects_Players.Set(players);
            ace_LandblockManager_Objects_Creatures.Set(creatures);
            ace_LandblockManager_Objects_Missiles.Set(missiles);
            ace_LandblockManager_Objects_Other.Set(other);
            ace_LandblockManager_Objects_Total.Set(total);

            if (ServerPerformanceMonitor.IsRunning)
            {
                // Calls from WorldManager.UpdateWorld()
                ace_ServerPerformanceMonitor_5m_PlayerManager_Tick_Average.Set(ServerPerformanceMonitor.GetEventHistory5m(ServerPerformanceMonitor.MonitorType.PlayerManager_Tick).AverageEventDuration);
                ace_ServerPerformanceMonitor_5m_NetworkManager_InboundClientMessageQueueRun_Average.Set(ServerPerformanceMonitor.GetEventHistory5m(ServerPerformanceMonitor.MonitorType.NetworkManager_InboundClientMessageQueueRun).AverageEventDuration);
                ace_ServerPerformanceMonitor_5m_actionQueue_RunActions_Average.Set(ServerPerformanceMonitor.GetEventHistory5m(ServerPerformanceMonitor.MonitorType.actionQueue_RunActions).AverageEventDuration);
                ace_ServerPerformanceMonitor_5m_DelayManager_RunActions_Average.Set(ServerPerformanceMonitor.GetEventHistory5m(ServerPerformanceMonitor.MonitorType.DelayManager_RunActions).AverageEventDuration);
                ace_ServerPerformanceMonitor_5m_UpdateGameWorld_Average.Set(ServerPerformanceMonitor.GetEventHistory5m(ServerPerformanceMonitor.MonitorType.UpdateGameWorld).AverageEventDuration);
                ace_ServerPerformanceMonitor_5m_NetworkManager_DoSessionWork_Average.Set(ServerPerformanceMonitor.GetEventHistory5m(ServerPerformanceMonitor.MonitorType.NetworkManager_DoSessionWork).AverageEventDuration);

                // WorldManager.UpdateGameWorld() time not including throttled returns
                ace_ServerPerformanceMonitor_5m_UpdateGameWorld_Entire_Average.Set(ServerPerformanceMonitor.GetEventHistory5m(ServerPerformanceMonitor.MonitorType.UpdateGameWorld_Entire).AverageEventDuration);
                ace_ServerPerformanceMonitor_5m_UpdateGameWorld_Entire_Longest.Set(ServerPerformanceMonitor.GetEventHistory5m(ServerPerformanceMonitor.MonitorType.UpdateGameWorld_Entire).LongestEvent);
                ace_ServerPerformanceMonitor_1h_UpdateGameWorld_Entire_Average.Set(ServerPerformanceMonitor.GetEventHistory1h(ServerPerformanceMonitor.MonitorType.UpdateGameWorld_Entire).AverageEventDuration);

                // Calls from WorldManager.UpdateGameWorld()
                ace_ServerPerformanceMonitor_5m_LandblockManager_TickPhysics_Average.Set(ServerPerformanceMonitor.GetEventHistory5m(ServerPerformanceMonitor.MonitorType.LandblockManager_TickPhysics).AverageEventDuration);
                ace_ServerPerformanceMonitor_5m_LandblockManager_TickMultiThreadedWork_Average.Set(ServerPerformanceMonitor.GetEventHistory5m(ServerPerformanceMonitor.MonitorType.LandblockManager_TickMultiThreadedWork).AverageEventDuration);
                ace_ServerPerformanceMonitor_5m_LandblockManager_TickSingleThreadedWork_Average.Set(ServerPerformanceMonitor.GetEventHistory5m(ServerPerformanceMonitor.MonitorType.LandblockManager_TickSingleThreadedWork).AverageEventDuration);

                // Calls from NetworkManager.DoSessionWork()
                ace_ServerPerformanceMonitor_5m_DoSessionWork_TickOutbound_Average.Set(ServerPerformanceMonitor.GetEventHistory5m(ServerPerformanceMonitor.MonitorType.DoSessionWork_TickOutbound).AverageEventDuration);
                ace_ServerPerformanceMonitor_5m_DoSessionWork_RemoveSessions_Average.Set(ServerPerformanceMonitor.GetEventHistory5m(ServerPerformanceMonitor.MonitorType.DoSessionWork_RemoveSessions).AverageEventDuration);

                // Calls from NetworkManager.ProcessPacket()
                //ace_ServerPerformanceMonitor_5m_ProcessPacket_0_Average.Set(ServerPerformanceMonitor.GetEventHistory5m(ServerPerformanceMonitor.MonitorType.ProcessPacket_0).AverageEventDuration);
                //ace_ServerPerformanceMonitor_5m_ProcessPacket_1_Average.Set(ServerPerformanceMonitor.GetEventHistory5m(ServerPerformanceMonitor.MonitorType.ProcessPacket_1).AverageEventDuration);
            }

            /*
            sb.Append($"Threading - WorldThreadCount: {ConfigManager.Config.Server.Threading.LandblockManagerParallelOptions.MaxDegreeOfParallelism}, Multithread Physics: {ConfigManager.Config.Server.Threading.MultiThreadedLandblockGroupPhysicsTicking}, Multithread Non-Physics: {ConfigManager.Config.Server.Threading.MultiThreadedLandblockGroupTicking}, DatabaseThreadCount: {ConfigManager.Config.Server.Threading.DatabaseParallelOptions.MaxDegreeOfParallelism}{'\n'}");

            sb.Append($"Physics Cache Counts - BSPCache: {BSPCache.Count:N0}, GfxObjCache: {GfxObjCache.Count:N0}, PolygonCache: {PolygonCache.Count:N0}, VertexCache: {VertexCache.Count:N0}{'\n'}");
            */

            ace_ServerObjectManager_ServerObjects.Set(ServerObjectManager.ServerObjects.Count);

            /*
            sb.Append($"World DB Cache Counts - Weenies: {DatabaseManager.World.GetWeenieCacheCount():N0}, LandblockInstances: {DatabaseManager.World.GetLandblockInstancesCacheCount():N0}, PointsOfInterest: {DatabaseManager.World.GetPointsOfInterestCacheCount():N0}, Cookbooks: {DatabaseManager.World.GetCookbookCacheCount():N0}, Spells: {DatabaseManager.World.GetSpellCacheCount():N0}, Encounters: {DatabaseManager.World.GetEncounterCacheCount():N0}, Events: {DatabaseManager.World.GetEventsCacheCount():N0}{'\n'}");
            sb.Append($"Shard DB Counts - Biotas: {DatabaseManager.Shard.BaseDatabase.GetBiotaCount():N0}{'\n'}");
            */

            /*
            sb.Append(GuidManager.GetDynamicGuidDebugInfo() + '\n');

            sb.Append($"Portal.dat has {DatManager.PortalDat.FileCache.Count:N0} files cached of {DatManager.PortalDat.AllFiles.Count:N0} total{'\n'}");
            sb.Append($"Cell.dat has {DatManager.CellDat.FileCache.Count:N0} files cached of {DatManager.CellDat.AllFiles.Count:N0} total{'\n'}");
            */

            ace_GuidManager_PlayerGuidAllocator_Min.Set(GuidManager.PlayerMin);
            ace_GuidManager_PlayerGuidAllocator_Current.Set(GuidManager.PlayerCurrent);
            ace_GuidManager_PlayerGuidAllocator_Max.Set(GuidManager.PlayerMax);

            ace_GuidManager_DynamicGuidAllocator_Min.Set(GuidManager.DynamicMin);
            ace_GuidManager_DynamicGuidAllocator_Current.Set(GuidManager.DynamicCurrent);
            ace_GuidManager_DynamicGuidAllocator_Max.Set(GuidManager.DynamicMax);
            ace_GuidManager_DynamicGuidAllocator_SequenceGapPairsTotal.Set(GuidManager.SequenceGapPairsTotal);
            ace_GuidManager_DynamicGuidAllocator_RecycledGuidsTotal.Set(GuidManager.RecycledGuidsTotal);

            ace_AllegianceManager_AllegiancesLoadedInMemory.Set(AllegianceManager.Allegiances.Count);
            ace_AllegianceManager_PlayersMappedInMemory.Set(AllegianceManager.Players.Count);

            ace_HouseManager_TotalOwnedHousing.Set(HouseManager.TotalOwnedHousing);
            if (HouseManager.TotalOwnedHousingByType.TryGetValue(ACE.Entity.Enum.HouseType.Apartment, out var totalOwnedApartments))
                ace_HouseManager_TotalOwnedApartments.Set(totalOwnedApartments);
            else
                ace_HouseManager_TotalOwnedApartments.Set(0);
            if (HouseManager.TotalOwnedHousingByType.TryGetValue(ACE.Entity.Enum.HouseType.Cottage, out var totalOwnedCottages))
                ace_HouseManager_TotalOwnedCottages.Set(totalOwnedCottages);
            else
                ace_HouseManager_TotalOwnedCottages.Set(0);
            if (HouseManager.TotalOwnedHousingByType.TryGetValue(ACE.Entity.Enum.HouseType.Villa, out var totalOwnedVillas))
                ace_HouseManager_TotalOwnedVillas.Set(totalOwnedVillas);
            else
                ace_HouseManager_TotalOwnedVillas.Set(0);
            if (HouseManager.TotalOwnedHousingByType.TryGetValue(ACE.Entity.Enum.HouseType.Mansion, out var totalOwnedMansions))
                ace_HouseManager_TotalOwnedMansions.Set(totalOwnedMansions);
            else
                ace_HouseManager_TotalOwnedMansions.Set(0);
        }
    }
}
