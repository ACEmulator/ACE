using System;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    /// <summary>
    /// This is a light weight object that holds a weak reference to a WorldObject.<para />
    /// In addition, it also caches values from that WorldObject incase the reference is released.
    /// This way, we can still access values that we may have needed from the WorldObject after it is gone.
    /// </summary>
    public class WorldObjectInfo<T> : WorldObjectInfo
    {
        public T Value;

        public WorldObjectInfo(WorldObject worldObject, T value) : base(worldObject)
        {
            Value = value;
        }

        public WorldObjectInfo(WorldObject worldObject) : base(worldObject)
        {
        }
    }

    /// <summary>
    /// This is a light weight object that holds a weak reference to a WorldObject.<para />
    /// In addition, it also caches values from that WorldObject incase the reference is released.
    /// This way, we can still access values that we may have needed from the WorldObject after it is gone.
    /// </summary>
    public class WorldObjectInfo
    {
        public readonly WeakReference<WorldObject> WorldObjectRef;

        public readonly ObjectGuid Guid;

        public readonly string Name;

        public readonly uint WeenieClassId;
        public readonly WeenieType WeenieType;

        public WorldObjectInfo(WorldObject worldObject)
        {
            WorldObjectRef = new WeakReference<WorldObject>(worldObject);

            Guid = worldObject.Guid;

            Name = worldObject.Name;

            WeenieClassId = worldObject.WeenieClassId;
            WeenieType = worldObject.WeenieType;
        }


        /// <summary>
        /// If you use this function, you should cache and re-use the result in a local code space.
        /// You should not hold on to the result, nor should you use this function too aggressively.
        /// </summary>
        public WorldObject TryGetWorldObject()
        {
            WorldObjectRef.TryGetTarget(out var worldObject);

            return worldObject;
        }
    }
}
