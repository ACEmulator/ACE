using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class RecipeRequirementsBool
    {
        public uint Id { get; set; }
        public uint RecipeId { get; set; }
        public sbyte Index { get; set; }
        public int Stat { get; set; }
        public bool Value { get; set; }
        public int Enum { get; set; }
        public string Message { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
