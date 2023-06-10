using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum EnvCellFlags
    {
        SeenOutside       = 0x1,
        HasStaticObjs     = 0x2,
        HasRestrictionObj = 0x8
    };
}
