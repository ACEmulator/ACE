using ACE.Entity.Enum;
using ACE.Common.Cryptography;

namespace ACE.Entity
{
    public class Account
    {
        public uint AccountId { get; private set; }

        public string Name { get; private set; }

        public AccessLevel AccessLevel { get; private set; }

        public string Salt { get; private set; }

        public string Digest { get; private set; }

        public string Password { private get; set; }

        public Account(uint accountId, string name, AccessLevel accessLevel, string salt, string password)
        {
            AccountId = accountId;
            Name = name;
            AccessLevel = accessLevel;
            Salt = salt;
            Password = password;
            Digest = SHA2.Hash(SHA2Type.SHA256, password + salt);
        }
    }
}
