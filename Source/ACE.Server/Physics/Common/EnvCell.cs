using System.Collections.Generic;
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
        public List<int> StaticObjectIDs;
        public List<AFrame> StaticObjectFrames;
        public List<PhysicsObj> StaticObjects;
        public List<ushort> LightArray;
        public int InCellTimestamp;
        public static Dictionary<uint, EnvCell> VisibleCellTable;
        public uint Bitfield;
        public uint EnvironmentID;

        static EnvCell()
        {
            VisibleCellTable = new Dictionary<uint, EnvCell>();
        }

        public EnvCell() : base()
        {
            Init();
        }

        public EnvCell(DatLoader.FileTypes.EnvCell envCell): base()
        {
            Bitfield = envCell.Bitfield;
            ShadowObjectIDs = envCell.Shadows;
            EnvironmentID = envCell.EnvironmentId;
            CellStructureID = envCell.CellStructure;
            Pos.Frame = new AFrame(envCell.Position);
            Portals = envCell.CellPortals;
            NumPortals = Portals.Count;
            LightArray = envCell.Lights;
            StabList = envCell.Stabs;
            NumStabs = StabList.Count;
            RestrictionObj = envCell.RestrictionObj;
            SeenOutside = envCell.SeenOutside;
        }

        public override TransitionState FindCollisions(Transition transition)
        {
            return FindObjCollisions(transition);
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

        public static new EnvCell GetVisible(uint cellID)
        {
            EnvCell envCell = null;
            VisibleCellTable.TryGetValue(cellID, out envCell);
            return envCell;
        }

        public new void Init()
        {
            CellStructure = new CellStruct();
            StaticObjectIDs = new List<int>();
            StaticObjectFrames = new List<AFrame>();
            StaticObjects = new List<PhysicsObj>();
            VisibleCellTable = new Dictionary<uint, EnvCell>();
        }

        public static EnvCell add_visible_cell(uint cellID)
        {
            var envCell = (EnvCell)DBObj.Get(new QualifiedDataID(3, cellID));
            VisibleCellTable.Add(cellID, envCell);
            return envCell;
        }

        public static void grab_visible(List<uint> stabs)
        {
            foreach (var stab in stabs)
                add_visible_cell(stab);
        }

        public override bool point_in_cell(Vector3 point)
        {
            var localPoint = Pos.Frame.GlobalToLocal(point);
            return CellStructure.point_in_cell(localPoint);
        }

        public static void release_visible(List<uint> stabs)
        {
            foreach (var stab in stabs)
                VisibleCellTable.Remove(stab);
        }
    }
}
