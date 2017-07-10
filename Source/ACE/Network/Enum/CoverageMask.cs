using System;

namespace ACE.Network.Enum
{
    /// <summary>
    /// Used during Calculation of Damage
    /// This data is sent in the priority field of the iilst (equiped items) portion of the player discription event F7B0 - 0013 Og II
    /// </summary>
    /// 
    [Flags]
    public enum CoverageMask : uint
    {
        UnderwearUpperLegs      = 0x00000002,
        UnderwearLowerLegs      = 0x00000004,
        UnderwearChest          = 0x00000008,
        UnderwearAbdomen        = 0x00000010,
        UnderwearUpperArms      = 0x00000020,
        UnderwearLowerArms      = 0x00000040,
        OuterwearUpperLegs      = 0x00000100,
        OuterwearLowerLegs      = 0x00000200,
        OuterwearChest          = 0x00000400,
        OuterwearAbdomen        = 0x00000800,
        OuterwearUpperArms      = 0x00001000,
        OuterwearLowerArms      = 0x00002000,
        Head                    = 0x00004000,
        Hands                   = 0x00008000,
        Feet                    = 0x00010000,
    }
}