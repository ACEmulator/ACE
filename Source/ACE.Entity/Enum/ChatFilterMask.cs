using System;

namespace ACE.Entity.Enum
{
    /// <summary>
    /// The ChatFilterMask identifies types of messages that are squelched or filtered (/messagetypes).<para />
    /// Used with F7B0 01F4: Game Event -> Squelch and Filter List
    /// </summary>
    [Flags]
    public enum ChatFilterMask
    {
        Speech          = 0x00000004,
        Tell            = 0x00000008,
        Combat          = 0x00000040,
        Magic           = 0x00000080,
        Emote           = 0x00001000,
        Appraisal       = 0x00010000,
        Spellcasting    = 0x00020000,
        Allegiance      = 0x00040000,
        Fellowship      = 0x00080000,
        Combat_Enemy    = 0x00200000,
        Combat_Self     = 0x00400000,
        Recall          = 0x00800000,
        Craft           = 0x01000000,
        Salvaging       = 0x02000000,
        AllMessageTypes = ~0
    }
}
