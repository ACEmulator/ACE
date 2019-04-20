using System.IO;

namespace ACE.DatLoader.Entity
{
    /// <summary>
    /// Info on texture UV mapping
    /// </summary>
    public class Vec2Duv : IUnpackable
    {
        public float U { get; private set; }
        public float V { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            U = reader.ReadSingle();
            V = reader.ReadSingle();
        }
    }
}
