using ACE.Server.Web.Model;
using Nancy;

namespace ACE.Server.Web.Modules
{
    public class BaseModule : NancyModule
    {
        public BaseModel BaseModel
        {
            get
            {
                BaseModel model = new BaseModel
                {
                    AccessLevelName = Context.CurrentUser.FindFirst("AccessLevelName").Value,
                    AccountName = Context.CurrentUser.Identity.Name
                };
                return model;
            }
        }
    }
}
