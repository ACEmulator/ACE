namespace ACE.Network.GameAction
{
    // order by opcode name
    public enum GameActionOpcode
    {
        AutonomousPosition = 0xF753,
        HouseQuery         = 0x021E,
        LoginComplete      = 0x00A1,
        MoveToState        = 0xF61C,
        PingRequest        = 0x01E9,
        UpdateRequest      = 0x001F
    }
}
