using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class AmbientSTBDesc
    {
        public uint STBId { get; set; }
        public List<AmbientSoundDesc> AmbientSounds { get; set; } = new List<AmbientSoundDesc>();

        public static AmbientSTBDesc Read(DatReader datReader)
        {
            AmbientSTBDesc obj = new AmbientSTBDesc();
            obj.STBId = datReader.ReadUInt32();

            uint num_ambient_sounds = datReader.ReadUInt32();
            for (uint i = 0; i < num_ambient_sounds; i++)
                obj.AmbientSounds.Add(AmbientSoundDesc.Read(datReader));

            return obj;
        }
    }
}
