using System.IO;
using System.Numerics;

namespace ACE.Server.Network.Structure
{
    public static class Extensions
    {
        public static Vector3 ReadVector3(this BinaryReader reader)
        {
            var v = new Vector3();
            v.X = reader.ReadSingle();
            v.Y = reader.ReadSingle();
            v.Z = reader.ReadSingle();
            return v;
        }
    }
}
