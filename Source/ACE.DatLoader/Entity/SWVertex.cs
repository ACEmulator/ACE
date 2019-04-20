using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace ACE.DatLoader.Entity
{
    /// <summary>
    /// A vertex position, normal, and texture coords
    /// </summary>
    public class SWVertex : IUnpackable
    {
        public Vector3 Origin { get; private set; }
        public Vector3 Normal { get; private set; }

        public List<Vec2Duv> UVs { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            var numUVs = reader.ReadUInt16();
            UVs = new List<Vec2Duv>(numUVs);

            Origin = reader.ReadVector3();
            Normal = reader.ReadVector3();

            UVs.Unpack(reader, numUVs);
        }
    }
}
