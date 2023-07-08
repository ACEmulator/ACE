using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Database.Models.Log
{
    public partial class ArenaEvent
    { 
        public uint Id { get; set; }

        public string EventType { get; set; }

        public int Status { get; set; }

        public uint Location { get; set; }

        [NotMapped]
        public List<ArenaPlayer> Players { get; set; }

        [NotMapped]
        public DateTime? PreEventCountdownStartDateTime { get; set; }

        [NotMapped]
        public DateTime? CountdownStartDateTime { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public Guid? WinningTeamGuid { get; set; }

        public string CancelReason { get; set; }
    }
}
