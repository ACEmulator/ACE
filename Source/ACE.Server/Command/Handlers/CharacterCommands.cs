using System;
using ACE.Database;
using ACE.Entity.Enum;
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
    }
}
