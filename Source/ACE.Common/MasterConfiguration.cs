namespace ACE.Common
{
    public class MasterConfiguration
    {
        public GameConfiguration Server { get; set; } = new GameConfiguration();

        public DatabaseConfiguration MySql { get; set; } = new DatabaseConfiguration();

        public OfflineConfiguration Offline { get; set; } = new OfflineConfiguration();

        public DDDConfiguration DDD { get; set; } = new DDDConfiguration();

        public MetricsConfiguration Metrics { get; set; } = new MetricsConfiguration();
    }
}
