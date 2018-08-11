using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class RecipeMod
    {
        public RecipeMod()
        {
            RecipeModsBool = new HashSet<RecipeModsBool>();
            RecipeModsDID = new HashSet<RecipeModsDID>();
            RecipeModsFloat = new HashSet<RecipeModsFloat>();
            RecipeModsIID = new HashSet<RecipeModsIID>();
            RecipeModsInt = new HashSet<RecipeModsInt>();
            RecipeModsString = new HashSet<RecipeModsString>();
        }

        public uint Id { get; set; }
        public uint RecipeId { get; set; }
        public bool ExecutesOnSuccess { get; set; }
        public int Health { get; set; }
        public int Stamina { get; set; }
        public int Mana { get; set; }
        public bool Unknown7 { get; set; }
        public int DataId { get; set; }
        public int Unknown9 { get; set; }
        public int InstanceId { get; set; }

        public Recipe Recipe { get; set; }
        public ICollection<RecipeModsBool> RecipeModsBool { get; set; }
        public ICollection<RecipeModsDID> RecipeModsDID { get; set; }
        public ICollection<RecipeModsFloat> RecipeModsFloat { get; set; }
        public ICollection<RecipeModsIID> RecipeModsIID { get; set; }
        public ICollection<RecipeModsInt> RecipeModsInt { get; set; }
        public ICollection<RecipeModsString> RecipeModsString { get; set; }
    }
}
