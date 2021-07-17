using System;

namespace ACE.Common
{
    /// <summary>
    /// <para>Represents an instant in time, typically expressed as a date, portal year and time of day in Dereth.</para>
    /// </summary>
    public class DerethDateTime
    {
        // Changing the four variables below will result in the Date and Time reported in ACClient no longer matching
        private static int hoursInADay      = 16;   // A Derethian day has 16 hours
        private static int daysInAMonth     = 30;   // A Derethian month has 30 days and does not vary like Earth months.
        private static int monthsInAYear    = 12;   // A Derethian year has 12 months
        private static double dayTicks      = 7620; // A Derethain day has 7620 ticks per day

        private static double hourTicks     = dayTicks / hoursInADay;
        private static double minuteTicks   = hourTicks / 60;
        private static double secondTicks   = minuteTicks / 60;

        private static double monthTicks    = dayTicks * daysInAMonth;
        private static double yearTicks     = monthTicks * monthsInAYear;

        private static double dayZeroTicks  = 0; // Morningthaw 1, 10 P.Y. - Morntide-and-Half
        private static double hourOneTicks  = 210; // Morningthaw 1, 10 P.Y. - Midsong
        private static double dayOneTicks   = dayZeroTicks + hourOneTicks + (hourTicks * 8); // Morningthaw 2, 10 P.Y. - Darktide
        private static double yearOneTicks  = dayOneTicks + (dayTicks * 359); // Morningthaw 1, 11 P.Y. - Darktide
        private static double yearZeroTicks = dayOneTicks + (dayTicks * 269); // Snowreap 1, 10 P.Y. - Darktide

        private static DateTime dayZero_RealWorld       = new DateTime(1999, 4, 1, 10, 30, 00);
        private static DateTime dayOne_RealWorld        = new DateTime(1999, 4, 2, 00, 00, 00);

        private static DateTime retailDayOne_RealWorld  = new DateTime(1999, 11, 2, 00, 00, 00);
        private static DateTime retailDayLast_RealWorld = new DateTime(2017, 1, 31, 12, 00, 00);

        /// <summary>
        /// <para>A <see cref="DerethDateTime"/> instance set to the Derethian Date, Portal Year and Time when the worlds first opened.</para>
        /// </summary>
        private static DerethDateTime retailDayOne_Derethian            = new DerethDateTime(10, Months.Leafcull, 1, Hours.Darktide);

        /// <summary>
        /// <para>A <see cref="DerethDateTime"/> instance set to the Lore Corrected Derethian Date, Portal Year and Time when the worlds first opened.</para>
        /// </summary>
        private static DerethDateTime retailDayOne_Derethian_Lore       = ConvertRealWorldToLoreDateTime(new DateTime(1999, 11, 2, 00, 00, 00));

        /// <summary>
        /// <para>A <see cref="DerethDateTime"/> instance set to the Derethian Date, Portal Year and Time when the worlds closed.</para>
        /// </summary>
        private static DerethDateTime retailDayLast_Derethian           = new DerethDateTime(206, Months.Solclaim, 24, Hours.Gloaming);

        /// <summary>
        /// <para>A <see cref="DerethDateTime"/> instance set to the Lore Corrected Derethian Date, Portal Year and Time when the worlds closed.</para>
        /// </summary>
        private static DerethDateTime retailDayLast_Derethian_Lore      = ConvertRealWorldToLoreDateTime(new DateTime(2017, 1, 31, 12, 00, 00));

        /// <summary>
        /// <para>Date: Morningthaw 1, 10 P.Y. | Time: Morntide-and-Half (0)</para>
        /// </summary>
        public static readonly double MinValue = 0; // Morningthaw 1, 10 P.Y. - Morntide-and-Half

        /// <summary>
        /// <para>Any value higher than this results in acclient crashing upon connection to server.</para>
        /// <para>Date: Thistledown 2, 401 P.Y. | Time: Morntide-and-Half (1073741828)</para>
        /// </summary>
        public static readonly double MaxValue = (yearTicks * 391) + (monthTicks * 5) + (dayTicks * 1) + 4; // Thistledown 2, 401 P.Y. - Morntide-and-Half (1073741828)

        /// <summary>
        /// Months of the Portal Year
        /// </summary>
        public enum Months
        {
            Snowreap    = -2,
            Coldeve     = -1,
            Wintersebb  = 0,
            Morningthaw = 1,
            Solclaim,
            Seedsow,
            Leafdawning,
            Verdantine,
            Thistledown,
            HarvestGain,
            Leafcull,
            Frostfell
        }

        /// <summary>
        /// Seasons of the Portal Year
        /// </summary>
        public enum Seasons
        {
            Winter,
            Spring,
            Summer,
            Autumn,
        }

        /// <summary>
        /// Hours of the Day
        /// </summary>
        public enum Hours
        {
            Darktide = 1,
            Darktide_and_Half,
            Foredawn,
            Foredawn_and_Half,
            Dawnsong,
            Dawnsong_and_Half,
            Morntide,
            Morntide_and_Half,
            Midsong,
            Midsong_and_Half,
            Warmtide,
            Warmtide_and_Half,
            Evensong,
            Evensong_and_Half,
            Gloaming,
            Gloaming_and_Half
        }

        /// <summary>
        /// Days of the Week
        /// </summary>
        public enum Days
        {
            FirstDay = 1,
            StarDay,
            EarthDay,
            MoonsDay,
            ElderDay,
            FreeDay,
        }

        /// <summary>
        /// Time of Day
        /// </summary>
        public enum Daytime
        {
            Day,
            Night
        }

        private double ticks    = MinValue;
        private int day         = 1;
        private int month       = (int)Months.Morningthaw;
        private int year        = 10;
        private int hour        = (int)Hours.Morntide_and_Half;

        /// <summary>
        /// Gets the number of ticks that represent the date and time of this instance.
        /// </summary>
        public double Ticks
        {
            get { return ticks; }
            private set
            {
                if ((value >= MinValue) && (value <= MaxValue))
                {
                    ticks = value;
                }
                if ((value <= MinValue))
                {
                    ticks   = MinValue;
                    day     = 1;
                    month   = (int)Months.Morningthaw;
                    year    = 10;
                    hour    = (int)Hours.Morntide_and_Half;
                }
                if ((value >= MaxValue))
                {
                    ticks   = MaxValue;
                    day     = 2;
                    month   = (int)Months.Thistledown;
                    year    = 401;
                    hour    = (int)Hours.Morntide_and_Half;
                }
            }
        }

        /// <summary>
        /// Gets the day of the month represented by this instance.
        /// </summary>
        public int Day
        {
            get { return day; }
            private set
            {
                if ((value >= 1) && (value <= 30))
                {
                    day = value;
                }
                if ((value < 1))
                {
                    Month--;
                    day = 30;
                }
                if ((value > 30))
                {
                    Month++;
                    day = 1;
                }
            }
        }

        /// <summary>
        /// Gets the month component of the date represented by this instance.
        /// </summary>
        public int Month
        {
            get { return month; }
            private set
            {
                if ((value >= (int)Months.Snowreap) && (value <= (int)Months.Frostfell))
                {
                    if (month == (int)Months.Morningthaw && value == (int)Months.Wintersebb)
                        Year--;
                    if (month == (int)Months.Wintersebb && value == (int)Months.Morningthaw)
                        Year++;
                    month = value;
                }
                if ((value < (int)Months.Snowreap))
                {
                    month = (int)Months.Snowreap;
                }
                if ((value > (int)Months.Frostfell))
                {
                    month = (int)Months.Snowreap;
                }
            }
        }

        /// <summary>
        /// Gets the month name component of the date represented by this instance.
        /// </summary>
        public Months MonthName { get { return (Months)Month; } }

        /// <summary>
        /// Gets the year component of the date represented by this instance.
        /// </summary>
        public int Year
        {
            get { return year; }
            private set
            {
                if ((value >= 10) && (value <= 401))
                {
                    year = value;
                }
                if ((value < 10))
                {
                    year = 10;
                }
                if ((value > 401))
                {
                    year = 401;
                }
            }
        }

        /// <summary>
        /// Gets the year component of the date represented by this instance.
        /// </summary>
        public int PY { get { return year; } }

        /// <summary>
        /// Gets the year component of the date represented by this instance.
        /// </summary>
        public int PortalYear { get { return year; } }

        /// <summary>
        /// Gets the hour component of the time represented by this instance.
        /// </summary>
        public int Hour
        {
            get { return hour; }
            private set
            {
                if ((value >= (int)Hours.Darktide) && (value <= (int)Hours.Gloaming_and_Half))
                {
                    hour = value;
                }
                if ((value < (int)Hours.Darktide))
                {
                    Day--;
                    hour = (int)Hours.Gloaming_and_Half;
                }
                if ((value > (int)Hours.Gloaming_and_Half))
                {
                    Day++;
                    hour = (int)Hours.Darktide;
                }
            }
        }

        /// <summary>
        /// Gets the hour name component of the time represented by this instance.
        /// </summary>
        public Hours HourName { get { return (Hours)Hour; } }

        /// <summary>
        /// Gets the hour component of the time represented by this instance.
        /// </summary>
        public int Time { get { return hour; } }

        /// <summary>
        /// Gets the hour name component of the time represented by this instance.
        /// </summary>
        public Hours TimeName { get { return (Hours)Hour; } }

        /// <summary>
        /// Gets the time of day for this instance.
        /// </summary>
        public Daytime TimeOfDay
        {
            get
            {
                if (Hour >= (int)Hours.Dawnsong && Hour <= (int)Hours.Warmtide_and_Half)
                    return Daytime.Day;
                return Daytime.Night;
            }
        }

        /// <summary>
        /// Indicates whether this instance of <see cref="DerethDateTime"/> is within the day time range for the current day.
        /// </summary>
        public bool IsDaytime
        {
            get
            {
                if (TimeOfDay == Daytime.Day)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Indicates whether this instance of <see cref="DerethDateTime"/> is within the night time range for the current day.
        /// </summary>
        public bool IsNighttime
        {
            get
            {
                if (TimeOfDay == Daytime.Night)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Indicates whether this instance of <see cref="DerethDateTime"/> is within the day time range for the current day.
        /// </summary>
        public bool IsDay
        {
            get
            {
                return IsDaytime;
            }
        }

        /// <summary>
        /// Indicates whether this instance of <see cref="DerethDateTime"/> is within the night time range for the current day.
        /// </summary>
        public bool IsNight
        {
            get
            {
                return IsNighttime;
            }
        }

        /// <summary>
        /// Gets the season for this instance.
        /// </summary>
        public Seasons Season
        {
            get
            {
                if (Month >= (int)Months.Snowreap && Month <= (int)Months.Wintersebb)
                    return Seasons.Winter;
                if (Month >= (int)Months.Morningthaw && Month <= (int)Months.Seedsow)
                    return Seasons.Spring;
                if (Month >= (int)Months.Leafdawning && Month <= (int)Months.Thistledown)
                    return Seasons.Summer;
                return Seasons.Autumn;
            }
        }

        /// <summary>
        /// Returns an indication whether the instance is within the specified season.
        /// </summary>
        public bool IsSeason(Seasons seasonToCheckFor)
        {
            if (Season == seasonToCheckFor)
                return true;
            return false;
        }

        /// <summary>
        /// Indicates whether this instance of <see cref="DerethDateTime"/> is within the winter season.
        /// </summary>
        public bool IsWinter
        {
            get
            {
                if (Season == Seasons.Winter)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Indicates whether this instance of <see cref="DerethDateTime"/> is within the spring season.
        /// </summary>
        public bool IsSpring
        {
            get
            {
                if (Season == Seasons.Spring)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Indicates whether this instance of <see cref="DerethDateTime"/> is within the summer season.
        /// </summary>
        public bool IsSummer
        {
            get
            {
                if (Season == Seasons.Summer)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Indicates whether this instance of <see cref="DerethDateTime"/> is within the autumn season.
        /// </summary>
        public bool IsAutumn
        {
            get
            {
                if (Season == Seasons.Autumn)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Indicates whether this instance of <see cref="DerethDateTime"/> is within the fall (autumn) season.
        /// </summary>
        public bool IsFall { get { return IsAutumn; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="DerethDateTime"/> class to set to <see cref="MinValue"/>.
        /// <para>Date: Morningthaw 1, 10 P.Y. | Time: Morntide-and-Half</para>
        /// </summary>
        public DerethDateTime()
        {
            // Morningthaw 1, 10 P.Y. - Morntide-and-Half
            Ticks = MinValue;
            Year = 10;
            Month = (int)Months.Morningthaw;
            Day = 1;
            Hour = (int)Hours.Morntide_and_Half;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DerethDateTime"/> class to a specified number of ticks.
        /// </summary>
        /// <param name="ticks">A date and time expressed in the number of "Derethian millisecond" intervals that have elapsed since Morningthaw 1, 10 P.Y. at Morntide-and-Half in the Derethian calendar.
        /// <para>Note that these ticks are not the same as <see cref="DateTime.Ticks"/></para></param>
        public DerethDateTime(double ticks = 0)
        {
            if (ticks < MinValue | ticks > MaxValue)
                throw new ArgumentOutOfRangeException("ticks", "ticks is less than DerethDateTime.MinValue or greater than DerethDateTime.MaxValue");

            Ticks = ticks;

            if (Ticks < MaxValue)
                SetDateTimeFromTicks();
        }

        /// <summary>
        /// Sets DerethDateTime mathematically from Ticks with requested Day, Month, Year and Hour.
        /// </summary>
        private void SetDateTimeFromTicks()
        {
            double hoursInTicks = Ticks / hourTicks;
            double round = Math.Round(hoursInTicks * 4, MidpointRounding.ToEven) / 4;
            int round_multiplier = 100;
            int round_decimals = (int)((round - (int)round) * round_multiplier);

            for (double i = 0; i < round; i++)
                Hour++;
            if (round_decimals == 25)
                Hour--;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DerethDateTime"/> class to the specified year, month, day and hour.
        /// </summary>
        /// <param name="year">The portal year (10 through 401).</param>
        /// <param name="month">The month (-2 through 9).
        /// <para>Note: The odd integer is due to the calendar portal year actually beginning on Morningthaw and not Snowreap.</para></param>
        /// <param name="day">The day (1 through 30).<para>There are 30 days for each month in the portal year.</para></param>
        /// <param name="hour">The hour or time (of day) (1 through 16).<para>Each day begins at Darktide and ends at Gloaming-and-Half.</para></param>
        public DerethDateTime(int year = 10, int month = (int)Months.Morningthaw, int day = 1, int hour = (int)Hours.Darktide)
        {
            if (year < 10 || year > 401)
                throw new ArgumentOutOfRangeException("year", "year is less than 10 or greater than 401");
            if (month < (int)Months.Snowreap || month > (int)Months.Frostfell)
                throw new ArgumentOutOfRangeException("month", "month is less than " + Months.Snowreap + " or greater than " + Months.Frostfell);
            if (day < 1 || day > 30)
                throw new ArgumentOutOfRangeException("day", "day is less than 1 or greater than 30");
            if (hour < (int)Hours.Darktide || hour > (int)Hours.Gloaming_and_Half)
                throw new ArgumentOutOfRangeException("hour", "time is less than " + Hours.Darktide + " or greater than " + Hours.Gloaming_and_Half);

            Year = year;
            Month = month;
            Day = day;

            Hour = hour;

            if (month == (int)Months.Wintersebb && year > 10)
                Ticks = GetTicksFromDateTime(true);
            else
                Ticks = GetTicksFromDateTime();
        }

        /// <summary>
        /// Mathematically derives Ticks from backing fields to match with Year, Month, Day, and Hour.
        /// </summary>
        /// <param name="fixWintersebbYearRollUnder">Used to correct the year calculation error that occurs when setting month to Wintersebb.</param>
        private double GetTicksFromDateTime(bool fixWintersebbYearRollUnder = false)
        {
            double ticks = 0;

            if (Year == 10 && Month == (int)Months.Morningthaw && Day == 1 && Hour < (int)Hours.Morntide_and_Half)
                return ticks;

            if (Year == 10 && Month == (int)Months.Morningthaw && Day == 1 && Hour == (int)Hours.Midsong)
                return hourOneTicks;

            ticks = ticks + hourOneTicks;

            if (Year == 10 && Month == (int)Months.Morningthaw && Day == 1 && Hour > (int)Hours.Midsong)
                return ticks + ((Hour - 9) * hourTicks);

            ticks = ticks + dayOneTicks;

            double yearsToAdd = 0;
            if (Month > (int)Months.Wintersebb)
                yearsToAdd = (Year - 10) * yearTicks;
            else
            {
                if (fixWintersebbYearRollUnder)
                    Year++;

                yearsToAdd = (Year - 9) * yearTicks;
            }

            double monthsToAdd = (Month - 1) * monthTicks;

            double daysToAdd = (Day - 2) * dayTicks;

            double hoursToAdd = (Hour - 1) * hourTicks;

            ticks = ticks + yearsToAdd + monthsToAdd + daysToAdd + hoursToAdd;

            return ticks;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DerethDateTime"/> class to the specified year, month, day and hour.
        /// </summary>
        /// <param name="year">The portal year (10 through 401).</param>
        /// <param name="month">The month (<see cref="Months"/>).
        /// <para>Note: The odd integer is due to the calendar portal year actually beginning on Morningthaw and not Snowreap.</para></param>
        /// <param name="day">The day (1 through 30).<para>There are 30 days for each month in the portal year.</para></param>
        /// <param name="time">The hour or time (of day) (<see cref="Hours"/>).<para>Each day begins at Darktide and ends at Gloaming-and-Half.</para></param>
        public DerethDateTime(int year = 10, Months month = Months.Morningthaw, int day = 1, Hours time = Hours.Darktide)
        {
            if (year < 10 || year > 401)
                throw new ArgumentOutOfRangeException("year", "year is less than 10 or greater than 401");
            if ((int)month < (int)Months.Snowreap || (int)month > (int)Months.Frostfell)
                throw new ArgumentOutOfRangeException("month", "month is less than " + Months.Snowreap + " or greater than " + Months.Frostfell);
            if (day < 1 || day > 30)
                throw new ArgumentOutOfRangeException("day", "day is less than 1 or greater than 30");
            if ((int)time < (int)Hours.Darktide || (int)time > (int)Hours.Gloaming_and_Half)
                throw new ArgumentOutOfRangeException("time", "time is less than " + Hours.Darktide + " or greater than " + Hours.Gloaming_and_Half);

            Year = year;
            Month = (int)month;
            Day = day;

            Hour = (int)time;

            if (month == Months.Wintersebb && year > 10)
                Ticks = GetTicksFromDateTime(true);
            else
                Ticks = GetTicksFromDateTime();
        }

        /// <summary>
        /// Returns a new <see cref="DerethDateTime"/> that adds the specified number of ticks to the value of this instance.
        /// </summary>
        /// <param name="numOfTicksToAdd">A number of ticks. The numOfTicksToAdd parameter can be negative or positive.</param>
        public DerethDateTime AddTicks(double numOfTicksToAdd)
        {
            if ((Ticks + numOfTicksToAdd) < MinValue | (Ticks + numOfTicksToAdd) > MaxValue)
                throw new ArgumentOutOfRangeException("numOfTicksToAdd", "numOfTicksToAdd results in less than DerethDateTime.MinValue or greater than DerethDateTime.MaxValue");

            return new DerethDateTime(ticks: Ticks + numOfTicksToAdd);
        }

        /// <summary>
        /// Returns a new <see cref="DerethDateTime"/> that subtracts the specified number of ticks from the value of this instance.
        /// </summary>
        /// <param name="numOfTicksToSubtract">A number of ticks. The numOfTicksToSubtract parameter can be negative or positive.</param>
        public DerethDateTime SubtractTicks(double numOfTicksToSubtract)
        {
            if ((Ticks + numOfTicksToSubtract) < MinValue | (Ticks + numOfTicksToSubtract) > MaxValue)
                throw new ArgumentOutOfRangeException("numOfTicksToSubtract", "numOfTicksToSubtract results in less than DerethDateTime.MinValue or greater than DerethDateTime.MaxValue");

            return new DerethDateTime(ticks: Ticks - numOfTicksToSubtract);
        }

        /// <summary>
        /// Returns a new <see cref="DerethDateTime"/> that adds the specified number of years to the value of this instance.
        /// </summary>
        /// <param name="numOfYearsToAdd">A number of years. The numOfYearsToAdd parameter can be negative or positive.</param>
        public DerethDateTime AddYears(int numOfYearsToAdd)
        {
            if ((Year + numOfYearsToAdd) < 10 | (Year + numOfYearsToAdd) > 401)
                throw new ArgumentOutOfRangeException("numOfYearsToAdd", "numOfYearsToAdd results in a portal year less than 10 or greater than 401");
            return new DerethDateTime(year: Year + numOfYearsToAdd, month: Month, day: Day, hour: Hour);
        }

        /// <summary>
        /// Returns a new <see cref="DerethDateTime"/> that subtracts the specified number of years from the value of this instance.
        /// </summary>
        /// <param name="numOfYearsToSubtract">A number of years. The numOfYearsToSubtract parameter can be negative or positive.</param>
        public DerethDateTime SubtractYears(int numOfYearsToSubtract)
        {
            if ((Year - numOfYearsToSubtract) < 10 | (Year - numOfYearsToSubtract) > 401)
                throw new ArgumentOutOfRangeException("numOfYearsToSubtract", "numOfYearsToSubtract results in a portal year less than 10 or greater than 401");
            return new DerethDateTime(year: Year - numOfYearsToSubtract, month: Month, day: Day, hour: Hour);
        }

        /// <summary>
        /// Returns a new <see cref="DerethDateTime"/> that adds the specified number of months to the value of this instance.
        /// </summary>
        /// <param name="numOfMonthsToAdd">A number of months. The numOfMonthsToAdd parameter can be negative or positive.</param>
        public DerethDateTime AddMonths(int numOfMonthsToAdd)
        {
            int newYear = Year;
            int newMonth = Month;

            if (numOfMonthsToAdd > 0)
            {
                for (double i = 0; i < numOfMonthsToAdd; i++)
                {
                    if (newMonth == (int)Months.Wintersebb)
                    {
                        newYear++;
                        newMonth++;
                    }
                    else if (newMonth == (int)Months.Frostfell)
                    {
                        newMonth = (int)Months.Snowreap;
                    }
                    else
                        newMonth++;
                }
            }
            else
            {
                for (double i = 0; i > numOfMonthsToAdd; i--)
                {
                    if (newMonth == (int)Months.Morningthaw)
                    {
                        newYear--;
                        newMonth--;
                    }
                    else if (newMonth == (int)Months.Snowreap)
                    {
                        newMonth = (int)Months.Frostfell;
                    }
                    else
                        newMonth--;
                }
            }

            if (newYear < 10 | newYear > 401)
                throw new ArgumentOutOfRangeException("numOfMonthsToAdd", "numOfMonthsToAdd results in a portal year less than 10 or greater than 401");

            return new DerethDateTime(year: newYear, month: newMonth, day: Day, hour: Hour);
        }

        /// <summary>
        /// Returns a new <see cref="DerethDateTime"/> that subtracts the specified number of months from the value of this instance.
        /// </summary>
        /// <param name="numOfMonthsToSubtract">A number of months. The numOfMonthsToSubtract parameter can be negative or positive.</param>
        public DerethDateTime SubtractMonths(int numOfMonthsToSubtract)
        {
            int opposite = numOfMonthsToSubtract * -1;

            return new DerethDateTime(ticks: Ticks).AddMonths(opposite);
        }

        /// <summary>
        /// Returns a new <see cref="DerethDateTime"/> that adds the specified number of days to the value of this instance.
        /// </summary>
        /// <param name="numOfDaysToAdd">A number of days. The numOfDaysToAdd parameter can be negative or positive.</param>
        public DerethDateTime AddDays(int numOfDaysToAdd)
        {
            int newYear = Year;
            int newMonth = Month;
            int newDay = Day;

            if (numOfDaysToAdd > 0)
            {
                for (double i = 0; i < numOfDaysToAdd; i++)
                {
                    if (newDay == 30)
                    {
                        if (newMonth == (int)Months.Wintersebb)
                        {
                            newYear++;
                            newMonth++;
                        }
                        else if (newMonth == (int)Months.Frostfell)
                        {
                            newMonth = (int)Months.Snowreap;
                        }
                        else
                            newMonth++;

                        newDay = 1;
                    }
                    else
                        newDay++;
                }
            }
            else
            {
                for (double i = 0; i > numOfDaysToAdd; i--)
                {
                    if (newDay == 1)
                    {
                        if (newMonth == (int)Months.Morningthaw)
                        {
                            newYear--;
                            newMonth--;
                        }
                        else if (newMonth == (int)Months.Snowreap)
                        {
                            newMonth = (int)Months.Frostfell;
                        }
                        else
                            newMonth--;

                        newDay = 30;
                    }
                    else
                        newDay--;
                }
            }

            if (newYear < 10 | newYear > 401)
                throw new ArgumentOutOfRangeException("numOfDaysToAdd", "numOfDaysToAdd results in a portal year less than 10 or greater than 401");

            return new DerethDateTime(year: newYear, month: newMonth, day: newDay, hour: Hour);
        }

        /// <summary>
        /// Returns a new <see cref="DerethDateTime"/> that subtracts the specified number of days from the value of this instance.
        /// </summary>
        /// <param name="numOfDaysToSubtract">A number of days. The numOfDaysToSubtract parameter can be negative or positive.</param>
        public DerethDateTime SubtractDays(int numOfDaysToSubtract)
        {
            int opposite = numOfDaysToSubtract * -1;

            return new DerethDateTime(ticks: Ticks).AddDays(opposite);
        }

        /// <summary>
        /// Returns a new <see cref="DerethDateTime"/> that adds the specified number of hours to the value of this instance.
        /// </summary>
        /// <param name="numOfHoursToAdd">A number of hours. The numOfHoursToAdd parameter can be negative or positive.</param>
        public DerethDateTime AddHours(int numOfHoursToAdd)
        {
            int newYear = Year;
            int newMonth = Month;
            int newDay = Day;
            int newHour = Hour;

            if (numOfHoursToAdd > 0)
            {
                for (double i = 0; i < numOfHoursToAdd; i++)
                {
                    if (newHour == (int)Hours.Gloaming_and_Half)
                    {
                        if (newDay == 30)
                        {
                            if (newMonth == (int)Months.Wintersebb)
                            {
                                newYear++;
                                newMonth++;
                            }
                            else if (newMonth == (int)Months.Frostfell)
                            {
                                newMonth = (int)Months.Snowreap;
                            }
                            else
                                newMonth++;

                            newDay = 1;
                        }
                        else
                            newDay++;

                        newHour = (int)Hours.Darktide;
                    }
                    else
                        newHour++;
                }
            }
            else
            {
                for (double i = 0; i > numOfHoursToAdd; i--)
                {
                    if (newHour == (int)Hours.Darktide)
                    {
                        if (newDay == 1)
                        {
                            if (newMonth == (int)Months.Morningthaw)
                            {
                                newYear--;
                                newMonth--;
                            }
                            else if (newMonth == (int)Months.Snowreap)
                            {
                                newMonth = (int)Months.Frostfell;
                            }
                            else
                                newMonth--;

                            newDay = 30;
                        }
                        else
                            newDay--;

                        newHour = (int)Hours.Gloaming_and_Half;
                    }
                    else
                        newHour--;
                }
            }

            if (newYear < 10 | newYear > 401)
                throw new ArgumentOutOfRangeException("numOfDaysToAdd", "numOfDaysToAdd results in a portal year less than 10 or greater than 401");

            return new DerethDateTime(year: newYear, month: newMonth, day: newDay, hour: newHour);
        }

        /// <summary>
        /// Returns a new <see cref="DerethDateTime"/> that subtracts the specified number of hours from the value of this instance.
        /// </summary>
        /// <param name="numOfHoursToSubtract">A number of hours. The numOfDaysToSubtract parameter can be negative or positive.</param>
        public DerethDateTime SubtractHours(int numOfHoursToSubtract)
        {
            int opposite = numOfHoursToSubtract * -1;

            return new DerethDateTime(ticks: Ticks).AddHours(opposite);
        }

        /// <summary>
        /// Converts the value of the current <see cref="DerethDateTime"/> object to its equivalent string representation using the following format:
        /// <para>Date: MonthName ##, ### P.Y.  Time: HourName</para>
        /// Example: Date: Wintersebb 1, 10 P.Y.  Time: Morntide-and-Half
        /// </summary>
        public override string ToString()
        {
            return "Date: " + Enum.GetName(typeof(Months), Month) + " " + Day + ", " + Year + " P.Y.  Time: " + Enum.GetName(typeof(Hours), Hour).Replace("_", "-");
        }

        /// <summary>
        /// Converts the value of the current <see cref="DerethDateTime.Month"/> object to its equivalent string representation.
        /// </summary>
        public string MonthToString()
        {
            return Enum.GetName(typeof(Months), Month);
        }

        /// <summary>
        /// Converts the value of the current <see cref="DerethDateTime.MonthName"/> object to its equivalent string representation.
        /// </summary>
        public string MonthNameToString()
        {
            return Enum.GetName(typeof(Months), MonthName);
        }

        /// <summary>
        /// Converts the value of the current <see cref="DerethDateTime.Hour"/> object to its equivalent string representation.
        /// </summary>
        public string HourToString()
        {
            return Enum.GetName(typeof(Hours), Hour).Replace("_", "-");
        }

        /// <summary>
        /// Converts the value of the current <see cref="DerethDateTime.HourName"/> object to its equivalent string representation.
        /// </summary>
        public string HourNameToString()
        {
            return Enum.GetName(typeof(Hours), HourName).Replace("_", "-");
        }

        /// <summary>
        /// Converts the value of the current <see cref="DerethDateTime.Time"/> object to its equivalent string representation.
        /// </summary>
        public string TimeToString()
        {
            return Enum.GetName(typeof(Hours), Time).Replace("_", "-");
        }

        /// <summary>
        /// Converts the value of the current <see cref="DerethDateTime"/> object to its equivalent string representation of the date.
        /// </summary>
        public string DateToString()
        {
            return Enum.GetName(typeof(Months), Month) + " " + Day + ", " + Year + " P.Y.";
        }

        /// <summary>
        /// Converts the value of the current <see cref="DerethDateTime"/> object to its equivalent string representation of the portal year.
        /// </summary>
        public string PYToString()
        {
            return Year + " P.Y.";
        }

        /// <summary>
        /// Converts the value of the current <see cref="DerethDateTime"/> object to its equivalent string representation of the portal year.
        /// </summary>
        public string PortalYearString()
        {
            return Year + " P.Y.";
        }

        /// <summary>
        /// Converts the value of the current <see cref="DerethDateTime"/> object to its equivalent string representation of the portal year.
        /// </summary>
        public string YearToString()
        {
            return Year + " P.Y.";
        }

        /// <summary>
        /// Returns a new <see cref="DerethDateTime"/> that coverts the value of argument to Derethian Date and Time.
        /// </summary>
        /// <param name="dateToBeConverted">A <see cref="DateTime"/> object.</param>
        private static DerethDateTime ConvertFrom_RealWorld_to_Derethian_PY(DateTime dateToBeConverted)
        {
            int convertedYear = 10;
            int convertedMonth = dateToBeConverted.Month - 3;
            int convertedDay = dateToBeConverted.Day;
            int convertedHour = 0;

            int yearsToAdd = dateToBeConverted.Year - retailDayOne_RealWorld.Year;

            convertedYear += yearsToAdd;

            if (convertedDay > 30)
                convertedDay = 30;

            if (dateToBeConverted.Hour >= 00 && dateToBeConverted.Hour <= 02)
            {
                if (dateToBeConverted.Hour == 00 || (dateToBeConverted.Hour == 01 && dateToBeConverted.Minute <= 29))
                    convertedHour = (int)Hours.Darktide;
                else
                    convertedHour = (int)Hours.Darktide_and_Half;
            }
            else if (dateToBeConverted.Hour >= 03 && dateToBeConverted.Hour <= 05)
            {
                if (dateToBeConverted.Hour == 03 || (dateToBeConverted.Hour == 04 && dateToBeConverted.Minute <= 29))
                    convertedHour = (int)Hours.Foredawn;
                else
                    convertedHour = (int)Hours.Foredawn_and_Half;
            }
            else if (dateToBeConverted.Hour >= 06 && dateToBeConverted.Hour <= 08)
            {
                if (dateToBeConverted.Hour == 06 || (dateToBeConverted.Hour == 07 && dateToBeConverted.Minute <= 29))
                    convertedHour = (int)Hours.Dawnsong;
                else
                    convertedHour = (int)Hours.Dawnsong_and_Half;
            }
            else if (dateToBeConverted.Hour >= 09 && dateToBeConverted.Hour <= 11)
            {
                if (dateToBeConverted.Hour == 09 || (dateToBeConverted.Hour == 10 && dateToBeConverted.Minute <= 29))
                    convertedHour = (int)Hours.Morntide;
                else
                    convertedHour = (int)Hours.Morntide_and_Half;
            }
            else if (dateToBeConverted.Hour >= 12 && dateToBeConverted.Hour <= (02 + 12))
            {
                if (dateToBeConverted.Hour == 12 || (dateToBeConverted.Hour == (01 + 12) && dateToBeConverted.Minute <= 29))
                    convertedHour = (int)Hours.Midsong;
                else
                    convertedHour = (int)Hours.Midsong_and_Half;
            }
            else if (dateToBeConverted.Hour >= (03 + 12) && dateToBeConverted.Hour <= (05 + 12))
            {
                if (dateToBeConverted.Hour == (03 + 12) || (dateToBeConverted.Hour == (04 + 12) && dateToBeConverted.Minute <= 29))
                    convertedHour = (int)Hours.Warmtide;
                else
                    convertedHour = (int)Hours.Warmtide_and_Half;
            }
            else if (dateToBeConverted.Hour >= (06 + 12) && dateToBeConverted.Hour <= (08 + 12))
            {
                if (dateToBeConverted.Hour == (06 + 12) || (dateToBeConverted.Hour == (07 + 12) && dateToBeConverted.Minute <= 29))
                    convertedHour = (int)Hours.Evensong;
                else
                    convertedHour = (int)Hours.Evensong_and_Half;
            }
            else if (dateToBeConverted.Hour >= (09 + 12) && dateToBeConverted.Hour <= (11 + 12))
            {
                if (dateToBeConverted.Hour == (09 + 12) || (dateToBeConverted.Hour == (10 + 12) && dateToBeConverted.Minute <= 29))
                    convertedHour = (int)Hours.Gloaming;
                else
                    convertedHour = (int)Hours.Gloaming_and_Half;
            }
            else
                convertedHour = (int)Hours.Darktide;

            return new DerethDateTime(year: convertedYear, month: convertedMonth, day: convertedDay, hour: convertedHour);
        }

        /// <summary>
        /// Converts the value of the current <see cref="DerethDateTime"/> object to Derethian PY Time
        /// </summary>
        /// <param name="dateTime">A <see cref="DateTime"/> object.</param>
        public static DerethDateTime ConvertRealWorldToLoreDateTime(DateTime dateTime)
        {
            return ConvertFrom_RealWorld_to_Derethian_PY(dateTime);
        }

        /// <summary>
        /// Converts the <see cref="DateTime.UtcNow"/> object to a new <see cref="DerethDateTime"/> object set to Lore Time.
        /// </summary>
        public static DerethDateTime UtcNowToLoreTime => ConvertRealWorldToLoreDateTime(DateTime.UtcNow);

        /// <summary>
        /// Converts the <see cref="DateTime.UtcNow"/> object to a new <see cref="DerethDateTime"/> object set to GDLE Time.
        /// </summary>
        public static DerethDateTime UtcNowToGDLETime => new DerethDateTime((DateTime.UtcNow - new DateTime(1999, 9, 1)).TotalSeconds);

        /// <summary>
        /// Converts the <see cref="DateTime.UtcNow"/> object to a new <see cref="DerethDateTime"/> object set to EMU Standard Sync Time.
        /// </summary>
        public static DerethDateTime UtcNowToEMUTime => new DerethDateTime((DateTime.UtcNow - TimeZoneInfo.ConvertTimeToUtc(retailDayLast_RealWorld, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"))).TotalSeconds);
    }
}
