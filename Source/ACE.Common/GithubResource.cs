using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Common
{
    /// <summary>
    /// Download information
    /// </summary>
    public class GithubResource
    {
        public string DatabaseName { get; set; }
        public string SourceUri { get; set; }
        public string SourcePath { get; set; }
        public GithubResourceType Type { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int? FileSize { get; set; }
        public string Hash { get; set; }
        // If you use the API for requesting individual files, it will return the content, impelment if needed:
        // public string Content { get; set; }
    }
}
