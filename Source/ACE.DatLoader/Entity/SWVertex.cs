using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace ACE.DatLoader.Entity
{
    /// <summary>
    /// This is actually different than just a "Vertex" class.
    /// </summary>
    public class SWVertex : IUnpackable
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        public float NormalX { get; private set; }
        public float NormalY { get; private set; }
        public float NormalZ { get; private set; }

        public List<Vec2Duv> UVs { get; } = new List<Vec2Duv>();

        public void Unpack(BinaryReader reader)
        {
            var numUVs = reader.ReadUInt16();

            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Z = reader.ReadSingle();

            NormalX = reader.ReadSingle();
            NormalY = reader.ReadSingle();
            NormalZ = reader.ReadSingle();

            UVs.Unpack(reader, numUVs);
        }

        public Vector3 ToVector()
        {
            return new Vector3(X, Y, Z);
        }
    }
}
