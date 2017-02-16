using ACE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Database
{
    public interface IAuthenticationDatabase
    {
        void CreateAccount(Account account);

        Task<Account> GetAccountByName(string accountName);

        uint GetMaxId();
    }
}
