
namespace ACE.Network.GameAction
{
    public enum GameActionOpcode
    {
        SetSingleCharacterOption    = 0x0005,
        Talk                        = 0x0015,
        RemoveFriend                = 0x0017,
        AddFriend                   = 0x0018,
        AllegianceUpdateRequest     = 0x001F,
        RemoveAllFriends            = 0x0025,
        TitleSet                    = 0x002C,
        RaiseVital                  = 0x0044,
        RaiseAbility                = 0x0045,
        RaiseSkill                  = 0x0046,
        Tell                        = 0x005D,
        LoginComplete               = 0x00A1,
        IdentifyObject              = 0x00C8,
        AdvocateTeleport            = 0x00D6,

        ChatChannel                 = 0x0147,
        Emote                       = 0x01E1,
        PingRequest                 = 0x01E9,

        HouseQuery                  = 0x021E,

        MoveToState                 = 0xF61C,
        AutonomousPosition          = 0xF753,
    }
}
