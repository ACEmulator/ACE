using System;
using System.Collections.Generic;
using System.Text;
using ACE.Entity;
using System.Numerics;
using LScape = ACE.Server.Physics.Common.LScape;

namespace ACE.Server.Entity
{
    public static class PositionExtensions
    {
        public static Vector3 ToGlobal(this Position p)
        {
            var landblock = LScape.get_landblock(p.Landblock);

            if (landblock.IsDungeon)
                return p.Pos;

            var x = p.LandblockId.LandblockX * Position.BlockLength + p.PositionX;
            var y = p.LandblockId.LandblockY * Position.BlockLength + p.PositionY;
            var z = p.PositionZ;

            return new Vector3(x, y, z);
        }

        public static Position FromGlobal(this Position p, Vector3 pos)
        {
            var landblock = LScape.get_landblock(p.Landblock);

            if (landblock.IsDungeon)
            {
                var iPos = new Position();
                iPos.LandblockId = p.LandblockId;
                iPos.Pos = new Vector3(pos.X, pos.Y, pos.Z);
                return iPos;
            }

            var blockX = (uint)pos.X / Position.BlockLength;
            var blockY = (uint)pos.Y / Position.BlockLength;

            var localX = pos.X % Position.BlockLength;
            var localY = pos.Y % Position.BlockLength;

            var landblockID = blockX << 24 | blockY << 16 | 0xFFFF;

            var position = new Position();
            position.LandblockId = new LandblockId((byte)blockX, (byte)blockY);
            position.PositionX = localX;
            position.PositionY = localY;
            position.PositionZ = pos.Z;
            return position;
        }
    }
}
