using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Entity.Enum
{
    // guessing on this one based on available data
    // Leather (2) is used for lots of things that aren't also animal hide... Shields, masks, etc.
    public enum ArmorType
    {
        None            = 0,
        Cloth           = 1,
        Leather         = 2,
        StuddedLeather  = 4,
        Scalemail       = 8,
        Chainmail       = 16,
        Metal           = 32
    };
}
