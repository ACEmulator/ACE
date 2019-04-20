using System;
namespace ACE.Entity.Enum
{
    // Thanks to tfarley (aclogview) for these enums
    [Flags]
    public enum CharacterOptionDataFlag : uint
    {
        Shortcut                    = 0x00000001,
        SquelchList                 = 0x00000002,
        MultiSpellList              = 0x00000004,
        DesiredComps                = 0x00000008,
        ExtendedMultiSpellLists     = 0x00000010,
        SpellbookFilters            = 0x00000020,
        CharacterOptions2           = 0x00000040,
        TimestampFormat             = 0x00000080,
        GenericQualitiesData        = 0x00000100,
        GameplayOptions             = 0x00000200,
        SpellLists8                 = 0x00000400
    }
}
