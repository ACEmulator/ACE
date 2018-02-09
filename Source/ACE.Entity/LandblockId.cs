using System;
using ACE.Entity.Enum;

namespace ACE.Entity
{
    public struct LandblockId
    {
        public uint Raw { get; }

        public LandblockId(uint raw)
        {
            Raw = raw;
        }

        public LandblockId(byte x, byte y)
        {
            Raw = (uint)x << 24 | (uint)y << 16;
        }

        public LandblockId East
        {
            get { return new LandblockId(Convert.ToByte(LandblockX + 1), LandblockY); }
        }

        public LandblockId West
        {
            get { return new LandblockId(Convert.ToByte(LandblockX - 1), LandblockY); }
        }

        public LandblockId North
        {
            get { return new LandblockId(LandblockX, Convert.ToByte(LandblockY + 1)); }
        }

        public LandblockId South
        {
            get { return new LandblockId(LandblockX, Convert.ToByte(LandblockY - 1)); }
        }

        public LandblockId NorthEast
        {
            get { return new LandblockId(Convert.ToByte(LandblockX + 1), Convert.ToByte(LandblockY + 1)); }
        }

        public LandblockId NorthWest
        {
            get { return new LandblockId(Convert.ToByte(LandblockX - 1), Convert.ToByte(LandblockY + 1)); }
        }

        public LandblockId SouthEast
        {
            get { return new LandblockId(Convert.ToByte(LandblockX + 1), Convert.ToByte(LandblockY - 1)); }
        }

        public LandblockId SouthWest
        {
            get { return new LandblockId(Convert.ToByte(LandblockX - 1), Convert.ToByte(LandblockY - 1)); }
        }

        public ushort Landblock
        {
            get { return (ushort)((Raw >> 16) & 0xFFFF); }
        }

        public byte LandblockX
        {
            get { return (byte)((Raw >> 24) & 0xFF); }
        }

        public byte LandblockY
        {
            get { return (byte)((Raw >> 16) & 0xFF); }
        }
        /// <summary>
        /// This is only used to calclate LandcellX and LandcellY - it has no other function.
        /// </summary>
        public ushort Landcell
        {
            get { return (byte)((Raw & 0x3F) - 1); }
        }

        public byte LandcellX
        {
            get { return Convert.ToByte((Landcell >> 3) & 0x7); }
        }

        public byte LandcellY
        {
            get { return Convert.ToByte(Landcell & 0x7); }
        }

        public MapScope MapScope
        {
            get { return (MapScope)((Raw & 0x0F00) >> 8); }
        }

        public static bool operator ==(LandblockId c1, LandblockId c2)
        {
            return c1.Landblock == c2.Landblock;
        }

        public static bool operator !=(LandblockId c1, LandblockId c2)
        {
            return c1.Landblock != c2.Landblock;
        }

        public bool IsAdjacentTo(LandblockId block)
        {
            return (Math.Abs(this.LandblockX - block.LandblockX) <= 1 && Math.Abs(this.LandblockY - block.LandblockY) <= 1);
        }
        public override bool Equals(object obj)
        {
            if (obj is LandblockId)
                return ((LandblockId)obj) == this;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
