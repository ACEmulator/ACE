using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class BiotaPropertiesAllegiance
    {
        public uint AllegianceId { get; set; }
        public uint CharacterId { get; set; }
        public bool Banned { get; set; }
        public bool ApprovedVassal { get; set; }

        public virtual Biota Allegiance { get; set; }
        public virtual Character Character { get; set; }
    }
}
