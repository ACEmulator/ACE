using Microsoft.Owin.Hosting;
using System;
using ACE.Common;
using ACE.Api.Common;

namespace ACE.AuthApi.Host
{
    class Program
    {
        public static void Main(string[] args)
        {
            ServiceConfig.LoadServiceConfig();
            if (ConfigManager.Config.AuthServer != null && ConfigManager.Config.AuthServer.ListenUrl?.Length > 0)
            {
                // Get the bind address and port from config
                var server = WebApp.Start<ACE.AuthApi.Startup>(url: ConfigManager.Config.AuthServer.ListenUrl);
                Console.WriteLine($"ACE Auth API listening at {ConfigManager.Config.AuthServer.ListenUrl}");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("There was an error in your AuthApi configuration.");
                Console.ReadLine(); // need a readline or the error flashes without being seen
            }
        }
    }
}
