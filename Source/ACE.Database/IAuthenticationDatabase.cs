using System.Threading.Tasks;

using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Database
{
    public interface IAuthenticationDatabase
    {
        void CreateAccount(Account account);

        void UpdateAccountAccessLevel(uint accountId, AccessLevel accessLevel);

        Account GetAccountByName(string accountName);

        Account GetAccountById(uint accountId);

        void GetAccountIdByName(string accountName, out uint id);

        void UpdateAccount(Account account);
    }
}
