using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Api.Common
{
    public class AcePrincipal : GenericPrincipal
    {
        public AcePrincipal(IIdentity identity, string[] roles) : base(identity, roles)
        {
        }
    }
}
