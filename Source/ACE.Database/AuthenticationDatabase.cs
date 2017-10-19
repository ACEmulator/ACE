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

            SubscriptionInsert,
            SubscriptionSelect,
            SubscriptionUpdate,
            SubscriptionGet,
            SubscriptionGetByAccount
        }

        protected override Type PreparedStatementType => typeof(AuthenticationPreparedStatement);

        protected override void InitializePreparedStatements()
        {
            ConstructStatement(AuthenticationPreparedStatement.AccountSelect, typeof(Account), ConstructedStatementType.Get);
            ConstructStatement(AuthenticationPreparedStatement.AccountInsert, typeof(Account), ConstructedStatementType.Insert);
            ConstructStatement(AuthenticationPreparedStatement.AccountUpdate, typeof(Account), ConstructedStatementType.Update);
            ConstructStatement(AuthenticationPreparedStatement.AccountSelectByName, typeof(AccountByName), ConstructedStatementType.Get);

            ConstructStatement(AuthenticationPreparedStatement.SubscriptionInsert, typeof(Subscription), ConstructedStatementType.Insert);
            ConstructStatement(AuthenticationPreparedStatement.SubscriptionSelect, typeof(Subscription), ConstructedStatementType.Get);
            ConstructStatement(AuthenticationPreparedStatement.SubscriptionUpdate, typeof(Subscription), ConstructedStatementType.Update);
            ConstructStatement(AuthenticationPreparedStatement.SubscriptionGet, typeof(Subscription), ConstructedStatementType.Get);
            ConstructStatement(AuthenticationPreparedStatement.SubscriptionGetByAccount, typeof(Subscription), ConstructedStatementType.GetList);
        }
        
        public void CreateAccount(Account account)
        {
            ExecuteConstructedInsertStatement(AuthenticationPreparedStatement.AccountInsert, typeof(Account), account);
        }

        public void CreateSubscription(Subscription sub)
        {
            ExecuteConstructedInsertStatement(AuthenticationPreparedStatement.SubscriptionInsert, typeof(Subscription), sub);
        }

        public void UpdateSubscriptionAccessLevel(uint subscriptionId, AccessLevel accessLevel)
        {
            var sub = GetSubscriptionById(subscriptionId);
            sub.AccessLevel = accessLevel;
            UpdateSubscription(sub);
        }

        public void UpdateAccount(Account account)
        {
            ExecuteConstructedUpdateStatement(AuthenticationPreparedStatement.AccountUpdate, typeof(Account), account);
        }

        public void UpdateSubscription(Subscription sub)
        {
            ExecuteConstructedUpdateStatement(AuthenticationPreparedStatement.SubscriptionUpdate, typeof(Subscription), sub);
        }

        public Account GetAccountById(uint accountId)
        {
            Account ret = new Account();
            var criteria = new Dictionary<string, object> { { "accountId", accountId } };
            bool success = ExecuteConstructedGetStatement(AuthenticationPreparedStatement.AccountSelect, typeof(Account), criteria, ret);
            return ret;
        }

        public Subscription GetSubscriptionById(uint subscriptionId)
        {
            Subscription ret = new Subscription();
            var criteria = new Dictionary<string, object> { { "subscriptionId", subscriptionId } };
            bool success = ExecuteConstructedGetStatement(AuthenticationPreparedStatement.SubscriptionGet, typeof(Subscription), criteria, ret);
            return ret;
        }

        public List<Subscription> GetSubscriptionsByAccount(Guid accountGuid)
        {
            var criteria = new Dictionary<string, object> { { "accountGuid", accountGuid.ToByteArray() } };
            var result = ExecuteConstructedGetListStatement<AuthenticationPreparedStatement, Subscription>(AuthenticationPreparedStatement.SubscriptionGetByAccount, criteria);
            return result;
        }

        public Account GetAccountByName(string accountName)
        {
            AccountByName ret = new AccountByName();
            var criteria = new Dictionary<string, object> { { "accountName", accountName } };
            bool success = ExecuteConstructedGetStatement(AuthenticationPreparedStatement.AccountSelectByName, typeof(AccountByName), criteria, ret);

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
            ExecuteConstructedGetStatement(AuthenticationPreparedStatement.AccountSelectByName, typeof(AccountByName), criteria, ret);
            id = ret.AccountId;
        }
    }
}
