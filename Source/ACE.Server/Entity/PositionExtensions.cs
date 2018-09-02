using ACE.Entity;
using System.Numerics;
using ACE.Server.Physics.Common;
using ACE.Server.Physics.Extensions;
using ACE.Server.Physics.Util;
using Position = ACE.Entity.Position;

namespace ACE.Server.Entity
{
    public static class PositionExtensions
    {
        public static Vector3 ToGlobal(this Position p)
        {
            var landblock = LScape.get_landblock(p.LandblockId.Raw);

            if (landblock.IsDungeon)
                return p.Pos;

            var x = p.LandblockId.LandblockX * Position.BlockLength + p.PositionX;
            var y = p.LandblockId.LandblockY * Position.BlockLength + p.PositionY;
            var z = p.PositionZ;

            return new Vector3(x, y, z);
        }

        public static Position FromGlobal(this Position p, Vector3 pos)
        {
            var landblock = LScape.get_landblock(p.LandblockId.Raw);

            if (landblock.IsDungeon)
            {
                var iPos = new Position();
                iPos.LandblockId = p.LandblockId;
                iPos.Pos = new Vector3(pos.X, pos.Y, pos.Z);
                iPos.Rotation = p.Rotation;
                iPos.LandblockId = new LandblockId(GetCell(iPos));
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
            position.Rotation = p.Rotation;
            position.LandblockId = new LandblockId(GetCell(position));
            return position;
        }

        /// <summary>
        /// Gets the cell ID for a position within a landblock
        /// </summary>
        public static uint GetCell(this Position p)
        {
            var landblock = LScape.get_landblock(p.LandblockId.Raw);

            // dungeons
            if (landblock.IsDungeon)
                return GetIndoorCell(p);

            // outside - could be on landscape, in building, or underground cave
            var cellID = GetOutdoorCell(p);
            var landcell = (LandCell)LScape.get_landcell(cellID);
            if (landcell.has_building())
            {
                var envCells = landcell.Building.get_building_cells();
                foreach (var envCell in envCells)
                    if (envCell.point_in_cell(p.Pos))
                        return envCell.ID;
            }

            // handle underground areas ie. caves
            // get the terrain Z-height for this X/Y
            Physics.Polygon walkable = null;
            var terrainPoly = landcell.find_terrain_poly(p.Pos, ref walkable);
            if (walkable != null)
            {
                Vector3 terrainPos = p.Pos;
                walkable.Plane.set_height(ref terrainPos);

                // are we below ground? if so, search all of the indoor cells for this landblock
                if (terrainPos.Z > p.Pos.Z)
                {
                    var envCells = landblock.get_envcells();
                    foreach (var envCell in envCells)
                        if (envCell.point_in_cell(p.Pos))
                            return envCell.ID;
                }
            }
            return cellID;
        }

        /// <summary>
        /// Gets an outdoor cell ID for a position within a landblock
        /// </summary>
        private static uint GetOutdoorCell(this Position p)
        {
            var cellX = (uint)p.PositionX / Position.CellLength;
            var cellY = (uint)p.PositionY / Position.CellLength;

            var cellID = cellX * Position.CellSide + cellY + 1;

            var blockCellID = (uint)((p.LandblockId.Raw & 0xFFFF0000) | cellID);
            return blockCellID;
        }

        /// <summary>
        /// Gets an indoor cell ID for a position within a dungeon
        /// </summary>
        private static uint GetIndoorCell(this Position p)
        {
            var adjustCell = AdjustCell.Get(p.Landblock);
            var envCell = adjustCell.GetCell(p.Pos);
            if (envCell != null)
                return envCell.Value;
            else
                return p.Cell;
        }
    }
}
