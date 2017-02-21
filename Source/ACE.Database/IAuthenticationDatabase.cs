using System.Threading.Tasks;

using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Database
{
    public interface IAuthenticationDatabase
    {
        void CreateAccount(Account account);

        void UpdateAccountAccessLevel(uint accountId, AccessLevel accessLevel);

        Task<Account> GetAccountByName(string accountName);

        uint GetMaxId();

        void GetAccountIdByName(string accountName, out uint id);
    }
}
