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
        public string EmailAddress { get; set; }
        public string CreateIP { get; set; }
        public DateTime CreateTime { get; set; }
        public string LastLoginIP { get; set; }
        public DateTime LastLoginTime { get; set; }
        public bool? IsBanned { get; set; }
        public DateTime? BannedTime { get; set; }
        public uint? BannedByAccountID { get; set; }
        public DateTime? BanExpireTime { get; set; }

        public Accesslevel AccessLevelNavigation { get; set; }
    }
}
