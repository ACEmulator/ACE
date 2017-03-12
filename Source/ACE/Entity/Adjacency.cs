using System;

namespace ACE.Entity
{
    /// <summary>
    /// not sure if this is going to be used.  i thought it was, but now i'm not sure
    /// </summary>
    public enum Adjacency : int
    {
        NorthWest       = 0,
        North           = 1,
        NorthEast       = 2,
        East            = 3,
        SouthEast       = 4,
        South           = 5,
        SouthWest       = 6,
        West            = 7            
    }

    /// <summary>
    /// original thoughts on adjacency were to split landblocks into 16 segments for ghost 
    /// broadcasting.  leaving this here in case I want to come back to it.
    /// </summary>
    //[Flags]
    //public enum Adjacency
    //{
    //    None = 0x0000,
    //    NorthWest = 0x0001,
    //    NorthNorthWest = 0x0002,
    //    NorthNorthEast = 0x0004,
    //    NorthEast = 0x0008,
    //    EastNorthEast = 0x0010,
    //    EastSouthEast = 0x0020,
    //    SouthEast = 0x0040,
    //    SouthSouthEast = 0x0080,
    //    SouthSouthWest = 0x0100,
    //    SouthWest = 0x0200,
    //    WestSouthWest = 0x0400,
    //    WestNorthWest = 0x0800,
    //    All = 0x0FFF,

    //    // predefine the relevant adjacencies of the 4 quadrants of a landblock
    //    OfNorthEast = NorthNorthWest | NorthNorthEast | NorthEast | EastNorthEast | EastSouthEast,
    //    OfSouthEast = EastNorthEast | EastSouthEast | SouthEast | SouthSouthEast | SouthSouthWest,
    //    OfSouthWest = SouthSouthEast | SouthSouthWest | SouthWest | WestSouthWest | WestNorthWest,
    //    OfNorthWest = WestSouthWest | WestNorthWest | NorthWest | NorthNorthWest | NorthNorthEast
    //}

}
