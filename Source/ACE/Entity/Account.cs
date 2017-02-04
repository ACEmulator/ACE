using ACE.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public class Account
    {
        public Account(uint accountId, string name, string salt, string password)
        {
            AccountId = accountId;
            Name = name;
            Salt = salt;
            Password = password;
            Digest = SHA2.Hash(SHA2Type.SHA256, password + salt);
        }

        public uint AccountId { get; private set; }

        public string Name { get; private set; }

        public string Salt { get; private set; }

        public string Digest { get; private set; }

        public string Password { private get; set; }
    }
}
