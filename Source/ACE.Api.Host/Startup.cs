using System;
using Microsoft.Owin.Hosting;
using ACE.Api.Common;
using ACE.Common;

namespace ACE.Api.Host
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            ServiceConfig.LoadServiceConfig();
            if(ConfigManager.Config.ApiServer != null && ConfigManager.Config.ApiServer.Url?.Length > 0)
            {
                // Get the bind address and port from config:
                var server = WebApp.Start<Api.Startup>(url: ConfigManager.Config.ApiServer.Url);
                Console.WriteLine($"ACE API listening at {ConfigManager.Config.ApiServer.Url}");
                Console.ReadLine();
            } else {
                Console.WriteLine("There was an error in your configuration.");
            }
        }
    }
}
