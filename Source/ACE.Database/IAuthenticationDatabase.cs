using System.Threading.Tasks;

using ACE.Entity;

namespace ACE.Database
{
    public interface IAuthenticationDatabase
    {
        void CreateAccount(Account account);

        Task<Account> GetAccountByName(string accountName);

        uint GetMaxId();
    }
}
