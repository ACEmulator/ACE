namespace ACE.Network.GameEvent
{
    // order by opcode name
    public enum GameEventOpcode
    {
        CharacterTitle    = 0x0029,
        FriendsListUpdate = 0x0021,
        HouseStatus       = 0x0226,
        PingResponse      = 0x01EA,
        PlayerDescription = 0x0013,
        PopupString       = 0x0004
    }
}
