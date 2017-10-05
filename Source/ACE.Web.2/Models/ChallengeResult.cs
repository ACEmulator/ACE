using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACE.Web.Models
{
    public class ChallengeResult : HttpUnauthorizedResult
    {
        // Used for XSRF protection when adding external logins
        public static string XsrfKey { get { return "MRil$PJN5UX0hZecHd*mT*4o!&1$i$"; } }

        public ChallengeResult(string provider, string redirectUri)
            : this(provider, redirectUri, null)
        { }

        public ChallengeResult(string provider, string redirectUri, string userId)
        {
            LoginProvider = provider;
            RedirectUri = redirectUri;
            UserId = userId;
        }

        public string LoginProvider { get; set; }
        public string RedirectUri { get; set; }
        public string UserId { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
            if (UserId != null)
                properties.Dictionary[XsrfKey] = UserId;

            var owin = context.HttpContext.GetOwinContext();
            owin.Authentication.Challenge(properties, LoginProvider);
        }
    }
}