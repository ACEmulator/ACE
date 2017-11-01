using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace ACE.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var error = Server.GetLastError();
            var cryptoEx = error as CryptographicException;

            // so, explanation is due here.  these Crypto exceptions fire when something about your cookie
            // goes bad.  changing ADFS password, machine key changes, cookie expires, etc.  we will
            // attempt to force a signout and if one of those are sucessful, we can clear the error.

            if (cryptoEx != null)
            {
                bool clear = false;

                try
                {
                    FederatedAuthentication.SessionAuthenticationModule.SignOut();
                    clear = true;
                }
                catch { }

                try
                {
                    FormsAuthentication.SignOut();
                    clear = true;
                }
                catch { }

                // clear and move on.
                if (clear)
                    Server.ClearError();
            }
        }
    }
}
