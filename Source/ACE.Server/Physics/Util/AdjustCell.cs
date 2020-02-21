using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Numerics;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;

namespace ACE.Server.Physics.Util
{
    public class AdjustCell
    {
        public List<Common.EnvCell> EnvCells;
        public static ConcurrentDictionary<uint, AdjustCell> AdjustCells = new ConcurrentDictionary<uint, AdjustCell>();

        public AdjustCell(uint dungeonID)
        {
            uint blockInfoID = dungeonID << 16 | 0xFFFE;
            var blockinfo = DatManager.CellDat.ReadFromDat<LandblockInfo>(blockInfoID);
            var numCells = blockinfo.NumCells;

            BuildEnv(dungeonID, numCells);
        }

        public void BuildEnv(uint dungeonID, uint numCells)
        {
            EnvCells = new List<Common.EnvCell>();
            uint firstCellID = 0x100;
            for (uint i = 0; i < numCells; i++)
            {
                uint cellID = firstCellID + i;
                uint blockCell = dungeonID << 16 | cellID;

                var objCell = Common.LScape.get_landcell(blockCell);
                var envCell = objCell as Common.EnvCell;
                if (envCell != null)
                    EnvCells.Add(envCell);
            }
        }

        public uint? GetCell(Vector3 point)
        {
            foreach (var envCell in EnvCells)
                if (envCell.point_in_cell(point))
                    return envCell.ID;
            return null;
        }

        public static AdjustCell Get(uint dungeonID)
        {
            AdjustCell adjustCell = null;
            AdjustCells.TryGetValue(dungeonID, out adjustCell);
            if (adjustCell == null)
            {
                adjustCell = new AdjustCell(dungeonID);
                AdjustCells.TryAdd(dungeonID, adjustCell);
            }
            return adjustCell;
        }
    }
}
