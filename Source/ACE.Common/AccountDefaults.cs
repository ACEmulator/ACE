using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Common
{
    public struct AccountDefaults
    {
        public bool OverrideCharacterPermissions { get; set; }

        public uint DefaultAccessLevel { get; set; }
    }
}
