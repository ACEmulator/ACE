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

        public uint GetRankPoints()
        {
            var winPts = (((float)TotalWins / (TotalMatches == 0 ? 1.0f : (float)TotalMatches)) * 100.0f);
            var drawPts = (((float)TotalDraws / (TotalMatches == 0 ? 1.0f : (float)TotalMatches)) * 10.0f);
            var matchPts = (TotalMatches / 100);
            var dmgPts = ((float)TotalDmgDealt / (TotalMatches == 0 ? 1.0f : (float)TotalMatches) / 1000.0f);
            var killPts = ((float)TotalKills / 10.0f);
            var dqPts = ((float)TotalDisqualified / 10.0f);
            var totalPts = winPts + drawPts + matchPts + dmgPts + killPts - dqPts;
            return Convert.ToUInt32(Math.Round(totalPts));
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
