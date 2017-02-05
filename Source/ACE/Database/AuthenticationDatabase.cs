using ACE.Entity;
using ACE.Managers;
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
            AccountSelect,
            AccountTableExistenceCheck
        }

        protected override Type preparedStatementType => typeof(AuthenticationPreparedStatement);
        protected override string nodeName { get { return "Authentication";  } }

        protected override void InitialisePreparedStatements()
        {
            AddPreparedStatement(AuthenticationPreparedStatement.AccountInsert, "INSERT INTO `account` (`id`, `account`, `password`, `salt`) VALUES (?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.VarString, MySqlDbType.VarString, MySqlDbType.VarString);
            AddPreparedStatement(AuthenticationPreparedStatement.AccountMaxIndex, "SELECT MAX(`id`) FROM `account`;");
            AddPreparedStatement(AuthenticationPreparedStatement.AccountSelect, "SELECT `id`, `account`, `password`, `salt` FROM `account` WHERE `account` = ?;", MySqlDbType.VarString);
        }

        protected override bool BaseSqlExecuted()
        {
            AddPreparedStatement(AuthenticationPreparedStatement.AccountTableExistenceCheck, "SELECT table_name FROM information_schema.tables WHERE table_schema = ? AND table_name = 'account';", MySqlDbType.VarChar);

            var config = ConfigManager.Config.MySql;
            var result = SelectPreparedStatement(AuthenticationPreparedStatement.AccountTableExistenceCheck, config.Authentication.Database);

            return (result.Count > 0);
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
