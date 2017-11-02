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
        [SwaggerResponse(HttpStatusCode.NotFound, "requested weenie id not found")]
        public HttpResponseMessage Get(uint weenieId)
        {
            var weenie = WorldDb.GetObject(weenieId);

            if (weenie != null)
                return Request.CreateResponse(HttpStatusCode.OK, weenie);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// updates a weenie.  this method is not implemented yet.  do not code against it.
        /// </summary>
        [HttpPost]
        [AceAuthorize(AccessLevel.Developer)]
        [SwaggerResponse(HttpStatusCode.OK, "weenie updated", typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "unable to save changes.")]
        public HttpResponseMessage Update([FromBody] AceObject request)
        {
            // force this to true, as we're explicitly doing an update
            request.HasEverBeenSavedToDatabase = true;

            if (WorldDb.ReplaceObject(request))
                return Request.CreateResponse(HttpStatusCode.OK, new { message = "changes saved." });
            else
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "Unable to save changes." });
        }

        /// <summary>
        /// creates a new weenie.  this method is not implemented yet.  do not code against it.
        /// </summary>
        [HttpPost]
        [AceAuthorize(AccessLevel.Developer)]
        [SwaggerResponse(HttpStatusCode.OK, "weenie created", typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "unable to save changes.")]
        public HttpResponseMessage Create([FromBody] AceObject request)
        {
            if (WorldDb.SaveObject(request))
                return Request.CreateResponse(HttpStatusCode.OK, new { message = "changes saved." });
            else
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "Unable to save changes." });
        }

        /// <summary>
        /// deletes a weenie.  this method is not implemented yet.  do not code against it.
        /// </summary>
        [HttpDelete]
        [AceAuthorize(AccessLevel.Developer)]
        [SwaggerResponse(HttpStatusCode.OK, "weenie deleted", typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "unable to save changes.")]
        public HttpResponseMessage Delete([FromBody] AceObject request)
        {
            if (WorldDb.DeleteObject(request))
                return Request.CreateResponse(HttpStatusCode.OK, new { message = "object deleted" });
            else
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "Unable to save changes." });
        }
    }
}
