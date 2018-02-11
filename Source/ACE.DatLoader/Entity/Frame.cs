using System.IO;
using System.Numerics;

namespace ACE.DatLoader.Entity
{
    /// <summary>
    /// Frame consists of a Vector3 Origin and a Quaternion Orientation
    /// </summary>
    public class Frame : IUnpackable
    {
        public Vector3 Origin { get; private set; }
        public Quaternion Orientation { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            Origin = reader.ReadVector3();

            var qw = reader.ReadSingle();
            var qx = reader.ReadSingle();
            var qy = reader.ReadSingle();
            var qz = reader.ReadSingle();
            Orientation = new Quaternion(qx, qy, qz, qw);
        }
    }
}
