using ACE.Api;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Common;
using ACE.Api.Common;

namespace ACE.AuthApi.Host
{
    class Program
    {
        public static void Main(string[] args)
        {
            ServiceConfig.LoadServiceConfig();
            if (ConfigManager.Config.AuthServer != null && ConfigManager.Config.AuthServer.Url?.Length > 0)
            {
                // Get the bind address and port from config:
                var server = WebApp.Start<ACE.Api.Startup>(url: ConfigManager.Config.AuthServer.Url);
                Console.WriteLine($"ACE API listening at {ConfigManager.Config.AuthServer.Url}");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("There was an error in your configuration.");
            }
        }
    }
}
