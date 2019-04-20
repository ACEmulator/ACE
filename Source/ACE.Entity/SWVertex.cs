using System.Numerics;

namespace ACE.Entity
{
    /// <summary>
    /// A runtime-definable copy of DatLoader.SWVertex
    /// </summary>
    public class SWVertex
    {
        public Vector3 Origin;
        public Vector3 Normal;
        //public List<Vec2Duv> UVs;     // texture coordinates excluded for server

        public SWVertex(Vector3 origin, Vector3 normal)
        {
            Origin = origin;    // ref?
            Normal = normal;
        }
    }
}
