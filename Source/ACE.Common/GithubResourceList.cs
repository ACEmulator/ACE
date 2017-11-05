using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Common
{
    /// <summary>
    /// Github Data resource for managing files and folders in the github api.
    /// </summary>
    public class GithubResourceList
    {
        public string DefaultDatabaseName { get; set; }
        public string ConfigDatabaseName { get; set; }
        public List<GithubResource> Downloads { get; set; } = new List<GithubResource>();
    }
}
