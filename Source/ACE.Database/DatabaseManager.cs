using ACE.Common;

namespace ACE.Database
{
    public class DatabaseManager
    {
        private static SerializedShardDatabase serializedShardDb;

        public static IAuthenticationDatabase Authentication { get; set; }

        public static ISerializedShardDatabase Shard { get; set; }

        public static IWorldDatabase World { get; set; }

        public static void Initialize()
        {
            var config = ConfigManager.Config.MySql;

            var authDb = new AuthenticationDatabase();
            authDb.Initialize(config.Authentication.Host, config.Authentication.Port, config.Authentication.Username, config.Authentication.Password, config.Authentication.Database);
            Authentication = authDb;

            var shardDb = new ShardDatabase();
            shardDb.Initialize(config.Shard.Host, config.Shard.Port, config.Shard.Username, config.Shard.Password, config.Shard.Database);
            serializedShardDb = new SerializedShardDatabase(shardDb);
            Shard = serializedShardDb;

            var worldDb = new WorldDatabase();
            worldDb.Initialize(config.World.Host, config.World.Port, config.World.Username, config.World.Password, config.World.Database);

            var cachingWorldDb = new CachingWorldDatabase(worldDb);
            World = cachingWorldDb;
        }

        public static void Start()
        {
            serializedShardDb.Start();
        }

        public static void Stop()
        {
            serializedShardDb.Stop();
        }
    }
}