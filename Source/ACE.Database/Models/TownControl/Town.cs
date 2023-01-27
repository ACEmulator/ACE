using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Database.Models.TownControl
{
    public partial class Town
    {
        public uint TownId { get; set; }

        public string TownName { get; set; }

        public uint? CurrentOwnerID { get; set; }

        public bool IsInConflict { get; set; }

        public DateTime? LastConflictStartDateTime { get; set; }

        public uint ConflictLength { get; set; }

        public uint? ConflictRespiteLength { get; set; }

        public uint AttackerAwardsPerPerson { get; set; }

        public uint AttackerAwardsTotal { get; set; }

        public uint DefenderAwardsPerPerson { get; set; }

        public uint DefenderAwardsTotal { get; set; }

    }
}
