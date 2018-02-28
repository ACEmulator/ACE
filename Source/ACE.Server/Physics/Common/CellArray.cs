using System.Collections.Generic;
using ACE.Server.Physics.Animation;

namespace ACE.Server.Physics.Common
{
    public class CellArray
    {
        public bool AddedOutside;
        public bool LoadCells;
        public List<ObjCell> Cells;
        public int NumCells;

        public void SetStatic()
        {
            AddedOutside = false;
            LoadCells = false;
            NumCells = 0;
        }

        public void SetDynamic()
        {
            AddedOutside = false;
            LoadCells = true;
            NumCells = 0;
        }

        public void add_cell(int cellID, ObjCell cell)
        {

        }

        public List<ObjCell> find_cell_list(SpherePath path)
        {
            return ObjCell.find_cell_list(path.CheckPos, path.NumSphere, path.GlobalSphere[0], this, path);
        }
    }
}
