using ACE.Entity;
using ACE.DatLoader.Entity;
using System.Collections.Generic;

namespace ACE.DatLoader.Entity
{
    public class ClothingBaseEffect
    {
        public uint SetupModel { get; set; }
        public List<CloObjectEffect> CloObjectEffects { get; set; } = new List<CloObjectEffect>();

        public ClothingBaseEffect()
        {
            this.SetupModel = 0;
            // this.Radius = 0;
        }
    }
}
