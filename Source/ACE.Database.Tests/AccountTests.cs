using ACE.Common;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Database.Tests
{
    [TestClass]
    public class AccountTests
    {
        private static AuthenticationDatabase authDb;

        [ClassInitialize]
        public static void TestSetup(TestContext context)
        {
            // copy config.json
            File.Copy(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\..\\ACE\\Config.json"), ".\\Config.json", true);

            ConfigManager.Initialize();
            authDb = new AuthenticationDatabase();
            authDb.Initialize(ConfigManager.Config.MySql.Authentication.Host,
                          ConfigManager.Config.MySql.Authentication.Port,
                          ConfigManager.Config.MySql.Authentication.Username,
                          ConfigManager.Config.MySql.Authentication.Password,
                          ConfigManager.Config.MySql.Authentication.Database);
        }

        [TestMethod]
        public void CreateAccount_GetAccountByName_ReturnsAccount()
        {
            Account newAccount = new Account();
            newAccount.SetName("testaccount1");
            newAccount.SetPassword("testpassword1");
            newAccount.SetAccessLevel(AccessLevel.Player);

            authDb.CreateAccount(newAccount);
            var results = authDb.GetAccountByName(newAccount.Name);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.AccessLevel == AccessLevel.Player);
        }

        [TestMethod]
        public void UpdateAccountAccessLevelToSentinel_ReturnsAccount()
        {
            Account newAccount = new Account();
            newAccount.SetName("testaccount1");            

            authDb.UpdateAccountAccessLevel(1, AccessLevel.Sentinel);
            var results = authDb.GetAccountByName(newAccount.Name);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.AccessLevel == AccessLevel.Sentinel);
        }

        [TestMethod]
        public void GetAccountIdByName_ReturnsAccount()
        {
            Account newAccount = new Account();
            newAccount.SetName("testaccount1");

            uint id;
            authDb.GetAccountIdByName(newAccount.Name, out id);
            var results = authDb.GetAccountById(id);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.AccountId == id);
            Assert.IsTrue(results.Name == newAccount.Name);
        }        
    }
}
