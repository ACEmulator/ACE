using System;
using System.Net;

using log4net;

using ACE.Database;
using ACE.Database.Models.Auth;
using ACE.Entity.Enum;
using ACE.Server.Network;

namespace ACE.Server.Command.Handlers
{
    public static class AccountCommands
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // accountcreate username password (accesslevel)
        [CommandHandler("accountcreate", AccessLevel.Admin, CommandHandlerFlag.None, 2,
            "Creates a new account.",
            "username password (accesslevel)\n" +
            "accesslevel can be a number or enum name\n" +
            "0 = Player | 1 = Advocate | 2 = Sentinel | 3 = Envoy | 4 = Developer | 5 = Admin")]
        public static void HandleAccountCreate(Session session, params string[] parameters)
        {
            AccessLevel defaultAccessLevel = (AccessLevel)Common.ConfigManager.Config.Server.Accounts.DefaultAccessLevel;

            if (!Enum.IsDefined(typeof(AccessLevel), defaultAccessLevel))
                defaultAccessLevel = AccessLevel.Player;

            var accessLevel = defaultAccessLevel;

            if (parameters.Length > 2)
            {
                if (Enum.TryParse(parameters[2], true, out accessLevel))
                {
                    if (!Enum.IsDefined(typeof(AccessLevel), accessLevel))
                        accessLevel = defaultAccessLevel;
                }
            }

            string articleAorAN = "a";
            if (accessLevel == AccessLevel.Advocate || accessLevel == AccessLevel.Admin || accessLevel == AccessLevel.Envoy)
                articleAorAN = "an";

            string message = "";

            var accountExists = DatabaseManager.Authentication.GetAccountByName(parameters[0]);
                      
            if (accountExists != null)
            {
                message= "Account already exists. Try a new name.";
            }
            else
            {
                try
                {
                    var account = DatabaseManager.Authentication.CreateAccount(parameters[0].ToLower(), parameters[1], accessLevel, IPAddress.Parse("127.0.0.1"));

                    if (DatabaseManager.AutoPromoteNextAccountToAdmin && accessLevel == AccessLevel.Admin)
                        DatabaseManager.AutoPromoteNextAccountToAdmin = false;

                    message = ("Account successfully created for " + account.AccountName + " (" + account.AccountId + ") with access rights as " + articleAorAN + " " + Enum.GetName(typeof(AccessLevel), accessLevel) + ".");
                }
                catch
                {
                    message = "Account already exists. Try a new name.";
                }
            }

            CommandHandlerHelper.WriteOutputInfo(session, message, ChatMessageType.WorldBroadcast);
        }
  
        [CommandHandler("accountget", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 1,
            "Gets an account.",
            "username")]
        public static void HandleAccountGet(Session session, params string[] parameters)
        {
            var account = DatabaseManager.Authentication.GetAccountByName(parameters[0]);
            Console.WriteLine($"User: {account.AccountName}, ID: {account.AccountId}");
        }

        // set-accountaccess accountname (accesslevel)
        [CommandHandler("set-accountaccess", AccessLevel.Admin, CommandHandlerFlag.None, 1, 
            "Change the access level of an account.", 
            "accountname (accesslevel)\n" +
            "accesslevel can be a number or enum name\n" +
            "0 = Player | 1 = Advocate | 2 = Sentinel | 3 = Envoy | 4 = Developer | 5 = Admin")]
        public static void HandleAccountUpdateAccessLevel(Session session, params string[] parameters)
        {
            string accountName  = parameters[0].ToLower();

            var accountId = DatabaseManager.Authentication.GetAccountIdByName(accountName);

            if (accountId == 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, "Account " + accountName + " does not exist.", ChatMessageType.Broadcast);
                return;
            }

            AccessLevel accessLevel = AccessLevel.Player;

            if (parameters.Length > 1)
            {
                if (Enum.TryParse(parameters[1], true, out accessLevel))
                {
                    if (!Enum.IsDefined(typeof(AccessLevel), accessLevel))
                        accessLevel = AccessLevel.Player;
                }
            }

            string articleAorAN = "a";
            if (accessLevel == AccessLevel.Advocate || accessLevel == AccessLevel.Admin || accessLevel == AccessLevel.Envoy)
                articleAorAN = "an";

            if (accountId == 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, "Account " + accountName + " does not exist.", ChatMessageType.Broadcast);
                return;
            }

            DatabaseManager.Authentication.UpdateAccountAccessLevel(accountId, accessLevel);

            if (DatabaseManager.AutoPromoteNextAccountToAdmin && accessLevel == AccessLevel.Admin)
                DatabaseManager.AutoPromoteNextAccountToAdmin = false;

            CommandHandlerHelper.WriteOutputInfo(session, "Account " + accountName + " updated with access rights set as " + articleAorAN + " " + Enum.GetName(typeof(AccessLevel), accessLevel) + ".", ChatMessageType.Broadcast);
        }

        // set-accountpassword accountname newpassword
        [CommandHandler("set-accountpassword", AccessLevel.Admin, CommandHandlerFlag.None, 2,
            "Set the account password.",
            "accountname newpassword\n")]
        public static void HandleAccountSetPassword(Session session, params string[] parameters)
        {
            string accountName = parameters[0].ToLower();

            var account = DatabaseManager.Authentication.GetAccountByName(accountName);

            if (account == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, "Account " + accountName + " does not exist.", ChatMessageType.Broadcast);
                return;
            }

            if (parameters.Length < 1)
            {
                CommandHandlerHelper.WriteOutputInfo(session, "You must specify a password for the account.", ChatMessageType.Broadcast);
                return;
            }

            account.SetPassword(parameters[1]);
            account.SetSaltForBCrypt();

            DatabaseManager.Authentication.UpdateAccount(account);

            CommandHandlerHelper.WriteOutputInfo(session, $"Account password for {accountName} successfully changed.", ChatMessageType.Broadcast);
        }

        /// <summary>
        /// Rate limiter for /passwd command
        /// </summary>
        private static readonly TimeSpan PasswdInterval = TimeSpan.FromSeconds(5);

        // passwd oldpassword newpassword
        [CommandHandler("passwd", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, 2,
            "Change your account password.",
            "oldpassword newpassword\n")]
        public static void HandlePasswd(Session session, params string[] parameters)
        {
            if (session == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, "This command is run from ingame client only", ChatMessageType.Broadcast);
                return;
            }

            log.Debug($"{session.Player.Name} is changing their password");

            var currentTime = DateTime.UtcNow;

            if (currentTime - session.LastPassTime < PasswdInterval)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"This command may only be run once every {PasswdInterval.TotalSeconds} seconds.", ChatMessageType.Broadcast);
                return;
            }
            session.LastPassTime = currentTime;

            if (parameters.Length <= 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, "You must specify the current password for the account.", ChatMessageType.Broadcast);
                return;
            }

            if (parameters.Length < 1)
            {
                CommandHandlerHelper.WriteOutputInfo(session, "You must specify a new password for the account.", ChatMessageType.Broadcast);
                return;
            }

            var account = DatabaseManager.Authentication.GetAccountById(session.AccountId);

            if (account == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Account {session.Account} ({session.AccountId}) wasn't found in the database! How are you in world without a valid account?", ChatMessageType.Broadcast);                
                return;
            }

            var oldpassword = parameters[0];
            var newpassword = parameters[1];

            if (account.PasswordMatches(oldpassword))
            {
                account.SetPassword(newpassword);
                account.SetSaltForBCrypt();
            }
            else
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Unable to change password: Password provided in first parameter does not match current account password for this account!", ChatMessageType.Broadcast);
                return;
            }


            DatabaseManager.Authentication.UpdateAccount(account);

            CommandHandlerHelper.WriteOutputInfo(session, "Account password successfully changed.", ChatMessageType.Broadcast);
        }
    }
}
