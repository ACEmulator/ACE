using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class RecipeComponent
    {
        public uint Id { get; set; }
        public uint RecipeId { get; set; }
        public double Percent { get; set; }
        public uint Unknown2 { get; set; }
        public string Message { get; set; }

        public Recipe Recipe { get; set; }
    }
}
