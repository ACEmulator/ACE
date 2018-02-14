using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class AceRecipe
    {
        public byte[] RecipeGuid { get; set; }
        public byte RecipeType { get; set; }
        public sbyte UserModified { get; set; }
        public uint SourceWcid { get; set; }
        public uint TargetWcid { get; set; }
        public ushort? SkillId { get; set; }
        public ushort? SkillDifficulty { get; set; }
        public ushort? PartialFailDifficulty { get; set; }
        public string SuccessMessage { get; set; }
        public string FailMessage { get; set; }
        public string AlternateMessage { get; set; }
        public uint? ResultFlags { get; set; }
        public uint? SuccessItem1Wcid { get; set; }
        public uint? SuccessItem2Wcid { get; set; }
        public uint? FailureItem1Wcid { get; set; }
        public uint? FailureItem2Wcid { get; set; }
        public ushort? HealingAttribute { get; set; }
    }
}
