using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ACE.Web.Startup))]
namespace ACE.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
