using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ACE.Entity;
// using Microsoft.IdentityModel.Tokens;
using System.Web.SessionState;
using JwtAuthForWebAPI;
using System.Security.Principal;
using System.IdentityModel.Tokens;
using System.IdentityModel.Protocols.WSTrust;

namespace ACE.Api
{
    public class JwtManager
    {
        /// <summary>
        /// jwt hmac secret, base64 encoded
        /// </summary>
        public static string Secret { get; private set; }

        static JwtManager()
        {
            // see if we have a secret
            string path = "secret.txt";

            if (!File.Exists(path))
            {
                // no secret file exists, so generate one randomly
                var hmac = new HMACSHA256();
                var key = Convert.ToBase64String(hmac.Key);
                File.WriteAllText(path, key);
                Secret = key;
            }
            else
            {
                Secret = File.ReadAllText(path);
            }
        }

        public static string GenerateToken(Account account, int expireMinutes = 20)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var symmetricKey = Convert.FromBase64String(Secret);
            var now = DateTime.UtcNow;

            var tokenDescriptor = new System.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, account.Name),
                    new Claim(ClaimTypes.Role, account.AccessLevel.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString())
                }),
                TokenIssuerName = "ACE",
                Lifetime = new Lifetime(now, now.AddMinutes(expireMinutes)),
                SigningCredentials = new SigningCredentials(new InMemorySymmetricSecurityKey(symmetricKey), "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256", "http://www.w3.org/2001/04/xmlenc#sha256")
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
