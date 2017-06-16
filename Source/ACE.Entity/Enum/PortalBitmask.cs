using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity.Enum
{
    public enum PortalBitmask : byte
    {
        NoPk        = 0x02,
        NoPKLite    = 0x04,
        NoNPK       = 0x08,
        NoSummon    = 0x10,
        NoRecall    = 0x20
    }
}
