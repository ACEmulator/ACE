﻿using ACE.Api.Common;
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
        /// </summary>
        [HttpGet]
        [AceAuthorize(AccessLevel.Developer)]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "Developer access level is required for this call.")]
        [SwaggerResponse(HttpStatusCode.OK, "Redeploy successful.  Return message contains the sql log." ,typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Error occurred.  Return message contains exception details.", typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.MethodNotAllowed, "You have unexported changes in your database.  Please specify 'force = true' in your request.", typeof(SimpleMessage))]
        public HttpResponseMessage RedeployWorldDatabase(RedeployRequest request)
        {
            // Only allow one request at a time:
            if (Database.Redeploy.RedeploymentActive)
                return Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "A Redeployment already in progress!");
            //TODO: Fix this hack, make the redeploy request object work properly:
            bool forceDeploy = false; //Request.RequestUri.Query.ToLowerInvariant().Contains("request.force=true") ? true : false;
            SourceSelectionOption dataSource = Request.RequestUri.Query.ToLowerInvariant().Contains("request.dataSource=") ? SourceSelectionOption.LocalDisk : SourceSelectionOption.Github;
            foreach (KeyValuePair<string, string> query in Request.GetQueryNameValuePairs())
            {
                if (query.Key.ToLowerInvariant() == "request.sourceselection")
                {
                    Enum.TryParse<SourceSelectionOption>(query.Value, out dataSource);
                }
                if (query.Key.ToLowerInvariant() == "request.force")
                {
                    // try to parse selection enum
                    if (Convert.ToBoolean(query.Value))
                    {
                        forceDeploy = true;
                    }
                }
            }
            // Check to determine if a userModified flag has been set in the database
            var modifiedFlagPresent = WorldDb.UserModifiedFlagPresent();
            if (!modifiedFlagPresent || forceDeploy)
            {
                string errorResult = Database.Redeploy.RedeployDatabaseFromSource(DatabaseSelectionOption.World, dataSource);
                if (errorResult == null)
                    return Request.CreateResponse(HttpStatusCode.OK, "The World Database has been redeployed!");
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, $"There was an error durring your request. {errorResult}");
            }
            return Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "You have unexported changes in your database.  Please specify 'force = true' in your request.");
        }

        /// <summary>
        /// Redeploys all sql scripts to reset all databases.  all changes that have not already been exported 
        /// WILL BE LOST, and `user_modified` flags will all be reset to false.
        /// </summary>
        [HttpGet]
        [AceAuthorize(AccessLevel.Developer)]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "Developer access level is required for this call.")]
        [SwaggerResponse(HttpStatusCode.OK, "Redeploy successful.  Return message contains the sql log.", typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Error occurred.  Return message contains exception details.", typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.MethodNotAllowed, "You have unexported changes in your database.  Please specify 'force = true' in your request.", typeof(SimpleMessage))]
        public HttpResponseMessage RedeployDatabase(RedeployRequest request)
        {
            // Only allow one request at a time:
            if (Database.Redeploy.RedeploymentActive)
                return Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "A Redeployment already in progress!");
            //TODO: Fix this hack, make the redeploy request object work properly:
            bool forceDeploy = false; //Request.RequestUri.Query.ToLowerInvariant().Contains("request.force=true") ? true : false;
            SourceSelectionOption dataSource = Request.RequestUri.Query.ToLowerInvariant().Contains("request.dataSource=") ? SourceSelectionOption.LocalDisk : SourceSelectionOption.Github;
            DatabaseSelectionOption databaseName = DatabaseSelectionOption.None;
            foreach (KeyValuePair<string, string> query in Request.GetQueryNameValuePairs())
            {
                if (query.Key.ToLowerInvariant() == "request.sourceselection")
                {
                    Enum.TryParse<SourceSelectionOption>(query.Value, out dataSource);
                }
                if (query.Key.ToLowerInvariant() == "request.databaseselection")
                {
                    // try to parse selection enum
                    Enum.TryParse<DatabaseSelectionOption>(query.Value, out databaseName);
                }
                if (query.Key.ToLowerInvariant() == "request.force")
                {
                    // try to parse selection enum
                    if (Convert.ToBoolean(query.Value))
                    {
                        forceDeploy = true;
                    }
                }
            }

            // Check to determine if a userModified flag has been set in the database
            var modifiedFlagPresent = WorldDb.UserModifiedFlagPresent();
            if (!modifiedFlagPresent || forceDeploy)
            {
                string errorResult = Database.Redeploy.RedeployDatabaseFromSource(databaseName, dataSource);

                if (errorResult == null)
                    return Request.CreateResponse(HttpStatusCode.OK, "Database(s) have been redeployed and should now be completely reset. Please remember to recreate your user accounts if you reset the authentication database!");
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, $"There was an error durring your request. {errorResult}");
            }
            return Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "You have unexported changes in your database.  Please specify 'force = true' in your request.");
        }

        /// <summary>
        /// Downloads Github content into local content data path.
        /// </summary>
        [HttpGet]
        [AceAuthorize(AccessLevel.Developer)]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "Developer access level is required for this call.")]
        [SwaggerResponse(HttpStatusCode.OK, "Download successful.  Return message contains the sql log.", typeof(SimpleMessage))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Error occurred.  Return message contains exception details.", typeof(SimpleMessage))]
        public HttpResponseMessage DownloadGithubContent()
        {
            var result = Database.Redeploy.GetDataFiles(SourceSelectionOption.Github);
            if (result != null)
            {
                if (result.Count > 0)
                    return Request.CreateResponse(HttpStatusCode.OK, $"{result.Count.ToString()} files received.");
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occurred.  Return message contains exception details.");
            }
            else
            {
                var couldNotDownload = Database.Redeploy.RemaingApiCalls == 0 ? $"limit reached, please wait until {Database.Redeploy.ApiResetTime.ToString()} and then try again." : "Unknown issue downloading.";

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Github API: {couldNotDownload}");
            }
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
