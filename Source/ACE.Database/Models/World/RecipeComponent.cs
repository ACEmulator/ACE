using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class RecipeComponent
    {
        public uint Id { get; set; }
        public uint RecipeId { get; set; }
        public double DestroyChance { get; set; }
        public uint DestroyAmount { get; set; }
        public string DestroyMessage { get; set; }

        public Recipe Recipe { get; set; }
    }
}
