using System;
using System.Threading.Tasks;

using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network;


// This pragma disables the warning for async mtheods that don't await.
// FIXME(ddevec): Right now we don't await in most methods (async to match handler interface),
//   so I just disable for the file.
//   This may change, then we should disable only for those methods (via disable then restore 1998)
#pragma warning disable 1998

namespace ACE.Command.Handlers
{
    public static class AccountCommands
    {
        // accountcreate username password (accesslevel)
        [CommandHandler("accountcreate", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 2, 
            "Creates a new account.", 
            "username password (accesslevel)\n" +
            "accesslevel can be a number or enum name\n" +
            "0 = Player | 1 = Advocate | 2 = Sentinel | 3 = Envoy | 4 = Developer | 5 = Admin")]
        public static async Task HandleAccountCreate(Session session, params string[] parameters)
        {
            Account newAccount = new Account();
            newAccount.Name = parameters[0].ToLower();
            newAccount.DisplayName = newAccount.Name; // default to this for command-line created accounts
            newAccount.SetPassword(parameters[1]);
            
            AccessLevel accessLevel         = AccessLevel.Player;
            AccessLevel defaultAccessLevel  = (AccessLevel)Common.ConfigManager.Config.Server.Accounts.DefaultAccessLevel;

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
            
            DatabaseManager.Authentication.CreateAccount(newAccount);

            // also create a default subscription with new accounts
            Subscription s = new Subscription();
            s.AccessLevel = accessLevel;
            s.AccountGuid = newAccount.AccountGuid;
            s.Name = "auto";
            DatabaseManager.Authentication.CreateSubscription(s);
            
            Console.WriteLine("Account successfully created for " + newAccount.Name + " (" + newAccount.AccountId + ") with access rights as " + articleAorAN + " " + Enum.GetName(typeof(AccessLevel), accessLevel) + ".");
        }
        
        [CommandHandler("accountget", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 1,
            "Gets an account.",
            "username")]
        public static async Task HandleAccountGet(Session session, params string[] parameters)
        {
            var account = DatabaseManager.Authentication.GetAccountByName(parameters[0]);
            Console.WriteLine($"User: {account.Name}, ID: {account.AccountId}");
        }
        #pragma warning restore 1998

        // set-accountaccess accountname (accesslevel)
        [CommandHandler("set-accountaccess", AccessLevel.Admin, CommandHandlerFlag.None, 1, 
            "Change the access level of an account.", 
            "accountname (accesslevel)\n" +
            "accesslevel can be a number or enum name\n" +
            "0 = Player | 1 = Advocate | 2 = Sentinel | 3 = Envoy | 4 = Developer | 5 = Admin")]
        #pragma warning disable 1998
        public static async Task HandleAccountUpdateAccessLevel(Session session, params string[] parameters)
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
                DatabaseManager.Authentication.UpdateSubscriptionAccessLevel(accountId, accessLevel);

            if (session == null)
                Console.WriteLine("Account " + accountName + " updated with access rights set as " + articleAorAN + " " + Enum.GetName(typeof(AccessLevel), accessLevel) + ".");
            else
                ChatPacket.SendServerMessage(session, "Account " + accountName + " updated with access rights set as " + articleAorAN + " " + Enum.GetName(typeof(AccessLevel), accessLevel) + ".", ChatMessageType.Broadcast);
        }
        #pragma warning restore 1998
    }
}
