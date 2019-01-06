using ACE.Server.Web.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nancy.Owin;

namespace ACE.Server.Web
{
    internal class KestrelStartup
    {
        private readonly IConfiguration config;
        public KestrelStartup(IHostingEnvironment env)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            config = builder.Build();
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            WebLogging.ConfigureLogger(loggerFactory);
            WebLogging.LoggerFactory = loggerFactory;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            env.ContentRootFileProvider = new Microsoft.Extensions.FileProviders.NullFileProvider();
            env.ContentRootPath = null;
            app.UseOwin(x => x.UseNancy(opt => opt.Bootstrapper = new NancyBootstrapper()));
        }
    }
}
