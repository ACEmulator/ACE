namespace ACE.Network.GameMessages
{
    // order by opcode name
    public enum GameMessageOpcode
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
        EmoteText                      = 0x01E2,
        FriendsOld                     = 0xF7CD,
        GameAction                     = 0xF7B1,
        GameEvent                      = 0xF7B0,
        ObjectCreate                   = 0xF745,
        ObjectDelete                   = 0xF747,
        PatchStatus                    = 0xF7EA,
        PlayerCreate                   = 0xF746,
        PlayerTeleport                 = 0xF751,
        PrivateUpdateAttribute         = 0x02E3,
        PrivateUpdatePropertyBool      = 0x02D1,
        PrivateUpdatePropertyDouble    = 0x02D3,
        PrivateUpdatePropertyInt       = 0x02CD,
        PrivateUpdatePropertyInt64     = 0x02CF,
        PrivateUpdatePropertyString    = 0x02D5,
        PrivateUpdateSkill             = 0x02DD,
        PrivateUpdateSkillLevel        = 0x02DF,
        PrivateUpdateVital             = 0x02E7,
        PublicUpdateAttribute          = 0x02E4,
        PublicUpdatePropertyBool       = 0x02D2,
        PublicUpdatePropertyDouble     = 0x02D4,
        PublicUpdatePropertyInt        = 0x02CE,
        PublicUpdatePropertyInt64      = 0x02D0,
        PublicUpdatePropertyString     = 0x02D6,
        PublicUpdateSkill              = 0x02DE,       
        PublicUpdateSkillLevel         = 0x02E0,      
        PublicUpdateVital              = 0x02E8,
        ServerName                     = 0xF7E1,
        SetState                       = 0xF74B,
        Sound                          = 0xF750,
        TextboxString                  = 0xF7E0,
        UpdatePosition                 = 0xF748,

        // to be named...
        Unknown75E5                    = 0xF7E5
    }
}
