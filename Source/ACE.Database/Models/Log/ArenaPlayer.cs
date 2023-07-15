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

        public string EventTypeDisplay
        {
            get
            {
                switch(EventType)
                {
                    case "ffa":
                        return "Free for All";

                    default:
                        return EventType;
                }
            }
        }

        public uint MonarchId { get; set; }

        public string MonarchName { get; set; }

        public string PlayerIP { get; set; }

        public uint? EventId { get; set; }

        public Guid? TeamGuid { get; set; }

        public DateTime CreateDateTime { get; set; }

        public bool IsEliminated { get; set; }

        [NotMapped]
        public bool IsDisqualified
        {
            get
            {
                return FinishPlace == -1;
            }
        }

        public int FinishPlace { get; set; }

        [NotMapped]
        public string FinishPlaceDisplay
        {
            get
            {
                string suffix = "";
                switch(Int32.Parse(FinishPlace.ToString().Last().ToString()))
                {
                    case 1:
                        suffix = "st";
                        break;
                    case 2:
                        suffix = "nd";
                        break;
                    case 3:
                        suffix = "rd";                    
                        break;
                    default:
                        if(FinishPlace > 3)
                            suffix = "th";
                        break;
                }

                return FinishPlace == -1? "Disqualified" : $"{FinishPlace}{suffix}";
            }
        }

        public uint TotalDmgReceived { get; set; }

        public uint TotalDmgDealt { get; set; }

        public uint TotalKills { get; set; }

        public uint TotalDeaths { get; set; }
        
    }
}
