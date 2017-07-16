using ACE.Entity;
using ACE.Managers;
using ACE.Network.GameEvent.Events;
using System;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionQueryHealth
    {
        [GameAction(GameActionType.QueryHealth)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint fullId = message.Payload.ReadUInt32();

            ObjectGuid guid = new ObjectGuid(fullId);

            if (guid.High == 80)
                guid.ChangeGuidType(GuidType.Player);
            else
                guid.ChangeGuidType(GuidType.Undef);

            session.Player.HandleActionQueryHealth(guid);
        }
    }
}
