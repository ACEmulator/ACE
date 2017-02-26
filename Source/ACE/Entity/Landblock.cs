using ACE.Entity.Events;
using System;
using System.Collections.Generic;
using System.Linq;

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
    internal class Landblock : IDisposable
    {
        private object objectCacheLocker = new object();
        private Dictionary<ObjectGuid, WorldObject> worldObjects = new Dictionary<ObjectGuid, WorldObject>();
        private List<Landblock> adjacencies;
        private const byte cellGridMaxX = 8;
        private const byte cellGridMaxY = 8;

        // not sure if a full object is necessary here.  I don't think a Landcell has any
        // inherent functionality that needs to be modelled in an object.
        private Landcell[,] cellGrid = new Landcell[cellGridMaxX, cellGridMaxY];

        public Landblock()
        {
            // this may not even be necessary, but we'll see
            for (int x = 0; x < cellGridMaxX; x++)
                for (int y = 0; y < cellGridMaxY; y++)
                    cellGrid[x, y] = new Landcell();

            // TODO: Load cell.dat contents
            // TODO: Load portal.dat contents
            // TODO: Load spawn data

        }

        public void SetAdjacencies(List<Landblock> adjacencies)
        {
            this.adjacencies = adjacencies;
        }

        public void AddWorldObject(WorldObject wo)
        {
            lock (objectCacheLocker)
            {
                this.worldObjects.Add(wo.Guid, wo);
            }

            if (wo is MutableWorldObject)
            {
                // players / monsters / doors / projectiles / etc
                (wo as MutableWorldObject).OnMove += MutableObject_Moved;
            }

            if (wo is IChatSender)
            { 
                (wo as IChatSender).OnChat += MutableObject_Chat;
            }
        }

        private void MutableObject_Chat(object sender, ChatMessageArgs e)
        {
            if (!(sender is MutableWorldObject))
            {
                throw new ArgumentException($"sender is not a Mutable Object, but was tied to the MutableObject_Chat handler.");
            }
        }

        private void MutableObject_Moved(object sender, EventArgs e)
        {
            if (!(sender is MutableWorldObject))
            {
                throw new ArgumentException($"sender is not a Mutable Object, but was tied to the MutableObject_Moved handler.");
            }
        }

        /// <summary>
        /// called when a landblock is to be unloaded
        /// </summary>
        public void Dispose()
        {
            // save all mutable objects, release memory
        }

        public void RemoveWorldObject(ObjectGuid objectId)
        {
            WorldObject wo = null;

            lock(objectCacheLocker)
            {
                if (this.worldObjects.ContainsKey(objectId))
                {
                    wo = this.worldObjects[objectId];
                    this.worldObjects.Remove(objectId);
                }
            }

            if (wo == null)
                return;

            if (wo is MutableWorldObject)
            {
                // unregister event handlers
                (wo as MutableWorldObject).OnMove -= MutableObject_Moved;
            }

            if (wo is IChatSender)
            {
                (wo as IChatSender).OnChat -= MutableObject_Chat;
            }

        }

        /// <summary>
        /// main game loop
        /// </summary>
        public void UseTime()
        {
            // here we'd move server objects in motion (subject to landscape) and do physics collision detection
        }

        public int GetPlayerCount()
        {
            // copy the list for thread safety of iteration
            var objectListCopy = this.worldObjects.Values.ToList();

            return objectListCopy.OfType<Player>().Count();
        }

        /// <summary>
        /// fired when a MutableObject leaves the landblock.  raising the event will let the landblock manager
        /// figure out where it it should go.
        /// </summary>
        public event EventHandler OnMutableObjectLeave;
    }
}
