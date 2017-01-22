using ACE.Managers;

namespace ACE.Database
{
    public static class DatabaseManager
    {
        public static Database Authentication { get; } = new AuthenticationDatabase();
        public static Database Character { get; } = new CharacterDatabase();
        public static Database World { get; } = new WorldDatabase();

        public static void Initialise()
        {
            var config = ConfigManager.Config.MySql;
            Authentication.Initialise(DatabaseType.Authentication, config.Authentication.Host, config.Authentication.Port, config.Authentication.Username, config.Authentication.Password, config.Authentication.Database);
            Character.Initialise(DatabaseType.Character, config.Character.Host, config.Character.Port, config.Character.Username, config.Character.Password, config.Character.Database);
            //World.Initialise(DatabaseType.World, config.World.Host, config.World.Port, config.World.Username, config.World.Password, config.World.Database);
        }
    }
}
