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
using ACE.Api.Common;
using ACE.Api.Models;

namespace ACE.Api.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class WeenieController : BaseController
    {
        /// <summary>
        /// Searches for Weenies according to the given criteria.  All criteria are applied as "AND", logically.
        /// </summary>
        [HttpPost]
        [AceAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<WeenieSearchResult>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        public HttpResponseMessage Search([FromBody] SearchWeeniesCriteria request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, WorldDb.SearchWeenies(request));
        }

        /// <summary>
        /// fetches a single weenie object.  this method is not implemented yet.  do not code against it.
        /// </summary>
        [HttpGet]
        [AceAuthorize(AccessLevel.Developer)]
        [SwaggerResponse(HttpStatusCode.OK, "success", typeof(AceObject))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        public HttpResponseMessage Get(uint weenieId)
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }

        /// <summary>
        /// updates a weenie.  this method is not implemented yet.  do not code against it.
        /// </summary>
        [HttpPost]
        [AceAuthorize(AccessLevel.Developer)]
        [SwaggerResponse(HttpStatusCode.OK, "weenie updated", typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        public HttpResponseMessage Update([FromBody] AceObject request)
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }

        /// <summary>
        /// creates a new weenie.  this method is not implemented yet.  do not code against it.
        /// </summary>
        [HttpPost]
        [AceAuthorize(AccessLevel.Developer)]
        [SwaggerResponse(HttpStatusCode.OK, "weenie created", typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        public HttpResponseMessage Create([FromBody] AceObject request)
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }

        /// <summary>
        /// deletes a weenie.  this method is not implemented yet.  do not code against it.
        /// </summary>
        [HttpDelete]
        [AceAuthorize(AccessLevel.Developer)]
        [SwaggerResponse(HttpStatusCode.OK, "weenie deleted", typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        public HttpResponseMessage Delete([FromBody] AceObject request)
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }
    }
}
