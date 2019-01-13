using ACE.Database;
using ACE.Database.Models.Auth;
using ACE.Entity.Enum;
using ACE.Server.API;
using Nancy;
using Nancy.Authentication.Forms;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace ACE.Server.Web
{
    public class UserMapper : IUserMapper
    {
        ClaimsPrincipal IUserMapper.GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            uint? userId = GUIDToUserIdCache.GetUserId(identifier);
            if (userId == null)
            {
                return null;
            }
            Account acct = null;
            Gate.RunGatedAction(() =>
            {
                acct = DatabaseManager.Authentication.GetAccountById(userId.Value);
            });
            if (acct != null)
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
            else
            {
                return null;
            }
        }
    }
}
