namespace ACE.Entity.Enum
{
    public enum ModificationOperation
    {
        None                    = 0,
        SetValue                = 1,
        Add                     = 2,
        CopyFromSourceToTarget  = 3,
        CopyFromSourceToResult  = 4,
        Unknown1                = 5,
        Unknown2                = 6,
        AddSpell                = 7,

        // assumed to have been added to handle some bitfield int properties?
        SetBitsOn               = 8,
        SetBitsOff              = 9
    };
}
