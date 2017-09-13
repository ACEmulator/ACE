using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum RecipeResult
    {
        SourceItemDestroyed      = 1,
        TargetItemDestroyed      = 2,
        SourceItemUsesDecrement  = 4,
        TargetItemUsesDecrement  = 8,
        CreateNewItem1           = 16,
        CreateNewItem2           = 32
    }
}
