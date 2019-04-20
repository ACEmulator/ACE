using System.Collections.Generic;
using System.Numerics;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;

namespace ACE.Server.Physics.Util
{
    public class AdjustCell
    {
        public List<Environment> EnvCells;
        public List<Common.EnvCell> NewEnvCells;
        public static Dictionary<uint, AdjustCell> AdjustCells = new Dictionary<uint, AdjustCell>();

        public AdjustCell(uint dungeonID)
        {
            uint blockInfoID = dungeonID << 16 | 0xFFFE;
            var blockinfo = DatManager.CellDat.ReadFromDat<LandblockInfo>(blockInfoID);
            var numCells = blockinfo.NumCells;

            BuildEnv(dungeonID, numCells);
        }

        public void BuildEnv(uint dungeonID, uint numCells)
        {
            EnvCells = new List<Environment>();
            NewEnvCells = new List<Common.EnvCell>();
            uint firstCellID = 0x100;
            for (uint i = 0; i < numCells; i++)
            {
                uint cellID = firstCellID + i;
                uint blockCell = dungeonID << 16 | cellID;
                var cell = DatManager.CellDat.ReadFromDat<EnvCell>(blockCell);
                EnvCells.Add(new Environment(cell));

                var objCell = Common.LScape.get_landcell(blockCell);
                var envCell = objCell as Common.EnvCell;
                if (envCell != null)
                    NewEnvCells.Add(envCell);
            }
        }

        public uint? GetCell(Vector3 point)
        {
            /*foreach (var envCell in EnvCells)
                if (envCell.BBox.Contains(point))
                    return envCell.EnvCell.Id;
            return null;*/

            foreach (var envCell in NewEnvCells)
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
                AdjustCells.Add(dungeonID, adjustCell);
            }
            return adjustCell;
        }
    }
}
