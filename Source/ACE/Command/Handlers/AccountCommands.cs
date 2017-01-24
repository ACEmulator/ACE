﻿using ACE.Cryptography;
using ACE.Database;
using ACE.Network;
using System.Diagnostics;
using System.IO;

namespace ACE.Command
{
    public static class AccountCommands
    {
        // accountcreate username password
        [CommandHandler("accountcreate", 2)]
        public static void HandleAccountCreate(Session session, params string[] parameters)
        {
            var result = DatabaseManager.Authentication.SelectPreparedStatement(AuthenticationPreparedStatement.AccountMaxIndex);
            Debug.Assert(result != null);

            //uint accountId = result.Read<uint>(0, "MAX(`id`)") + 1; No longer needed because the DB is taking care of this.
            string salt     = SHA2.Hash(SHA2Type.SHA256, Path.GetRandomFileName());
            string password = SHA2.Hash(SHA2Type.SHA256, parameters[1]);
            string digest   = SHA2.Hash(SHA2Type.SHA256, password + salt);

            DatabaseManager.Authentication.ExecutePreparedStatement(AuthenticationPreparedStatement.AccountInsert, parameters[0], digest, salt);
        }
    }
}
