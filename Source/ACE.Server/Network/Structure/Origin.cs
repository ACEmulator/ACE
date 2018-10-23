using System.IO;
using System.Numerics;
using ACE.Entity;

namespace ACE.Server.Network.Structure
{
    /// <summary>
    /// A cell and origin, without orientation
    /// </summary>
    public class Origin
    {
        public uint CellID;
        public Vector3 Position;

        public Origin() { }

        public Origin(uint cellID, Vector3 position)
        {
            CellID = cellID;
            Position = position;
        }

        public Origin(Position pos)
        {
            CellID = pos.Cell;
            Position = pos.Pos;
        }
    }

    public static class OriginExtensions
    {
        public static void Write(this BinaryWriter writer, Origin origin)
        {
            writer.Write(origin.CellID);
            writer.Write(origin.Position);
        }
    }
}
