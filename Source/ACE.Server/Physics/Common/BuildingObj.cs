using System.Collections.Generic;
using ACE.DatLoader.Entity;
using ACE.Server.Physics.BSP;
using ACE.Server.Physics.Animation;

namespace ACE.Server.Physics.Common
{
    public class BuildingObj: PhysicsObj
    {
        public List<CBldPortal> Portals;
        public List<PartCell> LeafCells;
        public List<ShadowPart> ShadowList;

        public BuildingObj() : base()
        {
            Portals = new List<CBldPortal>();
            LeafCells = new List<PartCell>();
            ShadowList = new List<ShadowPart>();
        }

        public void add_to_cell(SortCell newCell)
        {
            newCell.add_building(this);
            set_cell_id(newCell.ID);
            CurCell = newCell;
        }

        public void add_to_stablist(List<uint> blockStabList, ref int maxSize, ref int stabNum)
        {
            //foreach (var portal in Portals)
                //portal.add_to_stablist(blockStabList, maxSize, stabNum);
        }

        public TransitionState find_building_collisions(Transition transition)
        {
            if (PartArray == null)
                return TransitionState.OK;

            transition.SpherePath.BuildingCheck = true;
            var transitionState = PartArray.Parts[0].FindObjCollisions(transition);
            transition.SpherePath.BuildingCheck = false;

            if (transitionState != TransitionState.OK && !transition.ObjectInfo.State.HasFlag(ObjectInfoState.Contact))
                transition.CollisionInfo.CollidedWithEnvironment = true;

            return transitionState;
        }

        public void find_building_transit_cells(Position position, int numSphere, List<Sphere> sphere, CellArray cellArray, SpherePath path)
        {
            /*foreach (var portal in Portals)
            {
                var otherCell = portal.GetOtherCell();
                if (otherCell != null)
                    otherCell.check_building_transit(portal.OtherPortalID, numSphere, sphere, cellArray, path);
            }*/
        }

        public void find_building_transit_cells(int numParts, List<PhysicsPart> parts, CellArray cellArray)
        {
            /*foreach (var portal in Portals)
            {
                var otherCell = portal.GetOtherCell();
                if (otherCell != null)
                    otherCell.check_building_transit(portal.OtherPortalID, numParts, parts, cellArray);
            }*/
        }

        public PhysicsObj get_object(int objectID)
        {
            // visited cells?
            return null;
        }

        public static BuildingObj makeBuilding(uint buildingID, List<CBldPortal> portals, uint numLeaves)
        {
            // todo: initobj begin/end
            var buildingObj = new BuildingObj();
            buildingObj.ID = buildingID;
            buildingObj.Portals = portals;
            //buildingObj.NumLeaves = numLeaves;
            return buildingObj;
        }

        public void remove()
        {
            var sortCell = (SortCell)CurCell;
            sortCell.remove_building();
            set_cell_id(0);
            CurCell = null;
        }
    }
}
