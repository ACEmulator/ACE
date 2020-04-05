using System;
using System.Threading;

using log4net;

using ACE.Common;
using ACE.Database;
using ACE.Entity.Enum;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.Managers
{
    /// <summary>
    /// ServerManager handles unloading the server application properly.
    /// </summary>
    /// <remarks>
    ///   Possibly useful for:
    ///     1. Monitor for errors and performance issues in LandblockManager, GuidManager, WorldManager,
    ///         DatabaseManager, or AssetManager
    ///   Known issue:
    ///     1. No method to verify that everything unloaded properly.
    /// </remarks>
    public static class ServerManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Indicates advanced warning if the applcation will unload.
        /// </summary>
        public static bool ShutdownInitiated { get; private set; }

        /// <summary>
        /// The amount of seconds that the server will wait before unloading the application.
        /// </summary>
        public static uint ShutdownInterval { get; private set; }

        public static DateTime ShutdownTime { get; private set; } = DateTime.MinValue;

        /// <summary>
        /// Sets the Shutdown Interval in Seconds
        /// </summary>
        /// <param name="interval">postive value representing seconds</param>
        public static void SetShutdownInterval(uint interval)
        {
            log.Info($"Server shutdown interval reset: {interval}");
            ShutdownInterval = interval;
        }

        public static void Initialize()
        {
            // Loads the configuration for ShutdownInterval from the settings file.
            ShutdownInterval = ConfigManager.Config.Server.ShutdownInterval;
        }

        /// <summary>
        /// Starts the shutdown wait thread.
        /// </summary>
        public static void BeginShutdown()
        {
            ShutdownInitiated = true;

            var shutdownThread = new Thread(ShutdownServer);
            shutdownThread.Name = "Shutdown Server";
            shutdownThread.Start();
        }

        /// <summary>
        /// Calling this function will always cancel an in-progress shutdown (application unload). This will also
        /// stop the shutdown wait thread and alert users that the server will stay in operation.
        /// </summary>
        public static void CancelShutdown()
        {
            ShutdownInitiated = false;
            ShutdownTime = DateTime.MinValue;
        }

        public static void DoShutdownNow()
        {
            SetShutdownInterval(0);
            ShutdownInitiated = true;
            PlayerManager.BroadcastToAll(new GameMessageSystemChat("Broadcast from System> ATTENTION - This Asheron's Call Server is shutting down NOW!!!!", ChatMessageType.WorldBroadcast));
            ShutdownServer();
        }

        /// <summary>
        /// Threaded task created when performing a server shutdown
        /// </summary>
        private static void ShutdownServer()
        {
            var shutdownTime = DateTime.UtcNow.AddSeconds(ShutdownInterval);

            ShutdownTime = shutdownTime;

            var lastNoticeTime = DateTime.UtcNow;

            // wait for shutdown interval to expire
            while (shutdownTime != DateTime.MinValue && shutdownTime >= DateTime.UtcNow)
            {
                // this allows the server shutdown to be canceled
                if (!ShutdownInitiated)
                {
                    // reset shutdown details
                    string shutdownText = $"The server has canceled the shutdown procedure @ {DateTime.UtcNow} UTC";
                    log.Info(shutdownText);

                    // special text
                    foreach (var player in PlayerManager.GetAllOnline())
                        player.Session.WorldBroadcast(shutdownText);

                    // break function
                    return;
                }

                lastNoticeTime = NotifyPlayersOfPendingShutdown(lastNoticeTime, shutdownTime.AddSeconds(1));

                Thread.Sleep(10);
            }

            PropertyManager.ResyncVariables();
            PropertyManager.StopUpdating();

            log.Debug("Logging off all players...");

            // logout each player
            foreach (var player in PlayerManager.GetAllOnline())
                player.Session.LogOffPlayer(true);

            log.Info("Waiting for all players to log off...");

            // wait 10 seconds for log-off
            while (PlayerManager.GetOnlineCount() > 0)
                Thread.Sleep(10);

            log.Debug("Adding all landblocks to destruction queue...");

            // Queue unloading of all the landblocks
            // The actual unloading will happen in WorldManager.UpdateGameWorld
            LandblockManager.AddAllActiveLandblocksToDestructionQueue();

            log.Info("Waiting for all active landblocks to unload...");

            while (LandblockManager.GetLoadedLandblocks().Count > 0)
                Thread.Sleep(10);

            log.Debug("Stopping world...");

            // Disabled thread update loop
            WorldManager.StopWorld();

            log.Info("Waiting for world to stop...");

            // Wait for world to end
            while (WorldManager.WorldActive)
                Thread.Sleep(10);

            log.Info("Saving OfflinePlayers that have unsaved changes...");
            PlayerManager.SaveOfflinePlayersWithChanges();

            log.Info("Waiting for database queue to empty...");

            // Wait for the database queue to empty
            while (DatabaseManager.Shard.QueueCount > 0)
                Thread.Sleep(10);

            // Write exit to console/log
            log.Info($"Exiting at {DateTime.UtcNow}");

            // System exit
            Environment.Exit(Environment.ExitCode);
        }

        private static DateTime NotifyPlayersOfPendingShutdown(DateTime lastNoticeTime, DateTime shutdownTime)
        {
            var notify = false;

            var sdt = shutdownTime - DateTime.UtcNow;
                var timeHrs = $"{(sdt.Hours >= 1 ? $"{sdt.ToString("%h")}" : "")}{(sdt.Hours >= 2 ? $" hours" : sdt.Hours == 1 ? " hour" : "")}";
                var timeMins = $"{(sdt.Minutes != 0 ? $"{sdt.ToString("%m")}" : "")}{(sdt.Minutes >= 2 ? $" minutes" : sdt.Minutes == 1 ? " minute" : "")}";
                var timeSecs = $"{(sdt.Seconds != 0 ? $"{sdt.ToString("%s")}" : "")}{(sdt.Seconds >= 2 ? $" seconds" : sdt.Seconds == 1 ? " second" : "")}";
                var time = $"{(timeHrs != "" ? timeHrs : "")}{(timeMins != "" ? $"{((timeHrs != "") ? ", " : "")}" + timeMins : "")}{(timeSecs != "" ? $"{((timeHrs != "" || timeMins != "") ? " and " : "")}" + timeSecs : "")}";

            switch (time)
            {
                case "2 hours":
                case "1 hour":
                case "45 minutes":
                case "30 minutes":
                case "15 minutes":
                case "10 minutes":
                case "5 minutes":
                case "2 minutes":
                case "1 minute and 30 seconds":
                case "1 minute":
                case "30 seconds":
                case "15 seconds":
                case "10 seconds":
                case "5 seconds":
                    notify = true;
                    break;
            }

            // Console.WriteLine(time);

            if (notify && (DateTime.UtcNow - lastNoticeTime).TotalSeconds > 2)
            {
                foreach (var player in PlayerManager.GetAllOnline())
                    if (sdt.TotalSeconds > 10)
                        player.Session.WorldBroadcast($"Broadcast from System> {(sdt.TotalMinutes > 1.5 ? "ATTENTION" : "WARNING")} - This Asheron's Call Server is shutting down in {time}.{(sdt.TotalMinutes <= 3 ?  " Please log out." : "")}");
                    else
                        player.Session.WorldBroadcast($"Broadcast from System> ATTENTION - This Asheron's Call Server is shutting down NOW!!!!");

                return DateTime.UtcNow;
            }
            else
                return lastNoticeTime;
        }
    }
}
