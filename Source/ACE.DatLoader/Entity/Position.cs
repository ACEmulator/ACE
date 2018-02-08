using System.IO;

namespace ACE.DatLoader.Entity
{
    /// <summary>
    /// Position consists of a CellID + a Frame (Origin + Orientation)
    /// </summary>
    public class Position : IUnpackable
    {
        public uint ObjCellID { get; private set; }

        public Frame Frame = new Frame();

        public void Unpack(BinaryReader reader)
        {
            ObjCellID = reader.ReadUInt32();

            Frame.Unpack(reader);
        }
    }
}
