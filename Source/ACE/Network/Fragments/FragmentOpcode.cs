namespace ACE.Network
{
    public enum FragmentOpcode
    {
        None                    = 0x0000,
        CharacterCreateResponse = 0xF643,
        CharacterDelete         = 0xF655,
        CharacterCreate         = 0xF656,
        CharacterList           = 0xF658,
        PatchStatus             = 0xF7EA,
        ServerName              = 0xF7E1,

        // to be named...
        Unknown75E5             = 0xF7E5
    }
}
