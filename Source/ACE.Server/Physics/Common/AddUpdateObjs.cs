using System.Collections.Generic;

namespace ACE.Server.Physics.Common
{
    public class AddUpdateObjs
    {
        public List<PhysicsObj> AddObjects;
        public List<PhysicsObj> UpdateObjects;

        public AddUpdateObjs(List<PhysicsObj> addObjects, List<PhysicsObj> updateObjects)
        {
            AddObjects = addObjects;
            UpdateObjects = updateObjects;
        }
    }
}
