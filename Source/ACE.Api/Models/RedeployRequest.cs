using ACE.Entity.Enum;
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
        /// Database selection option
        /// </summary>
        [JsonProperty("databaseSelection")]
        public DatabaseSelectionOption DatabaseSelection { get; set; }

        /// <summary>
        /// Remote (Github) or Local data source selection option
        /// </summary>
        [JsonProperty("sourceSelection")]
        public SourceSelectionOption SourceSelection { get; set; }

        /// <summary>
        /// Default Redeployment Request
        /// </summary>
        public RedeployRequest() { }

        /// <summary>
        /// Database Redeployment Request with optional Parameter allowing force overwrite operations.
        /// </summary>
        /// <param name="forceDeploy">force a database overwrite/wipe</param>
        /// <param name="databaseSelection">database selection option</param>
        /// <param name="sourceSelection">source selection option</param>
        public RedeployRequest(bool forceDeploy, DatabaseSelectionOption databaseSelection, SourceSelectionOption sourceSelection)
        {
            DatabaseSelection = databaseSelection;
            SourceSelection = sourceSelection;
            ForceDeploy = forceDeploy;
        }
    }
}
