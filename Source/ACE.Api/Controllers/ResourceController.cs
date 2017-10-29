using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ACE.Api.Common;
using ACE.Api.Models;
using ACE.Entity.Enum.Properties;
using Swashbuckle.Swagger.Annotations;

namespace ACE.Api.Controllers
{
    /// <summary>
    /// Methods for accessing game resources such as Icons, Models, etc.
    /// </summary>
    public class ResourceController : BaseController
    {
        /// <summary>
        /// gets an icon
        /// </summary>
        [HttpGet]
        [AceAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Image))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        public HttpResponseMessage Icon(uint iconId)
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }

        /// <summary>
        /// gets the 3d model
        /// </summary>
        [HttpGet]
        [AceAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(AceModel))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        public HttpResponseMessage Model(uint modelId)
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }

    }
}
