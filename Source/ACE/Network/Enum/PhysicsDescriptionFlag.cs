using System;

namespace ACE.Network.Enum
{
    [Flags]
    public enum PhysicsDescriptionFlag
    {
        None                   = 0x00000,

        /// <summary>
        /// Model set resource id
        /// </summary>
        CSetup                 = 0x000001,

        /// <summary>
        /// animation set resource id
        /// </summary>
        MTable                 = 0x000002,

        /// <summary>
        /// velocity vector (x y z floats)
        /// </summary>
        Velocity               = 0x000004,

        /// <summary>
        /// acceleration vector (x y z floats)
        /// </summary>
        Acceleration           = 0x000008,

        /// <summary>
        /// rotation vector (x y z floats)
        /// </summary>
        Omega                  = 0x000010,

        /// <summary>
        /// equipper id and slot mask
        /// </summary>
        Parent                 = 0x000020,

        /// <summary>
        /// number of items equipped
        /// </summary>
        Children               = 0x000040,

        /// <summary>
        /// size of the object
        /// </summary>
        ObjScale               = 0x000080,
        Friction               = 0x000100,
        Elastcity              = 0x000200,
        Timestamp              = 0x000400,

        /// <summary>
        /// sound set resource id
        /// </summary>
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
