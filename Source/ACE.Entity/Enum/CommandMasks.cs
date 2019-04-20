using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum CommandMask: uint
    {
        Style       = 0x80000000,
        SubState    = 0x40000000,
        Modifier    = 0x20000000,
        Action      = 0x10000000,
        UI          = 0x08000000,
        Toggle      = 0x04000000,
        ChatEmote   = 0x02000000,
        Mappable    = 0x01000000,
        Command     = ~(Style | SubState | Modifier | Action | UI | Toggle | ChatEmote | Mappable)
    }
}
