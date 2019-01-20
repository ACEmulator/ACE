using ACE.WebApiServer.Managers;
using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;

namespace ACE.WebApiServer
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Main(string[] args)
        {
            Start();
        }

        public static void Start()
        {
            Server.Program.Start();

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            // Init our text encoding options. This will allow us to use more than standard ANSI text, which the client also supports.
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            var caller = Assembly.GetCallingAssembly().GetName().Name;
            var txtCalledFrom = (caller == "ACE.WebApiServer") ? "" : $" Caller is {caller}";

            Console.WriteLine();
            log.Info($"Starting ACE.WebApiServer.{txtCalledFrom}");
            Console.Title = @"ACEmulator + WebApi";

            log.Info("Initializing WebManager...");
            WebManager.Initialize();
        }
        private static void OnProcessExit(object sender, EventArgs e)
        {
            WebManager.Shutdown();
        }
    }
}
