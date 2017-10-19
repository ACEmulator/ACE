using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ACE.Database;
using ACE.Entity;
using System.Net.Http;
using ACE.Api.Common;

namespace ACE.Api.Controllers
{
    public class ContentController : ApiController
    {
        [HttpGet]
        [AceAuthorize]
        public HttpResponseMessage GetAllContent()
        {
            return null;
        }
    }
}
