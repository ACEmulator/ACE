using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class HairStyleCG
    {
        public uint IconImage { get; set; }
        public bool Bald { get; set; } = false;
        public uint AlternateSetup { get; set; }
        public ObjDesc ObjDesc { get; set; }
    }
}
