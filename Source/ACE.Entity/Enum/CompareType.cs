namespace ACE.Entity.Enum
{
    // todo: verify
    public enum CompareType
    {
        GreaterThan         = 0,
        LessThanEqual       = 1,
        LessThan            = 2,
        GreaterThanEqual    = 3,
        NotEqual            = 4,
        NotEqualNotExist    = 5,
        Equal               = 6,
        NotExist            = 7,
        Exist               = 8,

        // assumed to have been added to handle some bitfield int properties?
        NotHasBits          = 9,
        HasBits             = 10
    };
}
