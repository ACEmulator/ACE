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
using Swashbuckle.Swagger.Annotations;
using System.Net;

namespace ACE.Api.Controllers
{
    public class ContentController : BaseController
    {
        [HttpGet]
        [AceAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<Content>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        public HttpResponseMessage GetAllContent()
        {
            return Request.CreateResponse(HttpStatusCode.OK, WorldDb.GetAllContent());
        }
    }
}
