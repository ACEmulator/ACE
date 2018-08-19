
using ACE.Entity.Enum;
using ACE.Server.Command.Handlers.Processors;
using ACE.Server.Network;

namespace ACE.Server.Command.Handlers
{
    public static class DeveloperDatabaseCommands
    {
        [CommandHandler("databaseperftest", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Test server/database performance.", "biotasPerTest\n" + "optional parameter biotasPerTest if omitted 1000")]
        public static void HandleDatabasePerfTest(Session session, params string[] parameters)
        {
            int biotasPerTest = DatabasePerfTest.DefaultBiotasTestCount;

            if (parameters?.Length > 0)
                int.TryParse(parameters[0], out biotasPerTest);

            var processor = new DatabasePerfTest();
            processor.RunAsync(session, biotasPerTest);
        }
    }
}
