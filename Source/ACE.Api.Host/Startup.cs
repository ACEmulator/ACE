using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace ACE.Api.Host
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            // Start OWIN host 
            var server = WebApp.Start<ACE.Api.Startup>(url: "http://localhost:8000/");
            Console.WriteLine("ACE API listening at http://localhost:8000/");
            
            Console.ReadLine();
        }
    }
}
