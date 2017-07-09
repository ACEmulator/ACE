using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    // TODO: Refactor to merge with existing AnimationOverride object in ACE.
    public class AnimationPartChange
    {
        public byte PartIndex { get; set; }
        public uint PartID { get; set; }
    }
}
