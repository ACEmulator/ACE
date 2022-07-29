using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum MutateFilter
    {
        // custom
        Undef               = 0x0,
        ArmorModVsAcid      = 0x1,
        ArmorModVsCold      = 0x2,
        ArmorModVsElectric  = 0x4,
        ArmorModVsFire      = 0x8,
        EncumbranceVal      = 0x10,
        Icon                = 0x20,
        ItemWorkmanship     = 0x40,
        LongDesc            = 0x80,
        Name                = 0x100,
        ResistItemAppraisal = 0x200,
        Setup               = 0x400,
        ShieldValue         = 0x800,
        ShortDesc           = 0x1000,
        Value               = 0x2000,
        WeaponTime          = 0x4000,

        ArmorModVsType = ArmorModVsAcid | ArmorModVsCold | ArmorModVsElectric | ArmorModVsFire,

        // the 6 mutate filters in 16PY all have these fields
        Base = Icon | ItemWorkmanship | LongDesc | Name | ResistItemAppraisal | Setup | ShortDesc | Value,
    }
}
