using System;
using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.Extensions;
using ACE.DatLoader;

namespace ACE.Server.Physics.Common
{
    public class LandDefs
    {
        public enum Direction
        {
            Inside = 0x0,
            North = 0x1,
            South = 0x2,
            East = 0x3,
            West = 0x4,
            NorthWest = 0x5,
            SouthWest = 0x6,
            NorthEast = 0x7,
            SouthEast = 0x8,
            Unknown = 0x9
        };

        public enum WaterType
        {
            NotWater = 0x0,
            PartiallyWater = 0x1,
            EntirelyWater = 0x2,
        };

        public static readonly int BlockCellID = 0x0000FFFF;
        public static readonly int FirstEnvCellID = 0x100;
        public static readonly int LastEnvCellID = 0xFFFD;
        public static readonly int FirstLandCellID = 1;
        public static readonly int LastLandCellID = 64;

        public static readonly uint BlockMask = 0xFFFF0000;
        public static readonly int BlockX_Mask = 0xFF00;
        public static readonly int BlockY_Mask = 0x00FF;
        public static readonly int CellID_Mask = 0x0000FFFF;
        public static readonly int LandblockMask = 7;

        public static readonly int BlockPartShift = 16;
        public static readonly int LandblockShift = 3;
        public static readonly int MaxBlockShift = 8;

        public static readonly int BlockLength = 192;
        public static readonly int CellLength = 24;
        public static readonly int LandLength = 2040;

        public static readonly int BlockSide = 8;
        public static readonly int VertexPerCell = 1;
        public static readonly int HalfSquareLength = 12;

        public static List<float> LandHeightTable;

        static LandDefs()
        {
            LandHeightTable = DatManager.PortalDat.RegionDesc.LandDefs.LandHeightTable;
        }

        public static bool AdjustToOutside(Position pos)
        {
            return AdjustToOutside(ref pos.ObjCellID, ref pos.Frame.Origin);
        }

        public static bool AdjustToOutside(ref uint blockCellID, ref Vector3 loc)
        {
            var cellID = (uint)(blockCellID & CellID_Mask);

            if (cell_in_range(cellID))
            {
                if (Math.Abs(loc.X) < PhysicsGlobals.EPSILON)
                    loc.X = 0;
                if (Math.Abs(loc.Y) < PhysicsGlobals.EPSILON)
                    loc.Y = 0;

                var lcoord = get_outside_lcoord(blockCellID, loc.X, loc.Y);
                if (lcoord.HasValue)
                {
                    blockCellID = (uint)lcoord_to_gid(lcoord.Value.X, lcoord.Value.Y);
                    loc.X -= (float)Math.Floor(loc.X / BlockLength) * BlockLength;
                    loc.Y -= (float)Math.Floor(loc.Y / BlockLength) * BlockLength;
                    return true;
                }
            }
            blockCellID = 0;
            return false;
        }

        public static Vector3 GetBlockOffset(uint _cellFrom, uint _cellTo)
        {
            var cellFrom = (int)_cellFrom;
            var cellTo = (int)_cellTo;

            // refactor me
            if (cellFrom >> 16 == cellTo >> 16)
                return Vector3.Zero;

            int xShift21 = 0, xShift16 = 0;
            int yShift21 = 0, yShift16 = 0;

            if (cellFrom != 0)
            {
                xShift21 = (cellFrom >> 21) & 0x7F8;
                xShift16 = 8 * (cellFrom >> 16);
            }
            if (cellTo != 0)
            {
                yShift21 = (cellTo >> 21) & 0x7F8;
                yShift16 = 8 * ((cellTo >> 16) & 0xFF);
            }
            else
                yShift21 = yShift16 = cellFrom;

            var shift21Diff = (yShift21 - xShift21);
            var shift16Diff = (yShift16 - xShift16);

            return new Vector3(shift21Diff * 24, shift16Diff * 24, 0);
        }

        public static bool InBlock(Vector3 pos, float radius)
        {
            if (pos.X < radius || pos.Y < radius)
                return false;

            var dist = pos.Length2D() - radius;
            return pos.X < dist && pos.Y < dist;
        }

        public static Vector2? blockid_to_lcoord(uint cellID)
        {
            var x = (cellID >> BlockPartShift & BlockX_Mask) >> MaxBlockShift << LandblockShift;
            var y = (cellID >> BlockPartShift & BlockY_Mask) << LandblockShift;

            if (x < 0 || y < 0 || x >= LandLength || y >= LandLength)
                return null;
            else
                return new Vector2(x, y);
        }

        public static Vector2? gid_to_lcoord(uint cellID)
        {
            if (!inbound_valid_cellid(cellID))
                return null;

            if ((cellID & CellID_Mask) >= FirstEnvCellID)
                return null;

            var x = (cellID >> BlockPartShift & BlockX_Mask) >> MaxBlockShift << LandblockShift;
            var y = (cellID >> BlockPartShift & BlockY_Mask) << LandblockShift;

            x += (cellID & CellID_Mask) - FirstLandCellID >> LandblockShift;
            y += (cellID & CellID_Mask) - FirstLandCellID & LandblockMask;

            if (x < 0 || y < 0 || x >= LandLength || y >= LandLength)
                return null;

            return new Vector2(x, y);
        }

        public static Vector2? get_outside_lcoord(uint blockCellID, float _x, float _y)
        {
            var cellID = (uint)(blockCellID & CellID_Mask);
             
            if (cell_in_range(cellID))
            {
                var offset = blockid_to_lcoord(blockCellID);
                if (!offset.HasValue) return null;

                var x = offset.Value.X + (float)Math.Floor(_x / CellLength);
                var y = offset.Value.Y + (float)Math.Floor(_y / CellLength);

                if (x < 0 || y < 0 || x >= LandLength || y >= LandLength)
                    return null;
                else
                    return new Vector2(x, y);
            }
            return null;
        }

        public static bool cell_in_range(uint cellID)
        {
            return cellID == BlockCellID ||
                   cellID >= FirstLandCellID && cellID <= LastLandCellID ||
                   cellID >= FirstEnvCellID  && cellID <= LastEnvCellID;
        }

        public static int lcoord_to_gid(float _x, float _y)
        {
            var x = (int)_x;
            var y = (int)_y;

            if (x < 0 || y < 0 || x >= LandLength || y >= LandLength)
                return 0;

            var block = (x >> LandblockShift << MaxBlockShift) | (y >> LandblockShift);
            var cell = FirstLandCellID + ((x & LandblockMask) << LandblockShift) + (y & LandblockMask);

            return block << BlockPartShift | cell;
        }

        public static bool inbound_valid_cellid(uint blockCellID)
        {
            var cellID = (uint)(blockCellID & CellID_Mask);

            if (cell_in_range(cellID))
            {
                var block_x = (blockCellID >> BlockPartShift & BlockX_Mask) >> MaxBlockShift << LandblockShift;
                if (block_x >= 0 && block_x < LandLength)
                    return true;
            }
            return false;
        }
    }
}
