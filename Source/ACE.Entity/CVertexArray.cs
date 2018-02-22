using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Entity
{
    public class CVertexArray
    {
        public int VertexType { get; set; }
        public Dictionary<ushort, SWVertex> Vertices { get; set;  }
    }
}
