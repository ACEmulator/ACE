using ACE.Entity.Enum;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionChangeCombatMode
    {
        [GameAction(GameActionType.ChangeCombatMode)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint newCombatMode = message.Payload.ReadUInt32();

            session.Player.HandleGameActionChangeCombatMode((CombatMode)newCombatMode);
        }
    }
}
