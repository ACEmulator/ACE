using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class RecipeMod
    {
        public uint Id { get; set; }
        public uint RecipeId { get; set; }
        public int ModSetId { get; set; }
        public int Health { get; set; }
        public int Unknown2 { get; set; }
        public int Mana { get; set; }
        public int Unknown4 { get; set; }
        public int Unknown5 { get; set; }
        public int Unknown6 { get; set; }
        public bool Unknown7 { get; set; }
        public int DataId { get; set; }
        public int Unknown9 { get; set; }
        public int InstanceId { get; set; }

        public Recipe Recipe { get; set; }
    }
}
