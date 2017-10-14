using System;
using Owin;
using System.IdentityModel.Tokens;
using System.Web.Http;
using JwtAuthForWebAPI;
using Swashbuckle.Application;

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
            
            var keyBuilder = new SecurityTokenBuilder();
            var jwtHandler = new JwtAuthenticationMessageHandler
            {
                Issuer = "ACE",
                AllowedAudience = "ACE.API",
                SigningToken = keyBuilder.CreateFromKey(JwtManager.Secret)
            };
            config.MessageHandlers.Add(jwtHandler);

            // 

            //config.EnableSwagger(c =>
            //  {
            //      c.SingleApiVersion("v1", "MyAPI");
            //  })
            //    .EnableSwaggerUi();

            app.UseWebApi(config);
        }
    }
}
