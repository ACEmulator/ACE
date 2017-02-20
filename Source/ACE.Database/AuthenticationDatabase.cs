using System;
using System.Diagnostics;
using System.Threading.Tasks;

using ACE.Entity;
using ACE.Entity.Enum;

using MySql.Data.MySqlClient;

namespace ACE.Database
{
    public class AuthenticationDatabase : Database, IAuthenticationDatabase
    {
        private enum AuthenticationPreparedStatement
        {
            AccountInsert,
            AccountMaxIndex,
            AccountSelect,
            AccountUpdateAccessLevel
        }

        protected override Type preparedStatementType => typeof(AuthenticationPreparedStatement);

        protected override void InitialisePreparedStatements()
        {
            AddPreparedStatement(AuthenticationPreparedStatement.AccountInsert, "INSERT INTO `account` (`id`, `account`, `accesslevel`, `password`, `salt`) VALUES (?, ?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.VarString, MySqlDbType.UInt32, MySqlDbType.VarString, MySqlDbType.VarString);
            AddPreparedStatement(AuthenticationPreparedStatement.AccountMaxIndex, "SELECT MAX(`id`) FROM `account`;");
            AddPreparedStatement(AuthenticationPreparedStatement.AccountSelect, "SELECT `id`, `account`, `accesslevel`, `password`, `salt` FROM `account` WHERE `account` = ?;", MySqlDbType.VarString);
            AddPreparedStatement(AuthenticationPreparedStatement.AccountUpdateAccessLevel, "UPDATE `account` SET `accesslevel` = ? WHERE `id` = ?;", MySqlDbType.UInt32, MySqlDbType.UInt32);
        }

        public uint GetMaxId()
        {
            var result = SelectPreparedStatement(AuthenticationPreparedStatement.AccountMaxIndex);
            Debug.Assert(result != null);
            return result.Read<uint>(0, "MAX(`id`)") + 1;
        }

        public void CreateAccount(Account account)
        {
            ExecutePreparedStatement(AuthenticationPreparedStatement.AccountInsert, account.AccountId, account.Name, account.AccessLevel, account.Salt, account.Digest);
        }

        public void UpdateAccountAccessLevel(uint accountId, AccessLevel accessLevel)
        {
            ExecutePreparedStatement(AuthenticationPreparedStatement.AccountUpdateAccessLevel, accessLevel, accountId);
        }

        public async Task<Account> GetAccountByName(string accountName)
        {
            var result = await SelectPreparedStatementAsync(AuthenticationPreparedStatement.AccountSelect, accountName);

            uint id = result.Read<uint>(0, "id");
            string name = result.Read<string>(0, "account");
            uint accessLevel = result.Read<uint>(0, "accesslevel");
            string password = result.Read<string>(0, "password");
            string salt = result.Read<string>(0, "salt");

            Account account = new Account(id, name, (AccessLevel)accessLevel, salt, password);
            return account;
        }

        public void GetAccountIdByName(string accountName, out uint id)
        {
            var result = SelectPreparedStatement(AuthenticationPreparedStatement.AccountSelect, accountName);
            Debug.Assert(result != null);

            id = result.Read<uint>(0, "id");
        }
    }
}
