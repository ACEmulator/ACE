using ACE.Common.Extensions;
using ACE.Entity;
using ACE.Network.Enum;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionChangeCombatMode
    {
        [GameAction(GameActionType.ChangeCombatMode)]
        public static void Handle(ClientMessage message, Session session)
        {
            var newCombatMode = message.Payload.ReadUInt32();
            session.Player.SetCombatMode((CombatMode)newCombatMode, session.Player);
        }
    }
}
