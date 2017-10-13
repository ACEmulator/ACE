using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ACE.Entity;
using Microsoft.IdentityModel.Tokens;

namespace ACE.Api
{
    public class JwtManager
    {
        private static string secret;

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
                secret = key;
            }
            else
            {
                secret = File.ReadAllText(path);
            }
        }

        public static string GenerateToken(Account account, int expireMinutes = 20)
        {
            var symmetricKey = Convert.FromBase64String(secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, account.Name),
                    new Claim(ClaimTypes.Role, account.AccessLevel.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString())
                }),

                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }
    }
}
