namespace ACE.Network
{
    // order by opcode name
    public enum FragmentOpcode
    {
        None                           = 0x0000,
        CharacterCreate                = 0xF656,
        CharacterCreateResponse        = 0xF643,
        CharacterDelete                = 0xF655,
        CharacterEnterWorld            = 0xF657,
        CharacterEnterWorldRequest     = 0xF7C8,
        CharacterEnterWorldServerReady = 0xF7DF,
        CharacterError                 = 0xF659,
        CharacterList                  = 0xF658,
        CharacterRestore               = 0xF7D9,
        CharacterRestoreResponse       = 0xF643,
        GameAction                     = 0xF7B1,
        GameEvent                      = 0xF7B0,
        ObjectCreate                   = 0xF745,
        ObjectDelete                   = 0xF747,
        PatchStatus                    = 0xF7EA,
        PlayerCreate                   = 0xF746,
        PlayerTeleport                 = 0xF751,
        ServerName                     = 0xF7E1,
        SetState                       = 0xF74B,
        TextboxString                  = 0xF7E0,
        UpdatePosition                 = 0xF748,

        // to be named...
        Unknown75E5                    = 0xF7E5
    }
}
