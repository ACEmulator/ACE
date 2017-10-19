using System.Collections.Generic;
using System.Threading.Tasks;

using ACE.Entity;
using ACE.Entity.Enum;
using System;

namespace ACE.Database
{
    public interface IAuthenticationDatabase
    {
        void CreateAccount(Account account);
        
        Account GetAccountByName(string accountName);

        Account GetAccountById(uint accountId);

        void GetAccountIdByName(string accountName, out uint id);

        void UpdateAccount(Account account);

        void CreateSubscription(Subscription sub);

        void UpdateSubscription(Subscription sub);

        void UpdateSubscriptionAccessLevel(uint subscriptionId, AccessLevel accessLevel);

        Subscription GetSubscriptionById(uint subscriptionId);

        List<Subscription> GetSubscriptionsByAccount(Guid accountGuid);
    }
}
