using ACE.Api;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.AuthApi.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            // Start OWIN host 
            var server = WebApp.Start<Startup>(url: "http://localhost:8001/");
            Console.WriteLine("ACE Auth API listening at http://localhost:8001/");

            Console.ReadLine();
        }
    }
}
