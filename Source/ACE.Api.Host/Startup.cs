using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using ACE.Api.Common;

namespace ACE.Api.Host
{
    public class Startup
    {
        private static ServiceConfig config = ServiceConfig.Load();
        public static void Main(string[] args)
        {
            if (config != null)
            {
                string serviceUrl = $"http://{config.ApiBindAddress}:{config.ApiBindPort}/";
                var server = WebApp.Start<ACE.Api.Startup>(url: serviceUrl);
                Console.WriteLine($"ACE API listening at {serviceUrl}");
                Console.ReadLine();
            } else {
                Console.WriteLine("There was an error in your configuration.");
            }
        }
    }
}
