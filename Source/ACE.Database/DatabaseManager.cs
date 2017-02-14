namespace ACE.Database
{
    public class DatabaseManager
    {
        public static IAuthenticationDatabase Authentication { get; set; }

        public static ICharacterDatabase Character { get; set; }

        public static IWorldDatabase World { get; set; }

        public static IChartDatabase Charts { get; set; }

        public static void Initialise()
        {
            var config = ConfigManager.Config.MySql;

            var authDb = new AuthenticationDatabase();
            authDb.Initialise(config.Authentication.Host, config.Authentication.Port, config.Authentication.Username, config.Authentication.Password, config.Authentication.Database);
            Authentication = authDb;

            var charDb = new CharacterDatabase();
            charDb.Initialise(config.Character.Host, config.Character.Port, config.Character.Username, config.Character.Password, config.Character.Database);
            Character = charDb;

            var worldDb = new WorldDatabase();
            worldDb.Initialise(config.World.Host, config.World.Port, config.World.Username, config.World.Password, config.World.Database);
            World = worldDb;

            Charts = new ChartDatabase();
        }
    }
}
