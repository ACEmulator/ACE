using ACE.Api;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Api.Common;

namespace ACE.AuthApi.Host
{
    class Program
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
            }
            else
            {
                Console.WriteLine("There was an error in your configuration.");
            }
        }
    }
}
