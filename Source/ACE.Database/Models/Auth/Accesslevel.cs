using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Auth
{
    public partial class Accesslevel
    {
        public Accesslevel()
        {
            Account = new HashSet<Account>();
        }

        public uint Level { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }

        public virtual ICollection<Account> Account { get; set; }
    }
}
