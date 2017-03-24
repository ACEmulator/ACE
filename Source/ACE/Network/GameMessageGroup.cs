
namespace ACE.Network
{
    public enum GameMessageGroup
    {
        Group00 = 0x00, // Invalid
        Group01 = 0x01, // Event
        Group02 = 0x02, // Control
        Group03 = 0x03, // Weenie
        Group04 = 0x04, // Logon
        Group05 = 0x05, // Database
        Group07 = 0x07, // Secure Weenie
        Group08 = 0x08, // Secure Logon
        Group09 = 0x09, // UI
        Group0A = 0x0A, // SmartBox
        Group0B = 0x0B, // Observer
        Group0C = 0x0C  // Max
    }
}
