
using System;
using ACE.Common.Extensions;
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionType.QueryBirth)]
    public class GameActionQueryBirth : GameActionPacket
    {
        private string target;

        public GameActionQueryBirth(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Read()
        {
            target = Fragment.Payload.ReadString16L();
        }

        public override void Handle()
        {

            DateTime playerDOB = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            playerDOB = playerDOB.AddSeconds(Session.Player.PropertiesInt[Entity.Enum.Properties.PropertyInt.CreationTimestamp]).ToUniversalTime();

            var dobEvent = new GameMessages.Messages.GameMessageSystemChat($"You were born on {playerDOB.ToString("G")}.",ChatMessageType.Broadcast);

            Session.Network.EnqueueSend(dobEvent);
        }
    }
}
