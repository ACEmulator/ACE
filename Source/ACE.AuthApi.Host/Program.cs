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
            // TODO: move port to be configuration driven
            var server = WebApp.Start<Startup>(url: "http://*:8001/");
            Console.WriteLine("ACE Auth API listening at http://*:8001/");

            Console.ReadLine();
        }
    }
}
