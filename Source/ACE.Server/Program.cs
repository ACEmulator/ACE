using System;
using System.IO;

using log4net;
using log4net.Config;

using ACE.Common;
using ACE.Database;
using ACE.DatLoader;
using ACE.Server.Command;
using ACE.Server.Managers;
using ACE.Server.Network.Managers;

namespace ACE.Server
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            // Init our text encoding options. This will allow us to use more than standard ANSI text, which the client also supports.
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            log.Info("Starting ACEmulator...");
            Console.Title = @"ACEmulator";

            log.Info("Initializing ConfigManager...");
            ConfigManager.Initialize();

            log.Info("Initializing ServerManager...");
            ServerManager.Initialize();

            log.Info("Initializing DatManager...");
            DatManager.Initialize(ConfigManager.Config.Server.DatFilesDirectory);

            log.Info("Initializing DatabaseManager...");
            DatabaseManager.Initialize();

            log.Info("Starting DatabaseManager...");
            DatabaseManager.Start();

            log.Info("Initializing GuidManager...");
            GuidManager.Initialize();

            log.Info("Initializing InboundMessageManager...");
            InboundMessageManager.Initialize();

            log.Info("Initializing SocketManager...");
            SocketManager.Initialize();

            log.Info("Initializing WorldManager...");
            WorldManager.Initialize();

            log.Info("Initializing EventManager...");
            EventManager.Initialize();

            log.Info("Starting PropertyManager...");
            PropertyManager.Initialize();

            // This should be last
            log.Info("Initializing CommandManager...");
            CommandManager.Initialize();
        }

        private static void OnProcessExit(object sender, EventArgs e)
        {
            PropertyManager.StopUpdating();
            DatabaseManager.Stop();
        }
    }
}
