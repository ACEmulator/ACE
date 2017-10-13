using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ACE.Entity;

namespace ACE.Api.Controllers
{
    public class WeenieController : ApiController
    {
        [HttpPost]
        [Authorize]
        public List<AceObject> Search(WeenieSearchRequest request)
        {
            return new List<AceObject>();
        }
    }
}
