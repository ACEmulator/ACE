using ACE.Entity.Enum;
using ACE.Common.Cryptography;

namespace ACE.Entity
{
    public class Account
    {
        private string _password;

        public uint AccountId { get; private set; }

        public string Name { get; set; }

        public AccessLevel AccessLevel { get; set; }

        public string Salt { get; private set; }
        
        /// <summary>
        /// the hashed, nonrecoverable password.  if set, Salt must be populated.
        /// </summary>
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;

                if (string.IsNullOrWhiteSpace(Salt))
                    throw new System.InvalidOperationException("Cannot set password without a salt.");

                // rehash the password if it is ever set
                _password = SHA2.Hash(SHA2Type.SHA256, value + Salt);
            }
        }

        public string Email { get; set; }
        
        public Account(uint accountId, string name, AccessLevel accessLevel, string salt, string password)
        {
            AccountId = accountId;
            Name = name;
            AccessLevel = accessLevel;
            Salt = salt;
            Password = password;
        }
    }
}
