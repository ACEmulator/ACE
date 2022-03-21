using System;
using System.Threading;

using log4net;

using ACE.Common;
using ACE.Database;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Managers;

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
        /// Indicates server shutting down.
        /// </summary>
        public static bool ShutdownInProgress { get; private set; }

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
                    string shutdownText = $"The server shut down has been cancelled @ {DateTime.Now} ({DateTime.UtcNow} UTC)";
                    log.Info(shutdownText);

                    // special text
                    foreach (var player in PlayerManager.GetAllOnline())
                        player.Session.WorldBroadcast($"Broadcast from System> ATTENTION - This Asheron's Call Server shut down has been cancelled.");

                    // break function
                    return;
                }

                lastNoticeTime = NotifyPlayersOfPendingShutdown(lastNoticeTime, shutdownTime.AddSeconds(1));

                Thread.Sleep(10);
            }

            ShutdownInProgress = true;

            PropertyManager.ResyncVariables();
            PropertyManager.StopUpdating();

            WorldManager.EnqueueAction(new ActionEventDelegate(() =>
            {
                log.Debug("Logging off all players...");

                // logout each player
                foreach (var player in PlayerManager.GetAllOnline())
                    player.Session.LogOffPlayer(true);
            }));

            // Wait for all players to log out
            var logUpdateTS = DateTime.MinValue;
            int playerCount;
            var playerLogoffStart = DateTime.UtcNow;
            while ((playerCount = PlayerManager.GetOnlineCount()) > 0)
            {
                logUpdateTS = LogStatusUpdate(logUpdateTS, $"Waiting for {playerCount} player{(playerCount > 1 ? "s" : "")} to log off...");
                Thread.Sleep(10);
                if (playerCount > 0 && DateTime.UtcNow - playerLogoffStart > TimeSpan.FromMinutes(5))
                {
                    playerLogoffStart = DateTime.UtcNow;
                    log.Warn($"5 minute log off failsafe reached and there are {playerCount} player{(playerCount > 1 ? "s" : "")} still online.");
                    foreach (var player in PlayerManager.GetAllOnline())
                    {
                        log.Warn($"Player {player.Name} (0x{player.Guid}) appears to be stuck in world and unable to log off normally. Requesting Forced Logoff...");
                        player.ForcedLogOffRequested = true;
                        player.ForceLogoff();
                    }    
                }
            }

            WorldManager.EnqueueAction(new ActionEventDelegate(() =>
            {
                log.Debug("Disconnecting all sessions...");

                // disconnect each session
                NetworkManager.DisconnectAllSessionsForShutdown();
            }));

            // Wait for all sessions to drop out
            logUpdateTS = DateTime.MinValue;
            int sessionCount;
            while ((sessionCount = NetworkManager.GetAuthenticatedSessionCount()) > 0)
            {
                logUpdateTS = LogStatusUpdate(logUpdateTS, $"Waiting for {sessionCount} authenticated session{(sessionCount > 1 ? "s" : "")} to disconnect...");
                Thread.Sleep(10);
            }

            log.Debug("Adding all landblocks to destruction queue...");

            // Queue unloading of all the landblocks
            // The actual unloading will happen in WorldManager.UpdateGameWorld
            LandblockManager.AddAllActiveLandblocksToDestructionQueue();

            // Wait for all landblocks to unload
            logUpdateTS = DateTime.MinValue;
            int landblockCount;
            while ((landblockCount = LandblockManager.GetLoadedLandblocks().Count) > 0)
            {
                logUpdateTS = LogStatusUpdate(logUpdateTS, $"Waiting for {landblockCount} loaded landblock{(landblockCount > 1 ? "s" : "")} to unload...");
                Thread.Sleep(10);
            }

            log.Debug("Stopping world...");

            // Disabled thread update loop
            WorldManager.StopWorld();

            // Wait for world to end
            logUpdateTS = DateTime.MinValue;
            while (WorldManager.WorldActive)
            {
                logUpdateTS = LogStatusUpdate(logUpdateTS, "Waiting for world to stop...");
                Thread.Sleep(10);
            }

            log.Info("Saving OfflinePlayers that have unsaved changes...");
            PlayerManager.SaveOfflinePlayersWithChanges();

            // Wait for the database queue to empty
            logUpdateTS = DateTime.MinValue;
            int shardQueueCount;
            while ((shardQueueCount = DatabaseManager.Shard.QueueCount) > 0)
            {
                logUpdateTS = LogStatusUpdate(logUpdateTS, $"Waiting for database queue ({shardQueueCount}) to empty...");
                Thread.Sleep(10);
            }

            // Write exit to console/log
            log.Info($"Exiting at {DateTime.UtcNow}");

            // System exit
            Environment.Exit(Environment.ExitCode);
        }

        private static DateTime LogStatusUpdate(DateTime logUpdateTS, string logMessage)
        {
            if (logUpdateTS == DateTime.MinValue || DateTime.UtcNow > logUpdateTS.ToUniversalTime())
            {
                log.Info(logMessage);
                logUpdateTS = DateTime.UtcNow.AddSeconds(10);
            }

            return logUpdateTS;
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

        public static void StartupAbort()
        {
            ShutdownInitiated = true;
        }

        public static string ShutdownNoticeText()
        {
            var sdt = ShutdownTime - DateTime.UtcNow;

            var timeToShutdown = $"{(sdt.Hours > 0 ? $"{sdt.Hours} hour{(sdt.Hours > 1 ? "s" : "")}" : "")}";
            timeToShutdown += $"{(timeToShutdown.Length > 0 ? ", " : "")}{(sdt.Minutes > 0 ? $"{sdt.Minutes} minute{(sdt.Minutes > 1 ? "s" : "")}" : "")}";
            timeToShutdown += $"{(timeToShutdown.Length > 0 ? " and " : "")}{(sdt.Seconds > 0 ? $"{sdt.Seconds} second{(sdt.Seconds > 1 ? "s" : "")}" : "")}";

            if (sdt.TotalSeconds > 10)
               return $"Broadcast from System> {(sdt.TotalMinutes > 1.5 ? "ATTENTION" : "WARNING")} - This Asheron's Call Server is shutting down in {timeToShutdown}.{(sdt.TotalMinutes <= 3 ? " Please log out." : "")}";
            else
               return $"Broadcast from System> ATTENTION - This Asheron's Call Server is shutting down NOW!!!!";
        }
    }
}
