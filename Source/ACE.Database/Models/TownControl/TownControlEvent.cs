using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Database.Models.TownControl
{
    public partial class TownControlEvent
    {
        public uint EventId { get; set; }

        public uint TownId { get; set; }

        public DateTime? EventStartDateTime { get; set; }

        public DateTime? EventEndDateTime { get; set; }

        public uint AttackingClanId { get; set; }

        public uint? DefendingClanId { get; set; }

        public string AttackingClanName { get; set; }

        public string DefendingClanName { get; set; }

        public bool? IsAttackSuccess { get; set; }
    }
}
