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
            using (var result = DatabaseManager.World.SelectPreparedStatement(WorldPreparedStatement.TeleportLocationSelect))
            {
                for (uint i = 0u; i < result.Count; i++)
                {
                    teleportLocations.Add(result.Read<string>(0, "location"), new Position(result.Read<uint>(0, "cell"), result.Read<float>(0, "x"), result.Read<float>(0, "y"),
                        result.Read<float>(0, "z"), result.Read<float>(0, "qx"), result.Read<float>(0, "qy"), result.Read<float>(0, "qz"), result.Read<float>(0, "qw")));
                }
            }
        }

        public static Position GetTeleport(string location) { return teleportLocations.SingleOrDefault(t => t.Key == location).Value; }
    }
}
