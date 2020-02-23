using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Server
{
    public static partial class Constants
    {
        public static DateTime CompilationTimestampUtc
        {
            get
            {
                return new DateTime(0, 0, 0, 0, 0, 0, DateTimeKind.Utc);
            }
        }
    }
}
