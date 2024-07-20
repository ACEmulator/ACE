using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Auth;

public partial class Account
{
    public uint AccountId { get; set; }

    public string AccountName { get; set; }

    /// <summary>
    /// base64 encoded version of the hashed passwords.  88 characters are needed to base64 encode SHA512 output.
    /// </summary>
    public string PasswordHash { get; set; }

    /// <summary>
    /// This is no longer used, except to indicate if bcrypt is being employed for migration purposes. Previously: base64 encoded version of the password salt.  512 byte salts (88 characters when base64 encoded) are recommend for SHA512.
    /// </summary>
    public string PasswordSalt { get; set; }

    public uint AccessLevel { get; set; }

    public string EmailAddress { get; set; }

    public DateTime CreateTime { get; set; }

    public byte[] CreateIP { get; set; }

    public DateTime? LastLoginTime { get; set; }

    public byte[] LastLoginIP { get; set; }

    public uint TotalTimesLoggedIn { get; set; }

    public DateTime? BannedTime { get; set; }

    public uint? BannedByAccountId { get; set; }

    public DateTime? BanExpireTime { get; set; }

    public string BanReason { get; set; }

    public virtual Accesslevel AccessLevelNavigation { get; set; }
}
