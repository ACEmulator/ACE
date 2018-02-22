using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Entity
{
    public class SWVertex
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public float NormalX { get; set; }
        public float NormalY { get; set; }
        public float NormalZ { get; set; }

        public List<Vec2Duv> UVs { get; set; }
    }
}
