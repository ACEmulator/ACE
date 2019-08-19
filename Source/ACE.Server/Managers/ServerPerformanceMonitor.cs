using System;
using System.Collections.Generic;
using System.Text;

using ACE.Common;

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
            LandblockManager_Tick,

            // These are all found in Landblock.Tick()
            Landblock_Tick_RunActions,
            Landblock_Tick_Player_Tick,
            Landblock_Tick_Monster_Tick,
            Landblock_Tick_WorldObject_Heartbeat,
            Landblock_Tick_GeneratorUpdate,
            Landblock_Tick_GeneratorRegeneration,
            Landblock_Tick_Heartbeat,
            Landblock_Tick_Database_Save,

            // These are all found in various places and are cumulative per Landblock_Tick
            Monster_Awareness_FindNextTarget,
            Monster_Navigation_UpdatePosition_PUO,

            // These are all found in NetworkManager.DoSessionWork()
            DoSessionWork_TickOutbound,
            DoSessionWork_RemoveSessions,

            // These are all found in NetworkManager.ProcessPacket()
            ProcessPacket_0,
            ProcessPacket_1,

            MonitorMaxItems // Keep this at the end to properly size our monitors array
        }

        /// <summary>
        /// These are monitors that are resumed/paused many times over the course of a single game loop (WorldManager.UpdateGameWorld)<para />
        /// Their purpose is to give a performance value for a system and all the work it may process in a single loop (WorldManager.UpdateGameWorld).
        /// </summary>
        private static readonly HashSet<MonitorType> cumulativeMonitorTypes = new HashSet<MonitorType>
        {
            MonitorType.Landblock_Tick_RunActions,
            MonitorType.Landblock_Tick_Player_Tick,
            MonitorType.Landblock_Tick_Monster_Tick,
            MonitorType.Landblock_Tick_WorldObject_Heartbeat,
            MonitorType.Landblock_Tick_GeneratorUpdate,
            MonitorType.Landblock_Tick_GeneratorRegeneration,
            MonitorType.Landblock_Tick_Heartbeat,
            MonitorType.Landblock_Tick_Database_Save,

            MonitorType.Monster_Awareness_FindNextTarget,
            MonitorType.Monster_Navigation_UpdatePosition_PUO,
        };

        private static readonly RateMonitor[] monitors5m = new RateMonitor[(int)MonitorType.MonitorMaxItems];
        private static readonly RateMonitor[] monitors1h = new RateMonitor[(int)MonitorType.MonitorMaxItems];
        private static readonly RateMonitor[] monitors24h = new RateMonitor[(int)MonitorType.MonitorMaxItems];

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

                last5mClear = DateTime.UtcNow;
            }

            if (DateTime.UtcNow - last1hClear >= last1hClearInteval)
            {
                foreach (var monitor in monitors1h)
                    monitor.ClearEventHistory();

                last1hClear = DateTime.UtcNow;
            }

            if (DateTime.UtcNow - last24hClear >= last24hClearInterval)
            {
                foreach (var monitor in monitors24h)
                    monitor.ClearEventHistory();

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

            last5mClear = DateTime.UtcNow;
            last1hClear = DateTime.UtcNow;
            last24hClear = DateTime.UtcNow;
        }


        public static void RegisterEventStart(MonitorType monitorType)
        {
            if (!IsRunning)
                return;

            monitors24h[(int)monitorType].RegisterEventStart();
            monitors1h[(int)monitorType].RegisterEventStart();
            monitors5m[(int)monitorType].RegisterEventStart();

            // Reset cumulative monitors
            if (monitorType == MonitorType.UpdateGameWorld_Entire)
            {
                foreach (var entry in cumulativeMonitorTypes)
                {
                    monitors5m[(int)entry].ResetEvent();
                    monitors1h[(int)entry].ResetEvent();
                    monitors24h[(int)entry].ResetEvent();
                }
            }
        }

        public static void RegisterEventEnd(MonitorType monitorType)
        {
            if (!IsRunning)
                return;

            monitors5m[(int)monitorType].RegisterEventEnd();
            monitors1h[(int)monitorType].RegisterEventEnd();
            monitors24h[(int)monitorType].RegisterEventEnd();

            // Register end for cumulative monitors
            if (monitorType == MonitorType.UpdateGameWorld_Entire)
            {
                foreach (var entry in cumulativeMonitorTypes)
                {
                    monitors5m[(int)entry].RegisterEventEnd();
                    monitors1h[(int)entry].RegisterEventEnd();
                    monitors24h[(int)entry].RegisterEventEnd();
                }
            }
        }


        public static void ResumeEvent(MonitorType monitorType)
        {
            if (!IsRunning)
                return;

            monitors24h[(int)monitorType].ResumeEvent();
            monitors1h[(int)monitorType].ResumeEvent();
            monitors5m[(int)monitorType].ResumeEvent();
        }

        public static void PauseEvent(MonitorType monitorType)
        {
            if (!IsRunning)
                return;

            monitors5m[(int)monitorType].PauseEvent();
            monitors1h[(int)monitorType].PauseEvent();
            monitors24h[(int)monitorType].PauseEvent();
        }


        public static RateMonitor GetMonitor5m(MonitorType monitorType)
        {
            return monitors5m[(int) monitorType];
        }

        public static RateMonitor GetMonitor1h(MonitorType monitorType)
        {
            return monitors1h[(int)monitorType];
        }

        public static RateMonitor GetMonitor24h(MonitorType monitorType)
        {
            return monitors24h[(int)monitorType];
        }


        public new static string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"Monitoring Durations: ~5m {Monitors5mRunTime.TotalMinutes:N2} min, ~1h {Monitors1hRunTime.TotalMinutes:N2} min, ~24h {Monitors24hRunTime.TotalMinutes:N2} min{'\n'}");
            sb.Append($"~5m Hits   Avg  Long  Last Tot - ~1h Hits   Avg  Long  Last  Tot - ~24h Hits  Avg  Long  Last   Tot (s) - Name{'\n'}");

            sb.Append($"Calls from WorldManager.UpdateWorld(){'\n'}");
            for (int i = (int)MonitorType.PlayerManager_Tick; i <= (int)MonitorType.NetworkManager_DoSessionWork; i++)
                AddMonitorOutputToStringBuilder(monitors5m[i], monitors1h[i], monitors24h[i], (MonitorType)i, sb);

            sb.Append($"WorldManager.UpdateGameWorld() time not including throttled returns{'\n'}");
            AddMonitorOutputToStringBuilder(monitors5m[(int)MonitorType.UpdateGameWorld_Entire], monitors1h[(int)MonitorType.UpdateGameWorld_Entire], monitors24h[(int)MonitorType.UpdateGameWorld_Entire], MonitorType.UpdateGameWorld_Entire, sb);

            sb.Append($"Calls from WorldManager.UpdateGameWorld(){'\n'}");
            for (int i = (int)MonitorType.LandblockManager_TickPhysics; i <= (int)MonitorType.LandblockManager_Tick; i++)
                AddMonitorOutputToStringBuilder(monitors5m[i], monitors1h[i], monitors24h[i], (MonitorType)i, sb);

            sb.Append($"Calls from Landblock.Tick() - Cumulative over a single UpdateGameWorld Tick{'\n'}");
            for (int i = (int)MonitorType.Landblock_Tick_RunActions; i <= (int)MonitorType.Landblock_Tick_Database_Save; i++)
                AddMonitorOutputToStringBuilder(monitors5m[i], monitors1h[i], monitors24h[i], (MonitorType)i, sb);

            sb.Append($"Calls from Landblock.Tick() - Misc - Cumulative over a single UpdateGameWorld Tick{'\n'}");
            for (int i = (int)MonitorType.Monster_Awareness_FindNextTarget; i <= (int)MonitorType.Monster_Navigation_UpdatePosition_PUO; i++)
                AddMonitorOutputToStringBuilder(monitors5m[i], monitors1h[i], monitors24h[i], (MonitorType)i, sb);

            sb.Append($"Calls from NetworkManager.DoSessionWork(){'\n'}");
            for (int i = (int)MonitorType.DoSessionWork_TickOutbound; i <= (int)MonitorType.DoSessionWork_RemoveSessions; i++)
                AddMonitorOutputToStringBuilder(monitors5m[i], monitors1h[i], monitors24h[i], (MonitorType)i, sb);

            sb.Append($"Calls from NetworkManager.ProcessPacket(){'\n'}");
            for (int i = (int)MonitorType.ProcessPacket_0; i <= (int)MonitorType.ProcessPacket_1; i++)
                AddMonitorOutputToStringBuilder(monitors5m[i], monitors1h[i], monitors24h[i], (MonitorType)i, sb);

            return sb.ToString();
        }

        private static void AddMonitorOutputToStringBuilder(RateMonitor monitor5m, RateMonitor monitor1h, RateMonitor monitor24h, MonitorType monitorType, StringBuilder sb)
        {
            sb.Append($"{monitor5m.TotalEvents.ToString().PadLeft(7)} {monitor5m.AverageEventDuration:N4} {monitor5m.LongestEvent:N3} {monitor5m.LastEvent:N3} {((int)monitor5m.TotalSeconds).ToString().PadLeft(3)} - " +
                      $"{monitor1h.TotalEvents.ToString().PadLeft(7)} {monitor1h.AverageEventDuration:N4} {monitor1h.LongestEvent:N3} {monitor1h.LastEvent:N3} {((int)monitor1h.TotalSeconds).ToString().PadLeft(4)} - " +
                      $"{monitor24h.TotalEvents.ToString().PadLeft(7)} {monitor24h.AverageEventDuration:N4} {monitor24h.LongestEvent:N3} {monitor24h.LastEvent:N3} {((int)monitor24h.TotalSeconds).ToString().PadLeft(5)} - " +
                      $"{monitorType}{'\n'}");
        }
    }
}
