namespace ACE.Common
{
    public class MasterConfiguration
    {
        public GameConfiguration Server { get; set; }

        public DatabaseConfiguration MySql { get; set; }

        public OfflineConfiguration Offline { get; set; } = new OfflineConfiguration();
    }
}
