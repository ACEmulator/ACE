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
using ACE.Entity.Enum;

namespace ACE.Api.Common
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
            string debugPath = @"..\..\..\..\ACE\secret.txt"; // default path for debug
            string path = "secret.txt"; // default path for user installations

            if (!File.Exists(path) && File.Exists(debugPath))
                path = debugPath;

            if (!File.Exists(path))
            {
                string currentPath = Directory.GetCurrentDirectory();
                if (currentPath.EndsWith("Debug") || currentPath.EndsWith("Release"))
                    path = debugPath;

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
                    new Claim("account_name", account.Name),
                    new Claim("account_guid", account.AccountGuid.ToString()),
                    new Claim("account_id", account.AccountId.ToString())

                    // TODO: Iterate claims for each subscription
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
