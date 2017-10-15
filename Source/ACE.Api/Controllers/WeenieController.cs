using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ACE.Entity;

namespace ACE.Api.Controllers
{
    public class WeenieController : BaseController
    {
        [HttpPost]
        [Authorize]
        public List<WeenieSearchResult> Search([FromBody] SearchWeeniesCriteria request)
        {
            return WorldDb.SearchWeenies(request);
        }
    }
}
