using ACE.Common;

namespace ACE.Database
{
    public class DatabaseManager
    {
        public static IAuthenticationDatabase Authentication { get; set; }

        public static IShardDatabase Shard { get; set; }

        public static IWorldDatabase World { get; set; }

        public static void Initialize()
        {
            var config = ConfigManager.Config.MySql;

            var authDb = new AuthenticationDatabase();
            authDb.Initialize(config.Authentication.Host, config.Authentication.Port, config.Authentication.Username, config.Authentication.Password, config.Authentication.Database);
            Authentication = authDb;

            var shardDb = new ShardDatabase();
            shardDb.Initialize(config.Shard.Host, config.Shard.Port, config.Shard.Username, config.Shard.Password, config.Shard.Database);
            Shard = shardDb;

            var worldDb = new WorldDatabase();
            worldDb.Initialize(config.World.Host, config.World.Port, config.World.Username, config.World.Password, config.World.Database);

            var cachingWorldDb = new CachingWorldDatabase(worldDb);
            World = cachingWorldDb;
        }
    }
}