using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ACE.Database;
using ACE.Entity;

namespace ACE.Api.Controllers
{
    public class ContentController : ApiController
    {
        [HttpGet]
        [Authorize]
        public List<Content> GetAllContent()
        {
            return null;
        }
    }
}
