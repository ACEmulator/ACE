using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class TemplateCG
    {
        public string Name { get; set; }
        public uint IconImage { get; set; }
        public uint Title { get; set; }
        public uint Strength { get; set; }
        public uint Endurance { get; set; }
        public uint Coordination { get; set; }
        public uint Quickness { get; set; }
        public uint Focus { get; set; }
        public uint Self { get; set; }
        public List<uint> NormalSkillsList { get; set; } = new List<uint>();
        public List<uint> PrimarySkillsList { get; set; } = new List<uint>();
    }
}
