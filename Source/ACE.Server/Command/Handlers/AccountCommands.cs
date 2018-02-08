using System;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Network;

namespace ACE.Server.Command.Handlers
{
    public static class AccountCommands
    {
        // accountcreate username password (accesslevel)
        [CommandHandler("accountcreate", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 2,
            "Creates a new account.",
            "username password (accesslevel)\n" +
            "accesslevel can be a number or enum name\n" +
            "0 = Player | 1 = Advocate | 2 = Sentinel | 3 = Envoy | 4 = Developer | 5 = Admin")]
        public static void HandleAccountCreate(Session session, params string[] parameters)
        {
            Account newAccount = new Account();
            newAccount.SetName(parameters[0].ToLower());
            newAccount.SetPassword(parameters[1]);

            AccessLevel accessLevel        = AccessLevel.Player;
            AccessLevel defaultAccessLevel = (AccessLevel)Common.ConfigManager.Config.Server.Accounts.DefaultAccessLevel;

            if (!Enum.IsDefined(typeof(AccessLevel), defaultAccessLevel))
                defaultAccessLevel = AccessLevel.Player;

            accessLevel = defaultAccessLevel;
            if (parameters.Length > 2)
                if (Enum.TryParse(parameters[2], true, out accessLevel))
                    if (!Enum.IsDefined(typeof(AccessLevel), accessLevel))
                        accessLevel = defaultAccessLevel;

            string articleAorAN = "a";
            if (accessLevel == AccessLevel.Advocate || accessLevel == AccessLevel.Admin || accessLevel == AccessLevel.Envoy)
                articleAorAN = "an";

            newAccount.SetAccessLevel(accessLevel);
   
            // TODO and FIXME: this is not fully correct yet, needs more work. Not thread safe.
            var accountExists = DatabaseManager.Authentication.GetAccountByName(parameters[0]);

            if (accountExists != null)
            {
                Console.WriteLine("Account already exists. Try a new name.");
            }
            else
            {
                DatabaseManager.Authentication.CreateAccount(newAccount);
                Console.WriteLine("Account successfully created for " + newAccount.Name + " (" + newAccount.AccountId + ") with access rights as " + articleAorAN + " " + Enum.GetName(typeof(AccessLevel), accessLevel) + ".");
            }
        }
  
        [CommandHandler("accountget", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 1,
            "Gets an account.",
            "username")]
        public static void HandleAccountGet(Session session, params string[] parameters)
        {
            var account = DatabaseManager.Authentication.GetAccountByName(parameters[0]);
            Console.WriteLine($"User: {account.Name}, ID: {account.AccountId}");
        }

        // set-accountaccess accountname (accesslevel)
        [CommandHandler("set-accountaccess", AccessLevel.Admin, CommandHandlerFlag.None, 1, 
            "Change the access level of an account.", 
            "accountname (accesslevel)\n" +
            "accesslevel can be a number or enum name\n" +
            "0 = Player | 1 = Advocate | 2 = Sentinel | 3 = Envoy | 4 = Developer | 5 = Admin")]
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
