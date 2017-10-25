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
            if (!string.IsNullOrWhiteSpace(ConfigManager.Config.AuthServer?.ListenUrl))
            {
                // Get the bind address and port from config
                var server = WebApp.Start<Startup>(url: ConfigManager.Config.AuthServer.ListenUrl);
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
