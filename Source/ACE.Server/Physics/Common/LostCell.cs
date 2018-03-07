using System.Collections.Generic;

namespace ACE.Server.Physics.Common
{
    public class LostCell
    {
        public int NumObjects;
        public List<PhysicsObj> Objects;

        public LostCell()
        {
            Objects = new List<PhysicsObj>();
        }

        public void Clear()
        {
            Objects.Clear();
            NumObjects = 0;
        }

        public void remove_object(PhysicsObj obj)
        {
            Objects.Remove(obj);
            NumObjects = Objects.Count;
        }
    }
}
