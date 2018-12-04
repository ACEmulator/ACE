using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum PhysicsDescriptionFlag
    {
        None                   = 0x000000,
        CSetup                 = 0x000001,      // model resource
        MTable                 = 0x000002,      // motion table / animation set
        Velocity               = 0x000004,
        Acceleration           = 0x000008,
        Omega                  = 0x000010,
        Parent                 = 0x000020,
        Children               = 0x000040,
        ObjScale               = 0x000080,
        Friction               = 0x000100,
        Elasticity             = 0x000200,
        Timestamps             = 0x000400,
        STable                 = 0x000800,      // sound table
        PeTable                = 0x001000,      // physics effect table
        DefaultScript          = 0x002000,
        DefaultScriptIntensity = 0x004000,
        Position               = 0x008000,
        Movement               = 0x010000,
        AnimationFrame         = 0x020000,
        Translucency           = 0x040000,
    }
}
