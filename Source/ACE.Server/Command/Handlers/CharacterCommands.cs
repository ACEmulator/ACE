using System;
using System.Threading.Tasks;
using ACE.Database;
using ACE.Entity.Enum;
using ACE.Server.API.Entity;
using ACE.Server.Command.Handlers.Processors;
using ACE.Server.Managers;
using ACE.Server.Network;

namespace ACE.Server.Command.Handlers
{
    public static class CharacterCommands
    {
        // set-characteraccess charactername (accesslevel)
        [CommandHandler("set-characteraccess", AccessLevel.Admin, CommandHandlerFlag.None, 1,
            "Sets the access level for the character",
            "charactername (accesslevel)\n" +
            "accesslevel can be a number or enum name\n" +
            "0 = Player | 1 = Advocate | 2 = Sentinel | 3 = Envoy | 4 = Developer | 5 = Admin")]
        public static void HandleCharacterTokenization(Session session, params string[] parameters)
        {
            string characterName = parameters[0];

            AccessLevel accessLevel = AccessLevel.Player;
            if (parameters.Length > 1)
                if (Enum.TryParse(parameters[1], true, out accessLevel))
                    if (!Enum.IsDefined(typeof(AccessLevel), accessLevel))
                        accessLevel = AccessLevel.Player;

            DatabaseManager.Shard.SetCharacterAccessLevelByName(characterName.ToLower(), accessLevel, ((uint characterId) =>
            {
                if (characterId > 0)
                {
                    string articleAorAN = "a";
                    if (accessLevel == AccessLevel.Advocate || accessLevel == AccessLevel.Admin || accessLevel == AccessLevel.Envoy)
                        articleAorAN = "an";

                    CommandHandlerHelper.WriteOutputInfo(session, "Character " + characterName + " has been made " + articleAorAN + " " + Enum.GetName(typeof(AccessLevel), accessLevel) + ".", ChatMessageType.Broadcast);
                }
                else
                {
                    CommandHandlerHelper.WriteOutputInfo(session, "There is no character by the name of " + characterName + " found in the database. Has it been deleted?", ChatMessageType.Broadcast);
                }
            }));
        }

        //[CommandHandler("charexport", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, 0, "Export the currently logged in character.")]
        //public static async Task HandleCharacterExportAsync(Session session, params string[] parameters)
        //{
        //    string url = await TransferManager.CreatePackage(session, new PackageMetadata() { PackageType = PackageType.Export });
        //    session.WorldBroadcast($"Character ready for download.  A private link assigned for this download.  Do not share it!" +
        //        "\nTo use the secret cookie to either download your character with a browser or transfer your character to another server by" +
        //        "\nlogging into the target server and entering the command (replacing <name> with desired character name):" +
        //        $"\n@charimport {session.Player.Name} {url}");
        //}

        //[CommandHandler("charmove", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, 0, "Migrate the currently logged in character.")]
        //public static async Task HandleCharacterMigrateAsync(Session session, params string[] parameters)
        //{
        //    string url = await TransferManager.CreatePackage(session, new PackageMetadata() { PackageType = PackageType.Move });
        //    session.WorldBroadcast($"Character packaged and ready for move.  Upon downloading this packageA secret one-time cookie has been assigned for this download." +
        //        "\nTo use the secret cookie to either download your character with a browser or transfer your character to another server by" +
        //        "\nlogging into the target server and entering the command (replacing <name> with desired character name):" +
        //        $"\n@charimport <name> {url}");
        //}

        //[CommandHandler("charimport", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, 2, "Import a character from a given URL.")]
        //public static void HandleCharacterImport(Session session, params string[] parameters)
        //{
        //    TransferManager.Import(session, parameters);
        //}
    }
}
