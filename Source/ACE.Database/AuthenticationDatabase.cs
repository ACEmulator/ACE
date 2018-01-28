using System;
using System.Collections.Generic;
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
            AccountSelect,
            AccountUpdate,
            AccountSelectByName,
        }

        protected override Type PreparedStatementType => typeof(AuthenticationPreparedStatement);

        protected override void InitializePreparedStatements()
        {
            ConstructStatement(AuthenticationPreparedStatement.AccountSelect, typeof(Account), ConstructedStatementType.Get);
            ConstructStatement(AuthenticationPreparedStatement.AccountInsert, typeof(Account), ConstructedStatementType.Insert);
            ConstructStatement(AuthenticationPreparedStatement.AccountUpdate, typeof(Account), ConstructedStatementType.Update);
            ConstructStatement(AuthenticationPreparedStatement.AccountSelectByName, typeof(AccountByName), ConstructedStatementType.Get);
        }

        public void CreateAccount(Account account)
        {
            ExecuteConstructedInsertStatement(AuthenticationPreparedStatement.AccountInsert, typeof(Account), account);
        }

        public void UpdateAccountAccessLevel(uint accountId, AccessLevel accessLevel)
        {
            //ExecutePreparedStatement(AuthenticationPreparedStatement.AccountUpdateAccessLevel, accessLevel, accountId);
        }

        public Account GetAccountById(uint accountId)
        {
            Account ret = new Account();
            var criteria = new Dictionary<string, object> { { "accountId", accountId } };
            bool success = ExecuteConstructedGetStatement<Account, AuthenticationPreparedStatement>(AuthenticationPreparedStatement.AccountSelect, criteria, ret);
            return ret;
        }

        public Account GetAccountByName(string accountName)
        {
            AccountByName ret = new AccountByName();
            var criteria = new Dictionary<string, object> { { "accountName", accountName } };
            bool success = ExecuteConstructedGetStatement<AccountByName, AuthenticationPreparedStatement>(AuthenticationPreparedStatement.AccountSelectByName, criteria, ret);

            if (success)
            {
                return GetAccountById(ret.AccountId);
            }

            return null;
        }

        public void GetAccountIdByName(string accountName, out uint id)
        {
            AccountByName ret = new AccountByName();
            var criteria = new Dictionary<string, object> { { "accountName", accountName } };
            ExecuteConstructedGetStatement<AccountByName, AuthenticationPreparedStatement>(AuthenticationPreparedStatement.AccountSelectByName, criteria, ret);
            id = ret.AccountId;
        }

        //public async Task<Account> GetAccountByName(string accountName)
        //{
        //    var result = await SelectPreparedStatementAsync(AuthenticationPreparedStatement.AccountSelect, accountName);

        //    uint id = result.Read<uint>(0, "id");
        //    string name = result.Read<string>(0, "account");
        //    uint accessLevel = result.Read<uint>(0, "accesslevel");
        //    string password = result.Read<string>(0, "password");
        //    string salt = result.Read<string>(0, "salt");

        //    Account account = new Account(id, name, (AccessLevel)accessLevel, salt, password);
        //    return account;
        //}

        //public void GetAccountIdByName(string accountName, out uint id)
        //{
        //    var result = SelectPreparedStatement(AuthenticationPreparedStatement.AccountSelect, accountName);
        //    Debug.Assert(result != null, "Invalid prepared statement value.");

        //    id = result.Read<uint>(0, "id");
        //}
    }
}
