using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages;
using ACE.Network.Managers;
using ACE.Entity;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionIdentifyObject
    {
        [GameAction(GameActionType.IdentifyObject)]
        public static void Handle(ClientMessage message, Session session)
        {
            var id = message.Payload.ReadUInt32();

            ObjectGuid guid = new ObjectGuid(id);

            if (guid.High == 80)
                guid.ChangeGuidType(GuidType.Player);
            else
                guid.ChangeGuidType(GuidType.Undef);

            session.Player.HandleActionExamination(guid);
        }
    }
}