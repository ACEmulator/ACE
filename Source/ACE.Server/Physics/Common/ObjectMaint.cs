using System.Collections.Generic;

namespace ACE.Server.Physics.Common
{
    public class ObjectMaint
    {
        public bool IsActive;
        public Dictionary<int, LostCell> LostCellTable;
        public Dictionary<int, PhysicsObj> ObjectTable;
        public List<PhysicsObj> NullObjectTable;
        public Dictionary<int, WeenieObject> WeenieObjectTable;
        public List<WeenieObject> NullWeenieObjectTable;
        public List<int> VisibileObjectTable;
        public Dictionary<int, double> DestructionObjectTable;
        public Dictionary<int, int> ObjectInventoryTable;
        public Queue<double> ObjectDestructionQueue;

        public ObjectMaint()
        {
            LostCellTable = new Dictionary<int, LostCell>();
            ObjectTable = new Dictionary<int, PhysicsObj>();
            NullObjectTable = new List<PhysicsObj>();
            WeenieObjectTable = new Dictionary<int, WeenieObject>();
            NullWeenieObjectTable = new List<WeenieObject>();
            DestructionObjectTable = new Dictionary<int, double>();
            ObjectInventoryTable = new Dictionary<int, int>();
            ObjectDestructionQueue = new Queue<double>();
        }

        public void AddObject(PhysicsObj obj)
        {
            if (!ObjectTable.ContainsKey(obj.ID))
                ObjectTable.Add(obj.ID, obj);
            else
                ObjectTable[obj.ID] = obj;
        }

        public void AddObjectToBeDestroyed(int objectID)
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

        public LostCell GetLostCell(int cellID)
        {
            LostCell lostCell = null;
            LostCellTable.TryGetValue(cellID, out lostCell);
            return lostCell;
        }

        public PhysicsObj GetObjectA(int objectID)
        {
            PhysicsObj obj = null;
            ObjectTable.TryGetValue(objectID, out obj);
            return obj;
        }

        public bool GetObjectA(int objectID, ref PhysicsObj obj, ref WeenieObject wobj)
        {
            obj = GetObjectA(objectID);
            wobj = GetWeenieObject(objectID);
            return (obj != null || wobj != null);
        }

        public int GetObjectInventory(int objectID)
        {
            var inventory = 0;
            ObjectInventoryTable.TryGetValue(objectID, out inventory);
            return inventory;
        }

        public WeenieObject GetWeenieObject(int objectID)
        {
            WeenieObject wobj = null;
            WeenieObjectTable.TryGetValue(objectID, out wobj);
            return wobj;
        }

        public void GotoLostCell(PhysicsObj obj, int cellID)
        {
            if (obj.Parent != null) return;
            obj.set_cell_id(cellID);
            var lostCell = GetLostCell(obj.Position.ObjCellID);
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
            if (obj.Cell != null || obj.Parent != null) return;
            var lostCell = GetLostCell(obj.Position.ObjCellID);
            if (lostCell != null)
                lostCell.remove_object(obj);
        }

        public void RemoveObjectToBeDestroyed(int objectID)
        {
            if (DestructionObjectTable.ContainsKey(objectID))
                DestructionObjectTable.Remove(objectID);
        }
    }
}
