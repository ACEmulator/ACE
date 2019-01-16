using ACE.Server.WebApi.Managers;
using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;

namespace ACE.Server.WebApi
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

            var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            var caller = Assembly.GetCallingAssembly().GetName().Name;
            var txtCalledFrom = (caller == "ACE.Server.WebApi") ? "" : $" Caller is {caller}";

            Console.WriteLine();
            log.Info($"Starting WebApi.{txtCalledFrom}");
            Console.Title = @"ACEmulator + WebApi";

            log.Info("Initializing Web...");
            WebManager.Initialize();
        }
        private static void OnProcessExit(object sender, EventArgs e)
        {
            WebManager.Shutdown();
        }
    }
}
