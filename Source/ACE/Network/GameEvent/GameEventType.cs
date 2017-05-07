﻿namespace ACE.Network.GameEvent
{
    public enum GameEventType
    {
        PopupString                         = 0x0004,
        PlayerDescription                   = 0x0013,
        AllegianceUpdate                    = 0x0020,
        FriendsListUpdate                   = 0x0021,
        InventoryPutObjInContainer          = 0x0022,
        CharacterTitle                      = 0x0029,
        UpdateTitle                         = 0x002B,

        IdentifyObjectResponse              = 0x00C9,

        ChannelBroadcast                    = 0x0147,
        ChannelList                         = 0x0148,
        ChannelIndex                        = 0x0149,
        DropTemp                            = 0x019A, // I need to look at the client and pull this out.   This is a place holder. Og II

        YourDeath                           = 0x01AC, // You have died, as a player.
        DeathNotice                         = 0x01AD, // Something (player/creature) died.
        InflictMeleeDamage                  = 0x01B1,
        ReceiveMeleeDamage                  = 0x01B2,

        UpdateHealth                        = 0x01C0,
        QueryAgeResponse                    = 0x01C3,
        UseDone                             = 0x01C7,

        Emote                               = 0x01E2,
        PingResponse                        = 0x01EA,

        HouseStatus                         = 0x0226,
        DisplayStatusMessage                = 0x028A,
        DisplayParameterizedStatusMessage   = 0x028B,
        SetTurbineChatChannels              = 0x0295,
        Tell                                = 0x02BD,

        PurgeAllEnchantments                = 0x02C6 // Silent upon death
    }
}
