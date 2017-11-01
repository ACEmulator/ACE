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
using ACE.Api.Models;
using ACE.Entity.Enum;

namespace ACE.Api.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class ContentController : BaseController
    {
        /// <summary>
        ///
        /// </summary>
        [HttpGet]
        [AceAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<Content>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        public HttpResponseMessage GetAll()
        {
            return Request.CreateResponse(HttpStatusCode.OK, WorldDb.GetAllContent());
        }

        /// <summary>
        ///
        /// </summary>
        [HttpPost]
        [AceAuthorize(AccessLevel.Developer)]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Request invalid.", typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.OK, "Update successful", typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "An error occurred.  Details will be in the message.", typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        public HttpResponseMessage Create([FromBody]Content content)
        {
            if (string.IsNullOrWhiteSpace(content?.ContentName))
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "ContentName is required" });

            try
            {
                WorldDb.CreateContent(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { message = "creation successful." });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = ex.ToString() });
            }

        }

        /// <summary>
        ///
        /// </summary>
        [HttpPost]
        [AceAuthorize(AccessLevel.Developer)]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Request invalid.", typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.OK, "Update successful", typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "An error occurred.  Details will be in the message.", typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        public HttpResponseMessage Update([FromBody]Content content)
        {
            if (content?.ContentGuid == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "cannot update null content or content with a null ContentGuid." });

            if (string.IsNullOrWhiteSpace(content?.ContentName))
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "ContentName is required" });

            try
            {
                WorldDb.UpdateContent(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { message = "Update successful." });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = ex.ToString() });
            }

        }

        /// <summary>
        ///
        /// </summary>
        [HttpDelete]
        [AceAuthorize(AccessLevel.Developer)]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Request invalid.", typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.OK, "Delete successful", typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "An error occurred.  Details will be in the message.", typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        public HttpResponseMessage Delete(Guid? contentGuid)
        {
            if (contentGuid == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "cannot delete with a null ContentGuid." });

            try
            {
                WorldDb.DeleteContent(contentGuid.Value);
                return Request.CreateResponse(HttpStatusCode.OK, new { message = "Delete successful." });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = ex.ToString() });
            }

        }
    }
}
