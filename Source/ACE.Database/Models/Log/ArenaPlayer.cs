using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Database.Models.Log
{
    public partial class ArenaPlayer
    {
        public uint Id { get; set; }

        public uint CharacterId { get; set; }

        public string CharacterName { get; set; }

        public uint CharacterLevel { get; set; }

        public string EventType { get; set; }

        public uint MonarchId { get; set; }

        public string MonarchName { get; set; }

        public string PlayerIP { get; set; }

        public uint? EventId { get; set; }

        public Guid? TeamGuid { get; set; }

        public DateTime CreateDateTime { get; set; }

        public bool IsEliminated { get; set; }

        public uint TotalDmgReceived { get; set; }

        public uint TotalDmgDealt { get; set; }

        public uint TotalKills { get; set; }

        public uint TotalDeaths { get; set; }
        
    }
}
