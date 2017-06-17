using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Entity;

namespace ACE.DatLoader.Entity
{
    public class BuildInfo
    {
        public uint ModelId { get; set; } // 0x01 or 0x02 model of the building
        public Position Frame { get; set; } = new Position(); // specific @loc of the model
        public uint NumLeaves { get; set; } // unsure what this is used for
        public List<CBldPortal> Portals { get; set; } = new List<CBldPortal>(); // portals are things like doors, windows, etc.
    }
}
