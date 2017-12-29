using System;
using System.Threading.Tasks;

using ACE.Common.Extensions;
using ACE.Entity.Enum;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionQueryBirth
    {
        [GameAction(GameActionType.QueryBirth)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            var target = message.Payload.ReadString16L();
            DateTime playerDOB = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            playerDOB = playerDOB.AddSeconds(session.Player.CreationTimestamp).ToUniversalTime();

            var dobEvent = new GameMessages.Messages.GameMessageSystemChat($"You were born on {playerDOB.ToString("G")}.", ChatMessageType.Broadcast);

            session.Network.EnqueueSend(dobEvent);
        }
        #pragma warning restore 1998
    }
}
