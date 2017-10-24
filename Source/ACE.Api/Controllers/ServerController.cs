using ACE.Common;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ACE.Api.Controllers
{
    /// <summary>
    /// provides basic information about the server
    /// </summary>
    public class ServerController : BaseController
    {
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ServerInformation))]
        public HttpResponseMessage Information()
        {
            ServerInformation info = new ServerInformation();
            info.Description = ConfigManager.Config.Server.Description;
            info.Name = ConfigManager.Config.Server.WorldName;
            info.RequiresAuthentication = ConfigManager.Config.Server.SecureAuthentication;
            if (info.RequiresAuthentication)
                info.LoginServer = ConfigManager.Config.AuthServer.PublicUrl;

            return Request.CreateResponse(HttpStatusCode.OK, info);
        }
    }
}
