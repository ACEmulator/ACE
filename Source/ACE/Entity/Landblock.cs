using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    /// <summary>
    /// Rule #1: ACE Landblock != Real AC Landblock.  Grok that now.
    /// As of implementation time, the entire world is going to function as
    /// a single landblock.  We will carve this off later.
    /// 
    /// the gist of a landblock is that, generally, everything on it publishes
    /// to and subscribes to everything else in the landblock.
    /// </summary>
    internal class Landblock
    {
        private Dictionary<ObjectGuid, WorldObject> worldObjects = new Dictionary<ObjectGuid, WorldObject>();

        public Landblock()
        {

        }

        public void AddWorldObject(WorldObject wo)
        {
            this.worldObjects.Add(wo.Guid, wo);
        }

        public void RemoveWorldObject(ObjectGuid objectId)
        {
            if (this.worldObjects.ContainsKey(objectId))
                this.worldObjects.Remove(objectId);
        }


    }
}
