using System.IO;

namespace ACE.DatLoader.Entity
{
    // TODO: refactor to use existing Position object
    public class Loc : IUnpackable
    {
        public int Cell;
        public float X;
        public float Y;
        public float Z;
        public float QW;
        public float QX;
        public float QY;
        public float QZ;

        public Loc()
        {
        }

        public Loc(int cell, float x, float y, float z, float qw, float qx, float qy, float qz)
        {
            Cell = cell;
            X = x;
            Y = y;
            Z = z;
            QW = qw;
            QX = qx;
            QY = qy;
            QZ = qz;
        }

        public void Unpack(BinaryReader reader)
        {
            Cell = reader.ReadInt32();

            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Z = reader.ReadSingle();

            QW = reader.ReadSingle();
            QX = reader.ReadSingle();
            QY = reader.ReadSingle();
            QZ = reader.ReadSingle();
        }
    }
}
