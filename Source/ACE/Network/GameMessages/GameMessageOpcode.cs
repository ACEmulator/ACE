
namespace ACE.Network.GameMessages
{
    public enum GameMessageOpcode
    {
        None                            = 0x0000,

        EmoteText                       = 0x01E2,

        CreatureMessage                 = 0x02BB,
        PrivateUpdatePropertyInt        = 0x02CD,
        PublicUpdatePropertyInt         = 0x02CE,
        PrivateUpdatePropertyInt64      = 0x02CF,
        PublicUpdatePropertyInt64       = 0x02D0,
        PrivateUpdatePropertyBool       = 0x02D1,
        PublicUpdatePropertyBool        = 0x02D2,
        PrivateUpdatePropertyDouble     = 0x02D3,
        PublicUpdatePropertyDouble      = 0x02D4,
        PrivateUpdatePropertyString     = 0x02D5,
        PublicUpdatePropertyString      = 0x02D6,
        PrivateUpdateSkill              = 0x02DD,
        PublicUpdateSkill               = 0x02DE,
        PrivateUpdateSkillLevel         = 0x02DF,
        PublicUpdateSkillLevel          = 0x02E0,
        PrivateUpdateAttribute          = 0x02E3,
        PublicUpdateAttribute           = 0x02E4,
        PrivateUpdateVital              = 0x02E7,
        PublicUpdateVital               = 0x02E8,

        CharacterCreateResponse         = 0xF643,
        CharacterRestoreResponse        = 0xF643, // This is a duplicate...
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
        Sound                           = 0xF750,
        PlayerTeleport                  = 0xF751,

        PlayEffect                      = 0xF755,

        GameEvent                       = 0xF7B0,
        GameAction                      = 0xF7B1,

        CharacterEnterWorldRequest      = 0xF7C8,
        FriendsOld                      = 0xF7CD,
        CharacterRestore                = 0xF7D9,
        CharacterEnterWorldServerReady  = 0xF7DF,
        ServerMessage                   = 0xF7E0,
        ServerName                      = 0xF7E1,
        UnknownF7E5                     = 0xF7E5, // to be named...
        PatchStatus                     = 0xF7EA,

    }
}

