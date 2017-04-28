using System;

namespace ACE.Network.Enum
{
    [Flags]
    public enum IdentifyResponseFlags
    {
        None = 0x0000,
        PropertyInt32 = 0x0001,
        PropertyBool = 0x0002,
        PropertyDouble = 0x0004,
        PropertyString = 0x0008,
        Resource = 0x0100,
        PropertyInt64 = 0x2000,
        PropertySpell = 0x0010,       
        PropertyWeapon = 0x0020,
    }
}