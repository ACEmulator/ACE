using System.Collections.Generic;

namespace ACE.Server.Physics.Common
{
    public class ObjectMaint
    {
        public bool IsActive;
        public Dictionary<uint, LostCell> LostCellTable;
        public Dictionary<uint, PhysicsObj> ObjectTable;
        public List<PhysicsObj> NullObjectTable;
        public Dictionary<uint, WeenieObject> WeenieObjectTable;
        public List<WeenieObject> NullWeenieObjectTable;
        public List<int> VisibileObjectTable;
        public Dictionary<uint, double> DestructionObjectTable;
        public Dictionary<uint, int> ObjectInventoryTable;
        public Queue<double> ObjectDestructionQueue;

        public ObjectMaint()
        {
            LostCellTable = new Dictionary<uint, LostCell>();
            ObjectTable = new Dictionary<uint, PhysicsObj>();
            NullObjectTable = new List<PhysicsObj>();
            WeenieObjectTable = new Dictionary<uint, WeenieObject>();
            NullWeenieObjectTable = new List<WeenieObject>();
            DestructionObjectTable = new Dictionary<uint, double>();
            ObjectInventoryTable = new Dictionary<uint, int>();
            ObjectDestructionQueue = new Queue<double>();
        }

        public void AddObject(PhysicsObj obj)
        {
            if (!ObjectTable.ContainsKey(obj.ID))
                ObjectTable.Add(obj.ID, obj);
            else
                ObjectTable[obj.ID] = obj;
        }

        public void AddObjectToBeDestroyed(uint objectID)
        {
            var time = Timer.CurrentTime + 25.0f;
            if (!DestructionObjectTable.ContainsKey(objectID))
                DestructionObjectTable.Add(objectID, time);
            else
                DestructionObjectTable[objectID] = time;

            ObjectDestructionQueue.Enqueue(time);
        }

        public void AddWeenieObject(WeenieObject wobj)
        {
            if (!WeenieObjectTable.ContainsKey(wobj.ID))
                WeenieObjectTable.Add(wobj.ID, wobj);
            else
                WeenieObjectTable[wobj.ID] = wobj;
        }

        public LostCell GetLostCell(uint cellID)
        {
            LostCell lostCell = null;
            LostCellTable.TryGetValue(cellID, out lostCell);
            return lostCell;
        }

        public PhysicsObj GetObjectA(uint objectID)
        {
            PhysicsObj obj = null;
            ObjectTable.TryGetValue(objectID, out obj);
            return obj;
        }

        public bool GetObjectA(uint objectID, ref PhysicsObj obj, ref WeenieObject wobj)
        {
            obj = GetObjectA(objectID);
            wobj = GetWeenieObject(objectID);
            return (obj != null || wobj != null);
        }

        public int GetObjectInventory(uint objectID)
        {
            var inventory = 0;
            ObjectInventoryTable.TryGetValue(objectID, out inventory);
            return inventory;
        }

        public WeenieObject GetWeenieObject(uint objectID)
        {
            WeenieObject wobj = null;
            WeenieObjectTable.TryGetValue(objectID, out wobj);
            return wobj;
        }

        public void GotoLostCell(PhysicsObj obj, uint cellID)
        {
            if (obj.Parent != null) return;
            obj.set_cell_id(cellID);
            var lostCell = GetLostCell(obj.Position.ObjCellID);
            if (lostCell == null) return;
            lostCell.Objects.Add(obj);
            lostCell.NumObjects++;
        }

        public void InitObjCell(ObjCell cell)
        {
            var lostCell = GetLostCell(cell.ID);
            if (lostCell == null) return;
            foreach (var obj in lostCell.Objects)
                obj.reenter_visibility();
            lostCell.Clear();   // remove from list?
        }

        public void ReleaseObjCell(ObjCell objCell)
        {
            var removeList = new List<PhysicsObj>();

            foreach (var obj in objCell.ObjectList)
            {
                if (!obj.State.HasFlag(PhysicsState.Static) && obj.Parent == null)
                    removeList.Add(obj);
            }
            foreach (var obj in removeList)
            {
                objCell.ObjectList.Remove(obj);
                obj.leave_visibility();
            }
            objCell.NumObjects = objCell.ObjectList.Count;
        }

        public void RemoveFromLostCell(PhysicsObj obj)
        {
            if (obj.CurCell != null || obj.Parent != null) return;
            var lostCell = GetLostCell(obj.Position.ObjCellID);
            if (lostCell != null)
                lostCell.remove_object(obj);
        }

        public void RemoveObjectToBeDestroyed(uint objectID)
        {
            if (DestructionObjectTable.ContainsKey(objectID))
                DestructionObjectTable.Remove(objectID);
        }
    }
}
