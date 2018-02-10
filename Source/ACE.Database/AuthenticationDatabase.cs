using System.Linq;

using Microsoft.EntityFrameworkCore;

using ACE.Database.Models.ace_auth;
using ACE.Entity.Enum;

namespace ACE.Database
{
    public class AuthenticationDatabase : Database
    {
        public void AddAccount(Account account)
        {
            using (var context = new ace_authContext())
            {
                context.Account.Add(account);

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Will return null if the accountId was not found.
        /// </summary>
        public Account GetAccountById(uint accountId)
        {
            using (var context = new ace_authContext())
                return context.Account.FirstOrDefault(r => r.AccountId == accountId);
        }

        /// <summary>
        /// Will return null if the accountName was not found.
        /// </summary>
        public Account GetAccountByName(string accountName)
        {
            using (var context = new ace_authContext())
                return context.Account.FirstOrDefault(r => r.AccountName == accountName);
        }

        /// <summary>
        /// id will be 0 if the accountName was not found.
        /// </summary>
        public uint GetAccountIdByName(string accountName)
        {
            using (var context = new ace_authContext())
            {
                var result = context.Account.FirstOrDefault(r => r.AccountName == accountName);

                return (result != null) ? result.AccountId : 0;
            }
        }

        public void UpdateAccount(Account account)
        {
            using (var context = new ace_authContext())
            {
                context.Entry(account).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        public bool UpdateAccountAccessLevel(uint accountId, AccessLevel accessLevel)
        {
            using (var context = new ace_authContext())
            {
                var account = context.Account.First(r => r.AccountId == accountId);

                if (account == null)
                    return false;

                account.AccessLevel = (uint)accessLevel;

                context.SaveChanges();
            }

            return true;
        }
    }
}
