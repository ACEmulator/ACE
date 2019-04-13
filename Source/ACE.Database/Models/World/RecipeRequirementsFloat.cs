﻿using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class RecipeRequirementsFloat
    {
        public uint Id { get; set; }
        public uint RecipeId { get; set; }
        public sbyte Index { get; set; }
        public int Stat { get; set; }
        public double Value { get; set; }
        public int Enum { get; set; }
        public string Message { get; set; }

        public Recipe Recipe { get; set; }
    }
}
