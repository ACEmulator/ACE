using System;
using System.IO;

using ACE.Common.Cryptography;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network;

namespace ACE.Command.Handlers
{
    public static class AccountCommands
    {
        // accountcreate username password accesslevel
        [CommandHandler("accountcreate", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 2)]
        public static void HandleAccountCreate(Session session, params string[] parameters)
        {
            uint accountId          = DatabaseManager.Authentication.GetMaxId() + 1;
            string account          = parameters[0].ToLower();
            string salt             = SHA2.Hash(SHA2Type.SHA256, Path.GetRandomFileName());
            string password         = SHA2.Hash(SHA2Type.SHA256, parameters[1]);
            AccessLevel accessLevel = AccessLevel.Player;

            if (parameters.Length > 2)
            {
                switch (parameters[2].ToLower())
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

            string articleAorAN = "a";
            if (accessLevel == AccessLevel.Advocate || accessLevel == AccessLevel.Admin || accessLevel == AccessLevel.Envoy)
                articleAorAN = "an";

            Account acc = new Account(accountId, account, accessLevel, salt, password);

            DatabaseManager.Authentication.CreateAccount(acc);

            Console.WriteLine("Account successfully created for " + account + " with access rights as " + articleAorAN + " " + Enum.GetName(typeof(AccessLevel), accessLevel) + ".");
        }

        // set-accountaccess accountname (accesslevel)
        [CommandHandler("set-accountaccess", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 1)]
        public static void HandleAccountUpdateAccessLevel(Session session, params string[] parameters)
        {
            uint accountId      = 0;
            string accountName  = parameters[0].ToLower();

            try
            {
                DatabaseManager.Authentication.GetAccountIdByName(accountName, out accountId);
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Account " + accountName + " does not exist.");
                return;
            }

            AccessLevel accessLevel = AccessLevel.Player;

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

            string articleAorAN = "a";
            if (accessLevel == AccessLevel.Advocate || accessLevel == AccessLevel.Admin || accessLevel == AccessLevel.Envoy)
                articleAorAN = "an";

            if (accountId == 0)
            {
                Console.WriteLine("Account " + accountName + " does not exist.");
                return;
            }
            else
                DatabaseManager.Authentication.UpdateAccountAccessLevel(accountId, accessLevel);

            Console.WriteLine("Account " + accountName + " updated with access rights set as " + articleAorAN + " " + Enum.GetName(typeof(AccessLevel), accessLevel) + ".");
        }
    }
}
