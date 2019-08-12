using System;
using System.Globalization;
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

        // erasecharacter characterid
        [CommandHandler("erasecharacter", AccessLevel.Admin, CommandHandlerFlag.None, 1,
            "Erases a deleted character from the database",
            "characterId\n" +
            "Given the ID of a deleted character, this command erases that character from the database.  (You can find the ID of a deleted character using the @finger command.)\n" +
            "Can only erase deleted characters, characters not deleted are not safe to be erased with this command.")]
        public static void HandleCharacterErase(Session session, params string[] parameters)
        {
            var charId = parameters[0];

            if (charId.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase) || charId.StartsWith("&H", StringComparison.CurrentCultureIgnoreCase))
                charId = charId.Substring(2);

            if (!uint.TryParse(charId, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out var characterId))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"{parameters[0]} is not a valid character id", ChatMessageType.Broadcast);
                return;
            }

            if (DatabaseManager.Shard.CanEraseCharacter(characterId))
            {
                if (DatabaseManager.Shard.EraseCharacter(characterId))
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"Successfully erased character 0x{characterId:X8}.", ChatMessageType.Broadcast);
                }
                else
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"Erasing character 0x{characterId:X8} failed.", ChatMessageType.Broadcast);
                }
            }
            else
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Cannot erase character 0x{characterId:X8} because it hasn't been deleted. Delete the character first before erasing it.", ChatMessageType.Broadcast);
            }   
        }

        // erasecharacter characterid
        [CommandHandler("purgecharacters", AccessLevel.Admin, CommandHandlerFlag.None, 0,
            "Purges all deleted characters from the database",
            "(days)\n" +
            "If no parameter is specified, all deleted characters older than 30 days are purged.")]
        public static void HandleCharacterPurge(Session session, params string[] parameters)
        {
            var days = 30;

            if (parameters.Length > 0)
            {
                if (!int.TryParse(parameters[0], out days))
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"{parameters[0]} is not a valid number for days.", ChatMessageType.Broadcast);
                    return;
                }
            }

            if (DatabaseManager.Shard.PurgeCharacters(days, out var numberOfCharactersPurged))
                CommandHandlerHelper.WriteOutputInfo(session, $"Purged {numberOfCharactersPurged:N0} deleted characters, older than {days} days ({DateTime.UtcNow.AddDays(-days).ToLocalTime()}).", ChatMessageType.Broadcast);
        }
    }
}
