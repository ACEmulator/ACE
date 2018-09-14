using System;
using System.Threading;

using ACE.Common;

using log4net;

namespace ACE.Server.Managers
{
    /// <summary>
    /// Servermanager handles unloading the server application properly.
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

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            DateTime shutdownTime = DateTime.UtcNow.AddSeconds(ShutdownInterval);

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
                    foreach (var player in WorldManager.GetAll())
                        player.WorldBroadcast(shutdownText);

                    // break function
                    return;
                }

                Thread.Sleep(1);
            }

            // logout each player
            foreach (var player in WorldManager.GetAll())
                player.LogOffPlayer();

            // wait 6 seconds for log-off
            Thread.Sleep(6000);

            // TODO: Make sure that the landblocks unloads properly.

            // TODO: Make sure that the databasemanager unloads properly.

            // disabled thread update loop and halt application
            WorldManager.StopWorld();

            // wait for world to end
            while (WorldManager.WorldActive)
                ; // do nothing

            // write exit to console/log
            log.Info($"Exiting at {DateTime.UtcNow}");

            // system exit
            Environment.Exit(Environment.ExitCode);
        }
    }
}
