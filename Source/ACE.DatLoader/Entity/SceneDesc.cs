using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class SceneDesc
    {
        public List<SceneType> SceneTypes { get; set; } = new List<SceneType>();

        public static SceneDesc Read(DatReader datReader)
        {
            SceneDesc obj = new SceneDesc();

            uint num_scene_types = datReader.ReadUInt32();
            for (uint i = 0; i < num_scene_types; i++)
                obj.SceneTypes.Add(SceneType.Read(datReader));

            return obj;
        }
    }
}
