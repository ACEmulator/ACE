using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    /// <summary>
    /// Info on texture UV mapping
    /// </summary>
    public class Vec2Duv
    {
        public float U { get; set; }
        public float V { get; set; }

        public static Vec2Duv Read(DatReader datReader)
        {
            Vec2Duv obj = new Vec2Duv();

            obj.U = datReader.ReadSingle();
            obj.V = datReader.ReadSingle();

            return obj;
        }
    }
}
