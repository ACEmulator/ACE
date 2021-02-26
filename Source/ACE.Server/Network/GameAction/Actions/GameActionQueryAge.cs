using System;

using ACE.Common.Extensions;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionQueryAge
    {
        [GameAction(GameActionType.QueryAge)]
        public static void Handle(ClientMessage message, Session session)
        {
            var target = message.Payload.ReadString16L(); // unused?

            string ageMsg = "";

            var ageSeconds = session.Player.Age.Value;
            var years = ageSeconds / 31536000;
            var months = ageSeconds % 31536000 / 2592000;
            var _hours = ageSeconds % 31536000 % 2592000;
            var weeks = _hours / 604800;
            _hours %= 604800;
            var days = _hours / 86400;
            _hours %= 86400;
            var hours = _hours / 3600;
            _hours %= 3600;
            var seconds = _hours % 60;
            var minutes = _hours / 60;

            if (years > 0)
                ageMsg += $"{years}y ";
            if (months > 0)
                ageMsg += $"{months}mo ";
            if (weeks > 0)
                ageMsg += $"{weeks}w ";
            if (days > 0)
                ageMsg += $"{days}d ";
            if (hours > 0)
                ageMsg += $"{hours}h ";
            if (minutes > 0)
                ageMsg += $"{minutes}m ";
            if (seconds > 0)
                ageMsg += $"{seconds}s";

            ageMsg = ageMsg.TrimEnd();

            var ageEvent = new GameEvent.Events.GameEventQueryAgeResponse(session, "", ageMsg);

            session.Network.EnqueueSend(ageEvent);
        }
    }
}
