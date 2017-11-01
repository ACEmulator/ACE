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
    /// <summary>
    ///
    /// </summary>
    public class Startup
    {
        /// <summary>
        ///
        /// </summary>
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
        }
    }
}
