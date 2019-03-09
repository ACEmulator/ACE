namespace ACE.DatLoader
{
    public enum DatDatabaseType : byte
    {
        Portal  = 1, // client_portal.dat and client_highres.dat both have this value
        Cell    = 2, // client_cell_1.dat

        // This is not explicity defined in the client, but we may want to use these
        Language = 3 // client_local_English.dat
    }
}
