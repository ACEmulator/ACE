
namespace ACE.Network.GameEvent
{
    // order by opcode name
    public enum GameEventType
    {
        PopupString                     = 0x0004,
        PlayerDescription               = 0x0013,
        CharacterTitle                  = 0x0029,
        PingResponse                    = 0x01EA,
        HouseStatus                     = 0x0226,
        UpdateTitle                     = 0x002B
    }
}
