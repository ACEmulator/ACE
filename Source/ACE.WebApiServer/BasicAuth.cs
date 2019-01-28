using ACE.Database;
using ACE.Database.Models.Auth;
using ACE.Entity.Enum;
using Nancy.Authentication.Stateless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ACE.WebApiServer
{
    internal static class BasicAuth
    {
        private const bool REJECT_BLANK_CREDENTIALS = true;
        public static StatelessAuthenticationConfiguration GetAuthCfg()
        {
            return new StatelessAuthenticationConfiguration(ctx =>
            {
                KeyValuePair<string, IEnumerable<string>> auth = ctx.Request.Headers.FirstOrDefault(k => k.Key == "Authorization");
                if (auth.Key == null || auth.Value == null || auth.Value.Count() < 1)
                {
                    return null;
                }
                string val0 = auth.Value.First();
                if (string.IsNullOrWhiteSpace(val0) || val0.Length < 7 || !val0.StartsWith("Basic "))
                {
                    return null;
                }
                else
                {
                    val0 = val0.Substring(6);
                }
                string tuple = Encoding.UTF8.GetString(Convert.FromBase64String(val0));
                int delim = tuple.IndexOf(':');
                if (delim < 0)
                {
                    return null;
                }
                if (REJECT_BLANK_CREDENTIALS && (delim < 1 || tuple.Length == delim + 1))
                {
                    return null;
                }
                string user = tuple.Substring(0, delim);
                string pass = tuple.Substring(delim + 1);
                Account acct = null;
                Gate.RunGatedAction(() =>
                {
                    acct = DatabaseManager.Authentication.GetAccountByName(user);
                    if (acct == null)
                    {
                        //TO-DO: Thread.Sleep(average duration difference between (acct == null) and (acct != null));
                        return;
                    }
                    if (!acct.PasswordMatches(pass))
                    {
                        acct = null;
                    }
                });
                if (acct == null)
                {
                    //TO-DO: exponential fallback temporary ip address ban
                    return null;
                }
                else
                {
                    List<Claim> claims = new List<Claim>
                         {
                                new Claim(ClaimTypes.Name, acct.AccountName),
                                new Claim("AccountId", acct.AccountId.ToString()),
                                new Claim("AccessLevelId", acct.AccessLevel.ToString()),
                                new Claim("AccessLevelName", ((AccessLevel)acct.AccessLevel).ToString()),
                                new Claim(((AccessLevel)acct.AccessLevel).ToString(), "1")
                         };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "ACEmulator portal");
                    ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
                    return principal;
                }
            });
        }
    }
}
