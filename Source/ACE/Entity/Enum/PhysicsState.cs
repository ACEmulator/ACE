﻿using System;

namespace ACE.Entity
{
    [Flags]
    public enum PhysicsState
    {
        None                        = 0x00000000,
        Static                      = 0x00000001,
        Ethereal                    = 0x00000004,
        ReportCollision             = 0x00000008,
        IgnoreCollision             = 0x00000010,
        NoDraw                      = 0x00000020,
        Missile                     = 0x00000040,
        Pushable                    = 0x00000080,
        AlignPath                   = 0x00000100,
        PathClipped                 = 0x00000200,
        Gravity                     = 0x00000400,
        LightingOn                  = 0x00000800,
        ParticleEmitter             = 0x00001000,
        Hidden                      = 0x00004000,
        ScriptedCollision           = 0x00008000,
        HasPhysicsBsp               = 0x00010000,
        Inelastic                   = 0x00020000,
        HasDefaultAnim              = 0x00040000,
        HasDefaultScript            = 0x00080000,
        Cloaked                     = 0x00100000,
        ReportCollisionAsEnviroment = 0x00200000,
        EdgeSlide                   = 0x00400000,
        Sledding                    = 0x00800000,
        Frozen                      = 0x01000000
    }
}
