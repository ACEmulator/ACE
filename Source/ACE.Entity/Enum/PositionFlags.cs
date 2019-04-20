using System;

namespace ACE.Entity.Enum
{
    /// <summary>
    /// The PositionFlags indicate the fields present in the Position structure
    /// </summary>
    [Flags]
    public enum PositionFlags: uint
    {
        None              = 0x00,   // no data
        HasVelocity       = 0x01,   // velocity vector is present
        HasPlacementID    = 0x02,   // placement id is present
        IsGrounded        = 0x04,   // object is grounded
        OrientationHasNoW = 0x08,   // orientation quaternion has no w component
        OrientationHasNoX = 0x10,   // orientation quaternion has no x component
        OrientationHasNoY = 0x20,   // orientation quaternion has no y component
        OrientationHasNoZ = 0x40,   // orientation quaternion has no z component
    }
}
