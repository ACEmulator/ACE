using ACE.Cryptography;
using ACE.Database;
using ACE.Entity;
using ACE.Network;
using System.Diagnostics;
using System.IO;

namespace ACE.Command
{
    public static class AccountCommands
    {
        // accountcreate username password
        [CommandHandler("accountcreate", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 2)]
        public static void HandleAccountCreate(Session session, params string[] parameters)
        {
            uint accountId = DatabaseManager.Authentication.GetMaxId() + 1;
            string account  = parameters[0].ToLower();
            string salt     = SHA2.Hash(SHA2Type.SHA256, Path.GetRandomFileName());
            string password = SHA2.Hash(SHA2Type.SHA256, parameters[1]);
            Account acc = new Account(accountId, account, salt, password);

            DatabaseManager.Authentication.CreateAccount(acc);
        }

        // tokenize charactername accesslevel
        [CommandHandler("tokenize", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 1)]
        public static void HandleCharacterTokenization(Session session, params string[] parameters)
        {
            AccessLevel level;
            string accessName = "player";
            string accessNamePrefix = "";
            if (parameters.Length > 1)
            {
                switch (parameters[1].ToLower())
                {
                    case "advocate":
                        level = AccessLevel.Advocate;
                        accessName = "Advocate";
                        accessNamePrefix = "";
                        break;
                    case "sentinel":
                        level = AccessLevel.Sentinel;
                        accessName = "Sentinel";
                        accessNamePrefix = "+Sentinel";
                        break;
                    case "envoy":
                        level = AccessLevel.Envoy;
                        accessName = "Envoy";
                        accessNamePrefix = "+Envoy";
                        break;
                    case "developer":
                        level = AccessLevel.Developer;
                        accessName = "Developer";
                        accessNamePrefix = "+Developer";
                        break;
                    case "admin":
                        level = AccessLevel.Admin;
                        accessName = "Admin";
                        accessNamePrefix = "+Admin";
                        break;
                    default:
                        level = AccessLevel.Player;
                        accessName = "player";
                        accessNamePrefix = "";
                        break;
                }
            }
            else
            {
                level = AccessLevel.Player;
            }

            uint charId = DatabaseManager.Character.TokenizeByName(parameters[0].ToLower(),level);

            if (charId > 0)
            {
                string verbage = " has been made a ";
                if (level == AccessLevel.Advocate || level == AccessLevel.Admin || level == AccessLevel.Envoy) { verbage = " has been made an "; }

                System.Console.WriteLine("Character " + parameters[0] + verbage + accessName + ".");
            }
            else
            {
                System.Console.WriteLine("There is no character by the name " + parameters[0] + " available. Has it been deleted?");
            }
        }

        // renamechar oldcharactername newcharactername
        [CommandHandler("rename", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 2)]
        public static void HandleCharacterRename(Session session, params string[] parameters)
        {
            string fixupOldName = "";
            string fixupNewName = "";

            if (parameters[0].Remove(1) == "+")
            {
                fixupOldName = parameters[0].Remove(2).ToUpper() + parameters[0].Substring(2);
            }
            else
            {
                fixupOldName = parameters[0].Remove(1).ToUpper() + parameters[0].Substring(1);
            }

            if (parameters[1].Remove(1) == "+")
            {
                fixupNewName = parameters[1].Remove(2).ToUpper() + parameters[1].Substring(2);
            }
            else
            {
               fixupNewName = parameters[1].Remove(1).ToUpper() + parameters[1].Substring(1);
            }

            uint charId = DatabaseManager.Character.RenameCharacter(fixupOldName, fixupNewName);

            if (charId > 0)
            {
                System.Console.WriteLine("Character " + fixupOldName + " has been renamed to " + fixupNewName + ".");
            }
            else
            {
                System.Console.WriteLine("Either there is no character by the name " + fixupOldName + " currently in the database or " + fixupNewName + "is already taken as a name.");
            }
        }
    }
}
