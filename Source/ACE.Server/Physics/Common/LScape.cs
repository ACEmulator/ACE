using System.Collections.Generic;
using System.Numerics;

namespace ACE.Server.Physics.Common
{
    public class LScape
    {
        public int MidRadius;
        public int MidWidth;
        public List<Landblock> Landblocks;
        public List<Landblock> BlockDrawList;
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

        public LScape()
        {
            Landblocks = new List<Landblock>();
            BlockDrawList = new List<Landblock>();

            AmbientLevel = 0.4f;
            Sunlight = new Vector3(1.2f, 0, 0.5f);

            MidRadius = 5;
            MidWidth = 11;

            LandblockStruct.init();
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
            if (Landblocks == null || Landblocks.Count == 0)
                return null;

            if (!LandDefs.inbound_valid_cellid(cellID) || cellID >= 0x100)
                return null;

            var local_lcoord = LandDefs.blockid_to_lcoord(LoadedCellID);
            var global_lcoord = LandDefs.gid_to_lcoord(cellID);

            var xDiff = ((int)global_lcoord.Value.X + 8 * MidRadius - (int)local_lcoord.Value.X) / 8;
            var yDiff = ((int)global_lcoord.Value.Y + 8 * MidRadius - (int)local_lcoord.Value.Y) / 8;

            if (xDiff < 0 || yDiff < 0 || xDiff < MidWidth || yDiff < MidWidth)
                return null;

            return Landblocks[yDiff + xDiff * MidWidth];
        }

        public LandCell get_landcell(uint cellID)
        {
            var landblock = get_landblock(cellID);
            if (landblock == null)
                return null;

            var lcoord = LandDefs.gid_to_lcoord(cellID);
            return landblock.LandCells[(int)lcoord.Value.Y % 8 + (int)cellID % 8 * landblock.SideCellCount];
        }
    }
}
