using System;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    /// <summary>
    /// This is a light weight object that holds a weak reference to a WorldObject.<para />
    /// In addition, it also caches values from that WorldObject incase the reference is disposed.
    /// This way, we can still access values that we may have needed from the WorldObject after it is gone.
    /// </summary>
    public class WorldObjectInfo
    {
        public readonly WeakReference<WorldObject> WorldObjectRef;

        public readonly ObjectGuid Guid;

        public readonly string Name;

        public readonly uint WeenieClassId;
        public readonly string WeenieClassName;
        public readonly WeenieType WeenieType;

        public WorldObjectInfo(WorldObject worldObject)
        {
            WorldObjectRef = new WeakReference<WorldObject>(worldObject);

            Guid = worldObject.Guid;

            Name = worldObject.Name;

            WeenieClassId = worldObject.WeenieClassId;
            WeenieClassName = worldObject.WeenieClassName;
            WeenieType = worldObject.WeenieType;
        }
    }
}
