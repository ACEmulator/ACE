using System.Collections.Generic;
using ACE.Server.Physics.Animation;

namespace ACE.Server.Physics.Common
{
    public class SortCell: ObjCell
    {
        public BuildingObj Building;

        public SortCell(): base()
        {
            Init();
        }

        public SortCell(uint cellID): base(cellID)
        {
            Init();
        }

        public override TransitionState FindCollisions(Transition transition)
        {
            if (Building != null)
                return Building.find_building_collisions(transition);
            else
                return TransitionState.OK;
        }

        public void add_building(BuildingObj building)
        {
            Building = building;
        }

        public override void find_transit_cells(Position position, int numSphere, List<Sphere> sphere, CellArray cellArray, SpherePath path)
        {
            if (Building != null)
                Building.find_building_transit_cells(position, numSphere, sphere, cellArray, path);
        }

        public override void find_transit_cells(int numParts, List<PhysicsPart> parts, CellArray cellArray)
        {
            if (Building != null)
                Building.find_building_transit_cells(numParts, parts, cellArray);
        }

        public PhysicsObj get_object(int objectID)
        {
            var obj = GetObject(objectID);

            if (obj == null && Building != null)
                obj = Building.get_object(objectID);

            return obj;
        }

        public bool has_building()
        {
            return Building != null;
        }

        public void remove_building()
        {
            Building = null;
        }

        public new void Init()
        {
            //Building = new BuildingObj();
        }
    }
}
