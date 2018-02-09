using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.Common;
using ACE.Entity;
using ACE.Entity.Enum;

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
            File.Copy(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\..\\ACE.Server\\Config.json"), ".\\Config.json", true);

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
        public void UpdateAccountAccessLevelToSentinelAndBackToPlayer_ReturnsAccount()
        {
            Account newAccount = new Account();
            newAccount.SetName("testaccount1");            

            authDb.UpdateAccountAccessLevel(1, AccessLevel.Sentinel);
            var results = authDb.GetAccountByName(newAccount.Name);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.AccessLevel == AccessLevel.Sentinel);

            authDb.UpdateAccountAccessLevel(1, AccessLevel.Player);
            var results2 = authDb.GetAccountByName(newAccount.Name);
            Assert.IsNotNull(results2);
            Assert.IsTrue(results2.AccessLevel == AccessLevel.Player);
        }

        [TestMethod]
        public void GetAccountIdByName_ReturnsAccount()
        {
            Account newAccount = new Account();
            newAccount.SetName("testaccount1");

            authDb.GetAccountIdByName(newAccount.Name, out var id);
            var results = authDb.GetAccountById(id);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.AccountId == id);
            Assert.IsTrue(results.Name == newAccount.Name);
        }

        [TestMethod]
        public void GetAccountByName_TestPassword_ReturnsMatch()
        {
            var results = authDb.GetAccountByName("testaccount1");
            Assert.IsNotNull(results);
            Assert.IsTrue(results.PasswordMatches("testpassword1"));
        }

        [TestMethod]
        public void GetAccountByName_TestPassword_ReturnsNoMatch()
        {
            var results = authDb.GetAccountByName("testaccount1");
            Assert.IsNotNull(results);
            Assert.IsFalse(results.PasswordMatches("testpassword2"));
        }
    }
}
