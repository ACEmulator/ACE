
namespace ACE.Network.GameEvent
{
    // order by opcode name
    public enum GameEventOpcode
    {
        PopupString                     = 0x0004,
        PlayerDescription               = 0x0013,
        FriendsListUpdate               = 0x0021,
        CharacterTitle                  = 0x0029,
        PingResponse                    = 0x01EA,
        HouseStatus                     = 0x0226,
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

        Sound                           = 0xF750,

        UpdateTitle                     = 0x002B,
        Emote                           = 0x01E2
    }
}
