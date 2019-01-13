using ACE.Database;
using ACE.Database.Models.Auth;
using ACE.Server.API;
using ACE.Server.Web.Entities;
using ACE.Server.Web.Requests;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.ModelBinding;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ACE.Server.Web.Modules
{
    public class LoginModule : NancyModule
    {
        public LoginModule()
        {
            Get("/login", LogIn);
            Post("/login", LogInPost);
            Get("/logout", LogOut);
        }

        public async Task<object> LogOut(dynamic parameters)
        {
            return this.LogoutAndRedirect("~/");
        }

        private async Task<object> LogIn(dynamic parameters)
        {
            return new LoginModel();
        }

        public async Task<object> LogInPost(dynamic parameters)
        {
            LoginModel request = this.BindAndValidate<LoginModel>();
            if (!ModelValidationResult.IsValid)
            {
                return new LoginModel();
            }

            Account acct = null;
            Gate.RunGatedAction(() =>
            {
                Thread.Sleep(5000); // limit login attempts globally to 0.2 per second
                acct = DatabaseManager.Authentication.GetAccountByName(request.username);
                if (acct == null)
                {
                    return;
                }
                if (!acct.PasswordMatches(request.password))
                {
                    acct = null;
                }
            });
            if (acct == null)
            {
                return new LoginModel();
            }
            else
            {
                Guid token = Guid.NewGuid();
                GUIDToUserIdCache.SetUserId(token, acct.AccountId);
                return this.LoginAndRedirect(token);
            }
        }
    }
}
