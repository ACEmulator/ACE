using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ACE.Entity;
using System.IdentityModel.Tokens;
using System.IdentityModel.Protocols.WSTrust;

namespace ACE.Api
{
    public class JwtManager
    {
        public const string AceIssuerName = "ACE";
        public const string AceAudience = "http://auth.acemulator.org/";

        /// <summary>
        /// jwt hmac secret, base64 encoded
        /// </summary>
        public static string Secret { get; private set; }

        private static SigningCredentials signingCreds;

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

            var symmetricKey = Convert.FromBase64String(Secret);

            signingCreds = new SigningCredentials(new InMemorySymmetricSecurityKey(symmetricKey), 
                                                  "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256", 
                                                  "http://www.w3.org/2001/04/xmlenc#sha256");
        }

        public static string GenerateToken(Account account, int expireMinutes = 120)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, account.Name), // unique_name
                    new Claim(ClaimTypes.Role, account.AccessLevel.ToString()), // role
                    new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()) // nameid
                }),
                TokenIssuerName = AceIssuerName,
                AppliesToAddress = AceAudience,
                Lifetime = new Lifetime(now, now.AddMinutes(expireMinutes)),
                SigningCredentials = signingCreds
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
