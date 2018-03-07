using System.Collections.Generic;

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
            Cells.Add(cell);
            NumCells++;
        }

        public void remove_cell(ObjCell cell)
        {
            Cells.Remove(cell);
            NumCells = Cells.Count;
        }
    }
}
