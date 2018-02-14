
namespace ACE.Database
{
    public static class DatabaseManager
    {
        public static AuthenticationDatabase Authentication { get; } = new AuthenticationDatabase();

        public static WorldDatabase World { get; } = new WorldDatabase();

        private static SerializedShardDatabase serializedShardDb;

        public static SerializedShardDatabase Shard { get; private set; }

        public static void Initialize(bool autoRetry = true)
        {
            Authentication.Exists(true);
            World.Exists(true);

            var shardDb = new ShardDatabase();
            serializedShardDb = new SerializedShardDatabase(shardDb);
            Shard = serializedShardDb;

            shardDb.Exists(true);
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
