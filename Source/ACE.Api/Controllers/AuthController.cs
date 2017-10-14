using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ACE.Database;
using ACE.Entity;

namespace ACE.Api.Controllers
{
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        [HttpPost]
        public string GetToken([FromBody] AuthRequest request)
        {
            var account = CheckUser(request.Username, request.Password);
            if (account != null)
            {
                return JwtManager.GenerateToken(account);
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
