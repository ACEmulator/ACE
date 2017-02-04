using Sodium;
using ACE.Managers;

namespace ACE
{
    public class HashedPassword
    {
        public string Hash { get; set; }
        
        public bool Verify(string password)
        {
            return PasswordHash.ScryptHashStringVerify(this.Hash, password);
        }
    }

    public static class PasswordHasher
    {
        public const int MIN_PWLENGTH = 8;

        public static HashedPassword HashPassword(string password)
        {
            var dev_mode = ConfigManager.Config.Server.DevMode;

            if (!dev_mode &&password.Length < MIN_PWLENGTH)
            {
                    throw new System.ArgumentException("Password length should be at least " + MIN_PWLENGTH);
                
            }

            var hp = new HashedPassword();
            hp.Hash = PasswordHash.ScryptHashString(password, PasswordHash.Strength.Medium);

            return hp;
        }
    }
}