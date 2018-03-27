using ACE.Entity;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionMagicCastTargetedSpell
    {
        [GameAction(GameActionType.CastTargetedSpell)]
        public static void Handle(ClientMessage message, Session session)
        {
            var objectId = message.Payload.ReadUInt32();
            var spellId = message.Payload.ReadUInt32();

            ObjectGuid guid = new ObjectGuid(objectId);

            session.Player.HandleActionCastTargetedSpell(guid, spellId, session);
        }
    }
}
