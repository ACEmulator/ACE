using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class SoundDesc
    {
        public List<AmbientSTBDesc> STBDesc { get; set; } = new List<AmbientSTBDesc>();

        public static SoundDesc Read(DatReader datReader)
        {
            SoundDesc obj = new SoundDesc();

            uint num_stb_desc = datReader.ReadUInt32();
            for (uint i = 0; i < num_stb_desc; i++)
                obj.STBDesc.Add(AmbientSTBDesc.Read(datReader));

            return obj;
        }
    }
}