using Nancy;
using Nancy.Authentication.Basic;
using Nancy.Bootstrapper;
using Nancy.Session;
using Nancy.TinyIoc;

namespace ACE.Server.Web
{
    public class NancyBootstrapper : DefaultNancyBootstrapper
    {
        //private readonly IAppConfiguration appConfig;
        public NancyBootstrapper() { }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            //container.Register<IAppConfiguration>(appConfig);
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            //CookieBasedSessions.Enable(pipelines, new CookieBasedSessionsConfiguration
            //{
            //    Serializer = new DefaultObjectSerializer(),
            //    CookieName = "_ace"
            //});
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            pipelines.EnableBasicAuthentication(new BasicAuthenticationConfiguration(
                container.Resolve<IUserValidator>(),
                "ACEmulator"));
        }

    }
}
