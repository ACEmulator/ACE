namespace ACE.Network.GameEvent
{
    // order by opcode name
    public enum GameEventOpcode
    {
        PingResponse = 0x01EA,
        PopupString                     = 0x0004,
        PlayerDescription               = 0x0013,
        CharacterTitle                  = 0x0029,
        HouseStatus                     = 0x0226
    }
}
