namespace ACE.Network.GameAction
{
    // order by opcode name
    public enum GameActionOpcode
    {
        Talk               = 0x0015,
        RemoveFriend       = 0x0017,
        AddFriend          = 0x0018,
        UpdateRequest      = 0x001F,
        LoginComplete      = 0x00A1,
        AdvocateTeleport   = 0x00D6,
        PingRequest        = 0x01E9,
        HouseQuery         = 0x021E,
        MoveToState        = 0xF61C,
        AutonomousPosition = 0xF753
        
    }
}
