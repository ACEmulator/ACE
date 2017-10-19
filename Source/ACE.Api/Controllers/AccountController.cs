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
                return Request.CreateResponse(HttpStatusCode.OK, new AuthResponse() { AuthToken = JwtManager.GenerateToken(account) });
            }

            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        private Account CheckUser(string username, string password)
        {
            var account = AuthDb.GetAccountByName(username);
            if (!account.PasswordMatches(password))
                account = null;

            return account;
        }
    }
}
