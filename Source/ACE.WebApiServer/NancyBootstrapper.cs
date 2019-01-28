using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace ACE.WebApiServer
{
    public class NancyBootstrapper : DefaultNancyBootstrapper
    {
        public NancyBootstrapper() { }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);
            StatelessAuthentication.Enable(pipelines, BasicAuth.GetAuthCfg());
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            //Conventions.ViewLocationConventions.Add((viewName, model, viewLocationContext) => string.Concat("Web/Views/", viewName));
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
        }
    }
}
