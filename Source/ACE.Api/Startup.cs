﻿using System;
using Owin;
using System.IdentityModel.Tokens;
using System.Web.Http;

namespace ACE.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            SwaggerConfig.Register(config);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            
            config.MessageHandlers.Add(new AceJwtTokenHandler());
            
            app.UseWebApi(config);
        }
    }
}
