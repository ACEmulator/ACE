using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc.Filters;
using Newtonsoft.Json.Linq;

namespace ACE.Web
{
    public class JwtCookieManager : IAuthenticationFilter
    {
        public void OnAuthentication(System.Web.Mvc.Filters.AuthenticationContext filterContext)
        {
            SessionSecurityToken sst = null;
            try
            {
                if (FederatedAuthentication.SessionAuthenticationModule.TryReadSessionTokenFromCookie(out sst))
                {
                    filterContext.Principal = sst.ClaimsPrincipal;
                }
            }
            catch
            {

            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            // no-op
        }
        
        public static void SetCookie(string token)
        {
            var parts = token.Split('.');
            var bodyBase64 = parts[1]; // we only want to get the username out.  everything else just goes as is.

            // parse the header and body into objects
            var bodyJson = Encoding.UTF8.GetString(Base64UrlDecode(bodyBase64));
            var bodyData = JObject.Parse(bodyJson);

            string displayName = (string)bodyData["display_name"];
            string username = (string)bodyData["unique_name"];

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, displayName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, username));
            claims.Add(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ACE.Auth"));
            claims.Add(new Claim("ticket", token));
            ClaimsIdentity ci = new ClaimsIdentity(claims, "ACE");
            ClaimsPrincipal cp = new ClaimsPrincipal(ci);

            FederatedAuthentication.SessionAuthenticationModule.CookieHandler.HideFromClientScript = true;
            var sessionToken = FederatedAuthentication.SessionAuthenticationModule.CreateSessionSecurityToken(cp, null, DateTime.UtcNow, DateTime.UtcNow.AddHours(2), false);
            FederatedAuthentication.SessionAuthenticationModule.AuthenticateSessionSecurityToken(sessionToken, true);
        }

        public static void SignOut()
        {
            FederatedAuthentication.SessionAuthenticationModule.SignOut();
        }

        private static byte[] Base64UrlDecode(string input)
        {
            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: output += "=="; break; // Two pad chars
                case 3: output += "="; break; // One pad char
                default: throw new System.Exception("Illegal base64url string!");
            }
            var converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }
    }
}