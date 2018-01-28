using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.Entity
{
    public class LandDefs : IUnpackable
    {
        public int NumBlockLength { get; private set; }
        public int NumBlockWidth { get; private set; }
        public float SquareLength { get; private set; }
        public int LBlockLength { get; private set; }
        public int VertexPerCell { get; private set; }
        public float MaxObjHeight { get; private set; }
        public float SkyHeight { get; private set; }
        public float RoadWidth { get; private set; }

        public List<float> LandHeightTable { get; } = new List<float>();

        public void Unpack(BinaryReader reader)
        {
            NumBlockLength  = reader.ReadInt32();
            NumBlockWidth   = reader.ReadInt32();
            SquareLength    = reader.ReadSingle();
            LBlockLength    = reader.ReadInt32();
            VertexPerCell   = reader.ReadInt32();
            MaxObjHeight    = reader.ReadSingle();
            SkyHeight       = reader.ReadSingle();
            RoadWidth       = reader.ReadSingle();

            for (int i = 0; i < 256; i++)
                LandHeightTable.Add(reader.ReadSingle());
        }
    }
}
