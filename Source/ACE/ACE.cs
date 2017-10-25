using System;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.IO;

using ACE.Command;
using ACE.Common;
using ACE.Database;
using ACE.Managers;
using ACE.Network.Managers;
using ACE.DatLoader;
using log4net;

namespace ACE
{
    [System.ComponentModel.DesignerCategory("")]
    public class ACE : ServiceBase
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Handle for server startup thread, when executed as a service
        protected Thread mainProcessThread;

        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

            // Check if program was run as a normal application or as a service
            // Running as a service will mean that Environment.UserInteractive returns false
            if (Environment.UserInteractive)
            {
                string parameter = string.Concat(args);
                switch (parameter)
                {
                    case "--install":
                        ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                        break;
                    case "--uninstall":
                        ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                        break;
                    default:
                        log.Info("Starting ACEmulator...");
                        Console.Title = @"ACEmulator";

                        ConfigManager.Initialize();
                        ServerManager.Initialize();
                        DatabaseManager.Initialize();
                        AssetManager.Initialize();
                        InboundMessageManager.Initialize();
                        DatabaseManager.Start();
                        DatManager.Initialize();
                        GuidManager.Initialize();
                        SocketManager.Initialize();
                        WorldManager.Initialize();
                        RecipeManager.Initialize();
                        CommandManager.Initialize();
                        break;
                }
            }
            else
            {
                // Spawns the service constructor
                Run(new ACE());
            }
        }

        private static void OnProcessExit(object sender, EventArgs e)
        {
            DatabaseManager.Stop();
            Diagnostics.Diagnostics.LandBlockDiag = false;
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            log.Error(((Exception)e.ExceptionObject).Message + ((Exception)e.ExceptionObject).InnerException.Message);
        }

        public ACE()
        {
            ServiceName = "ACEmulator Service";
            log.Info("ACEmulator service initializing");

            // Set the current directory of the service to the current directory
            // of the executable to enable relative pathing to work
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        }

        protected override void OnStart(string[] args)
        {
            // Initiate the startup of the game server
            mainProcessThread = new Thread(MainProcess);
            mainProcessThread.Start();

            // Pass control back to the base service class
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            // Perform a server shutdown, when the service is instructed to stop
            Command.Handlers.ShardCommands.ShutdownServerNow(null);
            DatabaseManager.Stop();
            Diagnostics.Diagnostics.LandBlockDiag = false;

            log.Info("ACEmulator stopped");

            // Pass control back to the base service class
            base.OnStop();
        }

        protected void MainProcess()
        {
            ConfigManager.Initialize();
            ServerManager.Initialize();
            DatabaseManager.Initialize();
            AssetManager.Initialize();
            InboundMessageManager.Initialize();
            DatabaseManager.Start();
            DatManager.Initialize();
            GuidManager.Initialize();
            SocketManager.Initialize();
            WorldManager.Initialize();
            RecipeManager.Initialize();
            CommandManager.Initialize();

            log.Info("ACEmulator started");
        }
    }
}
