using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ACE.Entity;
using System.Net.Http;
using ACE.Entity.Enum;
using Swashbuckle.Swagger.Annotations;
using System.Net;

namespace ACE.Api.Controllers
{
    public class WeenieController : BaseController
    {
        /// <summary>
        /// Searches for Weenies according to the given criteria.  All criteria are applied as "AND", logically.
        /// </summary>
        [HttpPost]
        [AceAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<WeenieSearchResult>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        public HttpResponseMessage Search([FromBody] SearchWeeniesCriteria request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, WorldDb.SearchWeenies(request));
        }

        [HttpPost]
        [AceAuthorize(AccessLevel.Developer)]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        public HttpResponseMessage UpdateWeenie([FromBody] object request)
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }
    }
}
