using System.Web;
using System.Web.Mvc;

namespace ACE.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new JwtCookieManager());
        }
    }
}
