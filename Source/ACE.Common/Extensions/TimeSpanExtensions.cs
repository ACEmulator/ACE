using System;
using System.Text;

namespace ACE.Common.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string GetFriendlyString(this TimeSpan timeSpan)
        {
            // use datetime here?
            // this probably won't work with numDays...
            //var numYears = timeSpan.GetYears();
            //var numMonths = timeSpan.GetMonths();

            var numDays = timeSpan.ToString("%d");
            var numHours = timeSpan.ToString("%h");
            var numMinutes = timeSpan.ToString("%m");
            var numSeconds = timeSpan.ToString("%s");

            var sb = new StringBuilder();

            // did retail display months/years?

            //if (numYears > 0) sb.Append(numYears + "y ");
            //if (numMonths > 0) sb.Append(numMonths + "mo ");

            if (numDays != "0") sb.Append(numDays + "d ");
            if (numHours != "0") sb.Append(numHours + "h ");
            if (numMinutes != "0") sb.Append(numMinutes + "m ");
            if (numSeconds != "0") sb.Append(numSeconds + "s ");

            return sb.ToString().Trim();
        }

        public static string GetFriendlyLongString(this TimeSpan timeSpan)
        {
            var numDays = timeSpan.ToString("%d");
            var numHours = timeSpan.ToString("%h");
            var numMinutes = timeSpan.ToString("%m");
            var numSeconds = timeSpan.ToString("%s");

            var sb = new StringBuilder();

            if (numDays != "0") sb.Append(numDays + $" day{((timeSpan.Days > 1) ? "s" : "")} ");
            if (numHours != "0") sb.Append($"{((numDays != "0") ? ", " : "")}" + numHours + $" hour{((timeSpan.Hours > 1) ? "s" : "")} ");
            if (numMinutes != "0") sb.Append($"{((numDays != "0" || numHours != "0") ? ", " : "")}" + numMinutes + $" minute{((timeSpan.Minutes > 1) ? "s" : "")} ");
            if (numSeconds != "0") sb.Append($"{((numDays != "0" || numHours != "0" || numMinutes != "0") ? "and " : "")}" + numSeconds + $" second{((timeSpan.Seconds > 1) ? "s" : "")} ");

            return sb.ToString().Trim();
        }

        public static uint SecondsPerMonth = 60 * 60 * 24 * 30;      // 30-day estimate
        public static uint SecondsPerYear = 60 * 60 * 24 * 365;      // non-leap year

        public static uint GetMonths(this TimeSpan timeSpan)
        {
            return (uint)timeSpan.TotalSeconds / SecondsPerMonth;
        }

        public static uint GetYears(this TimeSpan timeSpan)
        {
            return (uint)timeSpan.TotalSeconds / SecondsPerYear;
        }
    }
}
