using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ACE.Common;
using ACE.Entity.Enum;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("vw_account_by_name")]
    public class AccountByName
    {
        /// <summary>
        /// creates a new account object and pre-creates a new, random salt
        /// </summary>
        public AccountByName()
        {
        }
        
        [DbField("accountId", (int)MySqlDbType.UInt32, Insert = false, Update = false)]
        public uint AccountId { get; set; }

        /// <summary>
        /// login name of the account.  for now, this is immutable.
        /// </summary>
        [DbField("accountName", (int)MySqlDbType.VarChar, IsCriteria = true, Update = false)]
        public string Name { get; set; }
    }
}
