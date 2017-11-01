using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ACE.AuthApi.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class ServerController : BaseController
    {
        /// <summary>
        ///
        /// </summary>
        [HttpPost]
        [Route("servers/register")]
        public HttpResponseMessage RegisterServer(ServerInfo serverInfo)
        {
            return new HttpResponseMessage(HttpStatusCode.NotImplemented);
        }

        /// <summary>
        ///
        /// </summary>
        [HttpPost]
        [Route("servers/keepalive")]
        public HttpResponseMessage KeepAlive(ServerInfo serverInfo)
        {
            return new HttpResponseMessage(HttpStatusCode.NotImplemented);
        }
        
        /// <summary>
        ///
        /// </summary>
        [HttpPost]
        [Route("servers/offline")]
        public HttpResponseMessage GoOffline(ServerInfo serverInfo)
        {
            return new HttpResponseMessage(HttpStatusCode.NotImplemented);
        }

        /// <summary>
        ///
        /// </summary>
        [HttpPost]
        [Route("servers/online")]
        public HttpResponseMessage GoOnline(ServerInfo serverInfo)
        {
            return new HttpResponseMessage(HttpStatusCode.NotImplemented);
        }
    }
}
