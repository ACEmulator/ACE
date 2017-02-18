using System;

namespace ACE.Network.Enum
{
    [Flags]
    public enum PhysicsDescriptionFlag
    {
        None                   = 0x00000,
        CSetup                 = 0x000001,
        MTable                 = 0x000002,
        Velocity               = 0x000004,
        Acceleration           = 0x000008,
        Omega                  = 0x000010,
        Parent                 = 0x000020,
        Children               = 0x000040,
        ObjScale               = 0x000080,
        Friction               = 0x000100,
        Elastcity              = 0x000200,
        Timestamp              = 0x000400,
        Stable                 = 0x000800,
        Petable                = 0x001000,
        DefaultScript          = 0x002000,
        DefaultScriptIntensity = 0x004000,
        Position               = 0x008000,
        Movement               = 0x010000,
        AnimationFrame         = 0x200000,
        Translucency           = 0x400000
    }
}
