namespace ACE.Common
{
    public class DatabaseConfiguration
    {
        public MySqlConfiguration Authentication { get; set; }

        public MySqlConfiguration Shard { get; set; }

        public MySqlConfiguration World { get; set; }
    }
}
