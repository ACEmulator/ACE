
using log4net;

namespace ACE.Database
{
    public static class DatabaseManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static AuthenticationDatabase Authentication { get; } = new AuthenticationDatabase();

        public static WorldDatabase World { get; } = new WorldDatabase();

        private static SerializedShardDatabase serializedShardDb;

        public static SerializedShardDatabase Shard { get; private set; }

        public static ServerPropertyDatabase ServerConfig { get; private set; } = new ServerPropertyDatabase();

        public static void Initialize(bool autoRetry = true)
        {
            Authentication.Exists(true);
            World.Exists(true);
            ServerConfig.Exists(true);

            var shardDb = new ShardDatabase();
            serializedShardDb = new SerializedShardDatabase(shardDb);
            Shard = serializedShardDb;

            shardDb.Exists(true);

            var playerWeenieLoadTest = World.GetCachedWeenie("human");
            if (playerWeenieLoadTest == null)
                log.Fatal($"Database does not contain the weenie for human (1). Characters cannot be created or logged into until the missing weenie is restored.");
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
