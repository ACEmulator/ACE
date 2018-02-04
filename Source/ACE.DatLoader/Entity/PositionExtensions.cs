
namespace ACE.DatLoader.Entity
{
    /// <summary>
    /// Helper function to read position data from dat files
    /// </summary>
    public static class PositionExtensions
    {
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
