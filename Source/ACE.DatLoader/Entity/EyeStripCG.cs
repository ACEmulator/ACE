using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class EyeStripCG
    {
        public uint IconImage { get; set; }
        public uint IconImageBald { get; set; }
        public ObjDesc ObjDesc { get; set; } = new ObjDesc();
        public ObjDesc ObjDescBald { get; set; } = new ObjDesc();
    }
}
