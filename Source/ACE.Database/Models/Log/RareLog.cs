using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Log
{
    public partial class RareLog
    {
        public uint Id { get; set; }

        public uint CharacterId { get; set; }

        public string CharacterName { get; set; }

        public uint ItemBiotaId { get; set; }

        public uint ItemWeenieId { get; set; }

        public string ItemName { get; set; }

        public DateTime? CreatedDateTime { get; set; }
    }
}
