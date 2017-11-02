using ACE.Api.Common;
using ACE.Common;
using ACE.Entity.Enum;
using ACE.Api.Models;
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
        /// <summary>
        ///
        /// </summary>
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

        /// <summary>
        /// Redeploys the world database from the current contents of your ACE-World repository.  all changes that
        /// have not already been exported WILL BE LOST, and `user_modified` flags will all be reset to false.
        /// TODO: fantom implement
        /// </summary>
        [HttpGet]
        [AceAuthorize(AccessLevel.Developer)]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "Developer access level is required for this call.")]
        [SwaggerResponse(HttpStatusCode.OK, "Redeploy successful.  Return message contains the sql log." ,typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Error occurred.  Return message contains exception details.", typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.MethodNotAllowed, "You have unexported changes in your database.  Please specify 'force = true' in your request.", typeof(SimpleMessage))]
        public HttpResponseMessage RedeployWorldDatabase(RedeployRequest request)
        {

            // Download the database from Github:
            // RemoteContentSync.RetreiveGithubFolder(ConfigManager.Config.ContentServer.DatabaseUrl);
            // Download the latest ACE-World release archive, extract contents into database dir, remove downloaded zip
            // Run all scripts, in the correct sequences.
            return Request.CreateResponse(HttpStatusCode.OK, "You win!");
        }

        /// <summary>
        /// exports anything in your database with a `user_modified` value of true.
        /// TODO: Mogwai implement
        /// </summary>
        [HttpGet]
        [AceAuthorize(AccessLevel.Developer)]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "Developer access level is required for this call.")]
        [SwaggerResponse(HttpStatusCode.OK, "Export successful.  Return message contains the sql log.", typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Error occurred.  Return message contains exception details.", typeof(SimpleMessage))]
        public HttpResponseMessage ExportCurrentChanges()
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }

        /// <summary>
        /// creates new scripts of anything that currently has an exported script.  this is used in the event of
        /// table changes that render some or all current scripts invalid.
        /// TODO: Mogwai implement
        /// </summary>
        [HttpGet]
        [AceAuthorize(AccessLevel.Developer)]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "Developer access level is required for this call.")]
        [SwaggerResponse(HttpStatusCode.OK, "Rebase successful.  Return message contains the sql log.", typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Error occurred.  Return message contains exception details.", typeof(SimpleMessage))]
        public HttpResponseMessage RebaseAllScripts()
        {
            // this method will have to identify all weenies currently exported in ACE-World\Database\7-ApiContent
            // and then regenerate the scripts for them.
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }
    }
}
