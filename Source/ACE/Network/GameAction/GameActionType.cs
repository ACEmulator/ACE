
namespace ACE.Network.GameAction
{
    public enum GameActionType
    {
        SetSingleCharacterOption    = 0x0005,
        Talk                        = 0x0015,
        RemoveFriend                = 0x0017,
        AddFriend                   = 0x0018,
        DropItem                    = 0x001B,
        AllegianceUpdateRequest     = 0x001F,
        RemoveAllFriends            = 0x0025,
        TitleSet                    = 0x002C,
        RaiseVital                  = 0x0044,
        RaiseAbility                = 0x0045,
        RaiseSkill                  = 0x0046,
        TrainSkill                  = 0x0047,
        Tell                        = 0x005D,
        LoginComplete               = 0x00A1,
        IdentifyObject              = 0x00C8,
        AdvocateTeleport            = 0x00D6,

        AddChannel                  = 0x0145,
        RemoveChannel               = 0x0146,
        ChatChannel                 = 0x0147,
        ListChannels                = 0x0148,
        IndexChannels               = 0x0149,

        SetCharacterOptions         = 0x01A1,

        QueryAge                    = 0x01C2,
        QueryBirth                  = 0x01C4,
        QueryHealth                 = 0x01BF,

        Emote                       = 0x01E1,
        PingRequest                 = 0x01E9,

        HouseQuery                  = 0x021E,

        TeleToMarketPlace           = 0x028D,

        MoveToState                 = 0xF61C,
        AutonomousPosition          = 0xF753,
    }
}
