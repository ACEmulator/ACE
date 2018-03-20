using System;
using System.Collections.Generic;
using System.Numerics;

namespace ACE.Server.Physics.Common
{
    public class LScape
    {
        public int MidRadius;
        public int MidWidth;
        public Dictionary<uint, Landblock> Landblocks;
        public Dictionary<uint, Landblock> BlockDrawList;
        public uint LoadedCellID;
        public uint ViewerCellID;
        public int ViewerXOffset;
        public int ViewerYOffset;
        //public GameSky GameSky;
        //public Surface LandscapeDetailSurface;
        //public Surface EnvironmentDetailSurface;
        //public Surface BuildingDetailSurface;
        //public Surface ObjectDetailSurface;

        public static float AmbientLevel;
        public static Vector3 Sunlight;
        public static LScape Instance;

        public static LScape GetInstance()
        {
            if (Instance == null)
                return new LScape();
            else
                return Instance;
        }

        private LScape()
        {
            Landblocks = new Dictionary<uint, Landblock>();
            BlockDrawList = new Dictionary<uint, Landblock>();

            AmbientLevel = 0.4f;
            Sunlight = new Vector3(1.2f, 0, 0.5f);

            MidRadius = 5;
            MidWidth = 11;

            //LandblockStruct.init();
            Instance = this;
        }

        public bool SetMidRadius(int radius)
        {
            if (radius < 1 || Landblocks == null)
                return false;

            MidRadius = radius;
            MidWidth = 2 * radius + 1;
            return true;
        }

        public static Landblock get_all(uint landblockID)
        {
            var landblock = (DatLoader.FileTypes.CellLandblock)DBObj.Get(new QualifiedDataID(1, landblockID));
            if (landblock != null)
                return new Landblock(landblock);
            else
                return null;
        }

        public Landblock get_landblock(uint cellID)
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
            landblock = get_all(landblockID);
            Landblocks.Add(landblockID, landblock);
            return landblock;
        }

        public LandCell get_landcell(uint cellID)
        {
            var landblock = get_landblock(cellID);
            if (landblock == null)
                return null;

            var lcoord = LandDefs.gid_to_lcoord(cellID);
            var landCellIdx = ((int)lcoord.Value.Y % 8) + ((int)lcoord.Value.X % 8) * landblock.SideCellCount;
            return landblock.LandCells[landCellIdx];
        }
    }
}
