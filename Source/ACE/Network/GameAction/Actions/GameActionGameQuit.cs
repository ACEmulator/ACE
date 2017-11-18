using System;
using ACE.Common.Extensions;
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionGameQuit
    {
        [GameAction(GameActionType.Quit2)]
        public static void Handle(ClientMessage message, Session session)
        {
            ////var target = message.Payload.ReadString16L();
            ////DateTime playerDOB = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            ////playerDOB = playerDOB.AddSeconds(session.Player.CreationTimestamp).ToUniversalTime();

            ////var dobEvent = new GameMessages.Messages.GameMessageSystemChat($"You were born on {playerDOB.ToString("G")}.", ChatMessageType.Broadcast);

            ////session.Network.EnqueueSend(dobEvent);

            // var gameId = message.Payload.ReadUInt32(); // object id of gameboard
            // var whichTeam = message.Payload.ReadUInt32(); // expecting 0xFFFFFFFF here

            var dobEvent = new GameMessages.Messages.GameMessageSystemChat($"Quit request ack.", ChatMessageType.Broadcast);

            // var msgJoinResponse = new GameEvent.Events.GameEventJoinGameResponse(session, gameId, 1);

            var gameId = 2056986625u;

            var msgGameOver = new GameEvent.Events.GameEventGameOver(session, gameId, -2);

            session.Network.EnqueueSend(dobEvent, msgGameOver);
        }
    }
}
