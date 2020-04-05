using System.Collections.Generic;

namespace ACE.Adapter.GDLE.Models
{
    public class Mod
    {
        public List<IntRequirement> IntRequirements { get; set; } = new List<IntRequirement>();
        public List<DIDRequirement> DIDRequirements { get; set; } = new List<DIDRequirement>();
        public List<IIDRequirement> IIDRequirements { get; set; } = new List<IIDRequirement>();
        public List<FloatRequirement> FloatRequirements { get; set; } = new List<FloatRequirement>();
        public List<StringRequirement> StringRequirements { get; set; } = new List<StringRequirement>();
        public List<BoolRequirement> BoolRequirements { get; set; } = new List<BoolRequirement>();

        public int ModifyHealth { get; set; }
        public int ModifyStamina { get; set; }
        public int ModifyMana { get; set; }

        public int RequiresHealth { get; set; }
        public int RequiresStamina { get; set; }
        public int RequiresMana { get; set; }

        public bool Unknown7 { get; set; }
        public int ModificationScriptId { get; set; }
        public int Unknown9 { get; set; }
        public int Unknown10 { get; set; }
    }
}
