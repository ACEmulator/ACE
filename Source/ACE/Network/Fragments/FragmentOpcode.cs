namespace ACE.Network
{
    public enum FragmentOpcode
    {
        None                     = 0x0000,
        CharacterCreate          = 0xF656,
        CharacterCreateResponse  = 0xF643,
        CharacterDelete          = 0xF655,
        CharacterRestore         = 0xF7D9,
        CharacterRestoreResponse = 0xF643,
        CharacterError           = 0xF659,
        CharacterList            = 0xF658,
        PatchStatus              = 0xF7EA,
        ServerName               = 0xF7E1,

        // to be named...
        Unknown75E5             = 0xF7E5
    }
}
