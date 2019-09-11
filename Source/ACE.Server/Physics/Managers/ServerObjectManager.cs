using System;
using System.Collections.Concurrent;

namespace ACE.Server.Physics.Managers
{
    public static class ServerObjectManager
    {
        /// <summary>
        /// Custom lookup table of PhysicsObjs for the server
        /// </summary>
        public static ConcurrentDictionary<uint, PhysicsObj> ServerObjects { get; } = new ConcurrentDictionary<uint, PhysicsObj>();

        /// <summary>
        /// Adds a PhysicsObj to the static list of server-wide objects
        /// </summary>
        public static void AddServerObject(PhysicsObj obj)
        {
            if (obj != null)
                ServerObjects[obj.ID] = obj;
        }

        /// <summary>
        /// Removes a PhysicsObj from the static list of server-wide objects
        /// </summary>
        public static void RemoveServerObject(PhysicsObj obj)
        {
            if (obj != null)
                ServerObjects.TryRemove(obj.ID, out _);
        }

        /// <summary>
        /// Returns a PhysicsObj for an object ID
        /// </summary>
        public static PhysicsObj GetObjectA(uint objectID)
        {
            ServerObjects.TryGetValue(objectID, out var obj);

            return obj;
        }
    }
}
