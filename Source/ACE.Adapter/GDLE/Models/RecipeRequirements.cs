using System.Collections.Generic;

namespace ACE.Adapter.GDLE.Models
{
    public class RecipeRequirements
    {
        public List<IntRequirement> IntRequirements { get; set; } = new List<IntRequirement>();
        public List<DIDRequirement> DIDRequirements { get; set; } = new List<DIDRequirement>();
        public List<IIDRequirement> IIDRequirements { get; set; } = new List<IIDRequirement>();
        public List<FloatRequirement> FloatRequirements { get; set; } = new List<FloatRequirement>();
        public List<StringRequirement> StringRequirements { get; set; } = new List<StringRequirement>();
        public List<BoolRequirement> BoolRequirements { get; set; } = new List<BoolRequirement>();
    }
}
