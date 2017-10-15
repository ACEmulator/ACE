using ACE.Entity.Enum;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ACE.Api
{
    /// <summary>
    /// this class exists because the alternative is playing version detection and management in about a dozen nuget packages
    /// that i really don't care to bother.  this is a simple-enough manual/explicit HS256 JWT validator
    /// </summary>
    public class AceJwtTokenHandler : DelegatingHandler
    {
        private const string authenticationType = "bearer";
        
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // if this is not a bearer header, we do not know how to process it, so just return
            var authorizationHeader = request.Headers.Authorization;

            if (string.IsNullOrWhiteSpace(authorizationHeader?.Scheme) || !authorizationHeader.Scheme.Equals(authenticationType, StringComparison.InvariantCultureIgnoreCase))
                return base.SendAsync(request, cancellationToken);

            try
            {
                // get the actual token
                var jwtToken = request.Headers.Authorization.Parameter;

                // parse the header to find out what algorithm to use
                var parts = jwtToken.Split('.');
                var headerBase64 = parts[0];
                var headerJson = Encoding.UTF8.GetString(JwtUtil.Base64UrlDecode(headerBase64));
                var headerData = JObject.Parse(headerJson);
                var principal = ValidateAndGetClaims(jwtToken);
                
                // setup context principal
                request.GetRequestContext().Principal = principal;
                
                return base.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                return Task.Run(() => request.CreateResponse(HttpStatusCode.Unauthorized, new { message = ex.Message }));
            }
        }

        private IPrincipal ValidateAndGetClaims(string token)
        {
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

            // verify signature
            byte[] bytesToSign = GetBytes(string.Join(".", headerBase64, bodyBase64));
            var alg = new HMACSHA256(Convert.FromBase64String(JwtManager.Secret));
            var hash = alg.ComputeHash(bytesToSign);
            var computedSig = JwtUtil.Base64UrlEncode(hash);

            if (computedSig != signature)
                throw new AuthenticationException("Invalid JWT signature");

            // verify expiration
            var expirationUtc = JwtUtil.ConvertFromUnixTimestamp((long)bodyData["exp"]);
            if (DateTime.UtcNow > expirationUtc)
                throw new AuthenticationException("Token has expired");

            // verify audience
            var jwtAudience = (string)bodyData["aud"];

            if (jwtAudience != JwtManager.AceAudience)
                throw new AuthenticationException($"Invalid audience '{jwtAudience}'.  Expected '{JwtManager.AceAudience}'.");

            string name = (string)bodyData["unique_name"];
            
            /// TODO: Convert to SecurityLevel instead of AccessLevel
            AccessLevel thisGuy = AccessLevel.Player;
            List<string> roles = new List<string>();
            if (Enum.TryParse((string)bodyData["role"], out thisGuy))
            {
                foreach (AccessLevel level in Enum.GetValues(typeof(AccessLevel)))
                {
                    if (thisGuy >= level)
                        roles.Add(level.ToString());
                }
            }

            uint accountId = uint.Parse((string)bodyData["nameid"]);

            return new GenericPrincipal(new GenericIdentity(name, authenticationType), roles.ToArray());
        }

        private static byte[] GetBytes(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }
    }
}
