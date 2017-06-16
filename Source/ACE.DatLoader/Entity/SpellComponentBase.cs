using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class SpellComponentBase
    {
        public string Name { get; set; }
        public uint Category { get; set; }
        public uint Icon { get; set; }
        public uint Type { get; set; }
        public uint Gesture { get; set; }
        public float Time { get; set; }
        public string Text { get; set; }
        public float CDM { get; set; } // Unsure what this is
    }
}
