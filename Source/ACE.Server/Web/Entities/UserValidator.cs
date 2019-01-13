using ACE.Database;
using ACE.Database.Models.Auth;
using ACE.Server.API;
using Nancy.Authentication.Basic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;

namespace ACE.Server.Web.Entities
{
    public class UserValidator : IUserValidator
    {
        public ClaimsPrincipal Validate(string username, string password)
        {
            Account acct = null;
            Gate.RunGatedAction(() =>
            {
                Thread.Sleep(5000); // limit login attempts globally to 0.2 per second
                acct = DatabaseManager.Authentication.GetAccountByName(username);
                if (!acct.PasswordMatches(password))
                {
                    acct = null;
                }
            });
            if (acct == null)
            {
                return null;
            }
            else
            {
                return new ClaimsPrincipal(new GenericIdentity(acct.AccountName));
            }
        }
    }
}
