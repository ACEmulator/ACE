using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum SurfaceType : uint
    {
        Base1Solid      = 0x1,
        Base1Image      = 0x2,
        Base1ClipMap    = 0x4,
        Translucent     = 0x10,
        Diffuse         = 0x20,
        Luminous        = 0x40,
        Alpha           = 0x100,
        InvAlpha        = 0x200,
        Additive        = 0x10000,
        Detail          = 0x20000,
        Gouraud         = 0x10000000,
        Stippled        = 0x40000000,
        Perspective     = 0x80000000
    }
}
