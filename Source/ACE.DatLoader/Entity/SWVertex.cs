using ACE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    /// <summary>
    /// This is actually different than just a "Vertex" class.
    /// </summary>
    public class SWVertex
    {
        public short VertId { get; set; } // referenced by a Polygon
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float NormalX { get; set; }
        public float NormalY { get; set; }
        public float NormalZ { get; set; }
        public List<Vec2Duv> UVs { get; set; } = new List<Vec2Duv>();

        public static SWVertex Read(DatReader datReader)
        {
            SWVertex obj = new SWVertex();

            short num_uvs = datReader.ReadInt16();

            obj.X = datReader.ReadSingle();
            obj.Y = datReader.ReadSingle();
            obj.Z = datReader.ReadSingle();
            obj.NormalX = datReader.ReadSingle();
            obj.NormalY = datReader.ReadSingle();
            obj.NormalZ = datReader.ReadSingle();

            for (short i = 0; i < num_uvs; i++)
                obj.UVs.Add(Vec2Duv.Read(datReader));
            
            return obj;
        }
    }
}
