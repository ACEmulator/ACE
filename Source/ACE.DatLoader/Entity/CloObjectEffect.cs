using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class CloObjectEffect
    {
        public uint Index { get; set; }
        public uint ModelId { get; set; }
        public List<CloTextureEffect> CloTextureEffects { get; set; } = new List<CloTextureEffect>();
    }
}
