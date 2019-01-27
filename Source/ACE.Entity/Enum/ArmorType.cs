using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Entity.Enum
{
    // guessing on this one based on available data
    public enum ArmorType
    {
        Undef           = 0,
        Cloth           = 1,
        Leather         = 2,
        StuddedLeather  = 4,
        Scalemail       = 8,
        Chainmail       = 16,
        Metal           = 32
    };
}
