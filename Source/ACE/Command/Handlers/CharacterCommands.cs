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

            AccessLevel accessLevel;
            if (parameters.Length > 1)
            {
                switch (parameters[1].ToLower())
                {
                    case "1":
                    case "advocate":
                        accessLevel = AccessLevel.Advocate;
                        break;
                    case "2":
                    case "sentinel":
                        accessLevel = AccessLevel.Sentinel;
                        break;
                    case "3":
                    case "envoy":
                        accessLevel = AccessLevel.Envoy;
                        break;
                    case "4":
                    case "developer":
                        accessLevel = AccessLevel.Developer;
                        break;
                    case "5":
                    case "admin":
                        accessLevel = AccessLevel.Admin;
                        break;
                    default:
                        accessLevel = AccessLevel.Player;
                        break;
                }
            }
            else
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
                    ChatPacket.SendSystemMessage(session, "Character " + characterName + " has been made " + articleAorAN + " " + Enum.GetName(typeof(AccessLevel), accessLevel) + ".");
            }
            else
                if (session == null)
                    Console.WriteLine("There is no character by the name of " + characterName + " found in the database. Has it been deleted?");
                else
                    ChatPacket.SendSystemMessage(session, "There is no character by the name of " + characterName + " found in the database. Has it been deleted?");
        }
    }
}
