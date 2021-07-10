using System;
using System.Collections.Generic;

using log4net;

namespace ACE.Server
{
    static class LandblockDebugger
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly List<WeakReference<Entity.Landblock>> entityLandblocks = new List<WeakReference<Entity.Landblock>>();
        private static readonly List<WeakReference<Physics.Common.Landblock>> physicsLandblocks = new List<WeakReference<Physics.Common.Landblock>>();

        public static void OnCtor(ACE.Server.Entity.Landblock landblock)
        {
            lock (entityLandblocks)
                entityLandblocks.Add(new WeakReference<Entity.Landblock>(landblock));
        }

        public static void OnCtor(ACE.Server.Physics.Common.Landblock landblock)
        {
            lock (physicsLandblocks)
                physicsLandblocks.Add(new WeakReference<Physics.Common.Landblock>(landblock));
        }

        public static void Audit()
        {
            var tempEntityLandblocks = new List<Entity.Landblock>();
            var tempPhysicsLandblocks = new List<Physics.Common.Landblock>();

            // First clean up the list and get the hard references
            lock (entityLandblocks)
            {
                for (int i = entityLandblocks.Count - 1; i >= 0; i--)
                {
                    if (entityLandblocks[i].TryGetTarget(out var target))
                        tempEntityLandblocks.Add(target);
                    else
                        entityLandblocks.RemoveAt(i);
                }
            }

            lock (physicsLandblocks)
            {
                for (int i = physicsLandblocks.Count - 1; i >= 0; i--)
                {
                    if (physicsLandblocks[i].TryGetTarget(out var target))
                        tempPhysicsLandblocks.Add(target);
                    else
                        physicsLandblocks.RemoveAt(i);
                }
            }

            // Remove physics landblocks that have a matching entity landblock
            for (int i = tempPhysicsLandblocks.Count - 1; i >= 0; i--)
            {
                foreach (var entityLandblock in tempEntityLandblocks)
                {
                    if (entityLandblock.PhysicsLandblock == tempPhysicsLandblocks[i])
                    {
                        tempPhysicsLandblocks.RemoveAt(i);
                        break;
                    }
                }
            }

            // What remains is out of sync physics landblocks

            Dictionary<uint, int> hitsByID = new Dictionary<uint, int>();

            foreach (var landblock in tempPhysicsLandblocks)
            {
                if (hitsByID.ContainsKey(landblock.ID))
                    hitsByID[landblock.ID]++;
                else
                    hitsByID[landblock.ID] = 1;
            }

            foreach (var kvp in hitsByID)
                log.Error($"[LANDBLOCK DEBUG] Physics landblock {kvp.Key:X8} has {kvp.Value} out of sync instances!");

            // Find server objects that still reference these out of sync physics landblocks

            foreach (var kvp in Physics.Managers.ServerObjectManager.ServerObjects)
            {
                if (tempPhysicsLandblocks.Contains(kvp.Value.CurLandblock))
                    log.Error($"[LANDBLOCK DEBUG] Physics object {kvp.Value.ID:X8}:{kvp.Value.Name} at position {kvp.Value.Position} holds on to out of sync landblock {kvp.Value.CurLandblock.ID:X8}!");
            }
        }
    }
}
