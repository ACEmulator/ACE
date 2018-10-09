using System;

namespace ACE.Entity.Enum
{
    /// <summary>
    /// Indicates the source and destination for life magic transfer spells
    /// </summary>
    [Flags]
    public enum TransferFlags
    {
        CasterSource = 0x1,
        TargetSource = 0x2,
        CasterDestination = 0x4,
        TargetDestination = 0x8
    };
}
