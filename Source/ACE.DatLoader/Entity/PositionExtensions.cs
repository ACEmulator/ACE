using System.IO;

namespace ACE.DatLoader.Entity
{
    /// <summary>
    /// Helper function to read position data from dat files
    /// </summary>
    public static class PositionExtensions
    {
        /// <summary>
        /// Reads just the X,Y,Z portion of a Position
        /// </summary>
        public static void ReadOrigin(this ACE.Entity.Position p, BinaryReader reader)
        {
            p.PositionX = reader.ReadSingle();
            p.PositionY = reader.ReadSingle();
            p.PositionZ = reader.ReadSingle();
        }

        /// <summary>
        /// Reads a full spatial position with X,Y,Z and Quaternion W,X,Y,Z values.
        /// </summary>
        public static void ReadFrame(this ACE.Entity.Position p, BinaryReader reader)
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
        /// Reads the cell ID and the full spatial position with X,Y,Z and Quaternion W,X,Y,Z values.
        /// </summary>
        public static void ReadPosition(this ACE.Entity.Position p, BinaryReader reader)
        {
            p.Cell      = reader.ReadUInt32();
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
        [System.Obsolete]
        public static ACE.Entity.Position ReadFrame(DatReader datReader)
        {
            var p = new ACE.Entity.Position();

            p.PositionX = datReader.ReadSingle();
            p.PositionY = datReader.ReadSingle();
            p.PositionZ = datReader.ReadSingle();
            p.RotationW = datReader.ReadSingle();
            p.RotationX = datReader.ReadSingle();
            p.RotationY = datReader.ReadSingle();
            p.RotationZ = datReader.ReadSingle();

            return p;
        }

        /// <summary>
        /// Reads and returns just the X,Y,Z portal of a Position
        /// </summary>
        [System.Obsolete]
        public static ACE.Entity.Position ReadOrigin(DatReader datReader)
        {
            var p = new ACE.Entity.Position();

            p.PositionX = datReader.ReadSingle();
            p.PositionY = datReader.ReadSingle();
            p.PositionZ = datReader.ReadSingle();

            return p;
        }
    }
}
