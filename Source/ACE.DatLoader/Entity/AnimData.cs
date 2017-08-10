using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class AnimData
    {
        public uint AnimId { get; set; }
        public uint LowFrame { get; set; }
        public uint HighFrame { get; set; }
        // Negative framerates play animation in reverse
        public float Framerate { get; set; }
    }
}
