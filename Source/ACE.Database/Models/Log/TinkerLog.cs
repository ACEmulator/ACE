using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Log
{
    public partial class TinkerLog
    {
        public uint Id { get; set; }        
        public uint CharacterId { get; set; }
        public string CharacterName{ get; set; }

        public uint ItemBiotaId { get; set; }
        public DateTime TinkDateTime { get; set; }
        public float SuccessChance { get; set; }
        public float Roll { get; set; }
        public bool IsSuccess { get; set; }
        public uint ItemNumPreviousTinks { get; set; }
        public uint ItemWorkmanship { get; set; }
        public string SalvageType { get; set; }
        public uint SalvageWorkmanship { get; set; }

    }
}
