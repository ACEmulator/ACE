using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Api.Models
{
    /// <summary>
    /// Api Request for Redeploying the Database.
    /// </summary>
    public class RedeployRequest
    {
        /// <summary>
        /// Boolean value representing the forced redeployment. Set this to true, too overwrite/wipe a database.
        /// </summary>
        [JsonProperty("force")]
        public bool ForceDeploy { get; set; } = false;

        /// <summary>
        /// Default Redeployment Request
        /// </summary>
        public RedeployRequest() { }

        /// <summary>
        /// Database Redeployment Request with optional Parameter allowing force overwrite operations.
        /// </summary>
        /// <param name="forceDeploy"></param>
        public RedeployRequest(bool forceDeploy)
        {
            ForceDeploy = forceDeploy;
        }
    }
}
