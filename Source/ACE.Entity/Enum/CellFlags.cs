using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum CellFlags
    {
        SeenOutside       = 0x1,
        HasStaticObjs     = 0x2,
        HasRestrictionObj = 0x8
    };
}
