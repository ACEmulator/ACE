using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Log
{
    public partial class AccountSessionLog
    {
        public uint Id { get; set; }
        public uint AccountId { get; set; }
        public string AccountName { get; set; }
        public string SessionIP { get; set; }
        public DateTime LoginDateTime { get; set; }

        public DateTime? LogoutDateTime { get; set; }
    }
}
