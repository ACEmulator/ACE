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
        [HttpGet]
        public string GetToken(string username, string password)
        {
            var account = CheckUser(username, password);
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
