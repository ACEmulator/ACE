using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ACE.Api.Common
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
                var principal = JwtManager.GetPrincipal(jwtToken);
                
                // setup context principal
                request.GetRequestContext().Principal = principal;
                
                return base.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                return Task.Run(() => request.CreateResponse(HttpStatusCode.Unauthorized, new { message = ex.Message }));
            }
        }

    }
}
