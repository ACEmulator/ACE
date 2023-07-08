using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Log
{
    public partial class CharacterLoginLog
    {
        public uint Id { get; set; }
        public uint AccountId { get; set; }
        public string AccountName { get; set; }
        public string SessionIP { get; set; }
        public uint CharacterId { get; set; }
        public string CharacterName{ get; set; }
        public DateTime LoginDateTime { get; set; }

        public DateTime? LogoutDateTime { get; set; }
    }
}
