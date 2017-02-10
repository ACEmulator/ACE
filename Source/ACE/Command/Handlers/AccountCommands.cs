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
    }
}
