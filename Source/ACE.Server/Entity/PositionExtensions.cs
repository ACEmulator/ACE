using System;
using System.Numerics;

using log4net;

using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.Physics.Common;
using ACE.Server.Physics.Extensions;
using ACE.Server.Physics.Util;
using ACE.Server.WorldObjects;

using Position = ACE.Entity.Position;

namespace ACE.Server.Entity
{
    public static class PositionExtensions
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Vector3 ToGlobal(this Position p, bool skipIndoors = false)
        {
            // TODO: Is this necessary? It seemed to be loading rogue physics landblocks. Commented out 2019-04 Mag-nus
            //var landblock = LScape.get_landblock(p.LandblockId.Raw);

            // TODO: investigate dungeons that are below actual traversable overworld terrain
            // ex., 010AFFFF
            //if (landblock.IsDungeon)
            if (p.Indoors && skipIndoors)
                return p.Pos;

            var x = p.LandblockId.LandblockX * Position.BlockLength + p.PositionX;
            var y = p.LandblockId.LandblockY * Position.BlockLength + p.PositionY;
            var z = p.PositionZ;

            return new Vector3(x, y, z);
        }

        public static Position FromGlobal(this Position p, Vector3 pos)
        {
            // TODO: Is this necessary? It seemed to be loading rogue physics landblocks. Commented out 2019-04 Mag-nus
            //var landblock = LScape.get_landblock(p.LandblockId.Raw);

            // TODO: investigate dungeons that are below actual traversable overworld terrain
            // ex., 010AFFFF
            //if (landblock.IsDungeon)
            if (p.Indoors)
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
            // TODO: investigate dungeons that are below actual traversable overworld terrain
            // ex., 010AFFFF
            //if (landblock.IsDungeon)
            if (p.Indoors)
                return GetIndoorCell(p);

            // outside - could be on landscape, in building, or underground cave
            var cellID = GetOutdoorCell(p);
            var landcell = LScape.get_landcell(cellID) as LandCell;

            if (landcell == null)
                return cellID;

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
        public static uint GetOutdoorCell(this Position p)
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

        /// <summary>
        /// Returns the greatest single-dimension square distance between 2 positions
        /// </summary>
        public static uint CellDist(this Position p1, Position p2)
        {
            if (!p1.Indoors && !p2.Indoors)
            {
                return Math.Max(p1.GlobalCellX, p1.GlobalCellY);
            }

            // handle dungeons
            /*var block1 = LScape.get_landblock(p1.LandblockId.Raw);
            var block2 = LScape.get_landblock(p2.LandblockId.Raw);
            if (block1.IsDungeon || block2.IsDungeon)
            {
                // 2 separate dungeons = infinite distance
                if (block1.ID != block2.ID)
                    return uint.MaxValue;

                return GetDungeonCellDist(p1, p2);
            }*/

            var _p1 = new Position(p1);
            var _p2 = new Position(p2);

            if (_p1.Indoors)
                _p1.LandblockId = new LandblockId(_p1.GetOutdoorCell());
            if (_p2.Indoors)
                _p2.LandblockId = new LandblockId(_p2.GetIndoorCell());

            return Math.Max(_p1.GlobalCellX, _p2.GlobalCellY);
        }

        public static uint GetDungeonCellDist(Position p1, Position p2)
        {
            // not implemented yet
            return uint.MaxValue;
        }

        public static Vector2? GetMapCoords(this Position pos)
        {
            // no map coords available for dungeons / indoors?
            if ((pos.Cell & 0xFFFF) >= 0x100)
                return null;

            var globalPos = pos.ToGlobal();

            // 1 landblock = 192 meters
            // 1 landblock = 0.8 map units

            // 1 map unit = 1.25 landblocks
            // 1 map unit = 240 meters

            var mapCoords = new Vector2(globalPos.X / 240, globalPos.Y / 240);

            // dereth is 204 map units across, -102 to +102
            mapCoords -= Vector2.One * 102;

            return mapCoords;
        }

        public static string GetMapCoordStr(this Position pos)
        {
            var mapCoords = pos.GetMapCoords();

            if (mapCoords == null)
                return null;

            var northSouth = mapCoords.Value.Y >= 0 ? "N" : "S";
            var eastWest = mapCoords.Value.X >= 0 ? "E" : "W";

            return string.Format("{0:0.0}", Math.Abs(mapCoords.Value.Y) - 0.05f) + northSouth + ", "
                 + string.Format("{0:0.0}", Math.Abs(mapCoords.Value.X) - 0.05f) + eastWest;
        }

        public static void AdjustMapCoords(this Position pos)
        {
            // adjust Z to terrain height
            pos.PositionZ = pos.GetTerrainZ();

            // adjust to building height, if applicable
            var sortCell = LScape.get_landcell(pos.Cell) as SortCell;
            if (sortCell != null && sortCell.has_building())
            {
                var building = sortCell.Building;

                var minZ = building.GetMinZ();

                if (minZ > 0 && minZ < float.MaxValue)
                    pos.PositionZ += minZ;

                pos.LandblockId = new LandblockId(pos.GetCell());
            }
        }

        public static void Translate(this Position pos, uint blockCell)
        {
            var newBlockX = blockCell >> 24;
            var newBlockY = (blockCell >> 16) & 0xFF;

            var xDiff = (int)newBlockX - pos.LandblockX;
            var yDiff = (int)newBlockY - pos.LandblockY;

            //pos.Origin.X -= xDiff * 192;
            pos.PositionX -= xDiff * 192;
            //pos.Origin.Y -= yDiff * 192;
            pos.PositionY -= yDiff * 192;

            //pos.ObjCellID = blockCell;
            pos.LandblockId = new LandblockId(blockCell);
        }

        public static void FindZ(this Position pos)
        {
            var envCell = DatManager.CellDat.ReadFromDat<DatLoader.FileTypes.EnvCell>(pos.Cell);
            pos.PositionZ = envCell.Position.Origin.Z;
        }

        public static float GetTerrainZ(this Position p)
        {
            var landblock = LScape.get_landblock(p.LandblockId.Raw);

            var cellID = GetOutdoorCell(p);
            var landcell = (LandCell)LScape.get_landcell(cellID);

            if (landcell == null)
                return p.Pos.Z;

            Physics.Polygon walkable = null;
            if (!landcell.find_terrain_poly(p.Pos, ref walkable))
                return p.Pos.Z;

            Vector3 terrainPos = p.Pos;
            walkable.Plane.set_height(ref terrainPos);

            return terrainPos.Z;
        }

        /// <summary>
        /// Returns TRUE if outdoor position is located on walkable slope
        /// </summary>
        public static bool IsWalkable(this Position p)
        {
            if (p.Indoors) return true;

            var landcell = (LandCell)LScape.get_landcell(p.Cell);

            Physics.Polygon walkable = null;
            var terrainPoly = landcell.find_terrain_poly(p.Pos, ref walkable);
            if (walkable == null) return false;

            return Physics.PhysicsObj.is_valid_walkable(walkable.Plane.Normal);
        }

        /// <summary>
        /// Returns TRUE if current cell is a House cell
        /// </summary>
        public static bool IsRestrictable(this Position p, Landblock landblock)
        {
            var cell = landblock.IsDungeon ? p.Cell : p.GetOutdoorCell();

            return HouseCell.HouseCells.ContainsKey(cell);
        }

        public static Position ACEPosition(this Physics.Common.Position pos)
        {
            return new Position(pos.ObjCellID, pos.Frame.Origin, pos.Frame.Orientation);
        }

        public static Physics.Common.Position PhysPosition(this Position pos)
        {
            return new Physics.Common.Position(pos.Cell, new Physics.Animation.AFrame(pos.Pos, pos.Rotation));
        }


        // differs from ac physics engine
        public static readonly float RotationEpsilon = 0.0001f;

        public static bool IsRotationValid(this Quaternion q)
        {
            if (q == Quaternion.Identity)
                return true;

            if (float.IsNaN(q.X) || float.IsNaN(q.Y) || float.IsNaN(q.Z) || float.IsNaN(q.W))
                return false;

            var length = q.Length();
            if (float.IsNaN(length))
                return false;

            if (Math.Abs(1.0f - length) > RotationEpsilon)
                return false;

            return true;
        }

        public static bool AttemptToFixRotation(this Position pos, WorldObject wo, PositionType positionType)
        {
            log.Warn($"detected bad quaternion x y z w for {wo.Name} (0x{wo.Guid}) | WCID: {wo.WeenieClassId} | WeenieType: {wo.WeenieType} | PositionType: {positionType}");
            log.Warn($"before fix: {pos.ToLOCString()}");

            var normalized = Quaternion.Normalize(pos.Rotation);

            var success = IsRotationValid(normalized);

            if (success)
                pos.Rotation = normalized;

            log.Warn($" after fix: {pos.ToLOCString()}");

            return success;
        }
    }
}
