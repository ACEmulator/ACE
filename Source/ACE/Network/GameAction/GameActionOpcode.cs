
namespace ACE.Network.GameAction
{
    public enum GameActionOpcode
    {
        Talk                = 0x0015,
        RemoveFriend        = 0x0017,
        AddFriend           = 0x0018,
        UpdateRequest       = 0x001F,
        TitleSet            = 0x002C,
        RaiseVital          = 0x0044,
        RaiseAbility        = 0x0045,
        RaiseSkill          = 0x0046,
        LoginComplete       = 0x00A1,
        AdvocateTeleport    = 0x00D6,
        Emote               = 0x01E1,
        PingRequest         = 0x01E9,
        HouseQuery          = 0x021E,
        MoveToState         = 0xF61C,
        AutonomousPosition  = 0xF753
    }
}
