using System.Collections.Generic;

using ACE.Common.Extensions;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionQueryAge
    {
        [GameAction(GameActionType.QueryAge)]
        public static void Handle(ClientMessage message, Session session)
        {
            var target = message.Payload.ReadString16L();  // unused?

            var ageMsg = CalculateAgeMessage(session.Player.Age ?? 0);

            session.Network.EnqueueSend(new GameEventQueryAgeResponse(session, string.Empty, ageMsg));
        }

        private const int SecondsPerMinute = 60;
        private const int MinutesPerHour = 60;
        private const int HoursPerDay = 24;

        private const int DaysPerWeek = 7;
        private const int DaysPerMonth = 30;
        private const int DaysPerYear = 365;

        private const int SecondsPerHour = SecondsPerMinute * MinutesPerHour;     // 3600
        private const int SecondsPerDay = SecondsPerHour * HoursPerDay;           // 86400
        private const int SecondsPerWeek = SecondsPerDay * DaysPerWeek;           // 604800
        private const int SecondsPerMonth = SecondsPerDay * DaysPerMonth;         // 2592000
        private const int SecondsPerYear = SecondsPerDay * DaysPerYear;           // 31536000

        public static string CalculateAgeMessage(int ageSeconds)
        {
            var years = 0;
            var months = 0;
            var weeks = 0;
            var hours = 0;
            var days = 0;
            var minutes = 0;
            var seconds = 0;

            var remaining = ageSeconds;

            if (remaining >= SecondsPerYear)
            {
                years = remaining / SecondsPerYear;
                remaining -= years * SecondsPerYear;
            }

            if (remaining >= SecondsPerMonth)
            {
                months = remaining / SecondsPerMonth;
                remaining -= months * SecondsPerMonth;
            }

            if (remaining >= SecondsPerWeek)
            {
                weeks = remaining / SecondsPerWeek;
                remaining -= weeks * SecondsPerWeek;
            }

            if (remaining >= SecondsPerDay)
            {
                days = remaining / SecondsPerDay;
                remaining -= days * SecondsPerDay;
            }

            if (remaining >= SecondsPerHour)
            {
                hours = remaining / SecondsPerHour;
                remaining -= hours * SecondsPerHour;
            }

            if (remaining >= SecondsPerMinute)
            {
                minutes = remaining / SecondsPerMinute;
                remaining -= minutes * SecondsPerMinute;
            }

            seconds = remaining;

            var pieces = new List<string>();

            if (years > 0)
                pieces.Add($"{years}y");
            if (months > 0)
                pieces.Add($"{months}mo");
            if (weeks > 0)
                pieces.Add($"{weeks}w");
            if (days > 0)
                pieces.Add($"{days}d");
            if (hours > 0)
                pieces.Add($"{hours}h");
            if (minutes > 0)
                pieces.Add($"{minutes}m");
            if (seconds > 0)
                pieces.Add($"{seconds}s");

            return string.Join(" ", pieces);
        }
    }
}
