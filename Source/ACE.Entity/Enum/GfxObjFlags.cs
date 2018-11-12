using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum GfxObjFlags: uint
    {
        HasPhysics      = 0x1,
        HasDrawing      = 0x2,
        Unknown         = 0x4,
        HasDIDDegrade   = 0x8
    }
}
