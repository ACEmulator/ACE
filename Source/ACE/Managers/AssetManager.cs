using System;
using ACE.Database;
using System.Collections.Generic;
using System.Linq;

namespace ACE.Managers
{
    public static class AssetManager
    {
        private static Dictionary<string, Position> teleportLocations = new Dictionary<string, Position>();

        public static void Initialise()
        {
            var locations = DatabaseManager.World.GetLocations();
            foreach(var loc in locations)
                teleportLocations.Add(loc.Location, loc.Position);
        }

        public static Position GetTeleport(string location) { return teleportLocations.SingleOrDefault(t => string.Equals(t.Key, location, StringComparison.OrdinalIgnoreCase)).Value; }
    }
}
