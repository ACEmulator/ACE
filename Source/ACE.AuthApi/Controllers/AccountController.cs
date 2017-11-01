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

namespace ACE.AuthApi.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class AccountController : BaseController
    {
        /// <summary>
        /// Used to get an authorization token for use with the API and/or launching of the client when connecting to an ACE server
        /// </summary>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, "Auth successful", typeof(AuthResponse))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "Incorrect username or password")]
        public HttpResponseMessage Authenticate([FromBody] AuthRequest request)
        {
            var account = CheckUser(request.Username, request.Password);
            if (account != null)
            {
                var subscriptions = AuthDb.GetSubscriptionsByAccount(account.AccountGuid);
                return Request.CreateResponse(HttpStatusCode.OK, new AuthResponse() { AuthToken = JwtManager.GenerateToken(account, (subscriptions.Count > 0) ? subscriptions[0].AccessLevel : Entity.Enum.AccessLevel.Player, JwtManager.HmacSigning) });
            }

            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        private Account CheckUser(string username, string password)
        {
            var account = AuthDb.GetAccountByName(username);

            if (account == null)
                return null;

            if (!account.PasswordMatches(password))
                account = null;

            return account;
        }

        /// <summary>
        ///
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, "Auth successful", typeof(RegisterResponse))]
        public HttpResponseMessage Register(RegisterRequest request)
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }

        /// <summary>
        ///
        /// </summary>
        [HttpGet]
        [AceAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Account))]
        public HttpResponseMessage Details(string accountGuid)
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }

        /// <summary>
        /// updates an account.  this must either be the authenticated account or the authenticated
        /// account must be an admin.  accountGuid and accountId are immutable
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Update(string accountGuid, [FromBody] Account account)
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }

        /// <summary>
        /// Validates the bearer token and returns the associated account.  this method is used
        /// by servers in distributed auth mode.
        /// </summary>
        [HttpGet]
        [AceAuthorize]
        [SwaggerResponse(HttpStatusCode.OK, "ticket is valid")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "ticket is invalid")]
        public HttpResponseMessage Validate()
        {
            // "AceAuthorize bit handles the 401s.  if we made it here it must be ok.
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
