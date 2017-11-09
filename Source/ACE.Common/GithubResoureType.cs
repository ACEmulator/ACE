using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Common
{
    public enum GithubResourceType
    {
        Unknown,
        String,
        File,
        Directory,
        SqlFile,
        TextFile,
        SqlUpdateFile,
        SqlBaseFile,
        Archive,
        WorldReleaseSqlFile
    }
}
