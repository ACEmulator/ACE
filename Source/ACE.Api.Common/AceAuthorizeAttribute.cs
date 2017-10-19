using ACE.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ACE.Api.Common
{
    /// <summary>
    /// TODO: Convert to SecurityLevel instead of AccessLevel
    /// </summary>
    public class AceAuthorizeAttribute : AuthorizeAttribute
    {
        public AceAuthorizeAttribute() : base()
        {
        }

        public AceAuthorizeAttribute(AccessLevel access) : base()
        {
            base.Roles = access.ToString();
        }
    }
}
