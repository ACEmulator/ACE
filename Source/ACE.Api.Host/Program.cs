using System;
using Microsoft.Owin.Hosting;
using ACE.Api.Common;
using ACE.Common;

namespace ACE.Api.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServiceConfig.LoadServiceConfig();
            if (!string.IsNullOrWhiteSpace(ConfigManager.Config.ApiServer?.ListenUrl))
            {
                // Get the bind address and port from config:
                var server = WebApp.Start<Startup>(url: ConfigManager.Config.ApiServer.ListenUrl);
                Console.WriteLine($"ACE API listening at {ConfigManager.Config.ApiServer.ListenUrl}");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("There was an error in your API configuration.");
                Console.ReadLine(); // need a readline or the error flashes without being seen
            }
        }
    }
}
