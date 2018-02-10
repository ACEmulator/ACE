namespace ACE.Server.Network
{
    public enum GameMessageGroup
    {
        InvalidQueue        = 0x00,
        EventQueue          = 0x01,
        ControlQueue        = 0x02,
        WeenieQueue         = 0x03,
        LoginQueue          = 0x04,
        DatabaseQueue       = 0x05,
        SecureControlQueue  = 0x06,
        SecureWeenieQueue   = 0x07, // Autonomous Position
        SecureLoginQueue    = 0x08,
        UIQueue             = 0x09,
        SmartboxQueue       = 0x0A,
        ObserverQueue       = 0x0B,
        QueueMax            = 0x0C
    }
}
