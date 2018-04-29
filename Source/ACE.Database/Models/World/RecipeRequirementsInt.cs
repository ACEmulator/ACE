﻿using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class RecipeRequirementsInt
    {
        public uint Id { get; set; }
        public uint RecipeId { get; set; }
        public int Stat { get; set; }
        public int Value { get; set; }
        public int Enum { get; set; }
        public string Message { get; set; }

        public Recipe Recipe { get; set; }
    }
}
