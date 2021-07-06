using System.Collections.Generic;

namespace ACE.Common
{
    public struct PluginsConfiguration
    {
        public bool Enabled { get; set; }
        public List<string> Plugins { get; set; }
    }
}
