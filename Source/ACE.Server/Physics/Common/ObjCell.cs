using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Combat;

namespace ACE.Server.Physics.Common
{
    public class ObjCell: PartCell
    {
        public static ObjectMaint ObjMaint;

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
        public List<int> VoyeurTable;
        public LandblockStruct CurLandblock;

        public void AddObject(PhysicsObj obj)
        {
            ObjectList.Add(obj);
            NumObjects++;
            if (obj.ID == 0 || obj.Parent != null || obj.State.HasFlag(PhysicsState.Hidden) || VoyeurTable == null)
                return;

            foreach (var voyeur_id in VoyeurTable)
            {
                if (voyeur_id != obj.ID && voyeur_id != 0)
                {
                    var voyeur = obj.GetObjectA(voyeur_id);
                    if (voyeur == null) continue;

                    var info = new DetectionInfo(obj.ID, DetectionType.EnteredDetection);
                    voyeur.receive_detection_update(info);
                }
            }
        }

        public void AddShadowObject(ShadowObj shadowObj)
        {
            ShadowObjectList.Add(shadowObj);
            NumShadowObjects++;     // can probably replace with .Count
            shadowObj.Cell = this;
        }

        public void CheckAttack(int attackerID, Position attackerPos, float attackerScale, AttackCone attackCone, AttackInfo attackInfo)
        {
            foreach (var shadowObj in ShadowObjectList)
            {
                var pObj = shadowObj.PhysicsObj;
                if (pObj.ID == attackerID || pObj.State.HasFlag(PhysicsState.Static)) continue;

                var hitLocation = pObj.check_attack(attackerPos, attackerScale, attackCone, attackInfo.AttackRadius);
                if (hitLocation != 0)
                    attackInfo.AddObject(pObj.ID, hitLocation);
            }
        }

        public TransitionState FindCollisions(Transition transition)
        {
            return TransitionState.Invalid; // ???
        }

        public TransitionState FindEnvCollisions(Transition transition)
        {
            return TransitionState.Invalid; // ???
        }

        public TransitionState FindObjCollisions(Transition transition)
        {
            var path = transition.SpherePath;

            if (path.InsertType == InsertType.InitialPlacement)
                return TransitionState.OK;

            foreach (var shadowObj in ShadowObjectList)
            {
                var pObj = shadowObj.PhysicsObj;

                if (pObj.Parent != null || pObj == transition.ObjectInfo.Object)
                    continue;

                var state = pObj.FindObjCollisions(transition);
                if (state != TransitionState.OK)
                    return state;
            }
            return TransitionState.OK;
        }

        public PhysicsObj GetObject(int id)
        {
            foreach (var obj in ObjectList)
            {
                if (obj != null && obj.ID == id)
                    return obj;
            }
            return null;
        }

        public static ObjCell GetVisible(int cellID)
        {
            if (cellID == 0) return null;

            if ((cellID & 0xFFFF)>= 0x100)
                return EnvCell.GetVisible(cellID);
            else
                return LandCell.Get(cellID);
        }

        public void ReleaseObjects()
        {
            while (NumShadowObjects > 0)
            {
                var shadowObj = ShadowObjectList[0];
                remove_shadow_object(shadowObj);

                shadowObj.PhysicsObj.remove_parts(this);
            }
            if (NumObjects > 0 && ObjMaint != null)
                ObjMaint.ReleaseObjCell(this);
        }

        public void RemoveObject(PhysicsObj obj)
        {
            // multiple objects?
            ObjectList.Remove(obj);
            NumObjects--;
            update_all_voyeur(obj, DetectionType.LeftDetection);
        }

        public bool check_collisions(PhysicsObj obj)
        {
            foreach (var shadowObj in ShadowObjectList)
            {
                var pObj = shadowObj.PhysicsObj;
                if (pObj.Parent == null && !pObj.Equals(obj) && pObj.check_collision(obj))
                    return true;
            }
            return false;
        }

        public static ObjCell find_cell_list(Position position, int numSphere, List<Sphere> sphere, CellArray cellArray, ObjCell currCell, SpherePath path)
        {
            cellArray.NumCells = 0;
            cellArray.AddedOutside = false;

            var cell = GetVisible(position.ObjCellID);

            if ((position.ObjCellID & 0xFFFF) >= 0x100)
            {
                if (path != null)
                    path.HitsInteriorCell = true;

                cellArray.add_cell(position.ObjCellID, cell);
            }
            else
                LandCell.add_all_outside_cells(position, numSphere, sphere, cellArray);

            if (cell != null && numSphere != 0)
            {
                foreach (var otherCell in cellArray.Cells)
                {
                    if (otherCell != null)
                        otherCell.find_transit_cells(position, numSphere, sphere, cellArray, path);
                }
                if (currCell != null)
                {
                    foreach (var otherCell in cellArray.Cells)
                    {
                        var blockOffset = LandDefs.GetBlockOffset(position.ObjCellID, otherCell.ID);
                        var localPoint = sphere[0].Center - blockOffset;

                        if (otherCell.point_in_cell(localPoint) && (otherCell.ID & 0xFFFF) >= 0x100)
                        {
                            if (path != null) path.HitsInteriorCell = true;
                            return otherCell;
                        }
                    }
                }
            }
            if (!cellArray.LoadCells && (position.ObjCellID & 0xFFFF) >= 0x100)
            {
                foreach (var otherCell in cellArray.Cells)
                {
                    if (cell.ID != otherCell.ID)
                        continue;

                    var found = false;
                    foreach (var stab in cell.StabList)
                    {
                        if (otherCell.ID == stab)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        cellArray.remove_cell(otherCell);
                }
            }
            return null;
        }

        public static void find_cell_list(Position position, int numCylSphere, List<CylSphere> cylSphere, CellArray cellArray, SpherePath path)
        {
            if (numCylSphere > 10)
                numCylSphere = 10;

            var spheres = new List<Sphere>();

            for (var i = 0; i < numCylSphere; i++)
            {
                var sphere = new Sphere();
                sphere.Center = position.LocalToGlobal(cylSphere[i].LowPoint);
                sphere.Radius = cylSphere[i].Radius;
                spheres.Add(sphere);
            }

            find_cell_list(position, numCylSphere, spheres, cellArray, null, path);
        }

        public static void find_cell_list(Position position, Sphere sphere, CellArray cellArray, SpherePath path)
        {
            var globalSphere = new Sphere();
            globalSphere.Center = position.LocalToGlobal(sphere.Center);
            globalSphere.Radius = sphere.Radius;

            find_cell_list(position, 1, globalSphere, cellArray, null, path);
        }

        public static ObjCell find_cell_list(CellArray cellArray, ObjCell checkCell, SpherePath path)
        {
            return find_cell_list(path.CheckPos, path.NumSphere, path.GlobalSphere, cellArray, checkCell, path);
        }

        public static ObjCell find_cell_list(Position position, int numSphere, Sphere sphere, CellArray cellArray, ObjCell currCell, SpherePath path)
        {
            return find_cell_list(position, numSphere, new List<Sphere>() { sphere }, cellArray, currCell, path);
        }

        public virtual void find_transit_cells(int numParts, List<PhysicsPart> parts, CellArray cellArray)
        {
            // override?
        }

        public virtual void find_transit_cells(Position position, int numSphere, List<Sphere> sphere, CellArray cellArray, SpherePath path)
        {
            // override?
        }

        public WaterType get_block_water_type()
        {
            if (CurLandblock != null)
                return CurLandblock.WaterType;
            else
                return WaterType.NotWater;
        }

        public float get_water_depth(Vector3 point)
        {
            if (WaterType == WaterType.NotWater)
                return 0.0f;

            if (WaterType == WaterType.EntirelyWater)
                return 0.89999998f;

            if (CurLandblock != null)
                return CurLandblock.calc_water_depth(ID, point);
            else
                return 0.1f;
        }

        public void hide_object(PhysicsObj obj)
        {
            update_all_voyeur(obj, DetectionType.LeftDetection);
        }

        public virtual bool point_in_cell(Vector3 point)
        {
            return false;   // override?
        }

        public void remove_shadow_object(ShadowObj shadowObj)
        {
            // multiple shadows?
            ShadowObjectList.Remove(shadowObj);
            shadowObj.Cell = null;
        }

        public void unhide_object(PhysicsObj obj)
        {
            update_all_voyeur(obj, DetectionType.EnteredDetection, false);
        }

        public void update_all_voyeur(PhysicsObj obj, DetectionType type, bool checkDetection = true)
        {
            if (obj.ID == 0 || obj.Parent != null || VoyeurTable == null)
                return;

            if (obj.State.HasFlag(PhysicsState.Hidden) && (checkDetection ? type == DetectionType.EnteredDetection : true))
                return;

            foreach (var voyeur_id in VoyeurTable)
            {
                if (voyeur_id != obj.ID && voyeur_id != 0)
                {
                    var voyeur = obj.GetObjectA(voyeur_id);
                    if (voyeur == null) continue;

                    var info = new DetectionInfo(obj.ID, type);
                    voyeur.receive_detection_update(info);
                }
            }
        }
    }
}
