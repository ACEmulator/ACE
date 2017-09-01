using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class SceneType
    {
        public List<uint> Scenes { get; set; } = new List<uint>();

        public static SceneType Read(DatReader datReader)
        {
            SceneType obj = new SceneType();

            // Not sure what this is...
            uint unknown = datReader.ReadUInt32();

            uint num_scenes = datReader.ReadUInt32();
            for (uint i = 0; i < num_scenes; i++)
                obj.Scenes.Add(datReader.ReadUInt32());

            return obj;
        }
    }
}
