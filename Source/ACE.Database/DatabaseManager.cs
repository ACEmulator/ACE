using ACE.Common;

namespace ACE.Database
{
    public static class DatabaseManager
    {
        public static AuthenticationDatabase Authentication { get; } = new AuthenticationDatabase();

        public static CachingWorldDatabase World { get; private set; }

        private static SerializedShardDatabase serializedShardDb;

        public static SerializedShardDatabase Shard { get; private set; }

        public static void Initialize(bool autoRetry = true)
        {
            var config = ConfigManager.Config.MySql;

            Authentication.Initialize(config.Authentication.Host, config.Authentication.Port, config.Authentication.Username, config.Authentication.Password, config.Authentication.Database, autoRetry);

            var worldDb = new WorldDatabase();
            worldDb.Initialize(config.World.Host, config.World.Port, config.World.Username, config.World.Password, config.World.Database, autoRetry);
            var cachingWorldDb = new CachingWorldDatabase(worldDb);
            World = cachingWorldDb;

            var shardDb = new ShardDatabase();
            shardDb.Initialize(config.Shard.Host, config.Shard.Port, config.Shard.Username, config.Shard.Password, config.Shard.Database, autoRetry);
            serializedShardDb = new SerializedShardDatabase(shardDb);
            Shard = serializedShardDb;
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
