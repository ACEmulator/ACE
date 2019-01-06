using Nancy;
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
    }
}
