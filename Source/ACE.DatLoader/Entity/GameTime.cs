using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class GameTime
    {
        public UInt64 ZeroTimeOfYear { get; set; }
        public uint ZeroYear { get; set; } // Year "0" is really "P.Y. 10" in the calendar.
        public uint DayLength { get; set; }
        public uint DaysPerYear { get; set; } // 360. Likely for easier math so each month is same length
        public string YearSpec { get; set; } // "P.Y."
        public List<TimeOfDay> TimesOfDay { get; set; } = new List<TimeOfDay>();
        public List<string> DaysOfTheWeek { get; set; } = new List<string>();
        public List<Season> Seasons { get; set; } = new List<Season>();

        public static GameTime Read(DatReader datReader)
        {
            GameTime obj = new GameTime();
            obj.ZeroTimeOfYear = datReader.ReadUInt64();
            obj.ZeroYear = datReader.ReadUInt32();
            obj.DayLength = datReader.ReadUInt32();
            obj.DaysPerYear = datReader.ReadUInt32();
            obj.YearSpec = datReader.ReadPString();
            datReader.AlignBoundary();

            uint numTimesOfDay = datReader.ReadUInt32();
            for (uint i = 0; i < numTimesOfDay; i++)
            {
                obj.TimesOfDay.Add(TimeOfDay.Read(datReader));
            }

            uint numDaysOfTheWeek = datReader.ReadUInt32();
            for (uint i = 0; i < numDaysOfTheWeek; i++)
            {
                obj.DaysOfTheWeek.Add(datReader.ReadPString());
                datReader.AlignBoundary();
            }

            uint numSeasons = datReader.ReadUInt32();
            for (uint i = 0; i < numSeasons; i++)
            {
                obj.Seasons.Add(Season.Read(datReader));
            }

            return obj;
        }
    }
}
