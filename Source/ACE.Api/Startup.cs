using System;
using Owin;
using System.IdentityModel.Tokens;
using System.Web.Http;
using ACE.Api.Common;
using Newtonsoft.Json;
using ACE.Common;
using System.IO;

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
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            
            config.MessageHandlers.Add(new AceJwtTokenHandler());
            
            app.UseWebApi(config);

            string debugPath = @"..\..\..\..\ACE\Config.json"; // default path for debug
            string path = "Config.json"; // default path for user installations

            if (!File.Exists(path) && File.Exists(debugPath))
                path = debugPath;

            var serverConfig = JsonConvert.DeserializeObject<MasterConfiguration>(File.ReadAllText(path));
            ConfigManager.Initialize(serverConfig);
        }
    }
}
