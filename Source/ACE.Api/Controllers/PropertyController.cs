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
using ACE.Entity;
using ACE.Entity.Enum.Properties;
using Swashbuckle.Swagger.Annotations;

namespace ACE.Api.Controllers
{
    /// <summary>
    /// lookups for all the property values.  note, descriptions are subject to change as we prettify them.  the ID values
    /// and their logical means will not.
    /// </summary>
    public class PropertyController : BaseController
    {
        /// <summary>
        /// gets a list of all the available string properties
        /// </summary>
        [HttpGet]
        [AceAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<AceProperty>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        public HttpResponseMessage Strings()
        {
            var t = Enum.GetValues(typeof(PropertyString)).Cast<PropertyString>().ToList();
            var result = t.Select(v => new AceProperty() { PropertyId = (uint)v, PropertyName = v.GetDescription() }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Gets a list of all the available double properties
        /// </summary>
        [HttpGet]
        [AceAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<AceProperty>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        public HttpResponseMessage Doubles()
        {
            var t = Enum.GetValues(typeof(PropertyDouble)).Cast<PropertyDouble>().ToList();
            var result = t.Select(v => new AceProperty() { PropertyId = (uint)v, PropertyName = v.GetDescription() }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Gets a list of all the available data id properties
        /// </summary>
        [HttpGet]
        [AceAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<AceProperty>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        public HttpResponseMessage DataIds()
        {
            var t = Enum.GetValues(typeof(PropertyDataId)).Cast<PropertyDataId>().ToList();
            var result = t.Select(v => new AceProperty() { PropertyId = (uint)v, PropertyName = v.GetDescription() }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Gets a list of all the available isntance id properties
        /// </summary>
        [HttpGet]
        [AceAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<AceProperty>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        public HttpResponseMessage InstanceIds()
        {
            var t = Enum.GetValues(typeof(PropertyInstanceId)).Cast<PropertyInstanceId>().ToList();
            var result = t.Select(v => new AceProperty() { PropertyId = (uint)v, PropertyName = v.GetDescription() }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Gets a list of all the available attribute properties
        /// </summary>
        [HttpGet]
        [AceAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<AceProperty>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        public HttpResponseMessage Attributes()
        {
            var t = Enum.GetValues(typeof(PropertyAttribute)).Cast<PropertyAttribute>().ToList();
            var result = t.Select(v => new AceProperty() { PropertyId = (uint)v, PropertyName = v.GetDescription() }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Gets a list of all the available boolean properties
        /// </summary>
        [HttpGet]
        [AceAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<AceProperty>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        public HttpResponseMessage Bools()
        {
            var t = Enum.GetValues(typeof(PropertyBool)).Cast<PropertyBool>().ToList();
            var result = t.Select(v => new AceProperty() { PropertyId = (uint)v, PropertyName = v.GetDescription() }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Gets a list of all the available integer properties
        /// </summary>
        [HttpGet]
        [AceAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<AceProperty>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        public HttpResponseMessage Ints()
        {
            var t = Enum.GetValues(typeof(PropertyInt)).Cast<PropertyInt>().ToList();
            var result = t.Select(v => new AceProperty() { PropertyId = (uint)v, PropertyName = v.GetDescription() }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Gets a list of all the available 64-bit integer properties
        /// </summary>
        [HttpGet]
        [AceAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<AceProperty>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        public HttpResponseMessage Int64s()
        {
            var t = Enum.GetValues(typeof(PropertyInt64)).Cast<PropertyInt64>().ToList();
            var result = t.Select(v => new AceProperty() { PropertyId = (uint)v, PropertyName = v.GetDescription() }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
