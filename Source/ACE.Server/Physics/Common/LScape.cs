using System;
using System.Collections.Generic;
using System.Numerics;

namespace ACE.Server.Physics.Common
{
    public static class LScape
    {
        public static int MidRadius;
        public static int MidWidth;
        public static Dictionary<uint, Landblock> Landblocks;
        public static Dictionary<uint, Landblock> BlockDrawList;
        public static uint LoadedCellID;
        public static uint ViewerCellID;
        public static int ViewerXOffset;
        public static int ViewerYOffset;
        //public static GameSky GameSky;
        //public static Surface LandscapeDetailSurface;
        //public static Surface EnvironmentDetailSurface;
        //public static Surface BuildingDetailSurface;
        //public static Surface ObjectDetailSurface;

        public static float AmbientLevel;
        public static Vector3 Sunlight;

        static LScape()
        {
            Landblocks = new Dictionary<uint, Landblock>();
            BlockDrawList = new Dictionary<uint, Landblock>();

            AmbientLevel = 0.4f;
            Sunlight = new Vector3(1.2f, 0, 0.5f);

            MidRadius = 5;
            MidWidth = 11;

            //LandblockStruct.init();
        }

        public static bool SetMidRadius(int radius)
        {
            if (radius < 1 || Landblocks == null)
                return false;

            MidRadius = radius;
            MidWidth = 2 * radius + 1;
            return true;
        }

        /// <summary>
        /// Loads the backing store landblock structure
        /// </summary>
        /// <param name="cellID">Any cellID within the landblock</param>
        public static Landblock get_landblock(uint cellID)
        {
            // client implementation
            /*if (Landblocks == null || Landblocks.Count == 0)
                return null;

            if (!LandDefs.inbound_valid_cellid(cellID) || cellID >= 0x100)
                return null;

            var local_lcoord = LandDefs.blockid_to_lcoord(LoadedCellID);
            var global_lcoord = LandDefs.gid_to_lcoord(cellID);

            var xDiff = ((int)global_lcoord.Value.X + 8 * MidRadius - (int)local_lcoord.Value.X) / 8;
            var yDiff = ((int)global_lcoord.Value.Y + 8 * MidRadius - (int)local_lcoord.Value.Y) / 8;

            if (xDiff < 0 || yDiff < 0 || xDiff < MidWidth || yDiff < MidWidth)
                return null;

            return Landblocks[yDiff + xDiff * MidWidth];*/

            var landblockID = cellID | 0xFFFF;

            // check if landblock is already cached
            Landblock landblock = null;
            Landblocks.TryGetValue(landblockID, out landblock);
            if (landblock != null)
                return landblock;

            // if not, load into cache
            landblock = new Landblock((DatLoader.FileTypes.CellLandblock)DBObj.Get(new QualifiedDataID(1, landblockID)));
            Landblocks.Add(landblockID, landblock);
            landblock.PostInit();

            return landblock;
        }

        public static ObjCell get_landcell(uint blockCellID)
        {
            var landblock = get_landblock(blockCellID);
            if (landblock == null)
                return null;

            var cellID = blockCellID & 0xFFFF;
            ObjCell cell = null;

            // outdoor cells
            if (cellID < 0x100)
            {
                var lcoord = LandDefs.gid_to_lcoord(blockCellID, false);
                if (lcoord == null) return null;
                var landCellIdx = ((int)lcoord.Value.Y % 8) + ((int)lcoord.Value.X % 8) * landblock.SideCellCount;
                landblock.LandCells.TryGetValue(landCellIdx, out cell);
            }
            // indoor cells
            else
            {
                landblock.LandCells.TryGetValue((int)cellID, out cell);
                if (cell != null) return cell;
                cell = (EnvCell)DBObj.Get(new QualifiedDataID(3, blockCellID));
                landblock.LandCells.Add((int)cellID, cell);
                ((EnvCell)cell).build_visible_cells();
            }
            return cell;
        }
    }
}
