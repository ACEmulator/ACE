using ACE.Cryptography;

namespace ACE.Entity
{
    public class Account
    {
        public uint AccountId { get; private set; }

        public string Name { get; private set; }

        public string Salt { get; private set; }

        public string Digest { get; private set; }

        public string Password { private get; set; }

        public Account(uint accountId, string name, string salt, string password)
        {
            AccountId = accountId;
            Name = name;
            Salt = salt;
            Password = password;
            Digest = SHA2.Hash(SHA2Type.SHA256, password + salt);
        }

    }
}
