using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ACE.Entity;
using Microsoft.AspNet.Identity;

namespace ACE.Web.Models
{
    public class ApplicationUser : IUser<string>
    {
        private readonly Account _account;

        public ApplicationUser()
        {
            _account = new Account();
        }

        public ApplicationUser(Account account)
        {
            _account = account;
        }

        public Account GetAccount()
        {
            return _account;
        }

        public string Id
        {
            get { return _account.AccountId.ToString(); }
        }

        public string UserName
        {
            get { return _account.Name; }
            set { _account.Name = value; }
        }

        public string PasswordHash
        {
            get { return _account.PasswordHash; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}