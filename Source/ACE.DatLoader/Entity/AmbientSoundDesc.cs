using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class AmbientSoundDesc
    {
        public uint SType { get; set; }
        public float Volume { get; set; }
        public float BaseChance { get; set; }
        public float MinRate { get; set; }
        public float MaxRate { get; set; }

        public static AmbientSoundDesc Read(DatReader datReader)
        {
            AmbientSoundDesc obj = new AmbientSoundDesc();
            obj.SType = datReader.ReadUInt32();
            obj.Volume = datReader.ReadSingle();
            obj.BaseChance = datReader.ReadSingle();
            obj.MinRate = datReader.ReadSingle();
            obj.MaxRate = datReader.ReadSingle();
            return obj;
        }
    }
}
