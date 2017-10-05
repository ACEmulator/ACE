using ACE.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ACE.Web.Controllers
{
    public class GithubController : Controller
    {
        public ActionResult Login()
        {

        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLinkLogin(string provider) //Google,Twitter etc.
        {
            return new ChallengeResult(provider, Url.Action("ExternalLinkLoginCallback"));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> ExternalLinkLoginCallback()
        {
            // Handle external Login Callback
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(ChallengeResult.XsrfKey, userId);
            if (loginInfo == null)
            {
                IdentitySignout(); // to be safe we log out
                return RedirectToAction("Register", new { message = "Unable to authenticate with external login." });
            }

            // Authenticated!
            string providerKey = loginInfo.Login.ProviderKey;
            string providerName = loginInfo.Login.LoginProvider;


            // Your code here…



            // when all good make sure to sign in user
            IdentitySignin(userId, name, providerKey, isPersistent: true);


            return RedirectToAction("Register");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider)
        {
            string returnUrl = Url.Action("New", "Snippet", null);
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback",
                "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = "~/";

            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
                return RedirectToAction("LogOn");

            // AUTHENTICATED!
            var providerKey = loginInfo.Login.ProviderKey;

            // Your code goes here.

            // when all good make sure to sign in user
            IdentitySignin(userId, name, providerKey, isPersistent: true);

            return Redirect(returnUrl);
        }

        // GET: Github
        public ActionResult Auth(string redirectUrl)
        {
            return View();
        }

        public void IdentitySignin(string userId, string name, string providerKey = null, bool isPersistent = false)
        {
            var claims = new List<Claim>();

            // create *required* claims
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
            claims.Add(new Claim(ClaimTypes.Name, name));

            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            // add to user here!
            AuthenticationManager.SignIn(new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = isPersistent,
                ExpiresUtc = DateTime.UtcNow.AddDays(7)
            }, identity);
        }

        public void IdentitySignout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie,
                DefaultAuthenticationTypes.ExternalCookie);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}