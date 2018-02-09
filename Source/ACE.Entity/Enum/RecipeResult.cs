using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum RecipeResult
    {
        SourceItemDestroyed      = 1,
        TargetItemDestroyed      = 2,
        SourceItemUsesDecrement  = 4,
        TargetItemUsesDecrement  = 8,
        SuccessItem1             = 16,
        SuccessItem2             = 32,
        FailureItem1             = 64,
        FailureItem2             = 128
    }
}
