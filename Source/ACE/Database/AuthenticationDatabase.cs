using ACE.Cryptography;
using ACE.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ACE.Database
{
    public class AuthenticationDatabase : Database, IAuthenticationDatabase
    {
        private enum AuthenticationPreparedStatement
        {
            AccountInsert,
            AccountMaxIndex,
            AccountSelect
        }

        protected override Type preparedStatementType { get { return typeof(AuthenticationPreparedStatement); } }

        protected override void InitialisePreparedStatements()
        {
            AddPreparedStatement(AuthenticationPreparedStatement.AccountInsert, "INSERT INTO `account` (`id`, `account`, `password`, `salt`) VALUES (?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.VarString, MySqlDbType.VarString, MySqlDbType.VarString);
            AddPreparedStatement(AuthenticationPreparedStatement.AccountMaxIndex, "SELECT MAX(`id`) FROM `account`;");
            AddPreparedStatement(AuthenticationPreparedStatement.AccountSelect, "SELECT `id`, `account`, `password`, `salt` FROM `account` WHERE `account` = ?;", MySqlDbType.VarString);
        }

        public uint GetMaxId()
        {
            var result = SelectPreparedStatement(AuthenticationPreparedStatement.AccountMaxIndex);
            Debug.Assert(result != null);
            return result.Read<uint>(0, "MAX(`id`)") + 1;
        }

        public void CreateAccount(Account account)
        {
            ExecutePreparedStatement(AuthenticationPreparedStatement.AccountInsert, account.AccountId, account.Name, account.Salt, account.Digest);
        }

        public async Task<Account> GetAccountByName(string accountName)
        {
            var result = await SelectPreparedStatementAsync(AuthenticationPreparedStatement.AccountSelect, accountName);

            uint id = result.Read<uint>(0, "id");
            string name = result.Read<string>(0, "account");
            string password = result.Read<string>(0, "password");
            string salt = result.Read<string>(0, "salt");

            Account account = new Account(id, name, salt, password);
            return account;
        }
    }
}
