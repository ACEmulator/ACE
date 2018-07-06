namespace ACE.DatLoader
{
    public enum DatDatabaseType : byte
    {
        Portal  = 1,
        Cell    = 2,

        // These are not defined in the client, but we may want to use these
        Highres = 3,
        Language = 4
    }
}
