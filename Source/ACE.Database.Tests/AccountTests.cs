using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.Common;
using ACE.Database.Models.Auth;
using ACE.Entity.Enum;
using System.Net;

[assembly: DoNotParallelize]

namespace ACE.Database.Tests
{
    [TestClass]
    public class AccountTests
    {
        private static AuthenticationDatabase authDb;

        [ClassInitialize]
        public static void TestSetup(TestContext context)
        {
            // copy config.js
            File.Copy(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\..\\ACE.Server\\Config.js"), ".\\Config.js", true);

            ConfigManager.Initialize();
            authDb = new AuthenticationDatabase();
        }

        [TestMethod]
        public void CreateAccount_GetAccountByName_ReturnsAccount()
        {
            var newAccount = authDb.CreateAccount("testaccount1", "testpassword1", AccessLevel.Player, IPAddress.Parse("127.0.0.1"));

            var results = authDb.GetAccountByName(newAccount.AccountName);
            Assert.IsNotNull(results);
            Assert.AreEqual((uint)AccessLevel.Player, results.AccessLevel);
        }

        [TestMethod]
        public void UpdateAccountAccessLevelToSentinelAndBackToPlayer_ReturnsAccount()
        {
            Account newAccount = new Account();
            newAccount.AccountName = "testaccount1";

            authDb.UpdateAccountAccessLevel(1, AccessLevel.Sentinel);
            var results = authDb.GetAccountByName(newAccount.AccountName);
            Assert.IsNotNull(results);
            Assert.AreEqual((uint)AccessLevel.Sentinel, results.AccessLevel);

            authDb.UpdateAccountAccessLevel(1, AccessLevel.Player);
            var results2 = authDb.GetAccountByName(newAccount.AccountName);
            Assert.IsNotNull(results2);
            Assert.AreEqual((uint)AccessLevel.Player, results2.AccessLevel);
        }

        [TestMethod]
        public void GetAccountIdByName_ReturnsAccount()
        {
            Account newAccount = new Account();
            newAccount.AccountName = "testaccount1";

            var id = authDb.GetAccountIdByName(newAccount.AccountName);
            var results = authDb.GetAccountById(id);
            Assert.IsNotNull(results);
            Assert.AreEqual(id, results.AccountId);
            Assert.AreEqual(newAccount.AccountName, results.AccountName);
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
