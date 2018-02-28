using System.Collections.Generic;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Combat;

namespace ACE.Server.Physics.Common
{
    public class ObjCell
    {
        public int ID;
        public WaterType WaterType;
        public Position Pos;
        public int NumObjects;
        public List<PhysicsObj> ObjectList;
        public int NumLights;
        public List<int> LightList;
        public int NumShadowObjects;
        public List<ShadowObj> ShadowObjectList;
        public int RestrictionObj;
        public List<int> ClipPlanes;
        public int NumStabs;
        public List<int> StabList;
        public bool SeenOutside;
        public HashSet<int> VoyeurTable;
        public int MyLandBlock;

        public void AddObject(PhysicsObj obj)
        {

        }

        public void AddPart(PhysicsPart part, List<int> clipPlanes, AFrame frame, int numParts)
        {
            // could go in PartCell
        }

        public void AddShadowObject(ShadowObj shadowObj)
        {
            ShadowObjectList.Add(shadowObj);
            NumShadowObjects++;     // can probably replace with .Count
        }

        public void CheckAttack(int attacker_id, Position attackerPos, float attacker_scale, AttackCone attackCone, AttackInfo attackInfo)
        {

        }

        public TransitionState FindCollisions(Transition transition)
        {
            return TransitionState.OK;
        }

        public static ObjCell GetVisible(int cellID)
        {
            return null;
        }

        public void RemoveObject(PhysicsObj obj)
        {

        }

        public void RemovePart(PhysicsPart part)
        {

        }

        public bool check_collisions()
        {
            return false;
        }

        public static void find_cell_list(Position pos, Sphere cylSphere, CellArray cellArray, SpherePath path)
        {

        }

        public static void find_cell_list(Position pos, int numCylSphere, CylSphere cylSphere, CellArray cellArray, SpherePath path)
        {

        }

        public static List<ObjCell> find_cell_list(Position pos, int numSphere, Sphere sphere, CellArray cellArray, SpherePath path)
        {
            return null;
        }

        public void find_transit_cells(int numParts, List<PhysicsPart> parts, CellArray cellArray)
        {

        }

        public void hide_object(PhysicsObj obj)
        {

        }

        public void remove_shadow_object(PhysicsObj obj)
        {

        }

        public void unhide_object(PhysicsObj obj)
        {

        }
    }
}
