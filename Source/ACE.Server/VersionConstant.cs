using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Server
{
    public static partial class VersionConstant
    {
        public static DateTime CompilationTimestampUtc
        {
            get
            {
                return new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            }
        }
    }
}
