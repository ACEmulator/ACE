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
            HashedPassword hp = PasswordHasher.HashPassword(parameters[1]);

            Account acc = new Account(accountId, account, hp);

            DatabaseManager.Authentication.CreateAccount(acc);
        }
    }
}
