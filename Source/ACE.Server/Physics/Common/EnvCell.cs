using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.Animation;

namespace ACE.Server.Physics.Common
{
    public class EnvCell: ObjCell
    {
        public int NumSurfaces;
        //public List<Surface> Surfaces;
        public CellStruct Structure;
        //public Environment Env;
        public int NumPortals;
        //public List<CellPortal> Portals;
        public int NumStaticObjects;
        public List<int> StaticObjectIDs;
        public List<AFrame> StaticObjectFrames;
        public List<PhysicsObj> StaticObjects;
        public List<int> LightArray;
        public int InCellTimestamp;
        public static Dictionary<int, EnvCell> VisibleCellTable;

        static EnvCell()
        {
            VisibleCellTable = new Dictionary<int, EnvCell>();
        }

        public ObjCell find_visible_child_cell(Vector3 origin, bool searchCells)
        {
            if (point_in_cell(origin))
                return this;

            // omitted portal search
            if (!searchCells) return null;

            foreach (var cellID in StabList)
            {
                var envCell = (EnvCell)GetVisible(cellID);
                if (envCell != null && envCell.point_in_cell(origin))
                    return envCell;
            }
            return null;
        }

        public static new EnvCell GetVisible(int cellID)
        {
            EnvCell envCell = null;
            VisibleCellTable.TryGetValue(cellID, out envCell);
            return envCell;
        }

        public override bool point_in_cell(Vector3 point)
        {
            var localPoint = Pos.Frame.GlobalToLocal(point);
            return Structure.point_in_cell(localPoint);
        }
    }
}
