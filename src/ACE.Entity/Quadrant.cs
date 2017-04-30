using System;

namespace ACE.Entity
{
    [Flags]
    public enum Quadrant
    {
        None            = 0,
        NorthWest       = 1,
        NorthEast       = 2,
        SouthEast       = 4,
        SouthWest       = 8,
        All             = 15
    }
}
