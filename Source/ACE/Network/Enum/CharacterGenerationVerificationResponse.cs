namespace ACE.Network
{
    public enum CharacterGenerationVerificationResponse
    {
        Undef,
        Ok,
        Pending,
        NameInUse,
        NameBanned,
        Corrupt,
        DatabaseDown,
        AdminPrivilegeDenied,
        Count
    }
}
