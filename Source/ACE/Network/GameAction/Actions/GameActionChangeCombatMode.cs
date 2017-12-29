using System.Threading.Tasks;

using ACE.Entity.Enum;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionChangeCombatMode
    {
        [GameAction(GameActionType.ChangeCombatMode)]
        public static async Task Handle(ClientMessage message, Session session)
        {
            uint newCombatMode = message.Payload.ReadUInt32();
            await session.Player.SetCombatMode((CombatMode)newCombatMode);
        }
    }
}
