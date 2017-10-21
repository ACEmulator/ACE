using ACE.Api.Common;
using ACE.Entity;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ACE.Api.Controllers
{
    /// <summary>
    /// controller for all actions of Subscriptions
    /// </summary>
    public class SubscriptionController : BaseController
    {
        /// <summary>
        /// gets a list of all subscriptions for the authenticated user.
        /// </summary>/returns>
        [HttpGet]
        [AceAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<Subscription>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        public HttpResponseMessage Get()
        {
            var principal = Request.GetRequestContext().Principal;
            Guid accountGuid = Guid.Parse(principal.Identity.Name);
            var subs = AuthDb.GetSubscriptionsByAccount(accountGuid);
            return Request.CreateResponse(HttpStatusCode.OK, subs);
        }

        /// <summary>
        /// creates a new subscription for the authenticated user with "Player" access level.  elevations 
        /// above player must be done manually by the server operator
        /// </summary>
        [HttpPost]
        [AceAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Subscription))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "missing or invalid authorization header.")]
        public HttpResponseMessage Create(string subscriptionName)
        {
            var principal = Request.GetRequestContext().Principal;
            Guid accountGuid = Guid.Parse(principal.Identity.Name);
            Subscription s = new Subscription();
            s.AccountGuid = accountGuid;
            s.Name = subscriptionName;
            s.AccessLevel = Entity.Enum.AccessLevel.Player;
            AuthDb.CreateSubscription(s);
            return Request.CreateResponse(HttpStatusCode.OK, s);
        }
    }
}
