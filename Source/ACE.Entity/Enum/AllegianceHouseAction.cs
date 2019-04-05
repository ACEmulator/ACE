namespace ACE.Entity.Enum
{
    /// <summary>
    /// Actions related to /allegiance house
    /// </summary>
    public enum AllegianceHouseAction: uint
    {
        Undef         = 0,
        Help          = 1,
        CheckStatus   = 1,  // client naming
        GuestOpen     = 2,
        GuestClose    = 3,
        StorageOpen   = 4,
        StorageClose  = 5
    }
}
