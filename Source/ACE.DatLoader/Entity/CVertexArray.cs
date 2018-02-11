using System;
using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.Entity
{
    /// <summary>
    /// This is actually different than just a "VertexArray" class.
    /// </summary>
    public class CVertexArray : IUnpackable
    {
        public int VertexType { get; private set; }
        public Dictionary<ushort, SWVertex> Vertices { get; } = new Dictionary<ushort, SWVertex>();

        public void Unpack(BinaryReader reader)
        {
            VertexType = reader.ReadInt32();

            var numVertices = reader.ReadUInt32();

            if (VertexType == 1)
                Vertices.Unpack(reader, numVertices);
            else
                throw new NotImplementedException();
        }
    }
}
