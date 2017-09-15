using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class Polygon
    {
        public byte NumPts { get; set; }
        public byte Stippling { get; set; } // Whether it has that textured/bumpiness to it
        public int SidesType { get; set; }
        public short PosSurface { get; set; }
        public short NegSurface { get; set; }
        public List<short> VertexIds { get; set; } = new List<short>(); 
        public List<byte> PosUVIndices { get; set; } = new List<byte>();
        public List<byte> NegUVIndices { get; set; } = new List<byte>();

        public static Polygon Read(DatReader datReader)
        {
            Polygon obj = new Polygon();

            obj.NumPts = datReader.ReadByte();
            obj.Stippling = datReader.ReadByte();
            obj.SidesType = datReader.ReadInt32();
            obj.PosSurface = datReader.ReadInt16();
            obj.NegSurface = datReader.ReadInt16();

            for (short i = 0; i < obj.NumPts; i++)
                obj.VertexIds.Add(datReader.ReadInt16());

            if ((obj.Stippling & 4) == 0)
            {
                for (short i = 0; i < obj.NumPts; i++)
                    obj.PosUVIndices.Add(datReader.ReadByte());
            }

            if (obj.SidesType == 2 && ((obj.Stippling & 8) == 0))
            {
                for (short i = 0; i < obj.NumPts; i++)
                    obj.NegUVIndices.Add(datReader.ReadByte());
            }

            if (obj.SidesType == 1)
            {
                obj.NegSurface = obj.PosSurface;
                obj.NegUVIndices = obj.PosUVIndices;
            }

            return obj;
        }
    }
}
