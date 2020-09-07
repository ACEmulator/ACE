using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class RecipeModsFloat
    {
        public uint Id { get; set; }
        public uint RecipeModId { get; set; }
        public sbyte Index { get; set; }
        public int Stat { get; set; }
        public double Value { get; set; }
        public int Enum { get; set; }
        public int Source { get; set; }

        public virtual RecipeMod RecipeMod { get; set; }
    }
}
