namespace ACE.Network.GameAction
{
    // order by opcode name
    public enum GameActionOpcode
    {
        AddFriend           = 0x0018,
        AdvocateTeleport    = 0x00D6,
        AutonomousPosition  = 0xF753,
        HouseQuery          = 0x021E,
        LoginComplete       = 0x00A1,
        MoveToState         = 0xF61C,
        PingRequest         = 0x01E9,
        RemoveFriend        = 0x0017,
        Talk                = 0x0015,        
        UpdateRequest       = 0x001F
    }
}
