using System;
using System.Collections.Generic;

using ACE.Entity;
using ACE.Entity.Enum;

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
            // ExecutePreparedStatement(AuthenticationPreparedStatement.AccountUpdateAccessLevel, accessLevel, accountId);
            var account = GetAccountById(accountId);
            account.SetAccessLevel(accessLevel);
            UpdateAccount(account);
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
                return GetAccountById(ret.AccountId);

            return null;
        }

        public void GetAccountIdByName(string accountName, out uint id)
        {
            AccountByName ret = new AccountByName();
            var criteria = new Dictionary<string, object> { { "accountName", accountName } };
            ExecuteConstructedGetStatement<AccountByName, AuthenticationPreparedStatement>(AuthenticationPreparedStatement.AccountSelectByName, criteria, ret);
            id = ret.AccountId;
        }

        public void UpdateAccount(Account account)
        {
            ExecuteConstructedUpdateStatement(AuthenticationPreparedStatement.AccountUpdate, typeof(Account), account);
        }
    }
}
