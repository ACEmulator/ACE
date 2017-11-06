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
using Newtonsoft.Json.Linq;
using System.Security.Principal;
using System.Security.Authentication;
using ACE.Common;
using RestSharp;

namespace ACE.Api.Common
{
    public class JwtManager
    {
        public const string AceIssuerName = "ACE";
        public const string AceAudience = "http://auth.acemulator.org/";

        /// <summary>
        /// jwt hmac secret, base64 encoded
        /// </summary>
        public static string HmacSecret { get; private set; }

        public static string RsaPublicKey { get; private set; }

        private static string RsaPrivateKey { get; set; }

        public static SigningCredentials HmacSigning { get; private set; }
        
        static JwtManager()
        {
            EnsureHmacKey();

            var hmacKey = Convert.FromBase64String(HmacSecret);
            HmacSigning = new SigningCredentials(new InMemorySymmetricSecurityKey(hmacKey), 
                                                  "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256", 
                                                  "http://www.w3.org/2001/04/xmlenc#sha256");
        }

        private static void EnsureHmacKey()
        {
            // see if we have a secret
            string debugPath = @"..\..\..\..\ACE\hmac_key.txt"; // default path for debug
            string path = "hmac_key.txt"; // default path for user installations

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
                HmacSecret = key;
            }
            else
            {
                HmacSecret = File.ReadAllText(path);
            }
        }

        public static string GenerateToken(Account account, AccessLevel role, SigningCredentials credentials, int expireMinutes = 120)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            
            List<string> roles = new List<string>();
            foreach (AccessLevel level in Enum.GetValues(typeof(AccessLevel)))
            {
                if ((int)role >= (int)level)
                    roles.Add(level.ToString());
            }

            var subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, account.Name),
                new Claim(ClaimTypes.NameIdentifier, account.AccountGuid.ToString()),
                new Claim("account_id", account.AccountId.ToString()),
                new Claim("issuing_server", ConfigManager.Config.AuthServer.PublicUrl),
                new Claim("display_name", account.DisplayName ?? account.Name)
            });

            roles.ForEach(r => subject.AddClaim(new Claim(ClaimTypes.Role, r)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                TokenIssuerName = AceIssuerName,
                AppliesToAddress = AceAudience,
                Lifetime = new Lifetime(now, now.AddMinutes(expireMinutes)),
                SigningCredentials = credentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static TokenInfo ParseToken(string token, bool validateHmac = true)
        {
            TokenInfo ti = new TokenInfo();
            
            var parts = token.Split('.');
            var headerBase64 = parts[0];
            var bodyBase64 = parts[1];
            var signature = parts[2];

            // parse the header and body into objects
            var headerJson = Encoding.UTF8.GetString(JwtUtil.Base64UrlDecode(headerBase64));
            var headerData = JObject.Parse(headerJson);
            var bodyJson = Encoding.UTF8.GetString(JwtUtil.Base64UrlDecode(bodyBase64));
            var bodyData = JObject.Parse(bodyJson);

            // verify algorithm
            var algorithm = (string)headerData["alg"];
            if (algorithm != "HS256")
                throw new NotSupportedException("Only HS256 is supported for this algorithm.");

            if (validateHmac)
            {
                // verify signature
                byte[] bytesToSign = GetBytes(string.Join(".", headerBase64, bodyBase64));
                var alg = new HMACSHA256(Convert.FromBase64String(HmacSecret));
                var hash = alg.ComputeHash(bytesToSign);
                var computedSig = JwtUtil.Base64UrlEncode(hash);

                if (computedSig != signature)
                    throw new AuthenticationException("Invalid JWT signature");
            }

            // verify expiration
            var expirationUtc = JwtUtil.ConvertFromUnixTimestamp((long)bodyData["exp"]);
            if (DateTime.UtcNow > expirationUtc)
                throw new AuthenticationException("Token has expired");

            // verify audience
            var jwtAudience = (string)bodyData["aud"];

            if (jwtAudience != JwtManager.AceAudience)
                throw new AuthenticationException($"Invalid audience '{jwtAudience}'.  Expected '{JwtManager.AceAudience}'.");

            ti.Name = (string)bodyData[ClaimTypes.Name] ?? (string)bodyData["unique_name"];

            var roles = bodyData[ClaimTypes.Role] ?? bodyData["role"];

            if (roles != null)
            {
                if (roles.HasValues)
                    ti.Roles = roles.Select(r => (string)r).ToList();
                else
                    ti.Roles = new List<string>() { (string)roles };
            }

            string accountGuid = (string)bodyData[ClaimTypes.NameIdentifier] ?? (string)bodyData["nameid"];
            ti.AccountGuid = Guid.Parse(accountGuid);
            ti.AccountId = uint.Parse((string)bodyData["account_id"]);
            ti.IssuingServer = (string)bodyData["issuing_server"];

            return ti;
        }

        public static IPrincipal GetPrincipal(TokenInfo ti)
        {
            return new GenericPrincipal(new GenericIdentity(ti.AccountGuid.ToString(), "Bearer"), ti.Roles.ToArray());
        }

        public static IPrincipal GetPrincipal(string token)
        {
            return GetPrincipal(ParseToken(token, true));
        }

        private static byte[] GetBytes(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public static TokenInfo ParseRemoteToken(string rawToken)
        {
             TokenInfo ti = ParseToken(rawToken, false);

            // check game config to see if the issuing server is allowed
            if (!ConfigManager.Config.Server.AllowedAuthServers.Contains(ti.IssuingServer))
                return null;

            // reach out to that server to validate the token
            RestClient subClient = new RestClient(ti.IssuingServer);
            var subsRequest = new RestRequest("/Account/Validate", Method.GET);
            subsRequest.AddHeader("Authorization", "Bearer " + rawToken);
            var subsResponse = subClient.Execute(subsRequest);

            bool ok = subsResponse.StatusCode == System.Net.HttpStatusCode.OK;
            return ok ? ti : null;
        }
    }
}
