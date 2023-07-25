using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Database.Models.Log
{
    public partial class ArenaCharacterStats
    {
        public uint Id { get; set; }

        public uint CharacterId { get; set; }

        public string CharacterName { get; set; }

        [NotMapped]
        public uint CharacterLevel { get; set; }

        public string EventType { get; set; }

        public uint GetRankPoints()
        {
            var winPts = TotalWins * 10;
            var dqPts = TotalDisqualified * 10;
            var lossPts = TotalLosses * 5;
            var rankPts = winPts + TotalDraws - lossPts - dqPts;
            return rankPts < 0 ? 0 : rankPts;
        }

        public uint RankPoints { get; set; }

        public uint TotalMatches { get; set; }

        public uint TotalWins { get; set; }

        public uint TotalDraws { get; set; }

        public uint TotalLosses { get; set; }

        public uint TotalDisqualified { get; set; }

        public uint TotalKills { get; set; }

        public uint TotalDeaths { get; set; }

        public uint TotalDmgReceived { get; set; }

        public uint TotalDmgDealt { get; set; }
        
    }
}
