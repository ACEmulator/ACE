
namespace ACE.Network.Fragments
{
    public enum FragmentOpcode
    {
        None                            = 0x0000,
        CharacterCreateResponse         = 0xF643,
        CharacterRestoreResponse        = 0xF643,
        CharacterDelete                 = 0xF655,
        CharacterCreate                 = 0xF656,
        CharacterEnterWorld             = 0xF657,
        CharacterList                   = 0xF658,
        CharacterError                  = 0xF659,
        ObjectCreate                    = 0xF745,
        PlayerCreate                    = 0xF746,
        ObjectDelete                    = 0xF747,
        UpdatePosition                  = 0xF748,
        SetState                        = 0xF74B,
        PlayerTeleport                  = 0xF751,
        GameEvent                       = 0xF7B0,
        GameAction                      = 0xF7B1,
        CharacterEnterWorldRequest      = 0xF7C8,
        FriendsOld                      = 0xF7CD,
        CharacterRestore                = 0xF7D9,
        CharacterEnterWorldServerReady  = 0xF7DF,
        TextboxString                   = 0xF7E0,
        ServerName                      = 0xF7E1,
        Unknown75E5                     = 0xF7E5,  // to be named...
        PatchStatus                     = 0xF7EA
        
        
    }
}
