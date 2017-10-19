using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using ACE.Common;
using ACE.Database;
using Newtonsoft.Json;

namespace ACE.Web.Models
{
    public class AceAccountStore : IUserStore<ApplicationUser>
    {
        public AceAccountStore()
        {
            var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin\\config.json");
            var masterConfig = JsonConvert.DeserializeObject<MasterConfiguration>(File.ReadAllText(path));
            ConfigManager.Initialize(masterConfig);
            DatabaseManager.Initialize(false);
        }

        public Task CreateAsync(ApplicationUser user)
        {
            DatabaseManager.Authentication.CreateAccount(user.GetAccount());

            return Task.FromResult(true);
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            // nothing?
        }

        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return Task.Run(() => new ApplicationUser(DatabaseManager.Authentication.GetAccountById(uint.Parse(userId))));
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return Task.Run(() => new ApplicationUser(DatabaseManager.Authentication.GetAccountByName(userName)));
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            return new Task(() => DatabaseManager.Authentication.UpdateAccount(user.GetAccount()));
        }
    }
}