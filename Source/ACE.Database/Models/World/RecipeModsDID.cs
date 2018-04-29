using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class RecipeModsDID
    {
        public uint Id { get; set; }
        public uint RecipeId { get; set; }
        public int ModSetId { get; set; }
        public int Stat { get; set; }
        public uint Value { get; set; }
        public int Enum { get; set; }
        public int Unknown1 { get; set; }

        public Recipe Recipe { get; set; }
    }
}
