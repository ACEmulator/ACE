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

        public static Quaternion ReadQuaternion(this BinaryReader reader)
        {
            // note that AC sends quaternions with the W-component first...
            var q = new Quaternion();

            q.W = reader.ReadSingle();
            q.X = reader.ReadSingle();
            q.Y = reader.ReadSingle();
            q.Z = reader.ReadSingle();

            return q;
        }
    }
}
