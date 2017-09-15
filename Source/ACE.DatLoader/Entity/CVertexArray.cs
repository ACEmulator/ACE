using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    /// <summary>
    /// This is actually different than just a "VertexArray" class.
    /// </summary>
    public class CVertexArray
    {
        public int VertexType { get; set; }
        public Dictionary<short, SWVertex> Vertices { get; set; } = new Dictionary<short, SWVertex>();

        public static CVertexArray Read(DatReader datReader)
        {
            CVertexArray obj = new CVertexArray();

            obj.VertexType = datReader.ReadInt32();

            uint num_vertices = datReader.ReadUInt32();

            if (obj.VertexType == 1)
            {
                for (uint i = 0; i < num_vertices; i++)
                {
                    short vert_id = datReader.ReadInt16();
                    obj.Vertices.Add(vert_id, SWVertex.Read(datReader));
                }
            }

            return obj;
        }
    }
}
