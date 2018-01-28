using System.IO;

using ACE.Entity;

namespace ACE.DatLoader.Entity
{
    /// <summary>
    /// Helper function to read position data from dat files
    /// </summary>
    public static class PositionExtensions
    {
        /// <summary>
        /// Reads a full spatial position with X,Y,Z and Quaternion W,X,Y,Z values.
        /// </summary>
        public static void ReadPosition(this Position position, BinaryReader reader)
        {
            position.Read(reader);
        }

        /// <summary>
        /// Reads a full spatial position with X,Y,Z and Quaternion W,X,Y,Z values.
        /// </summary>
        public static void Read(this Position p, BinaryReader reader)
        {
            p.PositionX = reader.ReadSingle();
            p.PositionY = reader.ReadSingle();
            p.PositionZ = reader.ReadSingle();
            p.RotationW = reader.ReadSingle();
            p.RotationX = reader.ReadSingle();
            p.RotationY = reader.ReadSingle();
            p.RotationZ = reader.ReadSingle();
        }


        /// <summary>
        /// Reads and returns a full spatial position with X,Y,Z and Quaternion W,X,Y,Z values.
        /// </summary>
        public static Position ReadPosition(DatReader datReader)
        {
            Position p = new Position();
            p.Read(datReader);
            return p;
        }

        /// <summary>
        /// Reads and returns just the X,Y,Z portal of a Position
        /// </summary>
        public static Position ReadPositionFrame(DatReader datReader)
        {
            Position p = new Position();
            p.ReadFrame(datReader);
            return p;
        }

        /// <summary>
        /// Reads and returns a full position with Landblock, X,Y,Z and Quaternion W,X,Y,Z values.
        /// </summary>
        public static Position ReadLandblockPosition(DatReader datReader)
        {
            Position p = new Position();
            p.Cell = datReader.ReadUInt32();
            p.Read(datReader);
            return p;
        }

        public static void Read(this Position p, DatReader datReader)
        {
            p.PositionX = datReader.ReadSingle();
            p.PositionY = datReader.ReadSingle();
            p.PositionZ = datReader.ReadSingle();
            p.RotationW = datReader.ReadSingle();
            p.RotationX = datReader.ReadSingle();
            p.RotationY = datReader.ReadSingle();
            p.RotationZ = datReader.ReadSingle();
        }

        public static void ReadFrame(this Position p, DatReader datReader)
        {
            p.PositionX = datReader.ReadSingle();
            p.PositionY = datReader.ReadSingle();
            p.PositionZ = datReader.ReadSingle();
        }
    }
}
