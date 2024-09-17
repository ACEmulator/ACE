using System;

namespace ACE.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToCommonString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd h:mm:ss tt");
        }
    }
}
