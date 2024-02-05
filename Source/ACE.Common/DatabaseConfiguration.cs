namespace ACE.Common
{
    public class DatabaseConfiguration
    {
        public MySqlConfiguration Authentication { get; set; } = new MySqlConfiguration()
        {
            Host     = "127.0.0.1",
            Port     = 3306,
            Database = "ace_auth",
            Username = "root",
            Password = ""
        };

        public MySqlConfiguration Shard { get; set; } = new MySqlConfiguration()
        {
            Host = "127.0.0.1",
            Port = 3306,
            Database = "ace_shard",
            Username = "root",
            Password = ""
        };

        public MySqlConfiguration World { get; set; } = new MySqlConfiguration()
        {
            Host = "127.0.0.1",
            Port = 3306,
            Database = "ace_world",
            Username = "root",
            Password = ""
        };
    }
}
