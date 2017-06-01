using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Database;
using ACE.Entity;

namespace ACE.Managers
{
    public static class AssetManager
    {
        private static Dictionary<string, Position> teleportLocations = new Dictionary<string, Position>();

        public static void Initialize()
        {
            var locations = DatabaseManager.World.GetPointsOfInterest();
            foreach (var loc in locations)
                teleportLocations.Add(loc.Location, loc.Position);
        }

        public static Position GetTeleport(string location) { return teleportLocations.SingleOrDefault(t => string.Equals(t.Key, location, StringComparison.OrdinalIgnoreCase)).Value; }
    }
}
