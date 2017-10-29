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
    /// allows for authenticating directly to the server instead of the login server.  warning, this is
    /// subject to deprecation without warning.
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
            return Request.CreateResponse(HttpStatusCode.Unauthorized, "Incorrect username or password.");
        }
        
        private Account CheckUser(string username, string password)
        {
            var account = AuthDb.GetAccountByName(username);
            if (account?.Name.Length > 0)
            {
                if (!account.PasswordMatches(password))
                    account = null;
            }
            return account;
        }
    }
}
