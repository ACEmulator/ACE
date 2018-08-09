using System.Collections.Generic;
using System.IO;
using ACE.Entity.Enum;

namespace ACE.DatLoader.Entity
{
    public class Polygon : IUnpackable
    {
        public byte NumPts { get; private set; }
        public StipplingType Stippling { get; private set; } // Whether it has that textured/bumpiness to it

        public CullMode SidesType { get; private set; }
        public short PosSurface { get; private set; }
        public short NegSurface { get; private set; }

        public List<short> VertexIds { get; } = new List<short>();

        public List<byte> PosUVIndices { get; } = new List<byte>();
        public List<byte> NegUVIndices { get; private set; } = new List<byte>();

        public List<SWVertex> Vertices;

        public void Unpack(BinaryReader reader)
        {
            NumPts      = reader.ReadByte();
            Stippling   = (StipplingType)reader.ReadByte();

            SidesType   = (CullMode)reader.ReadInt32();
            PosSurface  = reader.ReadInt16();
            NegSurface  = reader.ReadInt16();

            for (short i = 0; i < NumPts; i++)
                VertexIds.Add(reader.ReadInt16());

            if (!Stippling.HasFlag(StipplingType.NoPos))
            {
                for (short i = 0; i < NumPts; i++)
                    PosUVIndices.Add(reader.ReadByte());
            }

            if (SidesType == CullMode.Clockwise && !Stippling.HasFlag(StipplingType.NoNeg))
            {
                for (short i = 0; i < NumPts; i++)
                    NegUVIndices.Add(reader.ReadByte());
            }

            if (SidesType == CullMode.None)
            {
                NegSurface = PosSurface;
                NegUVIndices = PosUVIndices;
            }
        }

        public void LoadVertices(CVertexArray vertexArray)
        {
            Vertices = new List<SWVertex>();

            foreach (var id in VertexIds)
                Vertices.Add(vertexArray.Vertices[(ushort)id]);
        }
    }
}
