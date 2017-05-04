namespace ACE.Network.Enum
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