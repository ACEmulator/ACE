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
                if (Enum.TryParse(parameters[2], true, out accessLevel))
                    if (!Enum.IsDefined(typeof(AccessLevel), accessLevel))
                        accessLevel = AccessLevel.Player;

            string articleAorAN = "a";
            if (accessLevel == AccessLevel.Advocate || accessLevel == AccessLevel.Admin || accessLevel == AccessLevel.Envoy)
                articleAorAN = "an";

            Account acc = new Account(accountId, account, accessLevel, salt, password);

            DatabaseManager.Authentication.CreateAccount(acc);

            Console.WriteLine("Account successfully created for " + account + " with access rights as " + articleAorAN + " " + Enum.GetName(typeof(AccessLevel), accessLevel) + ".");
        }

        // set-accountaccess accountname (accesslevel)
        [CommandHandler("set-accountaccess", AccessLevel.Admin, CommandHandlerFlag.None, 1)]
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
                if (session == null)
                    Console.WriteLine("Account " + accountName + " does not exist.");
                else
                    ChatPacket.SendServerMessage(session, "Account " + accountName + " does not exist.", ChatMessageType.Broadcast);
                return;
            }

            AccessLevel accessLevel = AccessLevel.Player;

            if (parameters.Length > 1)
                if (Enum.TryParse(parameters[1], true, out accessLevel))
                    if (!Enum.IsDefined(typeof(AccessLevel), accessLevel))
                        accessLevel = AccessLevel.Player;

            string articleAorAN = "a";
            if (accessLevel == AccessLevel.Advocate || accessLevel == AccessLevel.Admin || accessLevel == AccessLevel.Envoy)
                articleAorAN = "an";

            if (accountId == 0)
            {
                if (session == null)
                    Console.WriteLine("Account " + accountName + " does not exist.");
                else
                    ChatPacket.SendServerMessage(session, "Account " + accountName + " does not exist.", ChatMessageType.Broadcast);
                return;
            }
            else
                DatabaseManager.Authentication.UpdateAccountAccessLevel(accountId, accessLevel);

            if (session == null)
                Console.WriteLine("Account " + accountName + " updated with access rights set as " + articleAorAN + " " + Enum.GetName(typeof(AccessLevel), accessLevel) + ".");
            else
                ChatPacket.SendServerMessage(session, "Account " + accountName + " updated with access rights set as " + articleAorAN + " " + Enum.GetName(typeof(AccessLevel), accessLevel) + ".", ChatMessageType.Broadcast);
        }
    }
}
