using System;

using ACE.Common.Extensions;
using ACE.Entity.Enum;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionQueryBirth
    {
        [GameAction(GameActionType.QueryBirth)]
        public static void Handle(ClientMessage message, Session session)
        {
            var target = message.Payload.ReadString16L();
            DateTime playerDOB = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            playerDOB = playerDOB.AddSeconds(session.Player.CreationTimestamp.Value).ToUniversalTime();

            var dobEvent = new GameMessages.Messages.GameMessageSystemChat($"You were born on {playerDOB:G}.", ChatMessageType.Broadcast);

            session.Network.EnqueueSend(dobEvent);
        }
    }
}
