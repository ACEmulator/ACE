using ACE.Cryptography;

namespace ACE.Entity
{
    public class Account
    {
        public uint AccountId { get; private set; }

        public string Name { get; private set; }

        public HashedPassword HashedPassword { get; private set; }

        public Account(uint accountId, string name, HashedPassword hp)
        {
            AccountId = accountId;
            Name = name;
            HashedPassword = hp;
        }
    }
}
