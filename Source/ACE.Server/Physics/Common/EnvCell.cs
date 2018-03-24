using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ACE.Server.Physics.Animation;

namespace ACE.Server.Physics.Common
{
    public class EnvCell: ObjCell
    {
        public int NumSurfaces;
        //public List<Surface> Surfaces;
        public CellStruct CellStructure;
        public uint CellStructureID;
        //public Environment Env;
        public int NumPortals;
        public List<DatLoader.Entity.CellPortal> Portals;
        public int NumStaticObjects;
        public List<uint> StaticObjectIDs;
        public List<AFrame> StaticObjectFrames;
        public List<PhysicsObj> StaticObjects;
        public List<ushort> LightArray;
        public int InCellTimestamp;
        public List<ushort> VisibleCellIDs;
        public Dictionary<uint, EnvCell> VisibleCells;
        public uint Bitfield;
        public uint EnvironmentID;

        public EnvCell() : base()
        {
            Init();
        }

        public EnvCell(DatLoader.FileTypes.EnvCell envCell): base()
        {
            Bitfield = envCell.Bitfield;
            ID = envCell.Id;
            ShadowObjectIDs = envCell.Shadows;
            EnvironmentID = envCell.EnvironmentId;
            CellStructureID = envCell.CellStructure;
            Pos.Frame = new AFrame(envCell.Position);
            Portals = envCell.CellPortals;
            NumPortals = Portals.Count;
            StaticObjectIDs = new List<uint>();
            StaticObjectFrames = new List<AFrame>();
            foreach (var stab in envCell.Stabs)
            {
                StaticObjectIDs.Add(stab.Id);
                StaticObjectFrames.Add(new AFrame(stab.Frame));
            }
            NumStabs = StaticObjectIDs.Count;
            VisibleCellIDs = envCell.VisibleCells;
            RestrictionObj = envCell.RestrictionObj;
            SeenOutside = envCell.SeenOutside;
        }

        public override TransitionState FindCollisions(Transition transition)
        {
            return FindObjCollisions(transition);
        }

        public void build_visible_cells()
        {
            VisibleCells = new Dictionary<uint, EnvCell>();

            foreach (var visibleCellID in VisibleCellIDs)
            {
                var blockCellID = ID & 0xFFFF0000 | visibleCellID;
                if (VisibleCells.ContainsKey(blockCellID)) continue;
                var cell = (EnvCell)LScape.get_landcell(blockCellID);
                VisibleCells.Add(blockCellID, cell);
            }
        }

        public ObjCell find_visible_child_cell(Vector3 origin, bool searchCells)
        {
            if (point_in_cell(origin))
                return this;

            // omitted portal search
            if (!searchCells) return null;

            foreach (var stab in StabList)
            {
                var envCell = GetVisible(stab.Id);
                if (envCell != null && envCell.point_in_cell(origin))
                    return envCell;
            }
            return null;
        }

        public new EnvCell GetVisible(uint cellID)
        {
            EnvCell envCell = null;
            VisibleCells.TryGetValue(cellID, out envCell);
            return envCell;
        }

        public new void Init()
        {
            CellStructure = new CellStruct();
            StaticObjectIDs = new List<uint>();
            StaticObjectFrames = new List<AFrame>();
            StaticObjects = new List<PhysicsObj>();
            VisibleCells = new Dictionary<uint, EnvCell>();
        }

        public EnvCell add_visible_cell(uint cellID)
        {
            var envCell = (EnvCell)DBObj.Get(new QualifiedDataID(3, cellID));
            VisibleCells.Add(cellID, envCell);
            return envCell;
        }

        public static ObjCell get_visible(uint cellID)
        {
            var cell = (EnvCell)LScape.get_landcell(cellID);
            return cell.VisibleCells.Values.First();
        }

        public void grab_visible(List<uint> stabs)
        {
            foreach (var stab in stabs)
                add_visible_cell(stab);
        }

        public override bool point_in_cell(Vector3 point)
        {
            var localPoint = Pos.Frame.GlobalToLocal(point);
            //return CellStructure.point_in_cell(localPoint);   // add cellstruct ref
            return false;
        }

        public void release_visible(List<uint> stabs)
        {
            foreach (var stab in stabs)
                VisibleCells.Remove(stab);
        }
    }
}
