namespace ACE.Server.Network.Enum
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