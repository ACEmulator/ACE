
namespace ACE.Common
{
    public class AccountDefaults
    {
        /// <summary>
        /// whether or not the server uses account level permissions or character level permissions (as AC retail did). for backwards compatibility, this is true
        /// by default.
        /// </summary>
        public bool OverrideCharacterPermissions { get; set; } = true;

        /// <summary>
        /// Default AccessLevel for new accounts. Used when accesslevel is not specified by user.  for backwards compatibility, this is 0 (Player)
        /// by default.
        /// </summary>
        public uint DefaultAccessLevel { get; set; } = 0;

        /// <summary>
        /// whether or not this server creates new accounts when one does not exist.  for backwards compatibility, this is true
        /// by default.
        /// </summary>
        public bool AllowAutoAccountCreation { get; set; } = true;

        /// <summary>
        /// Default WorkFactor for account passwords.
        /// 8 by default.
        /// </summary>
        public int PasswordHashWorkFactor { get; set; } = 8;

        /// <summary>
        /// Upgrade or downgrade passwords to match PasswordHashWorkFactor specified in config 
        /// True by default.
        /// </summary>
        public bool ForceWorkFactorMigration { get; set; } = true;
    }
}
