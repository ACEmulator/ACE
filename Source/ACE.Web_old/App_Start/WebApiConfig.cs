using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using ACE.Common;
using ACE.Database;
using Newtonsoft.Json;

namespace ACE.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
