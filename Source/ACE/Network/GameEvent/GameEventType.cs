
namespace ACE.Network.GameEvent
{
    public enum GameEventType
    {
        PopupString                         = 0x0004,
        PlayerDescription                   = 0x0013,
        AllegianceUpdate                    = 0x0020,
        FriendsListUpdate                   = 0x0021,
        CharacterTitle                      = 0x0029,
        UpdateTitle                         = 0x002B,

        IdentifyObjectResponse              = 0x00C9,

        ChannelBroadcast                    = 0x0147,
        ChannelList                         = 0x0148,
        ChannelIndex                        = 0x0149,

        Emote                               = 0x01E2,
        PingResponse                        = 0x01EA,

        HouseStatus                         = 0x0226,
        DisplayStatusMessage                = 0x028A,
        DisplayParameterizedStatusMessage   = 0x028B,
        SetTurbineChatChannels              = 0x0295,
        Tell                                = 0x02BD
    }
}
