using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class RecipeRequirementsString
    {
        public uint Id { get; set; }
        public uint RecipeId { get; set; }
        public sbyte Index { get; set; }
        public int Stat { get; set; }
        public string Value { get; set; }
        public int Enum { get; set; }
        public string Message { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
