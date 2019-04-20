using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Auth
{
    public partial class Account
    {
        public uint AccountId { get; set; }
        public string AccountName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public uint AccessLevel { get; set; }

        public Accesslevel AccessLevelNavigation { get; set; }
    }
}
