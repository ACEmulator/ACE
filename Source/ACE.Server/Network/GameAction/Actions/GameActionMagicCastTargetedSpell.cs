using System;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionMagicCastTargetedSpell
    {
        [GameAction(GameActionType.CastTargetedSpell)]
        public static void Handle(ClientMessage message, Session session)
        {
            var targetGuid = message.Payload.ReadUInt32();
            var spellId = message.Payload.ReadUInt32();

            //Console.WriteLine($"{session.Player.Name}.HandleActionCastTargetedSpell({targetGuid:X8}, {spellId})");

            session.Player.HandleActionCastTargetedSpell(targetGuid, spellId);
        }
    }
}
