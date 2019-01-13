using ACE.Entity.Enum;
using Nancy.Security;

namespace ACE.Server.Web.Modules
{
    public class IndexModule : BaseModule
    {
        public IndexModule()
        {
            this.RequiresAuthentication();

            this.RequiresAnyClaim(
                k => k.Type == AccessLevel.Admin.ToString(),
                k => k.Type == AccessLevel.Advocate.ToString(),
                k => k.Type == AccessLevel.Developer.ToString(),
                k => k.Type == AccessLevel.Envoy.ToString(),
                k => k.Type == AccessLevel.Player.ToString(),
                k => k.Type == AccessLevel.Sentinel.ToString());

            Get("/", parameters =>
            {
                return View["index", BaseModel];
            });
        }
    }
}
