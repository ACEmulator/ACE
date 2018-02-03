using System;
using System.IO;

using ACE.Command;
using ACE.Common;
using ACE.Database;
using ACE.Managers;
using ACE.Network.Managers;
using ACE.DatLoader;

using log4net;
using log4net.Config;

namespace ACE
{
    public class ACE
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            log.Info("Starting ACEmulator...");
            Console.Title = @"ACEmulator";

            ConfigManager.Initialize();
            ServerManager.Initialize();
            DatabaseManager.Initialize();
            AssetManager.Initialize();
            InboundMessageManager.Initialize();
            DatabaseManager.Start();
            DatManager.Initialize(ConfigManager.Config.Server.DatFilesDirectory);
            GuidManager.Initialize();
            SocketManager.Initialize();
            WorldManager.Initialize();
            CommandManager.Initialize();
            RecipeManager.Initialize();

            DatabaseManager.Start();
        }

        private static void OnProcessExit(object sender, EventArgs e)
        {
            DatabaseManager.Stop();
            // TODO: Diagnostics uses WinForms, which is not supported in .net standard/core.
            // TODO: We need a better way to expose diagnostic information moving forward.
            // Diagnostics.Diagnostics.LandBlockDiag = false;
        }
    }
}
