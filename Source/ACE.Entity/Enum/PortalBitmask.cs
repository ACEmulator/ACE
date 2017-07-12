using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum PortalBitmask : byte
    {
        Undef           = 0x00,
        Unrestricted    = 0x01,
        NoPk            = 0x02,
        NoPKLite        = 0x04,
        NoNPK           = 0x08,
        NoSummon        = 0x10,
        NoRecall        = 0x20
    }
}
