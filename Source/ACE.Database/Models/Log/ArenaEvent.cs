using Pomelo.EntityFrameworkCore.MySql.Storage.Internal;
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

        [NotMapped]
        public string EventTypeDisplay
        {
            get
            {
                switch (EventType)
                {
                    case "ffa":
                        return "Free for All";

                    default:
                        return EventType;
                }
            }
        }

        public int Status { get; set; }

        public uint Location { get; set; }

        [NotMapped]
        public List<ArenaPlayer> Players { get; set; }

        [NotMapped]
        public string PlayersDisplay
        {
            get
            {
                string returnMsg = "";

                Dictionary<Guid, List<ArenaPlayer>> teams = new Dictionary<Guid, List<ArenaPlayer>>();
                foreach (var player in Players)
                {
                    if (player.TeamGuid.HasValue && teams.ContainsKey(player.TeamGuid.Value))
                    {
                        teams[player.TeamGuid.Value].Add(player);
                    }
                    else
                    {
                        var playerList = new List<ArenaPlayer>();
                        playerList.Add(player);

                        teams.Add(player.TeamGuid.HasValue ? player.TeamGuid.Value : Guid.NewGuid(), playerList);
                    }
                }

                var recCount = 0;
                foreach (var team in teams)
                {
                    recCount++;
                    for (int i = 0; i < team.Value.Count(); i++)
                    {
                        var player = team.Value[i];

                        if (i == 0)
                        {
                            returnMsg += $"{player.CharacterName}";
                        }
                        else if (i == team.Value.Count() - 1)
                        {
                            returnMsg += $" and {player.CharacterName}";
                        }
                        else
                        {
                            returnMsg += $", {player.CharacterName}";
                        }
                    }

                    if (recCount < teams.Count())
                    {
                        returnMsg += " vs. ";
                    }
                }

                return string.IsNullOrEmpty(returnMsg) ? "no players" : returnMsg;
            }
        }


        [NotMapped]
        public DateTime? PreEventCountdownStartDateTime { get; set; }

        [NotMapped]
        public DateTime? CountdownStartDateTime { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public Guid? WinningTeamGuid { get; set; }

        public string CancelReason { get; set; }

        [NotMapped]
        public TimeSpan TimeRemaining
        {
            get
            {
                if (!StartDateTime.HasValue || EndDateTime.HasValue)
                    return TimeSpan.Zero;

                if (this.Status < 4)
                {
                    switch (this.EventType)
                    {
                        case "1v1":
                        case "2v2":
                            return new TimeSpan(0, 15, 0);
                        case "ffa":
                            return new TimeSpan(0, 25, 0);
                        default:
                            return TimeSpan.Zero;
                    }
                }
                else if (this.Status > 4)
                {
                    return TimeSpan.Zero;
                }
                else
                {
                    switch (this.EventType)
                    {
                        case "1v1":
                        case "2v2":
                            return this.StartDateTime.Value.AddMinutes(15) - DateTime.Now;
                        case "ffa":
                            return this.StartDateTime.Value.AddMinutes(25) - DateTime.Now;
                        default:
                            return TimeSpan.Zero;
                    }
                }
            }
        }

        public string TimeRemainingDisplay
        {
            get
            {
                return string.Format("{0:%h}h {0:%m}m {0:%s}s", TimeRemaining);
            }
        }
    }
}
