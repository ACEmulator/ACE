using System;

namespace ACE.Entity.Enum
{
    /// <summary>
    /// Used during Calculation of Damage
    /// This data is sent in the priority field of the list (equipped items) portion of the player description event F7B0 - 0013 Og II
    /// </summary>
    /// 
    [Flags]
    public enum CoverageMask : uint
    {
        Unknown                 = 0x00000001,
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

    public enum CoverageMaskHelper : uint
    {
        // for server comparison only
        Underwear = CoverageMask.UnderwearUpperLegs | CoverageMask.UnderwearLowerLegs | CoverageMask.UnderwearChest | CoverageMask.UnderwearAbdomen | CoverageMask.UnderwearUpperArms | CoverageMask.UnderwearLowerArms,
        Outerwear = CoverageMask.OuterwearUpperLegs | CoverageMask.OuterwearLowerLegs | CoverageMask.OuterwearChest | CoverageMask.OuterwearAbdomen | CoverageMask.OuterwearUpperArms | CoverageMask.OuterwearLowerArms | CoverageMask.Head | CoverageMask.Hands | CoverageMask.Feet,

        UnderwearLegs = CoverageMask.UnderwearUpperLegs | CoverageMask.UnderwearLowerLegs,
        UnderwearArms = CoverageMask.UnderwearUpperArms | CoverageMask.UnderwearLowerArms,

        OuterwearLegs = CoverageMask.OuterwearUpperLegs | CoverageMask.OuterwearLowerLegs,
        OuterwearArms = CoverageMask.OuterwearUpperArms | CoverageMask.OuterwearLowerArms,

        // exclude abdomen for searching
        UnderwearShirt = CoverageMask.UnderwearChest | CoverageMask.UnderwearUpperArms | CoverageMask.UnderwearLowerArms,
        UnderwearPants = CoverageMask.UnderwearUpperLegs | CoverageMask.UnderwearLowerLegs,

        Extremities = CoverageMask.Head | CoverageMask.Hands | CoverageMask.Feet,
    }
}
