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
            var result = DatabaseManager.Authentication.SelectPreparedStatement(AuthenticationPreparedStatement.AccountMaxIndex);
            Debug.Assert(result != null);

            uint accountId  = result.Read<uint>(0, "MAX(`id`)") + 1;
            string account  = parameters[0].ToLower();
            string salt     = SHA2.Hash(SHA2Type.SHA256, Path.GetRandomFileName());
            string password = SHA2.Hash(SHA2Type.SHA256, parameters[1]);
            string digest   = SHA2.Hash(SHA2Type.SHA256, password + salt);

            DatabaseManager.Authentication.ExecutePreparedStatement(AuthenticationPreparedStatement.AccountInsert, accountId, account, digest, salt);
        }

        // enable-autoaccountcreate
        [CommandHandler("enable-autoaccountcreate", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 0)]
        public static void HandleEnableAutoAccountCreate(Session session, params string[] parameters)
        {
            //var result = DatabaseManager.Authentication.SelectPreparedStatement(AuthenticationPreparedStatement.AccountMaxIndex);
            //Debug.Assert(result != null);

            //uint accountId = result.Read<uint>(0, "MAX(`id`)") + 1;
            //string account = parameters[0].ToLower();
            //string salt = SHA2.Hash(SHA2Type.SHA256, Path.GetRandomFileName());
            //string password = SHA2.Hash(SHA2Type.SHA256, parameters[1]);
            //string digest = SHA2.Hash(SHA2Type.SHA256, password + salt);

            //DatabaseManager.Authentication.ExecutePreparedStatement(AuthenticationPreparedStatement.AccountInsert, accountId, account, digest, salt);

            //TODO: setup bool flipper code

            var config = Managers.ConfigManager.Config.Server.EnableAutoAccountCreate;
            if (config != true)
            {
                // Managers.ConfigManager.Config.Server.EnableAutoAccountCreate = true;
            }
            System.Console.WriteLine("Enabled Automatic Account Creation for this server");
        }
    }
}
