using System;
using System.Runtime.InteropServices;

using ACE.Command;
using ACE.Common;
using ACE.Database;
using ACE.Managers;
using ACE.Network.Managers;
using ACE.DatLoader;
using log4net;

namespace ACE
{
    public class ACE
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

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
            CommandManager.Initialize();

            DatabaseManager.Start();
        }

        private static void OnProcessExit(object sender, EventArgs e)
        {
            DatabaseManager.Stop();
            Diagnostics.Diagnostics.LandBlockDiag = false;
        }
    }
}
