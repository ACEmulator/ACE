using System;
using System.IO;

using ACE.Common.Cryptography;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network;

namespace ACE.Command.Handlers
{
    public static class CharacterCommands
    {
        // set-characteraccess charactername accesslevel
        [CommandHandler("set-characteraccess", AccessLevel.Admin, CommandHandlerFlag.None, 1)]
        public static void HandleCharacterTokenization(Session session, params string[] parameters)
        {
            uint characterId = 0;
            string characterName = parameters[0];

            AccessLevel accessLevel = AccessLevel.Player;
            if (parameters.Length > 1)
                if (Enum.TryParse(parameters[1], true, out accessLevel))
                    if (!Enum.IsDefined(typeof(AccessLevel), accessLevel))
                        accessLevel = AccessLevel.Player;

            characterId = DatabaseManager.Character.SetCharacterAccessLevelByName(characterName.ToLower(), accessLevel);

            if (characterId > 0)
            {
                string articleAorAN = "a";
                if (accessLevel == AccessLevel.Advocate || accessLevel == AccessLevel.Admin || accessLevel == AccessLevel.Envoy)
                    articleAorAN = "an";
                
                if (session == null)
                    Console.WriteLine("Character " + characterName + " has been made " + articleAorAN + " " + Enum.GetName(typeof(AccessLevel), accessLevel) + ".");
                else
                    ChatPacket.SendServerMessage(session, "Character " + characterName + " has been made " + articleAorAN + " " + Enum.GetName(typeof(AccessLevel), accessLevel) + ".", ChatMessageType.Broadcast);
            }
            else
                if (session == null)
                    Console.WriteLine("There is no character by the name of " + characterName + " found in the database. Has it been deleted?");
                else
                    ChatPacket.SendServerMessage(session, "There is no character by the name of " + characterName + " found in the database. Has it been deleted?", ChatMessageType.Broadcast);
        }
    }
}
