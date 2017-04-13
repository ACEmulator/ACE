using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class HeritageGroupCG
    {
        public string Name { get; set; }
        public uint IconImage { get; set; }
        public uint SetupID { get; set; } // Basic character model
        public uint EnvironmentSetupID { get; set; } // This is the background environment during Character Creation
        public uint AttributeCredits { get; set; }
        public uint SkillCredits { get; set; }
        public List<int> PrimaryStartAreaList { get; set; } = new List<int>();
        public List<int> SecondaryStartAreaList { get; set; } = new List<int>();
        public List<SkillCG> SkillList { get; set; } = new List<SkillCG>();
        public List<TemplateCG> TemplateList { get; set; } = new List<TemplateCG>();
        public Dictionary<int, SexCG> SexList { get; set; } = new Dictionary<int, SexCG>();
    }
}
