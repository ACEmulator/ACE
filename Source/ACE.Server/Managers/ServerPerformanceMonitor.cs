using System;
using System.Text;

using ACE.Common.Performance;

namespace ACE.Server.Managers
{
    public static class ServerPerformanceMonitor
    {
        public static bool IsRunning;


        public enum MonitorType
        {
            // These are all found in WorldManager.UpdateWorld()
            PlayerManager_Tick,
            NetworkManager_InboundClientMessageQueueRun,
            actionQueue_RunActions,
            DelayManager_RunActions,
            UpdateGameWorld,
            NetworkManager_DoSessionWork,

            // These are all found in WorldManager.UpdateGameWorld()
            UpdateGameWorld_Entire,
            LandblockManager_TickPhysics,
            LandblockManager_TickMultiThreadedWork,
            LandblockManager_TickSingleThreadedWork,

            // These are all found in NetworkManager.DoSessionWork()
            DoSessionWork_TickOutbound,
            DoSessionWork_RemoveSessions,

            // These are all found in NetworkManager.ProcessPacket()
            ProcessPacket_0,
            ProcessPacket_1,

            MaxItems // Keep this at the end to properly size our monitors array
        }

        private static readonly RateMonitor[] monitors5m = new RateMonitor[(int)MonitorType.MaxItems];
        private static readonly RateMonitor[] monitors1h = new RateMonitor[(int)MonitorType.MaxItems];
        private static readonly RateMonitor[] monitors24h = new RateMonitor[(int)MonitorType.MaxItems];


        /// <summary>
        /// These are monitors that are resumed/paused many times over the course of a single game loop (WorldManager.UpdateGameWorld)<para />
        /// Their purpose is to give a performance value for a system and all the work it may process in a single loop (WorldManager.UpdateGameWorld).
        /// </summary>
        public enum CumulativeEventHistoryType
        {
            // These are found in Player_Tick.cs and WorldObject_Tick.cs
            Player_Tick_UpdateObjectPhysics,
            WorldObject_Tick_UpdateObjectPhysics,

            // These are all found in Landblock.TickLandblockGroupThreadSafeWork()
            Landblock_Tick_RunActions,
            Landblock_Tick_Monster_Tick,
            Landblock_Tick_GeneratorUpdate,
            Landblock_Tick_GeneratorRegeneration,
            Landblock_Tick_Heartbeat,
            Landblock_Tick_Database_Save,

            // These are all found in Landblock.TickSingleThreadedWork()
            Landblock_Tick_Player_Tick,
            Landblock_Tick_WorldObject_Heartbeat,

            // These are all found in various places and are cumulative per Landblock_Tick
            Monster_Awareness_FindNextTarget,
            Monster_Navigation_UpdatePosition_PUO,
            LootGenerationFactory_CreateRandomLootObjects,

            MaxItems // Keep this at the end to properly size our monitors array
        }

        private static readonly TimedEventHistory[] cumulative5m = new TimedEventHistory[(int)CumulativeEventHistoryType.MaxItems];
        private static readonly TimedEventHistory[] cumulative1h = new TimedEventHistory[(int)CumulativeEventHistoryType.MaxItems];
        private static readonly TimedEventHistory[] cumulative24h = new TimedEventHistory[(int)CumulativeEventHistoryType.MaxItems];


        private static readonly TimeSpan last5mClearInterval = TimeSpan.FromMinutes(5);
        private static readonly TimeSpan last1hClearInteval = TimeSpan.FromHours(1);
        private static readonly TimeSpan last24hClearInterval = TimeSpan.FromHours(24);

        private static DateTime last5mClear;
        private static DateTime last1hClear;
        private static DateTime last24hClear;

        private static TimeSpan Monitors5mRunTime => DateTime.UtcNow - last5mClear;
        private static TimeSpan Monitors1hRunTime => DateTime.UtcNow - last1hClear;
        private static TimeSpan Monitors24hRunTime => DateTime.UtcNow - last24hClear;


        static ServerPerformanceMonitor()
        {
            for (int i = 0; i < monitors5m.Length; i++)
            {
                monitors5m[i] = new RateMonitor();
                monitors1h[i] = new RateMonitor();
                monitors24h[i] = new RateMonitor();
            }

            for (int i = 0; i < cumulative5m.Length; i++)
            {
                cumulative5m[i] = new TimedEventHistory();
                cumulative1h[i] = new TimedEventHistory();
                cumulative24h[i] = new TimedEventHistory();
            }
        }


        public static void Start()
        {
            if (IsRunning)
                return;

            Reset();

            IsRunning = true;
        }

        public static void Stop()
        {
            if (!IsRunning)
                return;

            IsRunning = false;
        }

        public static void Tick()
        {
            if (!IsRunning)
                return;

            // check to see if we should clear history
            if (DateTime.UtcNow - last5mClear >= last5mClearInterval)
            {
                foreach (var monitor in monitors5m)
                    monitor.ClearEventHistory();

                foreach (var eventHistory in cumulative5m)
                    eventHistory.ClearHistory();

                last5mClear = DateTime.UtcNow;
            }

            if (DateTime.UtcNow - last1hClear >= last1hClearInteval)
            {
                foreach (var monitor in monitors1h)
                    monitor.ClearEventHistory();

                foreach (var eventHistory in cumulative1h)
                    eventHistory.ClearHistory();

                last1hClear = DateTime.UtcNow;
            }

            if (DateTime.UtcNow - last24hClear >= last24hClearInterval)
            {
                foreach (var monitor in monitors24h)
                    monitor.ClearEventHistory();

                foreach (var eventHistory in cumulative24h)
                    eventHistory.ClearHistory();

                last24hClear = DateTime.UtcNow;
            }
        }

        public static void Reset()
        {
            if (!IsRunning)
                return;

            for (int i = 0; i < monitors5m.Length; i++)
            {
                monitors5m[i].ClearEventHistory();
                monitors1h[i].ClearEventHistory();
                monitors24h[i].ClearEventHistory();
            }

            for (int i = 0; i < cumulative5m.Length; i++)
            {
                cumulative5m[i].ClearHistory();
                cumulative1h[i].ClearHistory();
                cumulative24h[i].ClearHistory();
            }

            last5mClear = DateTime.UtcNow;
            last1hClear = DateTime.UtcNow;
            last24hClear = DateTime.UtcNow;
        }


        public static void RestartEvent(MonitorType monitorType)
        {
            if (!IsRunning)
                return;

            monitors24h[(int)monitorType].Restart();
            monitors1h[(int)monitorType].Restart();
            monitors5m[(int)monitorType].Restart();
        }

        public static void RegisterEventEnd(MonitorType monitorType)
        {
            if (!IsRunning)
                return;

            monitors5m[(int)monitorType].RegisterEventEnd();
            monitors1h[(int)monitorType].RegisterEventEnd();
            monitors24h[(int)monitorType].RegisterEventEnd();
        }


        private static readonly double[] cumulativeSeconds = new double[(int)CumulativeEventHistoryType.MaxItems];

        public static void RestartCumulativeEvents()
        {
            if (!IsRunning)
                return;

            for (int i = 0; i < cumulativeSeconds.Length; i++)
                cumulativeSeconds[i] = 0;
        }

        public static void AddToCumulativeEvent(CumulativeEventHistoryType eventHistoryType, double seconds)
        {
            if (!IsRunning)
                return;

            lock (cumulative5m[(int)eventHistoryType])
                cumulativeSeconds[(int)eventHistoryType] += seconds;
        }

        public static void RegisterCumulativeEvents()
        {
            if (!IsRunning)
                return;

            for (int i = 0; i < cumulative5m.Length; i++)
            {
                cumulative5m[i].RegisterEvent(cumulativeSeconds[i]);
                cumulative1h[i].RegisterEvent(cumulativeSeconds[i]);
                cumulative24h[i].RegisterEvent(cumulativeSeconds[i]);
            }
        }


        public static TimedEventHistory GetEventHistory5m(MonitorType monitorType)
        {
            return monitors5m[(int) monitorType].EventHistory;
        }

        public static TimedEventHistory GetEventHistory1h(MonitorType monitorType)
        {
            return monitors1h[(int)monitorType].EventHistory;
        }

        public static TimedEventHistory GetEventHistory24h(MonitorType monitorType)
        {
            return monitors24h[(int)monitorType].EventHistory;
        }


        public new static string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"Monitoring Durations: ~5m {Monitors5mRunTime.TotalMinutes:N2} min, ~1h {Monitors1hRunTime.TotalMinutes:N2} min, ~24h {Monitors24hRunTime.TotalMinutes:N2} min{'\n'}");
            sb.Append($"~5m Hits   Avg  Long  Last Tot - ~1h Hits   Avg  Long  Last  Tot - ~24h Hits  Avg  Long  Last   Tot (s) - Name{'\n'}");

            sb.Append($"Calls from WorldManager.UpdateWorld(){'\n'}");
            for (int i = (int)MonitorType.PlayerManager_Tick; i <= (int)MonitorType.NetworkManager_DoSessionWork; i++)
                AddMonitorOutputToStringBuilder(monitors5m[i].EventHistory, monitors1h[i].EventHistory, monitors24h[i].EventHistory, ((MonitorType)i).ToString(), sb);

            sb.Append($"WorldManager.UpdateGameWorld() time not including throttled returns{'\n'}");
            AddMonitorOutputToStringBuilder(monitors5m[(int)MonitorType.UpdateGameWorld_Entire].EventHistory, monitors1h[(int)MonitorType.UpdateGameWorld_Entire].EventHistory, monitors24h[(int)MonitorType.UpdateGameWorld_Entire].EventHistory, MonitorType.UpdateGameWorld_Entire.ToString(), sb);

            sb.Append($"Calls from WorldManager.UpdateGameWorld(){'\n'}");
            for (int i = (int)MonitorType.LandblockManager_TickPhysics; i <= (int)MonitorType.LandblockManager_TickSingleThreadedWork; i++)
                AddMonitorOutputToStringBuilder(monitors5m[i].EventHistory, monitors1h[i].EventHistory, monitors24h[i].EventHistory, ((MonitorType)i).ToString(), sb);

            sb.Append($"Calls from Landblock.TickPhysics() - Cumulative over a single UpdateGameWorld Tick{'\n'}");
            for (int i = (int)CumulativeEventHistoryType.Player_Tick_UpdateObjectPhysics; i <= (int)CumulativeEventHistoryType.WorldObject_Tick_UpdateObjectPhysics; i++)
                AddMonitorOutputToStringBuilder(cumulative5m[i], cumulative1h[i], cumulative24h[i], ((CumulativeEventHistoryType)i).ToString(), sb);

            sb.Append($"Calls from Landblock.TickLandblockGroupThreadSafeWork() - Cumulative over a single UpdateGameWorld Tick{'\n'}");
            for (int i = (int)CumulativeEventHistoryType.Landblock_Tick_RunActions; i <= (int)CumulativeEventHistoryType.Landblock_Tick_Database_Save; i++)
                AddMonitorOutputToStringBuilder(cumulative5m[i], cumulative1h[i], cumulative24h[i], ((CumulativeEventHistoryType)i).ToString(), sb);

            sb.Append($"Calls from Landblock.TickLandblockGroupThreadSafeWork() - Misc - Cumulative over a single UpdateGameWorld Tick{'\n'}");
            for (int i = (int)CumulativeEventHistoryType.Monster_Awareness_FindNextTarget; i <= (int)CumulativeEventHistoryType.LootGenerationFactory_CreateRandomLootObjects; i++)
                AddMonitorOutputToStringBuilder(cumulative5m[i], cumulative1h[i], cumulative24h[i], ((CumulativeEventHistoryType)i).ToString(), sb);

            sb.Append($"Calls from Landblock.TickSingleThreadedWork() - Cumulative over a single UpdateGameWorld Tick{'\n'}");
            for (int i = (int)CumulativeEventHistoryType.Landblock_Tick_Player_Tick; i <= (int)CumulativeEventHistoryType.Landblock_Tick_WorldObject_Heartbeat; i++)
                AddMonitorOutputToStringBuilder(cumulative5m[i], cumulative1h[i], cumulative24h[i], ((CumulativeEventHistoryType)i).ToString(), sb);

            sb.Append($"Calls from NetworkManager.DoSessionWork(){'\n'}");
            for (int i = (int)MonitorType.DoSessionWork_TickOutbound; i <= (int)MonitorType.DoSessionWork_RemoveSessions; i++)
                AddMonitorOutputToStringBuilder(monitors5m[i].EventHistory, monitors1h[i].EventHistory, monitors24h[i].EventHistory, ((MonitorType)i).ToString(), sb);

            sb.Append($"Calls from NetworkManager.ProcessPacket(){'\n'}");
            for (int i = (int)MonitorType.ProcessPacket_0; i <= (int)MonitorType.ProcessPacket_1; i++)
                AddMonitorOutputToStringBuilder(monitors5m[i].EventHistory, monitors1h[i].EventHistory, monitors24h[i].EventHistory, ((MonitorType)i).ToString(), sb);

            return sb.ToString();
        }

        private static void AddMonitorOutputToStringBuilder(TimedEventHistory eventHistory5m, TimedEventHistory eventHistory1h, TimedEventHistory eventHistory24h, string name, StringBuilder sb)
        {
            sb.Append($"{eventHistory5m.TotalEvents.ToString().PadLeft(7)} {eventHistory5m.AverageEventDuration:N4} {eventHistory5m.LongestEvent:N3} {eventHistory5m.LastEvent:N3} {((int)eventHistory5m.TotalSeconds).ToString().PadLeft(3)} - " +
                      $"{eventHistory1h.TotalEvents.ToString().PadLeft(7)} {eventHistory1h.AverageEventDuration:N4} {eventHistory1h.LongestEvent:N3} {eventHistory1h.LastEvent:N3} {((int)eventHistory1h.TotalSeconds).ToString().PadLeft(4)} - " +
                      $"{eventHistory24h.TotalEvents.ToString().PadLeft(7)} {eventHistory24h.AverageEventDuration:N4} {eventHistory24h.LongestEvent:N3} {eventHistory24h.LastEvent:N3} {((int)eventHistory24h.TotalSeconds).ToString().PadLeft(5)} - " +
                      $"{name}{'\n'}");
        }
    }
}
