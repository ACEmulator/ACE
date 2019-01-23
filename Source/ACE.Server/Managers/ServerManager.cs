using System;
using System.Threading;

using log4net;

using ACE.Common;
using ACE.Database;

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
        }

        /// <summary>
        /// Threaded task created when performing a server shutdown
        /// </summary>
        private static void ShutdownServer()
        {
            var shutdownTime = DateTime.UtcNow.AddSeconds(ShutdownInterval);

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

                Thread.Sleep(10);
            }

            PropertyManager.ResyncVariables();
            PropertyManager.StopUpdating();

            log.Debug("Logging off all players...");

            // logout each player
            foreach (var player in PlayerManager.GetAllOnline())
                player.Session.LogOffPlayer();

            log.Info("Waiting for all players to log off...");

            // wait 10 seconds for log-off
            while (PlayerManager.GetAllOnline().Count > 0)
                Thread.Sleep(10);

            log.Debug("Adding all landblocks to destruction queue...");

            // Queue unloading of all the landblocks
            // The actual unloading will happen in WorldManager.UpdateGameWorld
            LandblockManager.AddAllActiveLandblocksToDestructionQueue();

            log.Info("Waiting for all active landblocks to unload...");

            while (LandblockManager.GetActiveLandblocks().Count > 0)
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
    }
}
